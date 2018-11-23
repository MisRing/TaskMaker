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

        private void Creators_Click(object sender, RoutedEventArgs e)
        {
            creators cre = new creators(MW);
            MW.Content = cre;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MW.Close();

        }

        private void FAQ_Click(object sender, RoutedEventArgs e)
        {
            HOW how = new HOW(MW);
            MW.Content = how;
        }
    }
}
