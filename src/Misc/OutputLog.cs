using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using MahApps.Metro.Controls;

namespace Tektronix_TDS_HardCopy_AR488
{
    public partial class HardCopy_Window : MetroWindow
    {
        private void Initialize_Output_Log_Collection()
        {
            Output_Log = new ObservableCollection<TextBlock>();
        }

        //inserts message to the output log
        private void insert_Log(string Message, int Code)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt");
            SolidColorBrush Color = Brushes.Black;
            string Status = "";
            if (Code == 1) //Error Message
            {
                Status = "[Error]";
                Color = Brushes.Red;
            }
            else if (Code == 0) //Success Message
            {
                Status = "[Success]";
                Color = Brushes.Green;
            }
            else if (Code == 2) //Warning Message
            {
                Status = "[Warning]";
                Color = Brushes.Orange;
            }
            else if (Code == 3) //Config Message
            {
                Status = "";
                Color = Brushes.DodgerBlue;
            }
            else if (Code == 4)//Standard Message
            {
                Status = "";
                Color = Brushes.Black;
            }
            else if (Code == 5) //Success Message (No label)
            {
                Status = "";
                Color = Brushes.Green;
            }
            else if (Code == 6) //Warning Message (No label)
            {
                Status = "";
                Color = Brushes.Orange;
            }
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
            {
                TextBlock Output_Log_Text = new TextBlock
                {
                    Foreground = Color,
                    Text = "[" + date + "]" + " " + Status + " " + Message.Trim()
                };
                Output_Log.Add(Output_Log_Text);
                Output_Log_Scroll.ScrollToBottom();
            }));
        }

        private void ClearOutputLog_Click(object sender, RoutedEventArgs e)
        {
            Output_Log.Clear();
        }
    }
}
