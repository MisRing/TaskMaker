using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace logic
{
    public class Logic
    {
        public static RichTextBox text;
        public static Window EW;
        public static void Interact(RichTextBox textBox, Window ew)
        {
            Logic.text = textBox;
            Logic.EW = ew;
        }
    }
}
