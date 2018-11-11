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
using System.Xml.Serialization;

namespace TaskMaker
{
    /// <summary>
    /// Логика взаимодействия для CreateS.xaml
    /// </summary>
    public partial class CreateS : Page
    {
        Window MW = new Window();
        string loadedFilePath = "none";

        public CreateS(Window win)
        {
            MW = win;
            InitializeComponent();
        }

        public CreateS(Window win, string loadedFile)
        {
            MW = win;
            loadedFilePath = loadedFile;
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
            if (choosedTheme != null)
            {
                MakeVariants VariantW = new MakeVariants();
                VariantW.cre = this;
                VariantW.Owner = MW;
                VariantW.ShowDialog();
                VariantW.Margin = new Thickness(0, 0, 0, 0);
            }
            else
            {
                MessageBox.Show("Для того, что бы сгенерировать варианты, вы должны выбрать тему.", "Выберите тему.");
            }
        }
        private void make_list_Click(object sender, RoutedEventArgs e)
        {
            MakeList ListW = new MakeList(this);
            ListW.Owner = MW;
            ListW.ShowDialog();
            ListW.Margin = new Thickness(0, 0, 0, 0);
        }
        private void theme_export_Click(object sender, RoutedEventArgs e)
        {
            Export exportW = new Export(this);
            exportW.Owner = MW;
            exportW.ShowDialog();
            exportW.Margin = new Thickness(0, 0, 0, 0);
        }

        public List<Theme> Themes = new List<Theme>();

        private void CreateTheme_button_Click(object sender, RoutedEventArgs e)
        {
            Theme t = new Theme("Новая тема", this, this.Content_scroll);
            Themes.Add(t);
            Content_scroll.Children.Remove(CreateTheme_button);
            Content_scroll.Children.Add(CreateTheme_button);
            themeSV.ScrollToVerticalOffset(Content_scroll.Children.Count * 60);
            SaveAll();
        }

        public Theme choosedTheme = null;
        public int choosedDif = -1;
        public void ChooseTheme(Theme theme)
        {
            choosedTheme = theme;

            if (choosedTheme == null)
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

                k_theme.Content = "Количество вопросов в теме: —";
                k_dif.Content = "Количество вопросов в сложности: —";
            }
            else
            {
                foreach (Theme t in Themes)
                {
                    t.theme_b.Background = Brushes.LightGray;
                }
                theme.theme_b.Background = Brushes.DarkGray;

                if (choosedDif == -1)
                {
                    __ChooseThemeName.Visibility = Visibility.Visible;
                    __ChooseThemeName.Text = "Выберите сложность";
                    QuestionPanel.Visibility = Visibility.Hidden;

                    int Tcount = 0;
                    foreach (List<Question> qm in choosedTheme.Questions)
                    {
                        Tcount += qm.Count;
                    }
                    k_theme.Content = "Количество вопросов в теме: " + Tcount.ToString();
                    k_dif.Content = "Количество вопросов в сложности: —";
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

                    int Tcount = 0;
                    int Dcount = choosedTheme.Questions[choosedDif - 1].Count;
                    foreach (List<Question> qm in choosedTheme.Questions)
                    {
                        Tcount += qm.Count;
                    }
                    k_theme.Content = "Количество вопросов в теме: " + Tcount.ToString();
                    k_dif.Content = "Количество вопросов в сложности: " + Dcount.ToString();
                }

            }
        }

