using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Tektronix_TDS_HardCopy_AR488
{
    public partial class HardCopy_Window : INotifyPropertyChanged
    {
        public ObservableCollection<TextBlock> Output_Log
        {
            get { return (ObservableCollection<TextBlock>)this.GetValue(Output_Log_Property); }
            set { this.SetValue(Output_Log_Property, value); }
        }

        private bool _isHardCopy_Config_Enabled = true;
        public bool isHardCopy_Config_Enabled 
        {
            get { return _isHardCopy_Config_Enabled; }
            set 
            {
                _isHardCopy_Config_Enabled = value;
                NotifyPropertyChanged();
            }
        }

        private int _Total_Bytes = 0;
        public int Total_Bytes 
        {
            get { return _Total_Bytes; }
            set 
            {
                _Total_Bytes = value;
                NotifyPropertyChanged();
            }
        }

        private int _Bytes_Read = 0;
        public int Bytes_Read
        {
            get { return _Bytes_Read; }
            set
            {
                _Bytes_Read = value;
                NotifyPropertyChanged();
            }
        }

        private static readonly DependencyProperty Output_Log_Property = DependencyProperty.Register(nameof(Output_Log), typeof(ObservableCollection<TextBlock>), typeof(TextBlock), new PropertyMetadata(default(ObservableCollection<TextBlock>)));

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
