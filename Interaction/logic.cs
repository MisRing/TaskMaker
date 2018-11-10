using System.Windows;
using System.Windows.Controls;

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
