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
using Task_Maker_v2;
using System.Text.RegularExpressions;

namespace Task_Maker_v2._0
{
    /// <summary>
    /// Логика взаимодействия для TaskPage.xaml
    /// </summary>
    public partial class TaskPage : Page
    {
        public Window MW = new Window();
        public List<Theme> Themes = new List<Theme>();
        public Theme OpenedTheme;
        public int OpenedDif = 0;
        public List<Button> DifButtons = new List<Button>();
        bool loaded = false;

        BrushConverter converter = new System.Windows.Media.BrushConverter();
        Brush brush;

        public TaskPage(Window win)
        {
            MW = win;
            brush = (Brush)converter.ConvertFromString("#FFFCB93E");
            InitializeComponent();
        }

        private void CreateTheme_button_Click(object sender, RoutedEventArgs e)
        {
            CreateTheme("Новая тема");
        }

        public void CreateTheme(string themeName)
        {
            Theme t = new Theme(themeName, Content_scroll, this);
            Content_scroll.Children.Add(t.Main);
            Themes.Add(t);
            Content_scroll.Children.Remove(CreateTheme_button);
            Content_scroll.Children.Add(CreateTheme_button);

            SaveThemes();
        }

        public void LoadThemes()
        {
            string path = "saves/";

            string[] saves = Directory.GetFiles(path, "*");
            foreach(string f in saves)
            {
                string root = Read_itc.GetRoot(f);

                string themeE = Read_itc.GetElements(root, "theme")[0];
                string name = Read_itc.GetValue(themeE, "name")[0];
                string uname = Read_itc.GetValue(themeE, "uname")[0];

                Theme theme = new Theme(name, Content_scroll, this, uname);

                List<string> dif = Read_itc.GetElements(root, "question");
                try
                {
                    foreach (string q in dif)
                    {
                        string text = Read_itc.GetValue(q, "text")[0];
                        int DIF = int.Parse(Read_itc.GetValue(q, "dif")[0]);

                        List<string> images_bytes = Read_itc.GetValue(q, "image");

                        List<System.Drawing.Image> images = new List<System.Drawing.Image>();
                        foreach(string img in images_bytes)
                        {
                            string[] bt = img.Split('*');
                            byte[] bytes = new byte[bt.Length];

                            for(int i = 0; i < bt.Length; i++)
                            {
                                bytes[i] = byte.Parse(bt[i]);
                            }

                            System.Drawing.Image im = byteArrayToImage(bytes);
                            images.Add(im);
                        }

                        theme.CreateQuestion(DIF, Questions_panel, Create_question, text, false, images);
                    }
                }
                catch { }

                Themes.Add(theme);
                Content_scroll.Children.Add(theme.Main);
            }

            Content_scroll.Children.Remove(CreateTheme_button);
            Content_scroll.Children.Add(CreateTheme_button);
        }

