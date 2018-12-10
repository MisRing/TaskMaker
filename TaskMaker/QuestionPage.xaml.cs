﻿using System;
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
using System.Text.RegularExpressions;

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
 
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            
            var range = new TextRange(Pole.Document.ContentStart, Pole.Document.ContentEnd);
            var fStream = new MemoryStream();

            range.Save(fStream, DataFormats.Rtf);

            string text = Encoding.UTF8.GetString(fStream.ToArray());
            ques.Text = text;
            FlowDocument fd = new FlowDocument();
            range = new TextRange(fd.ContentStart, fd.ContentEnd);
            range.Load(fStream, DataFormats.Rtf);
            ques.text.Document = fd;
            fStream.Close();

            range = new TextRange(ansPole.Document.ContentStart, ansPole.Document.ContentEnd);
            fStream = new MemoryStream();

            range.Save(fStream, DataFormats.Rtf);

            text = Encoding.UTF8.GetString(fStream.ToArray());
            ques.ansText = text;
            fStream.Close();

            MW.Content = cre;

            cre.SaveAll();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            changedRTB = Pole;
            Load();
        }

        public void Load()
        {
            try
            {
                string text = ques.Text;
                var fStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text));
                FlowDocument fd = new FlowDocument();
                var range = new TextRange(fd.ContentStart, fd.ContentEnd);
                range.Load(fStream, DataFormats.Rtf);
                Pole.Document = Question.ReturnIndexes(fd);
                fStream.Close();

                text = ques.ansText;
                fStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text));
                FlowDocument fd2 = new FlowDocument();
                range = new TextRange(fd2.ContentStart, fd2.ContentEnd);
                range.Load(fStream, DataFormats.Rtf);
                ansPole.Document = Question.ReturnIndexes(fd2);
                fStream.Close();
            }
            catch { }
        }

        //----------------------------------------------------------------------------------------------------------------

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Editor.MainWindow qe = new Editor.MainWindow();
            logic.Logic.Interact(changedRTB, qe);
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
            changedRTB.Paste();
            try { Clipboard.SetText(str); } catch { }
        }

        public RichTextBox changedRTB;

        private bool isBold = false;
        private void Bold_but_Click(object sender, RoutedEventArgs e)
        {
            changedRTB.Focus();
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
            try
            {
                changedRTB.Selection.Select(CurrentSelection.Start, CurrentSelection.End);
            }
            catch { }
        }

        private bool isItalian = false;
        private void Italian_but_Click(object sender, RoutedEventArgs e)
        {
            changedRTB.Focus();
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
            try
            {
                changedRTB.Selection.Select(CurrentSelection.Start, CurrentSelection.End);
            }
            catch { }
        }

        private bool isUnderline = false;
        private void underline_but_Click(object sender, RoutedEventArgs e)
        {
            changedRTB.Focus();
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
            try
            {
                changedRTB.Selection.Select(CurrentSelection.Start, CurrentSelection.End);
            }
            catch { }
        }

        private int scriptMode = 0;

        private void Subscipt_but_Click(object sender, RoutedEventArgs e)
        {
            changedRTB.Focus();
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
            try
            {
                changedRTB.Selection.Select(CurrentSelection.Start, CurrentSelection.End);
            }
            catch { }
        }

        private void Superscript_but_Click(object sender, RoutedEventArgs e)
        {
            changedRTB.Focus();
            if(CurrentSelection != null)
                changedRTB.Selection.Select(CurrentSelection.Start, CurrentSelection.End);

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
           // op.DefaultExt = "Png files (*.png)|*.png|Jpg files (*.jpg)|*.jpg|Bmp files (*.bmp)|*.bmp";
            op.SupportMultiDottedExtensions = true;
            op.Filter = "Any image|*.jpg;*.jpeg;*.png|Png files (*.png)|*.png|Jpg files (*.jpg)|*.jpg|Bmp files (*.bmp)|*.bmp";
            op.FilterIndex = 0;
            op.Title = "Выберите картинку";

            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = op.FileName;
            }
            if (path != "NA")
            {
                ImagePropertis ImP = new ImagePropertis(this, new System.Drawing.Bitmap(System.Drawing.Image.FromFile(path)));
                ImP.ShowDialog();
                /*System.Drawing.Bitmap b = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(path));
                int h = b.Height;
                int w = b.Width;
                System.Drawing.Size s = new System.Drawing.Size(w, h);


                /////////k


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
                changedRTB.Paste();
                System.Windows.Forms.Clipboard.SetImage(im);
                changedRTB.Paste();
                System.Windows.Forms.Clipboard.Clear();
                try
                {
                    System.Windows.Forms.Clipboard.SetText(clipText);
                }
                catch { }*/
            }
        }

        private void Pole_KeyDown(object sender, KeyEventArgs e)
        {
            TextPropertys();
        }
        //
        void ShowSubText()
        {
            String inputString = "NormalText";
            var nonDigitSymbolsTable = new Dictionary<char, char>();
            nonDigitSymbolsTable.Add('+', 'A');
            nonDigitSymbolsTable.Add('-', 'B');
            nonDigitSymbolsTable.Add('=', 'C');
            nonDigitSymbolsTable.Add('(', 'D');
            nonDigitSymbolsTable.Add(')', 'E');
            StringBuilder temp = new StringBuilder();
            int checkToDigit = 0;
            foreach (char t in "1234567890+-=()".ToCharArray())
            {
                if (int.TryParse(t.ToString(), out checkToDigit))
                    temp.Append("\\u208" + t);
                else
                    temp.Append("\\u208" + nonDigitSymbolsTable[t]);
            }

            MessageBox.Show(inputString + GetStringFromUnicodeSymbols(temp.ToString()));
        }
        string GetStringFromUnicodeSymbols(string unicodeString)
        {
            var stringBuilder = new StringBuilder();
            foreach (Match match in Regex.Matches(unicodeString, @"\\u(?<Value>[a-zA-Z0-9]{4})"))
            {
                stringBuilder.AppendFormat(@"{0}",
                                           (Char)int.Parse(match.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber));
            }

            return stringBuilder.ToString();
        }//

        TextSelection CurrentSelection = null;
        public void TextPropertys()
        {
            switch (scriptMode)
            {
                
                case (0):
                    changedRTB.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, BaselineAlignment.Baseline);
                    changedRTB.Selection.ApplyPropertyValue(Inline.FontSizeProperty, 14.0);
                    break;
                case (1):
                    changedRTB.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, BaselineAlignment.Subscript);
                    changedRTB.Selection.ApplyPropertyValue(Inline.FontSizeProperty, 11.0);
                    break;
                case (2):
                    changedRTB.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, BaselineAlignment.Superscript);
                    changedRTB.Selection.ApplyPropertyValue(Inline.FontSizeProperty, 11.5);
                    break;
            }

            if (isBold)
                changedRTB.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            else
                changedRTB.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);

            if (isItalian)
                changedRTB.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            else
                changedRTB.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);

            if (isUnderline)
                changedRTB.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
                changedRTB.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
        }

        private void Pole_SelectionChanged(object sender, RoutedEventArgs e)
        {
            CurrentSelection = changedRTB.Selection;
        }

        private void Pole_TextChanged(object sender, TextChangedEventArgs e)
        {
            string allText = (new TextRange(Pole.Document.ContentStart, Pole.Document.ContentEnd).Text).Replace(" ", "");
            int count = allText.Length - Pole.Document.Blocks.Count * 2;
            k_theme.Content = "Количество символов: " + count.ToString();
        }

        private void Page_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }

        private void Pole_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {

        }

        private void Pole_GotFocus(object sender, RoutedEventArgs e)
        {
            changedRTB = Pole;
        }

        private void ansPole_GotFocus(object sender, RoutedEventArgs e)
        {
            changedRTB = ansPole;
        }

        private void ansPole_KeyDown(object sender, KeyEventArgs e)
        {
            TextPropertys();
        }
    }
    //----------------------------------------------------------------------------------------------------------------
}
