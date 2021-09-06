using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace Tektronix_TDS_HardCopy_AR488
{
    public partial class HardCopy_Window : MetroWindow
    {
        private bool Enable_Alert = true;
        private bool Auto_Save_to_File = true;

        public HardCopy_Window()
        {
            InitializeComponent();
            Set_Culture();
            DataContext = this;
            Initialize_Output_Log_Collection();
            Get_COM_List();
            Load_COM_Config();
            Check_NetFramework47_IsIntalled();
            HardCopy_Canvas.DefaultDrawingAttributes.Color = Colors.Red;
            Show_Instructions();
        }
        private void Set_Culture()
        {
            if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
            }
        }

        private void Check_NetFramework47_IsIntalled() 
        {
            if (Type.GetType("System.Web.Caching.CacheInsertOptions, System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false) != null)
            {

            }
            else 
            {
                insert_Log(".Net Framework 4.7.2 may not be installed on this computer.", 1);
                insert_Log("It must be installed for this software to work properly.", 1);
            }
        }

        private void Show_Instructions() 
        {
            insert_Log("Should work with Tektronix TDS 500 600 700 series oscilloscopes.", 3);
            insert_Log("Select the COM Port from the list by double left clicking on it.", 3);
            insert_Log("Type the correct GPIB Address of your oscilloscope.", 3);
            insert_Log("Make sure AR488 adapter is flashed with latest firmware, default config.", 3);
            insert_Log("Verifty AR488 Adapter by clicking on ++ver button.", 3);
            insert_Log("Verify oscilloscope by clicking *IDN? button.", 3);
            insert_Log("When ready click on Mono or Color button.", 3);

        }

        private void AR488_ver_Click(object sender, RoutedEventArgs e)
        {
            (string Response, bool isResponseValid, bool AnyFault) = AR488_Read_Write(true, "++ver");
            if (isResponseValid & AnyFault == false) 
            {
                insert_Log(Response, 0);
            }
        }

        private void AR488_rst_Click(object sender, RoutedEventArgs e)
        {
            (string Response, bool isResponseValid, bool AnyFault) = AR488_Read_Write(false, "++rst");
            if (AnyFault == false) 
            {
                insert_Log("AR488 Reset Command Send.", 0);
            }
        }

        private void Tektronix_IDN_Click(object sender, RoutedEventArgs e)
        {
            (string Response, bool isResponseValid, bool AnyFault) = Tektronix_Read_Write(true, "*IDN?");
            if (isResponseValid & AnyFault == false)
            {
                insert_Log(Response, 0);
            }
        }

        private void HardCopy_Mono_Click(object sender, RoutedEventArgs e)
        {
            Get_HardCopy(false);
        }

        private void HardCopy_Color_Click(object sender, RoutedEventArgs e)
        {
            Get_HardCopy(true);
        }

        private void AutoSaveFile_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch AutoSaveFile_ToggleSwitch = sender as ToggleSwitch;
            if (AutoSaveFile_ToggleSwitch.IsOn == true)
            {
                Auto_Save_to_File = true;
            }
            else 
            {
                Auto_Save_to_File = false;
            }

        }

        private void AlertSound_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch AlertSound_ToggleSwitch = sender as ToggleSwitch;
            if (AlertSound_ToggleSwitch.IsOn == true)
            {
                Enable_Alert = true;
            }
            else
            {
                Enable_Alert = false;
            }
        }
    }
}
