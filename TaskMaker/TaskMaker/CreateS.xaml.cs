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
    /// Логика взаимодействия для CreateS.xaml
    /// </summary>
    public partial class CreateS : Page
    {
        Window MW = new Window();
        public CreateS(Window win)
        {

            MW = win;
            InitializeComponent();
        }

        private void menu_Click(object sender, RoutedEventArgs e)
        {
            mainS main = new mainS(MW);
            MW.Content = main;
        }

        private void Search_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (Search.Text == "")
            {
                SearchStatic.Visibility = Visibility.Visible;
            }
        }
        private void Search_TextInput(object sender, TextCompositionEventArgs e)
        {
            SearchStatic.Visibility = Visibility.Hidden;
        }

        private void create_variants_Click(object sender, RoutedEventArgs e)
        {
            MakeVariants VariantW = new MakeVariants();
            VariantW.Owner = MW;
            VariantW.ShowDialog();
            VariantW.Margin = new Thickness(0, 0, 0,0);
        }

        private void make_list_Click(object sender, RoutedEventArgs e)
        {
            MakeList ListW = new MakeList();
            ListW.Owner = MW;
            ListW.ShowDialog();
            ListW.Margin = new Thickness(0, 0, 0, 0);
        }

        private void theme_export_Click(object sender, RoutedEventArgs e)
        {
            Export exportW = new Export();
            exportW.Owner = MW;
            exportW.ShowDialog();
            exportW.Margin = new Thickness(0, 0, 0, 0);
        }
    }
}
