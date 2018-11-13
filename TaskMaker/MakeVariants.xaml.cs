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
using Spire.Doc;
using System.Security.Cryptography;
using System.Reflection;

namespace TaskMaker
{
    /// <summary>
    /// Логика взаимодействия для MakeVariants.xaml
    /// </summary>
    public partial class MakeVariants : Window
    {
        public CreateS cre;

        public MakeVariants()
        {
            InitializeComponent();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            if (v_count.Text != "" && v_count.Text != null && q_count.Text != "" && q_count.Text != null)
            {
                System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();

                if (fileType.SelectedIndex == 0)
                {
                    sfd.FileName = "NewTest.pdf";
                    sfd.Filter = "pdf files (*.pdf)|*.pdf";
                    sfd.FilterIndex = 1;
                }
                else
                {
                    sfd.FileName = "NewTest.docx";
                    sfd.Filter = "docx files (*.docx)|*.docx|doc files (*.doc)|*.doc";
                    sfd.FilterIndex = 2;
                }
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (sfd.FileName != null)
                    {
                        try
                        {
                            if (fileType.SelectedIndex == 0)
                            {
                                CreatePdfDocument(RandomiseVariants("pdf.docx"), sfd.FileName);
                                File.Delete("pdf.docx");
                            }
                            else
                            {
                                RandomiseVariants(sfd.FileName).Close();
                            }
                            MessageBox.Show("Генерация завершена.", "Готово");
                        }
                        catch
                        {
                            MessageBox.Show("Не удаётся получить доступ к файлу.", "Ошибка");
                        }
                    }
                }
            }
            
        }

