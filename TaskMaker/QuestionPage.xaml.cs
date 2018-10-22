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
using Interaction;
using Editor;

namespace TaskMaker
{
    /// <summary>
    /// Логика взаимодействия для QuestionPage.xaml
    /// </summary>
    public partial class QuestionPage : Page
    {
        grec gr = new grec();
        math ma = new math();
        spec sp = new spec();
        public Window MW;
        CreateS cre;
        public QuestionPage(CreateS main, Window win)
        {
            cre = main;
            MW = win;
            InitializeComponent();
            Pole.Focus();
        }

        private void richTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            MW.Content = cre;
        }
        //----------------------------------------------------------------------------------------------------------------

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Formula");
            Editor.MainWindow qe = new Editor.MainWindow();
            logic.Logic.Interact(Pole, qe);
            //eq.Activate();
            qe.Show();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            MW.Content = cre;
        }

        private void spec_click(object sender, RoutedEventArgs e)
        {
            f1.Content = sp;
         
        }

        private void grec_Click(object sender, RoutedEventArgs e)
        {
            f1.Content = gr;
        }

        private void math_click(object sender, RoutedEventArgs e)
        {
            f1.Content = ma;
        }
    }
    //----------------------------------------------------------------------------------------------------------------
}
