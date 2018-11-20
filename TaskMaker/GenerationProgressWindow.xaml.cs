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
using System.Windows.Shapes;

namespace TaskMaker
{
    /// <summary>
    /// Логика взаимодействия для GenerationProgressWindow.xaml
    /// </summary>
    public partial class GenerationProgressWindow : Window
    {
        public int NeedProgress, currentProgress = 0;
        public GenerationProgressWindow(int needProg)
        {
            NeedProgress = needProg;
            
            InitializeComponent();
        }

        public void ChangeValue(int value)
        {
            currentProgress += value;
            ProgressBar.Value = currentProgress;

            if(ProgressBar.Value == ProgressBar.Maximum)
            {
                button.Visibility = Visibility.Visible;
                Top_Text.Text = "Готово!";
                Top_Text.TextAlignment = TextAlignment.Center;
            }
        }

        public void ChangeValue(int value, string text)
        {
            currentProgress += value;
            ProgressBar.Value = currentProgress;
            Top_Text.Text = text;

            if (ProgressBar.Value == ProgressBar.Maximum)
            {
                button.Visibility = Visibility.Visible;
                Top_Text.Text = "Готово!";
                Top_Text.TextAlignment = TextAlignment.Center;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProgressBar.Minimum = currentProgress;
            ProgressBar.Maximum = NeedProgress;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); ;
        }
    }
}
