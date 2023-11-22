using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace World.Helpers
{
    internal class WindControl
    {
        public static void SetRedBlockControll(Window wnd, string textBlockName, string tooltip)
        {
            Control textBlock = wnd.FindName(textBlockName) as Control;
            textBlock.Background = Brushes.IndianRed;
            textBlock.ToolTip = $"{tooltip}";
        }

        public static void SetTransparentBlockControll(Window wnd, string textBlockName)
        {
            Control textBlock = wnd.FindName(textBlockName) as Control;
            textBlock.Background = Brushes.Transparent;
            textBlock.ToolTip = null;
        }
    }
}
