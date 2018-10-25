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
using System.IO;

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
        Question ques;
        public QuestionPage(CreateS main, Window win, Question q)
        {
            ques = q;
            cre = main;
            MW = win;
            InitializeComponent();
            Pole.Focus();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            var range = new TextRange(Pole.Document.ContentStart, Pole.Document.ContentEnd);
            var fStream = new MemoryStream();
            range.Save(fStream, System.Windows.DataFormats.Rtf);

            string text = Encoding.UTF8.GetString(fStream.ToArray());

            ques.Text = text;
            ques.text.Text = new TextRange(Pole.Document.ContentStart, Pole.Document.ContentEnd).Text;
            fStream.Close();

            MW.Content = cre;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Load();
        }

        public void Load()
        {
            try
            {
                string text = ques.Text;
                var fStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text));

                var range = new TextRange(Pole.Document.ContentStart, Pole.Document.ContentEnd);
                range.Load(fStream, System.Windows.DataFormats.Rtf);
                fStream.Close();
            }
            catch { }
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
            foreach(object obj in (sp.Content as Grid).Children)
            {
                Button but = obj as Button;
                but.Click += InputChar;
            }
        }

        private void grec_Click(object sender, RoutedEventArgs e)
        {
            f1.Content = gr;
            foreach (object obj in (gr.Content as Grid).Children)
            {
                try
                {
                    Button but = obj as Button;
                    but.Click += InputChar;
                }
                catch
                {
                    foreach (object ob in (obj as Grid).Children)
                    {
                        Button but = ob as Button;
                        but.Click += InputChar;
                    }
                }
            }
        }

        private void math_click(object sender, RoutedEventArgs e)
        {
            f1.Content = ma;
            foreach (object obj in (ma.Content as Grid).Children)
            {
                Button but = obj as Button;
                but.Click += InputChar;
            }
        }

        public void InputChar(object sender, RoutedEventArgs e)
        {
            string str = Clipboard.GetText();
            Clipboard.SetText((sender as Button).Content.ToString());
            Pole.Paste();
            try { Clipboard.SetText(str); } catch { }
        }

        private bool isBold = false;
        private void Bold_but_Click(object sender, RoutedEventArgs e)
        {
            Pole.Focus();
            Button thisButton = sender as Button;
            if(isBold)
            {
                thisButton.Background = Brushes.LightGray;
            }
            else
            {
                thisButton.Background = Brushes.DarkGray;
            }
            isBold = !isBold;
            TextPropertys();
            Pole.Selection.Select(CurrentSelection.Start, CurrentSelection.End);
        }

        private bool isItalian = false;
        private void Italian_but_Click(object sender, RoutedEventArgs e)
        {
            Pole.Focus();
            Button thisButton = sender as Button;
            if (isItalian)
            {
                thisButton.Background = Brushes.LightGray;
                thisButton.FontWeight = FontWeights.Normal;
            }
            else
            {
                thisButton.Background = Brushes.DarkGray;
                thisButton.FontWeight = FontWeights.Bold;
            }
            isItalian = !isItalian;
            TextPropertys();
            Pole.Selection.Select(CurrentSelection.Start, CurrentSelection.End);
        }

        private bool isUnderline = false;
        private void underline_but_Click(object sender, RoutedEventArgs e)
        {
            Pole.Focus();
            Button thisButton = sender as Button;
            if (isUnderline)
            {
                thisButton.Background = Brushes.LightGray;
                thisButton.FontWeight = FontWeights.Normal;
            }
            else
            {
                thisButton.Background = Brushes.DarkGray;
                thisButton.FontWeight = FontWeights.Bold;
            }
            isUnderline = !isUnderline;
            TextPropertys();
            Pole.Selection.Select(CurrentSelection.Start, CurrentSelection.End);
        }

        private int scriptMode = 0;

        private void Subscipt_but_Click(object sender, RoutedEventArgs e)
        {
            Pole.Focus();
            Button thisButton = sender as Button;
            if (scriptMode == 1)
            {
                thisButton.Background = Brushes.LightGray;
                thisButton.FontWeight = FontWeights.Normal;

                scriptMode = 0;
            }
            else
            {
                thisButton.Background = Brushes.DarkGray;
                thisButton.FontWeight = FontWeights.Bold;

                up_ind.Background = Brushes.LightGray;
                up_ind.FontWeight = FontWeights.Normal;

                scriptMode = 1;
            }
            TextPropertys();
            Pole.Selection.Select(CurrentSelection.Start, CurrentSelection.End);
        }

        private void Superscript_but_Click(object sender, RoutedEventArgs e)
        {
            Pole.Focus();
            Pole.Selection.Select(CurrentSelection.Start, CurrentSelection.End);

            Button thisButton = sender as Button;
            if (scriptMode == 2)
            {
                thisButton.Background = Brushes.LightGray;
                thisButton.FontWeight = FontWeights.Normal;

                scriptMode = 0;
            }
            else
            {
                thisButton.Background = Brushes.DarkGray;
                thisButton.FontWeight = FontWeights.Bold;

                down_ind.Background = Brushes.LightGray;
                down_ind.FontWeight = FontWeights.Normal;

                scriptMode = 2;
            }
            TextPropertys();
        }

        private void picture_but_Click(object sender, RoutedEventArgs e)
        {
            string path = "NA";
            System.Windows.Forms.OpenFileDialog op = new System.Windows.Forms.OpenFileDialog();
            op.DefaultExt = "png";
            op.Filter = "Png files (*.png)|*.png|Jpg files (*.jpg)|*.jpg|Bmp files (*.bmp)|*.bmp";
            op.Title = "Выберите картинку";

            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = op.FileName;
            }
            if (path != "NA")
            {
                System.Drawing.Bitmap b = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(path));
                int h = b.Height;
                int w = b.Width;
                System.Drawing.Size s = new System.Drawing.Size(w, h);

                bool ok = false;

                while (!ok)
                {
                    if (h > 450 || w > 900)
                    {
                        w = (int)((w * 6f) / 10f);
                        h = (int)((h * 6f) / 10f);
                        s = new System.Drawing.Size(w, h);
                    }
                    else
                    {
                        ok = true;
                    }
                }

                b = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(path), s);
                System.Drawing.Image im = (System.Drawing.Image)b;
                string clipText = System.Windows.Forms.Clipboard.GetText();
                System.Windows.Forms.Clipboard.SetText(" ");
                Pole.Paste();
                System.Windows.Forms.Clipboard.SetImage(im);
                Pole.Paste();
                System.Windows.Forms.Clipboard.Clear();
                try
                {
                    System.Windows.Forms.Clipboard.SetText(clipText);
                }
                catch { }
            }
        }

        private void Pole_KeyDown(object sender, KeyEventArgs e)
        {
            TextPropertys();
        }

        TextSelection CurrentSelection = null;
        public void TextPropertys()
        {
            switch (scriptMode)
            {
                
                case (0):
                    Pole.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, BaselineAlignment.Baseline);
                    Pole.Selection.ApplyPropertyValue(Inline.FontSizeProperty, 14.0);
                    break;
                case (1):
                    Pole.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, BaselineAlignment.Subscript);
                    Pole.Selection.ApplyPropertyValue(Inline.FontSizeProperty, 11.0);
                    break;
                case (2):
                    Pole.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, BaselineAlignment.Superscript);
                    Pole.Selection.ApplyPropertyValue(Inline.FontSizeProperty, 11.0);
                    break;
            }

            if (isBold)
                Pole.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            else
                Pole.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);

            if (isItalian)
                Pole.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            else
                Pole.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);

            if (isUnderline)
                Pole.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
                Pole.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
        }

        private void Pole_SelectionChanged(object sender, RoutedEventArgs e)
        {
            CurrentSelection = Pole.Selection;

            // Неработающая хрень
            /*switch((BaselineAlignment)CurrentSelection.GetPropertyValue(Inline.BaselineAlignmentProperty))
            {
                case (BaselineAlignment.Superscript):
                    Superscript_but_Click(up_ind, null);
                    break;
                case (BaselineAlignment.Subscript):
                    Superscript_but_Click(down_ind, null);
                    break;
                default:
                    up_ind.Background = Brushes.LightGray;
                    up_ind.FontWeight = FontWeights.Normal;

                    down_ind.Background = Brushes.LightGray;
                    down_ind.FontWeight = FontWeights.Normal;

                    scriptMode = 0;
                    break;
            }*/
        }

        private void Pole_TextChanged(object sender, TextChangedEventArgs e)
        {
            string allText = (new TextRange(Pole.Document.ContentStart, Pole.Document.ContentEnd).Text).Replace(" ", "");
            int count = allText.Length - Pole.Document.Blocks.Count * 2;
            k_theme.Content = "Количество символов: " + count.ToString();
        }
    }
    //----------------------------------------------------------------------------------------------------------------
}
