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
    /// Логика взаимодействия для creators.xaml
    /// </summary>
    public partial class creators : Page
    {
        Window MW = new Window();
        public creators(Window win)
        {
            MW = win;
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            mainS main = new mainS(MW);
            MW.Content = main;
        }
    }
}
