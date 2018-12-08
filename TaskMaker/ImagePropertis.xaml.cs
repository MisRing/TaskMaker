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
using System.Text.RegularExpressions;

namespace TaskMaker
{
    /// <summary>
    /// Логика взаимодействия для ImagePropertis.xaml
    /// </summary>
    public partial class ImagePropertis : Window
    {
        QuestionPage QP;
        System.Drawing.Bitmap image;

        public ImagePropertis(QuestionPage qp, System.Drawing.Bitmap im)
        {
            QP = qp;
            image = im;
            Owner = ((Page)QP).Parent as Window;
            InitializeComponent();
        }

        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Char.IsDigit(e.Text, 0) || (e.Text == ","))
            {
                e.Handled = false;
            }
            else e.Handled = true;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Bitmap b = image;
            int h = b.Height;
            int w = b.Width;
            System.Drawing.Size s = new System.Drawing.Size(w, h);


            if(comboBox.SelectedIndex == 0)
            {
                w = (int)(w * int.Parse(textBox.Text) / 100f);
                h = (int)(h * int.Parse(textBox2.Text) / 100f);
                s = new System.Drawing.Size(w, h);
            }
            else if (comboBox.SelectedIndex == 1)
            {
                w = int.Parse(textBox.Text);
                h = int.Parse(textBox2.Text);
                s = new System.Drawing.Size(w, h);
            }

            b = new System.Drawing.Bitmap(b, s);
            System.Drawing.Image im = (System.Drawing.Image)b;
            string clipText = System.Windows.Forms.Clipboard.GetText();
            System.Windows.Forms.Clipboard.SetText(" ");
            QP.changedRTB.Paste();
            System.Windows.Forms.Clipboard.SetImage(im);
            QP.changedRTB.Paste();
            System.Windows.Forms.Clipboard.Clear();
            try
            {
                System.Windows.Forms.Clipboard.SetText(clipText);
            }
            catch { }

            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (comboBox.SelectedIndex == 0)
                {
                    textBox.Text = "100";
                    textBox2.Text = "100";
                }
                else if (comboBox.SelectedIndex == 1)
                {
                    System.Drawing.Bitmap b = image;
                    int h = b.Height;
                    int w = b.Width;
                    textBox.Text = h.ToString();
                    textBox2.Text = w.ToString();
                }
            }
            catch { }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeImage();
        }

        public void ChangeImage()
        {
            System.Drawing.Bitmap b = image;
            int h = b.Height;
            int w = b.Width;
            System.Drawing.Size s = new System.Drawing.Size(w, h);


            if (comboBox.SelectedIndex == 0)
            {
                w = (int)(w * int.Parse(textBox.Text) / 100f);
                h = (int)(h * int.Parse(textBox2.Text) / 100f);
                s = new System.Drawing.Size(w, h);
            }
            else if (comboBox.SelectedIndex == 1)
            {
                w = int.Parse(textBox.Text);
                h = int.Parse(textBox2.Text);
                s = new System.Drawing.Size(w, h);
            }

            b = new System.Drawing.Bitmap(b, s);
            System.Drawing.Image im = (System.Drawing.Image)b;

            imagePole.Source = ToWpfImage(b);
            imagePole.Width = b.Width;
            imagePole.Height = b.Height;
        }

        public static BitmapImage ToWpfImage(System.Drawing.Image img)
        {
            MemoryStream ms = new MemoryStream();  // no using here! BitmapImage will dispose the stream after loading
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            BitmapImage ix = new BitmapImage();
            ix.BeginInit();
            ix.CacheOption = BitmapCacheOption.OnLoad;
            ix.StreamSource = ms;
            ix.EndInit();
            return ix;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ChangeImage();
            }
            catch { }
        }
    }
}
