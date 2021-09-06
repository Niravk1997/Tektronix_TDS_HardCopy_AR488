using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace Tektronix_TDS_HardCopy_AR488
{
    public partial class HardCopy_Window : MetroWindow
    {
        private void CopyImage_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap HardCopy_Bitmap = new RenderTargetBitmap(640, 480, 96, 96, PixelFormats.Pbgra32);
            HardCopy_Bitmap.Render(HardCopy_Image);
            HardCopy_Bitmap.Freeze();
            Clipboard.SetImage(HardCopy_Bitmap);
        }

        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap HardCopy_Bitmap = new RenderTargetBitmap(640, 480, 96, 96, PixelFormats.Pbgra32);
            HardCopy_Bitmap.Render(HardCopy_Image);
            HardCopy_Bitmap.Freeze();
            PngBitmapEncoder HardCopy_PNG = new PngBitmapEncoder();
            HardCopy_PNG.Frames.Add(BitmapFrame.Create(HardCopy_Bitmap));
            try
            {
                var Save_Image_Window = new SaveFileDialog
                {
                    FileName = "HardCopy" + "_" + DateTime.Now.ToString("yyyy-MM-dd h-mm-ss tt") + ".png",
                    Filter = "PNG Files (*.png)|*.png;*.png" +
                      "|All files (*.*)|*.*"
                };

                if (Save_Image_Window.ShowDialog() is true)
                {
                    using (Stream fileStream = File.Create(Save_Image_Window.FileName))
                    {
                        HardCopy_PNG.Save(fileStream);
                    }
                }
            }
            catch (Exception Ex)
            {
                insert_Log("Could not save HardCopy Image.", 1);
                insert_Log(Ex.Message, 1);
            }
        }

        private void CopyImagePlus_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap HardCopy_Bitmap = new RenderTargetBitmap(640, 480, 96, 96, PixelFormats.Pbgra32);
            HardCopy_Bitmap.Render(HardCopy_Area);
            HardCopy_Bitmap.Freeze();
            Clipboard.SetImage(HardCopy_Bitmap);
        }

        private void SaveImagePlus_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap HardCopy_Bitmap = new RenderTargetBitmap(640, 480, 96, 96, PixelFormats.Pbgra32);
            HardCopy_Bitmap.Render(HardCopy_Area);
            HardCopy_Bitmap.Freeze();
            PngBitmapEncoder HardCopy_PNG = new PngBitmapEncoder();
            HardCopy_PNG.Frames.Add(BitmapFrame.Create(HardCopy_Bitmap));
            try
            {
                var Save_Image_Window = new SaveFileDialog
                {
                    FileName = "HardCopy" + "_" + DateTime.Now.ToString("yyyy-MM-dd h-mm-ss tt") + ".png",
                    Filter = "PNG Files (*.png)|*.png;*.png" +
                      "|All files (*.*)|*.*"
                };

                if (Save_Image_Window.ShowDialog() is true)
                {
                    using (Stream fileStream = File.Create(Save_Image_Window.FileName))
                    {
                        HardCopy_PNG.Save(fileStream);
                    }
                }
            }
            catch (Exception Ex)
            {
                insert_Log("Could not save HardCopy Image.", 1);
                insert_Log(Ex.Message, 1);
            }
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            }
            catch (Exception)
            {
                insert_Log("Cannot open software location directory.", 1);
            }
        }
    }
}
