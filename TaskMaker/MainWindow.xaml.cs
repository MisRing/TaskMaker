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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool check=true;
        public MainWindow()
        {         
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (check)
            {
                mainS main = new mainS(this);
                this.Content = main;
                check=!check;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if (!FileAssociation.IsAssociated(".task"))
            //    FileAssociation.Associate(".task", "ClassID.ProgID", "ext File", "file.ico", "YourApplication.exe");
            string[] args = Environment.GetCommandLineArgs();

            try
            {
                string path = args[1];
                CreateS create = new CreateS(this, path);
                this.Content = create;
            }
            catch { }
        }
    }
}
