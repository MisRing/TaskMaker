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
            if(v_count.Text != "" && v_count.Text != null && q_count.Text != "" && q_count.Text != null)
            {
                bool Do = true;
                //MemoryStream ms = new MemoryStream();
                //try
                //{
                //ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(RandomiseVariants()));
                //RandomiseVariants().
                //}
                //catch
                //{
                //    MessageBox.Show("Ошибка. Для создания такой работы недостаточно вопросов", "Ошибка");
                //    Do = false;
                //}

                if (Do)
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
        }

        public Microsoft.Office.Interop.Word.Document RandomiseVariants(string path)
        {
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
                            var fStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(q.Text));
                            range.Load(fStream, System.Windows.DataFormats.Rtf);
                        }
                        catch
                        {
                            range.Text = "no text";
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
                    v_text.SetCurrentValue(Inline.FontFamilyProperty, FontFamily); // need font
                    FlowDocument to = new FlowDocument();
                    TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                    MemoryStream stream = new MemoryStream();
                    range.Save(stream, DataFormats.Rtf);
                    TextRange range2 = new TextRange(to.ContentEnd, to.ContentEnd);
                    range2.Load(stream, DataFormats.Rtf);
                    stream.Close();

                    foreach (Block b in to.Blocks)
                    {
                        if (b is Paragraph)
                        {
                            Paragraph par = b as Paragraph;
                            par.SetCurrentValue(Inline.FontWeightProperty, FontWeights.Bold);
                            par.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize + 8));
                            par.SetCurrentValue(Inline.FontFamilyProperty, FontFamily); // need font
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
                            TextRange rrrange = new TextRange(newQuestions[dif][questionsID[dif]].ContentStart, newQuestions[dif][questionsID[dif]].ContentEnd);
                            MemoryStream sstream = new MemoryStream();
                            rrrange.Save(sstream, DataFormats.Rtf);
                            TextRange rrrange2 = new TextRange(currentFD.ContentEnd, currentFD.ContentEnd);
                            rrrange2.Load(sstream, DataFormats.Rtf);
                            sstream.Close();

                            List<Block> blocks = currentFD.Blocks.ToList();
                            foreach (Block b in blocks)
                            {
                                b.TextAlignment = TextAlignment.Left;
                                b.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize));
                                b.SetCurrentValue(Inline.FontFamilyProperty, FontFamily); // need font
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
                                var images = FindAllImagesInParagraph(par);
                                if (images == null)
                                {
                                    TextRange tr1 = new TextRange(par.ContentStart, par.ContentEnd);
                                    tr1.Text = number + tr1.Text;
                                    blocks.RemoveAt(0);
                                    par.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize));
                                    par.SetCurrentValue(Inline.FontFamilyProperty, FontFamily); // need font
                                    thisVariant.Blocks.Add(par);
                                }
                                else
                                {
                                    Paragraph newPar = new Paragraph(new Run(number));
                                    newPar.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize));
                                    newPar.SetCurrentValue(Inline.FontFamilyProperty, FontFamily); // need font
                                    newPar.TextAlignment = TextAlignment.Left;
                                    thisVariant.Blocks.Add(newPar);
                                }
                            }
                            else
                            {
                                Paragraph newPar = new Paragraph(new Run(number));
                                newPar.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize));
                                newPar.SetCurrentValue(Inline.FontFamilyProperty, FontFamily); // need font
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
                    //Variants.Add(thisVariant);

                    object ffileName = "test" + v.ToString() + ".docx";
                    MemoryStream ms = new MemoryStream();
                    var rrrrange = new TextRange(thisVariant.ContentStart, thisVariant.ContentEnd);
                    var fffStream = new MemoryStream();
                    rrrrange.Save(fffStream, System.Windows.DataFormats.Rtf);
                    string t = Encoding.UTF8.GetString(fffStream.ToArray());
                    ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(t));
                    CreateWordDocument((string)ffileName, ms);
                    Variants.Add(System.IO.Path.GetFullPath((string)ffileName));
                }
            }
            catch
            {
                MessageBox.Show("Ошибка. Для создания такой работы недостаточно вопросов", "Ошибка");
            }
            object mmissing = Missing.Value;
            object rreadOnly = false;
            object isVvisible = false;
            Microsoft.Office.Interop.Word.Application WordApp2 = new Microsoft.Office.Interop.Word.Application();
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

                //Microsoft.Office.Interop.Word.Application WordApp = new Microsoft.Office.Interop.Word.Application();
                object sst = st;
                object missing = Missing.Value;
                object missing2 = Missing.Value;
                object readOnly = false;
                object isVisible = false;
                Microsoft.Office.Interop.Word.Document wDoc = WordApp2.Documents.Open(ref sst,
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
                if((num < NotFinalDoc.ComputeStatistics(stat, ref missing2) || alwaysBreake) && Variants.IndexOf(st) != 0)
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
            WordApp2.Quit();
            File.Delete((string)aabsPath2);
            foreach (string st in Variants)
            {
                File.Delete(st);
            }
            return FinalDoc;
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
            wordDocument.Close();
            wordDocument.Application.Quit();
        }

        private void Say_NO_to_letters(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789 ,".IndexOf(e.Text) < 0;
        }
    }
}