        public void SaveThemes()
        {
            foreach(Theme t in Themes)
            {
                List<string> ThemeValues = new List<string>();
                ThemeValues.Add(Read_itc.SetValue("name", t.Name));
                ThemeValues.Add(Read_itc.SetValue("uname", t.Uname));
                string themeEl = Read_itc.SetElement("theme", ThemeValues);

                List<string> DifEl = new List<string>();
                DifEl.Add(themeEl);
                foreach(List<Question> ql in t.Questions)
                {
                    foreach(Question q in ql)
                    {
                        List<string> Values = new List<string>();

                        Values.Add(Read_itc.SetValue("text", q.q_text));
                        Values.Add(Read_itc.SetValue("dif", q.Dif.ToString()));

                        foreach(System.Drawing.Image img in q.im)
                        {
                            byte[] byt = ImageToByte(img);
                            string bytes = "";

                            foreach (byte b in byt)
                            {
                                bytes += b.ToString() + "*";
                            }
                            bytes = bytes.Remove(bytes.Length - 1);

                            Values.Add(Read_itc.SetValue("image", bytes));
                        }
                        DifEl.Add(Read_itc.SetElement("question", Values, 2));
                    }
                }

                string root = Read_itc.SetRoot("Task Maker v2.0", DifEl);
                Read_itc.SaveRoot(root, t.Uname + ".itc", "saves");
            }
        }

        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            System.Drawing.Image returnImage = null;
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
                ms.Write(byteArrayIn, 0, byteArrayIn.Length);
                returnImage = System.Drawing.Image.FromStream(ms, true);
            }
            catch { }
            return returnImage;
        }

        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
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

        public void ChoseThisTheme(Theme theme)
        {
            if (theme != OpenedTheme && theme != null)
            {
                OpenedTheme = theme;
                OpenedDif = 0;

                foreach (Button d in DifButtons)
                {
                    d.Background = brush;
                }

                foreach (Theme t in Themes)
                {
                    t.Main.Background = brush;
                }

                theme.Main.Background = Brushes.LightCyan;

                Questions_panel.Children.Clear();
            }
            else if(theme == null)
            {
                OpenedTheme = null;
                OpenedDif = 0;

                foreach (Button d in DifButtons)
                {
                    d.Background = brush;
                }

                foreach (Theme t in Themes)
                {
                    t.Main.Background = brush;
                }

                Questions_panel.Children.Clear();
            }
        }

        private void back(object sender, RoutedEventArgs e)
        {
            MainPage main = new MainPage(MW);
            MW.Content = main;
        }

        private void DeliteAllThemes(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult dialogResult =
                   System.Windows.Forms.MessageBox.Show(
                     "Вы действительно хотите удалить ВСЕ темы?", "",
                     System.Windows.Forms.MessageBoxButtons.YesNo);


            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                foreach(Theme t in Themes)
                {
                    try
                    {
                        Directory.Delete("_Save/" + t.Uname + ".itc", true);
                    }
                    catch { }
                }

                Themes.Clear();
                Content_scroll.Children.Clear();
                Content_scroll.Children.Add(CreateTheme_button);

                SaveThemes();
            }
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            if (!loaded)
            {
                Questions_panel.Children.Clear();

                DifButtons.Add(difficulty1);
                DifButtons.Add(difficulty2);
                DifButtons.Add(difficulty3);
                DifButtons.Add(difficulty4);
                DifButtons.Add(difficulty5);
                DifButtons.Add(difficulty6);
                DifButtons.Add(difficulty7);
                DifButtons.Add(difficulty8);
                DifButtons.Add(difficulty9);
                DifButtons.Add(difficulty10);

                LoadThemes();
                loaded = true;
            }
        }

        private void ChoseDif(object sender, RoutedEventArgs e)
        {
            if (OpenedTheme != null)
            {
                Button b = sender as Button;

                OpenedDif = int.Parse(b.Content.ToString().Replace("Сложность ", ""));

                foreach(Button d in DifButtons)
                {
                    d.Background = brush;
                }

                b.Background = Brushes.LightCyan;
                Questions_panel.Children.Clear();
                foreach (Question q in OpenedTheme.Questions[OpenedDif-1])
                {
                    Questions_panel.Children.Add(q.can);
                }

                Questions_panel.Children.Add(Create_question);
            }
        }

        private void CreateQuestion(object sender, RoutedEventArgs e)
        {
            if (OpenedDif != 0 && OpenedTheme != null)
            {
                OpenedTheme.CreateQuestion(OpenedDif, Questions_panel, Create_question);
            }

            SaveThemes();
        }
    }

    public class Theme : Window
    {
        public Canvas can;
        public Button Main;
        public Button Close;
        public TextBox NameBox;
        public StackPanel c_s;
        public string Name, Uname;
        public TaskPage MW;
        private bool delited = false;
        public List<List<Question>> Questions = new List<List<Question>>();

        public Theme(string name, StackPanel con_sc, TaskPage mw, string uname = "_false")
        {
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#FFFCB93E");
            if (uname == "_false")
                Uname = RandomString(50);
            else
                Uname = uname;

            MW = mw;
            c_s = con_sc;
            Main = new Button()
            {
                Style = (Style)Application.Current.Resources["ButtonStyle1"],
                Width = 120d,
                Height = 80d,
                Background = brush,
                Margin = new Thickness(0, 6, 0, 0),
                BorderThickness = new Thickness(3, 3, 3, 3),
            };
            can = new Canvas();
            can.Height = 60d;
            Main.Content = can;
            can.Margin = new Thickness(-40, -15, 0, 0);

            Name = name;

            for(int i = 0; i < 10; i++)
            {
                Questions.Add(new List<Question>());
            }

            NameBox = new TextBox()
            {
                Style = (Style)Application.Current.Resources["Arrow_Cursor"],
                BorderThickness = new Thickness(0, 0, 0, 0),
                IsReadOnly = true,
                Background = Brushes.Transparent,
                Width = 113.7,
                Height = 63.7,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Text = name,
                FontSize = 12,
                Margin = new Thickness(-17.5, 11, 0, 0),
            };
            NameBox.TextChanged += NameChanges;
            NameBox.PreviewMouseDoubleClick += ChangeName;
            NameBox.PreviewMouseDown += ChoseThis;
            NameBox.LostFocus += ChangeName_close2;
            NameBox.KeyDown += new KeyEventHandler(OnKeyDownHandler);


            can.Children.Add(NameBox);

            Close = new Button()
            {
                Style = (Style)Application.Current.Resources["ButtonStyle1"],
                Width = 15,
                Height = 15,
                Margin = new Thickness(82, 0, 0, 0),
            };
            can.Children.Add(Close);
            Close.Click += Delit;
            Close.Background = Brushes.Transparent;
            Close.BorderThickness = new Thickness(0, 0, 0, 0);
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            ImageSourceConverter imgs = new ImageSourceConverter();
            img.SetValue(System.Windows.Controls.Image.SourceProperty, imgs.ConvertFromString("resources/cross.png"));
            Close.Content = img;
            Main.Click += ChoseThis;
        }

        public void CreateQuestion(int dif, StackPanel sp, Button add, string text = "Новый вопрос.", bool create = true, List<System.Drawing.Image> images = null)
        {
            sp.Children.Remove(add);
            Question q = new Question(text, sp, MW, this, dif, create);
            if(images != null)
                q.im = images;
            Questions[dif - 1].Add(q);
            if(MW.OpenedDif != 0)
                sp.Children.Add(add);
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                NameBox.IsReadOnly = true;
                NameBox.Style = (Style)Application.Current.Resources["Arrow_Cursor"];
                Keyboard.Focus(null);
                try
                {
                    Main.Focus();
                }
                catch { }
            }
        }

        private bool ch = false;
        public void ChangeName(object sender, MouseButtonEventArgs e)
        {
            NameBox.IsReadOnly = false;
            NameBox.Style = (Style)Application.Current.Resources["I_Cursor"];
            Keyboard.Focus(NameBox);
            NameBox.SelectAll();
            ch = true;

        }

        public void ChangeName_close2(object sender, EventArgs e)
        {
            NameBox.IsReadOnly = true;
            NameBox.Style = (Style)Application.Current.Resources["Arrow_Cursor"];
            Keyboard.Focus(null);
            Main.Focus();
        }

        public void ChangeName_close(object sender, MouseButtonEventArgs e)
        {
            if (ch)
            {
                ch = false;
            }
            else
            {
                NameBox.IsReadOnly = true;
                NameBox.Style = (Style)Application.Current.Resources["Arrow_Cursor"];
                Keyboard.Focus(null);
                Main.Focus();

            }
        }


        public void ChoseThis(object sender, EventArgs e)
        {
            if (!delited)
                MW.ChoseThisTheme(this);
        }

        public void Delit(object sender, EventArgs e)
        {
            System.Windows.Forms.DialogResult dialogResult =
                    System.Windows.Forms.MessageBox.Show(
                      "Вы действительно хотите удалить эту тему?", "",
                      System.Windows.Forms.MessageBoxButtons.YesNo);


            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                Delite_v();
            }
        }

        public void Delite_v()
        {
            try
            {
                File.Delete("saves/" + Uname + ".itc");
            }
            catch { }

            delited = true;
            MW.Themes.Remove(this);
            c_s.Children.Remove(Main);
            try
            {
                if (MW.OpenedTheme.Uname == this.Uname)
                {
                    MW.ChoseThisTheme(null);
                }
            }
            catch { }
            MW.SaveThemes();
        }

        private static Random random = new Random((int)DateTime.Now.Ticks);
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public void NameChanges(Object sender, EventArgs e)
        {
            Name = NameBox.Text;
            //MainWindow.SaveThemes();
        }
    }

    public class Question : Window
    {

        public Canvas can;
        public Button close = new Button();
        public TextBlock text = new TextBlock();
        public string q_text;
        public StackPanel qPan;
        public List<System.Drawing.Image> im = new List<System.Drawing.Image>();
        public TaskPage MW;
        public Theme theme;
        public int Dif = 0;

        public Question(string question_text, StackPanel stack, TaskPage win, Theme t, int dif, bool create = true)
        {
            Dif = dif;
            MW = win;
            theme = t;
            qPan = stack;
            can = new Canvas();
            if(create)
                qPan.Children.Add(can);
            can.Children.Add(close);
            can.Width = 730;
            can.Height = 60;
            can.Margin = new Thickness(0, 6, 0, 0);
            can.Background = Brushes.White;
            q_text = question_text;
            text.Text = q_text;
            can.Children.Add(text);
            text.TextWrapping = TextWrapping.Wrap;
            text.Width = 625;
            text.MaxWidth = 625;
            text.Height = 50;
            text.MaxHeight = 50;
            text.Margin = new Thickness(2, 0, 0, 30);
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.VerticalAlignment = VerticalAlignment.Center;
            close.BorderThickness = new Thickness(0, 0, 0, 0);
            close.Background = Brushes.White;
            close.Height = 15;
            close.Width = 15;
            close.Margin = new Thickness(715, 0, 0, 0);
            close.Click += DeliteQuestion;
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            ImageSourceConverter imgs = new ImageSourceConverter();
            img.SetValue(System.Windows.Controls.Image.SourceProperty, imgs.ConvertFromString("resources/cross.png"));
            close.Content = img;
            can.MouseUp += OpenText;
        }

        public void DeliteQuestion(object sender, RoutedEventArgs e)
        {
            theme.Questions[Dif - 1].Remove(this);
            qPan.Children.Remove(can);
            MW.SaveThemes();
        }

        public void OpenText(object sender, RoutedEventArgs e)
        {
            QuestionPage qp = new QuestionPage(MW.MW, MW);
            MW.MW.Content = qp;
            
        }

        public void InterText(string _text)
        {
            text.Text = _text;
            q_text = _text.Replace("\n", "@:n:@");
            char[] denied = new[] { '\n', '\t', '\r' };
            StringBuilder newString = new StringBuilder();
            foreach (var ch in q_text)
                if (!denied.Contains(ch))
                    newString.Append(ch);

            q_text = newString.ToString();
        }

        void SaveQ(object sender, RoutedEventArgs e)
        {
           // MainWindow.SaveQuestions();
        }

        public void SaveQuestion()
        {
            text.Text = q_text.Replace("@:n:@", "\n");
            //MainWindow.SaveQuestions();
        }
    }


    public static class Read_itc
        {
            public static string GetRoot(string path)
            {
                string root = "";

                try
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            root += line;
                        }
                    }
                }
                catch
                {
                    root = "_notFound";
                }

                return root;
            }

            public static List<string> GetElements(string root, string name)
            {
                List<string> elements = new List<string>();

                root = root.Substring(root.IndexOf('{') + 1, root.LastIndexOf('}') - root.IndexOf('{') - 1);

                while (true)
                {
                    int i = root.IndexOf("(" + name + ")");
                    if (i != -1)
                    {
                        root = root.Substring(i);
                        string element;
                        element = root.Substring(root.IndexOf('{') + 1, root.IndexOf('}') - root.IndexOf('{') - 1);
                        elements.Add(element);
                        root = root.Substring(root.IndexOf('}'));
                    }
                    else
                    {
                        break;
                    }
                }
                return elements;
            }

            public static List<string> GetValue(string element, string name)
            {
                List<string> values = new List<string>();

                while (true)
                {
                    int i = element.IndexOf("<" + name + ":");
                    if (i != -1)
                    {
                        element = element.Substring(element.IndexOf("<" + name + ":") + 1);
                        element = element.Substring(element.IndexOf(":") + 1);
                        string value = "";
                        value = element.Substring(0, element.IndexOf('>'));

                        string _value = "";
                        string[] code = value.Split('^');

                        foreach (string ch in code)
                        {
                            _value += (char)(int.Parse(ch));
                        }

                        value = _value.Replace("@:n:@", "\n");
                        values.Add(value);
                        element = element.Substring(element.IndexOf('>') + 1);
                    }
                    else
                    {
                        break;
                    }
                }

                return values;
            }



            public static string SetValue(string name, string value)
            {
                string Value = "";

                value = value.Replace("\n", "@:n:@");

                foreach (char ch in value)
                {
                    Value += (int)ch + "^";
                }
                try
                {
                    Value = Value.Remove(Value.Length - 1);
                }
                catch { }

                Value = "       <" + name + ":" + Value + ">";
                return Value;
            }

            public static string SetElement(string name, List<string> values, int index = 1)
            {
                string element = "";

                for(int i = 0; i < index; i++)
                {
                    element += "    ";
                }

                element += "(" + name + ")\n";

                for (int i = 0; i < index; i++)
                {
                    element += "    ";
                }

                element += "{";

                foreach (string value in values)
                {
                    element += "\n" + value;
                }

                element += "\n";

                for (int i = 0; i < index; i++)
                {
                    element += "    ";
                }

                element += "}";

                return element;
            }

            public static string SetRoot(string name, List<string> elements)
            {
                string root = "(" + name + ")\n{";

                foreach (string element in elements)
                {
                    root += "\n" + element;
                }

                root += "\n}";

                return root;
            }

            public static int SaveRoot(string root, string filename, string path = "no path")
            {
                if (path != "no path")
                {
                    Directory.CreateDirectory(path);
                    path += "/";
                }
                else
                {
                    path = "";
                }


                using (StreamWriter save = new StreamWriter(path + filename, false))
                {
                    save.Write(root);
                }

                return 0;
            }

        }
}
