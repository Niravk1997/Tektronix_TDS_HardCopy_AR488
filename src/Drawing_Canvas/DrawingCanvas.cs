using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace Tektronix_TDS_HardCopy_AR488
{
    public partial class HardCopy_Window : MetroWindow
    {
        // 0 = Pen, 1 = Highlighter, 2 = Eraser
        private int InkCanvas_Tool_Selected = 0;

        private void Color_Red_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Select_Color(Colors.Red);
        }

        private void Color_Green_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Select_Color(Colors.LimeGreen);
        }

        private void Color_Blue_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Select_Color(Colors.Blue);
        }

        private void Color_Aqua_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Select_Color(Colors.Aqua);
        }

        private void Color_Yellow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Select_Color(Colors.Yellow);
        }

        private void Color_Magenta_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Select_Color(Colors.Fuchsia);
        }

        private void Select_Color(Color color)
        {
            if (InkCanvas_Tool_Selected == 0)
            {
                HardCopy_Canvas.DefaultDrawingAttributes.Color = color;
            }
            else if (InkCanvas_Tool_Selected == 1)
            {
                HardCopy_Canvas.DefaultDrawingAttributes.Color = Color.FromArgb(128, color.R, color.G, color.B);
            }
        }

        private void Pen_Click(object sender, RoutedEventArgs e)
        {
            HardCopy_Canvas.EditingMode = InkCanvasEditingMode.Ink;
            InkCanvas_Tool_Selected = 0;
            Select_Color(Colors.Red);
            Set_InkStroke_Size(2);
            InkStroke_Size_Value.Text = "2";
        }

        private void HighLighter_Click(object sender, RoutedEventArgs e)
        {
            HardCopy_Canvas.EditingMode = InkCanvasEditingMode.Ink;
            InkCanvas_Tool_Selected = 1;
            Select_Color(Colors.Yellow);
            Set_InkStroke_Size(20);
            InkStroke_Size_Value.Text = "20";
        }

        private void Eraser_Click(object sender, RoutedEventArgs e)
        {
            HardCopy_Canvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
            InkCanvas_Tool_Selected = 2;
            InkStroke_Size_Value.Text = "NaN";
        }

        private void SizeUp()
        {
            if (InkCanvas_Tool_Selected == 0 || InkCanvas_Tool_Selected == 1)
            {
                double Size;
                bool isValid = double.TryParse(InkStroke_Size_Value.Text, out Size);
                if (isValid)
                {
                    if (Size < 100)
                    {
                        Size++;
                        InkStroke_Size_Value.Text = Size.ToString();
                        Set_InkStroke_Size(Size);
                    }
                    else
                    {
                        Set_InkStroke_DefaultSize();
                    }
                }
                else
                {
                    Set_InkStroke_DefaultSize();
                }
            }
        }

        private void SizeDown()
        {
            if (InkCanvas_Tool_Selected == 0 || InkCanvas_Tool_Selected == 1)
            {
                double Size;
                bool isValid = double.TryParse(InkStroke_Size_Value.Text, out Size);
                if (isValid)
                {
                    if (Size > 1 & Size < 100)
                    {
                        Size--;
                        InkStroke_Size_Value.Text = Size.ToString();
                        Set_InkStroke_Size(Size);
                    }
                    else
                    {
                        Set_InkStroke_DefaultSize();
                    }
                }
                else
                {
                    Set_InkStroke_DefaultSize();
                }
            }
        }

        private void SizeUp_Click(object sender, RoutedEventArgs e)
        {
            SizeUp();
        }

        private void SizeDown_Click(object sender, RoutedEventArgs e)
        {
            SizeDown();
        }

        private void Set_InkStroke_Size(double Size)
        {
            HardCopy_Canvas.DefaultDrawingAttributes.Width = Size;
            HardCopy_Canvas.DefaultDrawingAttributes.Height = Size;
        }

        private void Set_InkStroke_DefaultSize()
        {
            if (InkCanvas_Tool_Selected == 0)
            {
                HardCopy_Canvas.DefaultDrawingAttributes.Width = 2;
                HardCopy_Canvas.DefaultDrawingAttributes.Height = 2;
                InkStroke_Size_Value.Text = "2";
            }
            else if (InkCanvas_Tool_Selected == 1)
            {
                HardCopy_Canvas.DefaultDrawingAttributes.Width = 20;
                HardCopy_Canvas.DefaultDrawingAttributes.Height = 20;
                InkStroke_Size_Value.Text = "20";
            }
            else if (InkCanvas_Tool_Selected == 2)
            {
                HardCopy_Canvas.DefaultDrawingAttributes.Width = 20;
                HardCopy_Canvas.DefaultDrawingAttributes.Height = 20;
                InkStroke_Size_Value.Text = "20";
            }
        }

        private void InkCanvas_Size_TextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                SizeUp();
            }
            else if (e.Delta < 0)
            {
                SizeDown();
            }
        }
    }
}

