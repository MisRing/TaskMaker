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

namespace Task_Maker_v2._0
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        Window MW = new Window();       
        public MainPage(Window win)
        {
            MW = win;
            InitializeComponent();
        }

        private void OpenTask(object sender, RoutedEventArgs e)
        {
            TaskPage task = new TaskPage(MW);
            MW.Content = task;
        }
        private void ShowCreators(object sender, RoutedEventArgs e)
        {
            Creators cre = new Creators(MW);
            MW.Content = cre;
        }

        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            About faq = new About(MW);
            MW.Content = faq;
        }

        private void WindowClose(object sender, RoutedEventArgs e)
        {
            MW.Close();
        }
    }
}
