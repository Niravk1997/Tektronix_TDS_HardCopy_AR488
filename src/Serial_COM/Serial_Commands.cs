using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;

namespace Tektronix_TDS_HardCopy_AR488
{
    public partial class HardCopy_Window : MetroWindow
    {
        private (string, bool, bool) AR488_Read_Write(bool isQuery, string Command)
        {
            string Serial_Response = "";
            bool isResponse_Valid = false;
            bool AnyFault = false;
            if (COM_Config_Updater())
            {
                try
                {
                    using (var Serial = new SerialPort(COM_Port_Name, COM_BaudRate_Value, (Parity)COM_Parity_Value, COM_DataBits_Value, (StopBits)COM_StopBits_Value))
                    {
                        Serial.WriteTimeout = COM_WriteTimeout_Value;
                        Serial.ReadTimeout = COM_ReadTimeout_Value;
                        Serial.Handshake = (Handshake)COM_Handshake_Value;
                        Serial.RtsEnable = COM_RtsEnable;
                        Serial.Encoding = System.Text.Encoding.GetEncoding(28591);
                        Serial.Open();
                        Serial.WriteLine(Command);
                        if (isQuery) 
                        {
                            Serial_Response = Serial.ReadLine();
                            isResponse_Valid = true;
                        }
                        Serial.Close();
                        AnyFault = false;
                    }
                }
                catch (Exception Ex)
                {
                    insert_Log(Ex.Message, 1);
                    AnyFault = true;
                }
                return (Serial_Response, isResponse_Valid, AnyFault);
            }
            else
            {
                insert_Log("Serial COM settings are not valid. Try again.", 2);
                AnyFault = true;
                return (Serial_Response, isResponse_Valid, AnyFault);
            }
        }

        private (string, bool, bool) Tektronix_Read_Write(bool isQuery, string Command)
        {
            string Serial_Response = "";
            bool isResponse_Valid = false;
            bool AnyFault = false;
            if (COM_Config_Updater())
            {
                try
                {
                    using (var Serial = new SerialPort(COM_Port_Name, COM_BaudRate_Value, (Parity)COM_Parity_Value, COM_DataBits_Value, (StopBits)COM_StopBits_Value))
                    {
                        Serial.WriteTimeout = COM_WriteTimeout_Value;
                        Serial.ReadTimeout = COM_ReadTimeout_Value;
                        Serial.Handshake = (Handshake)COM_Handshake_Value;
                        Serial.RtsEnable = COM_RtsEnable;
                        Serial.Encoding = System.Text.Encoding.GetEncoding(28591);
                        Serial.Open();
                        Serial.WriteLine("++addr " + COM_GPIB_Address_Value);
                        Serial.WriteLine("++eor 7");
                        Serial.WriteLine("++eoi 1");
                        Serial.WriteLine(Command);
                        if (isQuery)
                        {
                            Serial.WriteLine("++read");
                            Serial_Response = Serial.ReadLine();
                            isResponse_Valid = true;
                        }
                        Serial.Close();
                        AnyFault = false;
                    }
                }
                catch (Exception Ex)
                {
                    insert_Log(Ex.Message, 1);
                    AnyFault = true;
                }
                return (Serial_Response, isResponse_Valid, AnyFault);
            }
            else
            {
                insert_Log("Serial COM settings are not valid. Try again.", 2);
                AnyFault = true;
                return (Serial_Response, isResponse_Valid, AnyFault);
            }
        }

        private void Get_HardCopy(bool isBMPColor)
        {
            if (COM_Config_Updater())
            {
                Task.Run(() => 
                {
                    try
                    {
                        isHardCopy_Config_Enabled = false;
                        List<byte> BMP_Image_Data = new List<byte>();
                        int BMP_Total_Byte = 0;
                        int BMP_Total_Bytes_Read = 0;
                        using (var Serial = new SerialPort(COM_Port_Name, COM_BaudRate_Value, (Parity)COM_Parity_Value, COM_DataBits_Value, (StopBits)COM_StopBits_Value))
                        {
                            Serial.WriteTimeout = COM_WriteTimeout_Value;
                            Serial.ReadTimeout = COM_ReadTimeout_Value;
                            Serial.Handshake = (Handshake)COM_Handshake_Value;
                            Serial.RtsEnable = COM_RtsEnable;
                            Serial.Encoding = System.Text.Encoding.GetEncoding(28591);
                            Serial.Open();
                            Serial.WriteLine("++addr " + COM_GPIB_Address_Value);
                            Serial.WriteLine("++eor 7");
                            Serial.WriteLine("++eoi 1");
                            Serial.WriteLine("++read_tmo_ms 8000");
                            if (isBMPColor)
                            {
                                Serial.WriteLine("HARDCopy:FORMat BMPCOLOR");
                                BMP_Total_Byte = 308278;
                                Total_Bytes = BMP_Total_Byte;
                            }
                            else 
                            {
                                Serial.WriteLine("HARDCopy:FORMat BMP");
                                BMP_Total_Byte = 38462;
                                Total_Bytes = BMP_Total_Byte;
                            }
                            Serial.WriteLine("HARDCopy:LAYout PORTRAIT");
                            Serial.WriteLine("HARDCopy:PALEtte CURRent");
                            Serial.WriteLine("HARDCopy:PORT GPib");

                            Serial.WriteLine("HARDCopy STARt");
                            Serial.WriteLine("++read");

                            Stopwatch Established_Time = new Stopwatch();
                            Established_Time.Start();

                            Stopwatch Timeout_Timer = new Stopwatch();
                            Timeout_Timer.Start();

                            while (BMP_Total_Bytes_Read != BMP_Total_Byte) 
                            {
                                var BytesToRead = Serial.BytesToRead;
                                byte[] Byte_Array = new byte[BytesToRead];
                                Serial.Read(Byte_Array, 0, BytesToRead);
                                BMP_Image_Data.AddRange(Byte_Array);
                                Thread.Sleep(1);
                                BMP_Total_Bytes_Read += BytesToRead;
                                Bytes_Read = BMP_Total_Bytes_Read;
                                if (BytesToRead != 0)
                                {
                                    Timeout_Timer.Restart();
                                }
                                if (Timeout_Timer.Elapsed.TotalSeconds > 5) 
                                {
                                    break;
                                }
                            }
                            Serial.Close();
                            Established_Time.Stop();

                            insert_Log("HardCopy Completed. Total Established Time: " + Established_Time.Elapsed.TotalSeconds + " seconds", 5);

                            if (Auto_Save_to_File) 
                            {
                                File.WriteAllBytes("HardCopy" + "_" + DateTime.Now.ToString("yyyy-MM-dd h-mm-ss tt") + ".bmp", BMP_Image_Data.ToArray());
                            }
                            if (Enable_Alert) 
                            {
                                SystemSounds.Beep.Play();
                            }

                            Convert_to_Image(BMP_Image_Data.ToArray());

                            isHardCopy_Config_Enabled = true;
                        }
                    }
                    catch (Exception Ex) 
                    {
                        isHardCopy_Config_Enabled = true;
                        insert_Log(Ex.Message, 1);
                    }
                });
            }
            else 
            {
                insert_Log("Serial COM settings are not valid. Try again.", 2);
            }
        }

        private void Convert_to_Image(byte[] imageData) 
        {
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            this.Dispatcher.Invoke(() => 
            {
                HardCopy_Image.Source = image;
            });
        }
    }
}
