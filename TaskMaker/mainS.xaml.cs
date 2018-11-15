using System.Windows;
using System.Windows.Controls;


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

        private void About_Click(object sender, RoutedEventArgs e)
        {
            
              About a = new About(MW);
              MW.Content = a;
            
        }
    }
}
