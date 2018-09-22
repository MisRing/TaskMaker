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
    /// Логика взаимодействия для mainS.xaml
    /// </summary>
    public partial class mainS : Page
    {
        Window MW = new Window();
        public mainS(Window win)
        {      MW = win;
   
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            CreateS create = new CreateS(MW);
            MW.Content = create;

        }
    }
}
