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
using System.IO;
using TaskMaker;
using System.Text.RegularExpressions;

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
            VariantW.Margin = new Thickness(0, 0, 0, 0);
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

        public List<Theme> Themes = new List<Theme>();

        private void CreateTheme_button_Click(object sender, RoutedEventArgs e)
        {
            Theme t = new Theme("Новая тема", this, this.Content_scroll);
            Themes.Add(t);
        }

        public Theme choosedTheme = null;
        public int choosedDif = -1;
        public void ChooseTheme(Theme theme)
        {
            choosedTheme = theme;

            if(choosedTheme == null)
            {
                __ChooseThemeName.Visibility = Visibility.Visible;
                __ChooseThemeName.Text = "Выберите тему";
                QuestionPanel.Visibility = Visibility.Hidden;

                foreach (Button dif in DifButtons)
                {
                    dif.Background = Brushes.LightGray;
                    dif.FontWeight = FontWeights.Normal;
                }

                choosedDif = -1;

                QuestionStackPanel.Children.Clear();
                QuestionStackPanel.Children.Add(createQeust_button);

            }
            else
            {
                foreach(Theme t in Themes)
                {
                    t.theme_b.Background = Brushes.LightGray;
                }
                theme.theme_b.Background = Brushes.DarkGray;

                if(choosedDif == -1)
                {
                    __ChooseThemeName.Visibility = Visibility.Visible;
                    __ChooseThemeName.Text = "Выберите сложность";
                    QuestionPanel.Visibility = Visibility.Hidden;
                }
                else
                {
                    __ChooseThemeName.Visibility = Visibility.Hidden;
                    QuestionPanel.Visibility = Visibility.Visible;

                    QuestionStackPanel.Children.Clear();

                    foreach (Question q in choosedTheme.Questions[choosedDif - 1])
                    {
                        QuestionStackPanel.Children.Add(q.can);
                    }

                    QuestionStackPanel.Children.Add(createQeust_button);
                }
                
            }
        }

        private void DeliteAllThemes(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Вы действительно хотите удалить все темы?", "Удаление тем",  MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach(Theme t in Themes)
                {
                    t.del_theme();
                }

                Themes.Clear();
            }
        }

        public Button[] DifButtons = new Button[10];
        private void ThemePageLoaded(object sender, RoutedEventArgs e)
        {
            DifButtons[0] = dif1;
            DifButtons[1] = dif2;
            DifButtons[2] = dif3;
            DifButtons[3] = dif4;
            DifButtons[4] = dif5;
            DifButtons[5] = dif6;
            DifButtons[6] = dif7;
            DifButtons[7] = dif8;
            DifButtons[8] = dif9;
            DifButtons[9] = dif10;
        }

        private void ChooseDif(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            if(choosedTheme != null)
            {
                __ChooseThemeName.Visibility = Visibility.Hidden;
                QuestionPanel.Visibility = Visibility.Visible;

                foreach (Button dif in DifButtons)
                {
                    dif.Background = Brushes.LightGray;
                    dif.FontWeight = FontWeights.Normal;
                }

                thisButton.Background = Brushes.DarkGray;
                thisButton.FontWeight = FontWeights.Bold;
                choosedDif = int.Parse(thisButton.Name.Remove(0, 3));

                QuestionStackPanel.Children.Clear();

                foreach(Question q in choosedTheme.Questions[choosedDif - 1])
                {
                    QuestionStackPanel.Children.Add(q.can);
                }

                QuestionStackPanel.Children.Add(createQeust_button);
            }
        }

        private void CreateQuestion(object sender, RoutedEventArgs e)
        {
            if(choosedTheme != null && choosedDif != -1)
            {
                Question q = new Question("", QuestionStackPanel, this, choosedTheme, choosedDif, MW);

                QuestionStackPanel.Children.Remove(createQeust_button);
                QuestionStackPanel.Children.Add(createQeust_button);
            }
        }
    }
}





