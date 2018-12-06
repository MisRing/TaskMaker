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
using System.Xml.Serialization;
using System.IO;

namespace TaskMaker
{
    /// <summary>
    /// Логика взаимодействия для Export.xaml
    /// </summary>
    public partial class Export : Window
    {
        CreateS cre;
        List<CheckBox> RadioButtons = new List<CheckBox>();

        public Export(CreateS _cre)
        {
            cre = _cre;
            InitializeComponent();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(Theme t in cre.Themes)
            {
                CheckBox rb = new CheckBox();
                rb.Margin = new Thickness(0, 5, 0, 5);
                rb.FontSize = 18;
                rb.Foreground = Brushes.White;
                rb.VerticalContentAlignment = VerticalAlignment.Center;
                rb.HorizontalContentAlignment = HorizontalAlignment.Left;
                rb.Content = t.ThemeName;
                Stack.Children.Add(rb);
                RadioButtons.Add(rb);
            }
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            List<Theme> ThemesToExport = new List<Theme>();
            foreach(CheckBox rb in RadioButtons)
            {
                if(rb.IsChecked.Value)
                {
                    ThemesToExport.Add(cre.Themes[RadioButtons.IndexOf(rb)]);
                }
            }

            if(ThemesToExport.Count != 0)
            {
                System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
                if(ThemesToExport.Count == 1)
                    sfd.FileName = ThemesToExport[0].ThemeName + ".task";
                else
                    sfd.FileName = "Themes.task";
                sfd.Filter = "Task Maker files (*.task)|*.task";
                sfd.FilterIndex = 1;
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (sfd.FileName != null)
                    {
                        try
                        {
                            ExportThemes(sfd.FileName, ThemesToExport);
                            MessageBox.Show("Экспорт завершен.", "Готово");
                        }
                        catch
                        {
                            MessageBox.Show("Не удаётся получить доступ к файлу.", "Ошибка");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Для экспорта необходимо выбрать 1 или более тем.", "Ошибка");
            }
        }

        public void ExportThemes(string SaveFilePath, List<Theme> Themes)
        {
            if (File.Exists(SaveFilePath))
                File.Delete(SaveFilePath);
            List<ThemeData> TD = new List<ThemeData>();
            foreach (Theme t in Themes)
            {
                List<List<QuestionData>> QD = new List<List<QuestionData>>();
                for (int i = 0; i < 10; i++)
                {
                    QD.Add(new List<QuestionData>());
                    foreach (Question q in t.Questions[i])
                    {
                        QD[i].Add(new QuestionData() { Dif = q.Dif, Text = q.Text, ansText = q.ansText});
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
    }
}
