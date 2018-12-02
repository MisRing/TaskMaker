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
using System.IO;
using System.Reflection;

namespace TaskMaker
{
    /// <summary>
    /// Логика взаимодействия для MakeList.xaml
    /// </summary>
    public partial class MakeList : Window
    {
        public CreateS cre;

        public MakeList(CreateS _cre)
        {
            cre = _cre;
            InitializeComponent();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            FlowDocument flowDoc = CreateList();

            if (flowDoc != null)
            {
                MemoryStream ms = new MemoryStream();
                var rrange = new TextRange(flowDoc.ContentStart, flowDoc.ContentEnd);
                var ffStream = new MemoryStream();
                rrange.Save(ffStream, System.Windows.DataFormats.Rtf);
                string stream = Encoding.UTF8.GetString(ffStream.ToArray()).Replace(@"\fs16", @"\fs" + ((int)(int.Parse((FontSizeBox.SelectedItem as ComboBoxItem).Content.ToString()) * 2)).ToString() + @"\sub");
                stream = stream.Replace(@"\fs17", @"\fs" + ((int)(int.Parse((FontSizeBox.SelectedItem as ComboBoxItem).Content.ToString()) * 2)).ToString() + @"\super");
                ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(stream));
                System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();

                if (FormatBox.SelectedIndex == 0)
                {
                    if(DifBox.SelectedIndex == 10)
                        sfd.FileName = cre.Themes[ThemeBox.SelectedIndex].ThemeName + " Questions.pdf";
                    else
                        sfd.FileName = cre.Themes[ThemeBox.SelectedIndex].ThemeName + " Dificult " + DifBox.SelectedIndex.ToString() + " Questions.pdf";
                    sfd.Filter = "pdf files (*.pdf)|*.pdf";
                    sfd.FilterIndex = 1;
                }
                else
                {
                    if (DifBox.SelectedIndex == 10)
                        sfd.FileName = cre.Themes[ThemeBox.SelectedIndex].ThemeName + " Questions.docx";
                    else
                        sfd.FileName = cre.Themes[ThemeBox.SelectedIndex].ThemeName + " Dificult " + DifBox.SelectedIndex.ToString() + " Questions.docx";
                    sfd.Filter = "docx files (*.docx)|*.docx|doc files (*.doc)|*.doc";
                    sfd.FilterIndex = 1;
                }
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (sfd.FileName != null)
                    {
                        try
                        {
                            if (FormatBox.SelectedIndex == 0)
                            {
                                CreateWordDocument("bbb.docx", ms);
                                object missing = Missing.Value;
                                object readOnly = false;
                                object isVisible = false;
                                object sst = System.IO.Path.GetFullPath("bbb.docx");
                                Microsoft.Office.Interop.Word.Application WordApp = new Microsoft.Office.Interop.Word.Application();
                                Microsoft.Office.Interop.Word.Document wDoc = WordApp.Documents.Open(ref sst,
                                                           ref missing, ref readOnly, ref missing,
                                                           ref missing, ref missing, ref missing,
                                                           ref missing, ref missing, ref missing,
                                                           ref missing, ref isVisible);
                                CreatePdfDocument(wDoc, sfd.FileName);
                                File.Delete("bbb.docx");
                            }
                            else
                            {
                                CreateWordDocument(sfd.FileName, ms);
                            }
                            MessageBox.Show("Список успешно экспортирован.", "Готово");
                        }
                        catch
                        {
                            MessageBox.Show("Не удаётся получить доступ к файлу.", "Ошибка");
                        }
                    }
                }

            }
        }

        public FlowDocument CreateList()
        {
            FontFamily font = new FontFamily("font.ttf");

            FlowDocument flowDoc = new FlowDocument();

            if (ThemeBox.SelectedIndex != -1)
            {
                List<Theme> Themes = cre.Themes;
                int themeID = ThemeBox.SelectedIndex;
                int difID = DifBox.SelectedIndex;
                float fontSize = int.Parse((FontSizeBox.SelectedItem as ComboBoxItem).Content.ToString()) / 0.75f;

                if(difID == 10)
                {
                    int questionsCount = 0;
                    foreach(List<Question> ql in Themes[themeID].Questions)
                    {
                        questionsCount += ql.Count;
                    }
                    if (questionsCount != 0)
                    {
                        Paragraph nameText = new Paragraph(new Run(Themes[themeID].ThemeName));
                        nameText.TextAlignment = TextAlignment.Center;
                        nameText.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize + 4));
                        nameText.SetCurrentValue(Inline.FontWeightProperty, FontWeights.Bold);
                        nameText.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                        flowDoc.Blocks.Add(nameText);
                        flowDoc.Blocks.Add(new Paragraph(new Run("")));
                        int dif = 1;
                        foreach (List<Question> ql in Themes[themeID].Questions)
                        {
                            int number = 1;
                            if (ql.Count != 0)
                            {
                                Paragraph difText = new Paragraph(new Run("Вопросы " + dif.ToString() + " сложности"));
                                difText.TextAlignment = TextAlignment.Center;
                                difText.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize + 4));
                                difText.SetCurrentValue(Inline.FontWeightProperty, FontWeights.Bold);
                                difText.SetCurrentValue(Inline.FontFamilyProperty, font); // need font

                                flowDoc.Blocks.Add(difText);

                                foreach (Question q in ql)
                                {
                                    FlowDocument currentQuestion = new FlowDocument();
                                    try
                                    {
                                        MemoryStream sstream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(q.Text));
                                        TextRange range = new TextRange(currentQuestion.ContentStart, currentQuestion.ContentEnd);
                                        range.Load(sstream, DataFormats.Rtf);
                                        currentQuestion = Question.ReturnIndexes(currentQuestion);
                                        sstream.Close();
                                    }
                                    catch
                                    {
                                        currentQuestion.Blocks.Add(new Paragraph(new Run("no text")));
                                    }

                                    List<Block> blocks = currentQuestion.Blocks.ToList();
                                    foreach (Block b in blocks)
                                    {
                                        b.TextAlignment = TextAlignment.Left;
                                        b.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize));
                                        b.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                                    }

                                    string numberS = "";
                                    switch (NumberTypeBox.SelectedIndex)
                                    {
                                        case (0):
                                            numberS = "    " + (number).ToString() + ") ";
                                            break;
                                        case (1):
                                            numberS = "    " + (number).ToString() + ". ";
                                            break;
                                        case (2):
                                            numberS = "    №" + (number).ToString() + " ";
                                            break;
                                    }
                                    if (blocks.Count != 0)
                                    {
                                        if (blocks[0] is Paragraph)
                                        {
                                            Paragraph par = blocks[0] as Paragraph;

                                            par.Inlines.InsertBefore(par.Inlines.First(), new Run(numberS));
                                        }
                                        else
                                        {
                                            Paragraph newPar = new Paragraph(new Run(numberS));
                                            newPar.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize));
                                            newPar.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                                            newPar.TextAlignment = TextAlignment.Left;
                                            flowDoc.Blocks.Add(newPar);
                                        }
                                    }

                                    flowDoc.Blocks.AddRange(blocks);
                                    flowDoc.Blocks.Add(new Paragraph(new Run("")));

                                    number++;
                                }
                            }
                            flowDoc.Blocks.Add(new Paragraph(new Run("")));
                            dif++;
                        }
                        //
                    }
                    else
                    {
                        MessageBox.Show("Данная тема не имеет ни единого вопроса.", "Ошибка");
                        return null;
                    }
                }
                else
                {
                    if(Themes[themeID].Questions[difID].Count != 0)
                    {
                        Paragraph nameText = new Paragraph(new Run(Themes[themeID].ThemeName));
                        nameText.TextAlignment = TextAlignment.Center;
                        nameText.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize + 4));
                        nameText.SetCurrentValue(Inline.FontWeightProperty, FontWeights.Bold);
                        nameText.SetCurrentValue(Inline.FontFamilyProperty, font); // need font

                        Paragraph difText = new Paragraph(new Run("Вопросы " + (difID + 1).ToString() + " сложности"));
                        difText.TextAlignment = TextAlignment.Center;
                        difText.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize + 4));
                        difText.SetCurrentValue(Inline.FontWeightProperty, FontWeights.Bold);
                        difText.SetCurrentValue(Inline.FontFamilyProperty, font); // need font

                        flowDoc.Blocks.Add(nameText);
                        flowDoc.Blocks.Add(difText);
                        flowDoc.Blocks.Add(new Paragraph(new Run("")));

                        int number = 1;
                        foreach(Question q in Themes[themeID].Questions[difID])
                        {
                            FlowDocument currentQuestion = new FlowDocument();
                            MemoryStream sstream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(q.Text));
                            TextRange range = new TextRange(currentQuestion.ContentStart, currentQuestion.ContentEnd);
                            range.Load(sstream, DataFormats.Rtf);
                            currentQuestion = Question.ReturnIndexes(currentQuestion);
                            sstream.Close();

                            List<Block> blocks = currentQuestion.Blocks.ToList();
                            foreach (Block b in blocks)
                            {
                                b.TextAlignment = TextAlignment.Left;
                                b.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize));
                                b.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                            }

                            string numberS = "";
                            switch (NumberTypeBox.SelectedIndex)
                            {
                                case (0):
                                    numberS = "    " + (number).ToString() + ") ";
                                    break;
                                case (1):
                                    numberS = "    " + (number).ToString() + ". ";
                                    break;
                                case (2):
                                    numberS = "    №" + (number).ToString() + " ";
                                    break;
                            }
                            if (blocks[0] is Paragraph)
                            {
                                Paragraph par = blocks[0] as Paragraph;
                                par.Inlines.InsertBefore(par.Inlines.First(), new Run(numberS));
                            }
                            else
                            {
                                Paragraph newPar = new Paragraph(new Run(numberS));
                                newPar.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize));
                                newPar.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                                newPar.TextAlignment = TextAlignment.Left;
                                flowDoc.Blocks.Add(newPar);
                            }

                            flowDoc.Blocks.AddRange(blocks);
                            flowDoc.Blocks.Add(new Paragraph(new Run("")));

                            number++;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Данная сложность не имеет ни единого вопроса.", "Ошибка");
                        return null;
                    }
                }

            }
            else
            {
                MessageBox.Show("Для экспорта вопросов необходимо выбрать тему.", "Ошибка");
                return null;
            }

            return flowDoc;
        }

        private void CreateWordDocument(string filePath, MemoryStream ms)
        {
            Microsoft.Office.Interop.Word.Application word_app = new Microsoft.Office.Interop.Word.Application();

            FileStream fs = new FileStream("test.rtf", FileMode.OpenOrCreate);
            ms.WriteTo(fs);
            fs.Close();

            object input_file = System.IO.Path.GetFullPath("test.rtf");
            object missing = Type.Missing;
            word_app.Documents.Open(ref input_file, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing,
                ref missing, ref missing, ref missing, ref missing);

            object output_file = System.IO.Path.GetFullPath(filePath);
            object format_doc = (int)16;    // 16 for docx, 0 for doc.
            Microsoft.Office.Interop.Word.Document active_document = word_app.ActiveDocument;
            active_document.SaveAs(ref output_file, ref format_doc,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing);

            active_document.Close();
            word_app.Quit();
            File.Delete("test.rtf");
        }

        private void CreatePdfDocument(Microsoft.Office.Interop.Word.Document wordDocument, string path)
        {
            wordDocument.ExportAsFixedFormat(path, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
            Microsoft.Office.Interop.Word.Application applic = wordDocument.Application;
            wordDocument.Close();
            applic.Quit();
        }

        List<Image> FindAllImagesInParagraph(Paragraph paragraph)
        {
            List<Image> result = null;

            foreach (var inline in paragraph.Inlines)
            {
                var inlineUIContainer = inline as InlineUIContainer;
                if (inlineUIContainer != null)
                {
                    var image = inlineUIContainer.Child as Image;

                    if (image != null)
                    {
                        if (result == null)
                            result = new List<Image>();

                        result.Add(image);
                    }
                }
            }

            return result;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(Theme t in cre.Themes)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = t.ThemeName;
                ThemeBox.Items.Add(cbi);
            }
        }
    }
}