        public Microsoft.Office.Interop.Word.Document RandomiseVariants(string path)
        {
            FontFamily font = new FontFamily("font.ttf");
            List<string> Variants = new List<string>();
            try
            {
                float fontSize = int.Parse((fontSizeBox.SelectedItem as ComboBoxItem).Content.ToString()) / 0.75f;
                List<Question>[] Questions = cre.choosedTheme.Questions;
                int variants = int.Parse(v_count.Text);
                int questions = int.Parse(q_count.Text);

                int[] quesOfDif = new int[10];

                int ii = 0;
                for (int i = 0; i < questions; i++)
                {
                    quesOfDif[ii]++;
                    ii++;
                    ii = (ii > 10) ? 0 : ii;
                }

                List<FlowDocument>[] newQuestions = new List<FlowDocument>[10];

                for (int i = 0; i < 10; i++)
                {
                    newQuestions[i] = new List<FlowDocument>();
                    foreach (Question q in Questions[i])
                    {
                        FlowDocument fd = new FlowDocument();
                        var range = new TextRange(fd.ContentStart, fd.ContentEnd);

                        try
                        {
                            MemoryStream sstream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(q.Text));
                            fd = System.Windows.Markup.XamlReader.Load(sstream) as FlowDocument;
                            sstream.Close();
                        }
                        catch
                        {
                            fd.Blocks.Add(new Paragraph(new Run("no text")));
                        }

                        newQuestions[i].Insert(GetNextRnd(0, newQuestions[i].Count), fd);
                    }
                }
                int[] questionsID = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                for (int v = 0; v < variants; v++)
                {
                    FlowDocument thisVariant = new FlowDocument();
                    Paragraph v_text = new Paragraph(new Run("Вариант " + (v + 1).ToString()));
                    v_text.TextAlignment = TextAlignment.Center;
                    v_text.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize + 8));
                    v_text.SetCurrentValue(Inline.FontWeightProperty, FontWeights.Bold);
                    v_text.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                    FlowDocument to = new FlowDocument();
                    MemoryStream stream = new MemoryStream();
                    System.Windows.Markup.XamlWriter.Save(richTextBox.Document, stream);
                    to = System.Windows.Markup.XamlReader.Load(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(stream.ToArray())))) as FlowDocument;
                    stream.Close();

                    foreach (Block b in to.Blocks)
                    {
                        if (b is Paragraph)
                        {
                            Paragraph par = b as Paragraph;
                            par.SetCurrentValue(Inline.FontWeightProperty, FontWeights.Bold);
                            par.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize + 8));
                            par.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                            par.TextAlignment = TextAlignment.Center;
                        }
                    }

                    thisVariant.Blocks.AddRange(to.Blocks.ToList());
                    thisVariant.Blocks.Add(v_text);
                    thisVariant.Blocks.Add(new Paragraph(new Run("")));

                    int globalQ = 1;
                    for (int dif = 0; dif < 10; dif++)
                    {
                        for (int q = 0; q < quesOfDif[dif]; q++)
                        {
                            FlowDocument currentFD = new FlowDocument();
                            MemoryStream sstream = new MemoryStream();
                            System.Windows.Markup.XamlWriter.Save(newQuestions[dif][questionsID[dif]], sstream);
                            currentFD = System.Windows.Markup.XamlReader.Load(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(sstream.ToArray())))) as FlowDocument;
                            sstream.Close();

                            List<Block> blocks = currentFD.Blocks.ToList();
                            foreach (Block b in blocks)
                            {
                                b.TextAlignment = TextAlignment.Left;
                                b.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize));
                                b.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                            }

                            string number = "";
                            switch (comboBox3.SelectedIndex)
                            {
                                case (0):
                                    number = "    " + (globalQ).ToString() + ") ";
                                    break;
                                case (1):
                                    number = "    " + (globalQ).ToString() + ". ";
                                    break;
                                case (2):
                                    number = "    №" + (globalQ).ToString() + " ";
                                    break;
                            }
                            if (blocks[0] is Paragraph)
                            {
                                Paragraph par = blocks[0] as Paragraph;

                                par.Inlines.InsertBefore(par.Inlines.First(), new Run(number));
                            }
                            else
                            {
                                Paragraph newPar = new Paragraph(new Run(number));
                                newPar.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize));
                                newPar.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                                newPar.TextAlignment = TextAlignment.Left;
                                thisVariant.Blocks.Add(newPar);
                            }

                            thisVariant.Blocks.AddRange(blocks);
                            thisVariant.Blocks.Add(new Paragraph(new Run("")));

                            globalQ++;
                            questionsID[dif]++;
                            questionsID[dif] = (questionsID[dif] > newQuestions[dif].Count - 1) ? 0 : questionsID[dif];
                        }
                    }
                thisVariant.FontFamily = font;

                    object ffileName = "test" + v.ToString() + ".docx";
                    MemoryStream ms = new MemoryStream();
                    var rrrrange = new TextRange(thisVariant.ContentStart, thisVariant.ContentEnd);
                    var fffStream = new MemoryStream();
                    rrrrange.Save(fffStream, System.Windows.DataFormats.Rtf);
                    string t = (Encoding.UTF8.GetString(fffStream.ToArray())).Replace(@"\fs16", @"\fs21\sub");
                    t = t.Replace(@"\fs17", @"\fs21\super");
                    ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(t));
                    CreateWordDocument((string)ffileName, ms);
                    Variants.Add(System.IO.Path.GetFullPath((string)ffileName));
                }


                object mmissing = Missing.Value;
                object rreadOnly = false;
                object isVvisible = false;
                Microsoft.Office.Interop.Word.Application WordApp = new Microsoft.Office.Interop.Word.Application();
                object aabsPath = System.IO.Path.GetFullPath(path);
                Microsoft.Office.Interop.Word.Document FinalDoc = new Microsoft.Office.Interop.Word.Document();
                object aabsPath2 = System.IO.Path.GetFullPath("notFull.docx");
                Microsoft.Office.Interop.Word.Document NotFinalDoc = new Microsoft.Office.Interop.Word.Document();
                NotFinalDoc.SaveAs2(aabsPath2);

                bool alwaysBreake = false;
                foreach (string st in Variants)
                {
                    Microsoft.Office.Interop.Word.Range FinalDocRange = FinalDoc.Range(FinalDoc.Content.End - 1, FinalDoc.Content.End);
                    Microsoft.Office.Interop.Word.Range NotFinalDocRange = NotFinalDoc.Range(NotFinalDoc.Content.End - 1, NotFinalDoc.Content.End);

                    object sst = st;
                    object missing = Missing.Value;
                    object missing2 = Missing.Value;
                    object readOnly = false;
                    object isVisible = false;
                    Microsoft.Office.Interop.Word.Document wDoc = WordApp.Documents.Open(ref sst,
                                                               ref missing, ref readOnly, ref missing,
                                                               ref missing, ref missing, ref missing,
                                                               ref missing, ref missing, ref missing,
                                                               ref missing, ref isVisible);
                    Microsoft.Office.Interop.Word.WdStatistic stat = Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages;
                    int num = NotFinalDoc.ComputeStatistics(stat, ref missing2);
                    wDoc.Range(wDoc.Content.Start, wDoc.Content.End).Copy();
                    NotFinalDocRange.Paste();
                    if (Variants.IndexOf(st) == 0 && num < NotFinalDoc.ComputeStatistics(stat, ref missing2))
                        alwaysBreake = true;
                    if ((num < NotFinalDoc.ComputeStatistics(stat, ref missing2) || alwaysBreake) && Variants.IndexOf(st) != 0)
                    {
                        FinalDocRange.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak);
                        FinalDocRange = FinalDoc.Range(FinalDoc.Content.End - 1, FinalDoc.Content.End);
                        wDoc.Range(wDoc.Content.Start, wDoc.Content.End).Copy();
                        FinalDocRange.Paste();
                    }
                    else
                    {
                        wDoc.Range(wDoc.Content.Start, wDoc.Content.End).Copy();
                        FinalDocRange.Paste();
                    }

                    NotFinalDocRange = NotFinalDoc.Range(NotFinalDoc.Content.Start, NotFinalDoc.Content.End);
                    NotFinalDocRange.Delete();
                    FinalDocRange = FinalDoc.Range(FinalDoc.Content.Start, FinalDoc.Content.End);
                    FinalDocRange.Copy();
                    NotFinalDocRange.Paste();
                    NotFinalDoc.Save();
                    wDoc.Close();
                }
                NotFinalDoc.Save();
                NotFinalDoc.Close();
                FinalDoc.SaveAs2(ref aabsPath, ref mmissing, ref rreadOnly, ref mmissing, ref mmissing, ref mmissing, ref mmissing,
                                                             ref mmissing, ref mmissing, ref mmissing, ref mmissing, ref isVvisible);
                File.Delete((string)aabsPath2);
                foreach (string st in Variants)
                {
                    File.Delete(st);
                }
                return FinalDoc;
            }
            catch
            {
                MessageBox.Show("Ошибка. Для создания такой работы недостаточно вопросов", "Ошибка");
            }

            return null;
        }

        private static RNGCryptoServiceProvider _RNG = new RNGCryptoServiceProvider();

        private static int GetNextRnd(int min, int max)
        {
            byte[] rndBytes = new byte[4];
            _RNG.GetBytes(rndBytes);
            int rand = BitConverter.ToInt32(rndBytes, 0);
            const Decimal OldRange = (Decimal)int.MaxValue - (Decimal)int.MinValue;
            Decimal NewRange = max - min;
            Decimal NewValue = ((Decimal)rand - (Decimal)int.MinValue) / OldRange * NewRange + (Decimal)min;
            return (int)NewValue;
        }

        private void CreateWordDocument(string filePath, MemoryStream ms)
        {
            Document document = new Document();
            document.LoadFromStream(ms, FileFormat.Rtf);
            document.SaveToFile(filePath, FileFormat.Docx);
            ms.Close();
        }

        private void CreatePdfDocument(Microsoft.Office.Interop.Word.Document wordDocument, string path)
        {
            wordDocument.ExportAsFixedFormat(path, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
            Microsoft.Office.Interop.Word.Application applic = wordDocument.Application;
            wordDocument.Close();
            applic.Quit();
        }

        private void Say_NO_to_letters(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789 ,".IndexOf(e.Text) < 0;
        }
    }
}
