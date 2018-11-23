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

namespace TaskMaker
{
    /// <summary>
    /// Логика взаимодействия для HOW.xaml
    /// </summary>
    public partial class HOW : Page
    {
        Window MW = new Window();
        public HOW(Window win)
        {
            MW = win;
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        { 
            mainS main = new mainS(MW);
            MW.Content = main;
        }

        private void B1_Click(object sender, RoutedEventArgs e)
        {
            a1 b1 = new a1(MW);
            MW.Content = b1;
        }

        private void B2_Click(object sender, RoutedEventArgs e)
        {
            a2 b2 = new a2(MW);
            MW.Content = b2;
        }

        private void B3_Click(object sender, RoutedEventArgs e)
        {
            a3 b3 = new a3(MW);
            MW.Content = b3;
        }

        private void B4_Click(object sender, RoutedEventArgs e)
        {
            a4 b4 = new a4(MW);
            MW.Content = b4;
        }

        private void B6_Click(object sender, RoutedEventArgs e)
        {
            a6 b6 = new a6(MW);
            MW.Content = b6;
        }
    }

}
