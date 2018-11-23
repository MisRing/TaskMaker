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
    /// Логика взаимодействия для a1.xaml
    /// </summary>
    public partial class a4 : Page
    {
        Window MW = new Window();
        public a4(Window win)
        {
            MW = win;
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            mainS main = new mainS(MW);
            MW.Content = main;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            HOW h = new HOW(MW);
            MW.Content = h;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            a6 a = new a6(MW);
            MW.Content = a;
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            a3 b = new a3(MW);
            MW.Content = b;
        }
    }
}