        private void DeliteAllThemes(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить все темы?", "Удаление тем", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (Theme t in Themes)
                {
                    t.del_theme();
                }

                Themes.Clear();
                SaveAll();

                k_theme.Content = "Количество вопросов в теме: —";
                k_dif.Content = "Количество вопросов в сложности: —";
            }
        }

        public bool WasLoaded = false;
        public Button[] DifButtons = new Button[10];
        public string SaveFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "save.xml";
        private void ThemePageLoaded(object sender, RoutedEventArgs e)
        {
            if (!WasLoaded)
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

                if (File.Exists(SaveFilePath))
                {
                    try
                    {
                        LoadAll();
                        if (loadedFilePath != "none")
                        {
                            if (MessageBox.Show("Импортировать темы из этого файла?", "Импорт тем", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                Import(loadedFilePath);
                                MessageBox.Show("Импорт завершен.", "Готово");
                            }
                            loadedFilePath = "none";
                        }
                    }
                    catch
                    {
                        MessageBox.Show("При загрузке сохраненных данных произошла ошибка. \nВаши сохраненные данные не были утеряны и были скопированны в следующий файл: " + SaveFilePath.Replace(".xml", "") + "_Error.xml", "Ошибка загрузки.");
                        File.Copy(SaveFilePath, SaveFilePath.Replace(".xml", "") + "_Error.xml", true);
                    }
                }

                WasLoaded = true;
            }
        }

        private void ChooseDif(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            if (choosedTheme != null)
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

                foreach (Question q in choosedTheme.Questions[choosedDif - 1])
                {
                    QuestionStackPanel.Children.Add(q.can);
                }

                QuestionStackPanel.Children.Add(createQeust_button);


                int count = choosedTheme.Questions[choosedDif - 1].Count;
                k_dif.Content = "Количество вопросов в сложности: " + count.ToString();
            }
        }

        private void CreateQuestion(object sender, RoutedEventArgs e)
        {
            if (choosedTheme != null && choosedDif != -1)
            {
                Question q = new Question(null, QuestionStackPanel, this, choosedTheme, choosedDif, MW);

                QuestionStackPanel.Children.Remove(createQeust_button);
                QuestionStackPanel.Children.Add(createQeust_button);
                SaveAll();

                int Tcount = 0;
                int Dcount = choosedTheme.Questions[choosedDif - 1].Count;
                foreach (List<Question> qm in choosedTheme.Questions)
                {
                    Tcount += qm.Count;
                }
                k_theme.Content = "Количество вопросов в теме: " + Tcount.ToString();
                k_dif.Content = "Количество вопросов в сложности: " + Dcount.ToString();
            }
        }

        public void SaveAll()
        {
            if (File.Exists(SaveFilePath))
                File.Delete(SaveFilePath);
            List<ThemeData> TD = new List<ThemeData>();
            foreach(Theme t in Themes)
            {
                List<List<QuestionData>> QD = new List<List<QuestionData>>();
                for (int i = 0; i < 10; i++)
                {
                    QD.Add(new List<QuestionData>());
                    foreach(Question q in t.Questions[i])
                    {
                        QD[i].Add(new QuestionData() { Dif = q.Dif, Text = q.Text });
                    }
                }

                TD.Add(new ThemeData() { Questions = QD, ThemeName = t.ThemeName });
            }

            XmlSerializer formatter = new XmlSerializer(TD.GetType());

            using (FileStream fs = new FileStream(SaveFilePath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, TD);
            }
        }

        public void LoadAll()
        {
            Themes.Clear();
            Content_scroll.Children.Clear();
            List<ThemeData> TD = new List<ThemeData>();
            XmlSerializer formatter = new XmlSerializer(TD.GetType());

            using (FileStream fs = new FileStream(SaveFilePath, FileMode.OpenOrCreate))
            {
                TD = (List<ThemeData>)formatter.Deserialize(fs);
            }

            foreach(ThemeData td in TD)
            {
                Theme theme = new Theme();

                theme.ThemeName = td.ThemeName;

                List<Question>[] QD = new List<Question>[10];
                for (int i = 0; i < 10; i++)
                {
                    QD[i] = new List<Question>();

                    foreach (QuestionData qd in td.Questions[i])
                    {
                        Question q = new Question();
                        q.LoadQuestion(qd.Text, QuestionStackPanel, this, theme, qd.Dif, MW);
                        QD[i].Add(q);
                    }
                }

                theme.LoadThisTheme(this, Content_scroll, QD);
                Themes.Add(theme);
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = Search.Text.ToLower();

            if(text.Replace(" ", "").Length == 0)
            {
                Content_scroll.Children.Clear();

                foreach(Theme t in Themes)
                {
                    Content_scroll.Children.Add(t.theme_b);
                }

                Content_scroll.Children.Add(CreateTheme_button);
            }
            else
            {
                Regex trimmer = new Regex(@"\s\s+");

                text = trimmer.Replace(text, " ");
                text = (text[0].ToString() == " ") ? text.Remove(0, 1) : text;

                Content_scroll.Children.Clear();

                foreach (Theme t in Themes)
                {
                    try
                    {
                        string text2 = t.ThemeName.ToLower();
                        text2 = trimmer.Replace(text2, " ");
                        text2 = (text2[0].ToString() == " ") ? text2.Remove(0, 1) : text2;
                        text2 = text2.Substring(0, text.Length);
                        if (string.Compare(text2, text) == 0)
                            Content_scroll.Children.Add(t.theme_b);
                    }
                    catch { }
                }
            }
        }

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchStatic.Visibility = Visibility.Hidden;
        }

        private void theme_import_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog sfd = new System.Windows.Forms.OpenFileDialog();
            sfd.Filter = "Task Maker files (*.task)|*.task";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (sfd.FileName != null)
                {
                    try
                    {
                        Import(sfd.FileName);
                        MessageBox.Show("Импорт завершен.", "Готово");
                    }
                    catch
                    {
                        MessageBox.Show("Не удаётся получить доступ к файлу.", "Ошибка");
                    }
                }
            }
        }

        public void Import(string path)
        {
            List<ThemeData> TD = new List<ThemeData>();
            XmlSerializer formatter = new XmlSerializer(TD.GetType());

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                TD = (List<ThemeData>)formatter.Deserialize(fs);
            }

            foreach (ThemeData td in TD)
            {
                Theme theme = new Theme();

                theme.ThemeName = td.ThemeName;

                List<Question>[] QD = new List<Question>[10];
                for (int i = 0; i < 10; i++)
                {
                    QD[i] = new List<Question>();

                    foreach (QuestionData qd in td.Questions[i])
                    {
                        Question q = new Question();
                        q.LoadQuestion(qd.Text, QuestionStackPanel, this, theme, qd.Dif, MW);
                        QD[i].Add(q);
                    }
                }

                theme.LoadThisTheme(this, Content_scroll, QD);
                Themes.Add(theme);
            }
            Content_scroll.Children.Remove(CreateTheme_button);
            Content_scroll.Children.Add(CreateTheme_button);

            SaveAll();
        }
    }
}





