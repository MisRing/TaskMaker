using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для QuestionPage.xaml
    /// </summary>
    public partial class QuestionPage : Page
    {
        Window MW = new Window();
        TaskPage taskPage;

        public QuestionPage(Window win, TaskPage tp)
        {
            MW = win;
            taskPage = tp;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(questionInput);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            MW.Content = taskPage;
        }
    }
}
