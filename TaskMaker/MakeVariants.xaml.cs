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
                    sfd.FilterIndex = 1;
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
                                Microsoft.Office.Interop.Word.Document doc = RandomiseVariants(sfd.FileName);
                                Microsoft.Office.Interop.Word.Application app = doc.Application;
                                doc.Close();
                                app.Quit();
                            }
                            //GPwin.ChangeValue(1);

                            MessageBox.Show("Генерация успешно завершена!", "Готово!");
                        }
                        catch
                        {
                            //try
                            //{
                                //GPwin.Close();
                            //}
                            //catch { }
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
            List<string> VariantsAnswers = new List<string>();
            try
            {

                float fontSize = int.Parse((fontSizeBox.SelectedItem as ComboBoxItem).Content.ToString()) / 0.75f;
                if(int.Parse((fontSizeBox.SelectedItem as ComboBoxItem).Content.ToString()) == 8)
                    fontSize = 7.5f / 0.75f;
                List<Question>[] Questions = cre.choosedTheme.Questions;
                int variants = int.Parse(v_count.Text);
                int questions = int.Parse(q_count.Text);

                int qCount = 0;
                foreach(List<Question>  q in Questions)
                {
                    qCount += q.Count;
                }
                //GPwin = new GenerationProgressWindow(4 + questions + qCount + variants * (questions + 2));
                //GPwin.Owner = this;
                //GPwin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                //GPwin.Show();
                //GPwin.Margin = new Thickness(0, 0, 0, 0);
                

                //GPwin.ChangeValue(1, "Идет генерация работы...");

                int[] quesOfDif = new int[10];

                int ii = 0;
                for (int i = 0; i < questions; i++)
                {
                    quesOfDif[ii]++;
                    ii++;
                    ii = (ii > 10) ? 0 : ii;

                    //GPwin.ChangeValue(1);
                }

                List<FlowDocument>[] newQuestions = new List<FlowDocument>[10];
                List<FlowDocument>[] newAnswers = new List<FlowDocument>[10];

                for (int i = 0; i < 10; i++)
                {
                    newQuestions[i] = new List<FlowDocument>();
                    newAnswers[i] = new List<FlowDocument>();

                    foreach (Question q in Questions[i])
                    {
                        FlowDocument fd = new FlowDocument();
                        var range = new TextRange(fd.ContentStart, fd.ContentEnd);

                        try
                        {
                            MemoryStream sstream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(q.Text));
                            range.Load(sstream, DataFormats.Rtf);
                            fd = Question.ReturnIndexes(fd);
                            sstream.Close();
                        }
                        catch
                        {
                            fd.Blocks.Add(new Paragraph(new Run("no text")));
                        }

                        int ind = GetNextRnd(0, newQuestions[i].Count);

                        newQuestions[i].Insert(ind, fd);

                        if (setAnswers.IsChecked == true)
                        {
                            FlowDocument fd2 = new FlowDocument();
                            var range2 = new TextRange(fd2.ContentStart, fd2.ContentEnd);

                            try
                            {
                                MemoryStream sstream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(q.ansText));
                                range2.Load(sstream, DataFormats.Rtf);
                                fd2 = Question.ReturnIndexes(fd2);
                                sstream.Close();
                            }
                            catch
                            {
                                fd2.Blocks.Add(new Paragraph(new Run("———")));
                            }

                            newAnswers[i].Insert(ind, fd2);
                        }

                        //GPwin.ChangeValue(1);
                    }
                }
                int[] questionsID = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                for (int v = 0; v < variants; v++)
                {
                    // Variant
                    FlowDocument thisVariant = new FlowDocument();
                    Paragraph v_text = new Paragraph(new Run("Вариант " + (v + 1).ToString()));
                    v_text.TextAlignment = TextAlignment.Center;
                    v_text.FontSize = (double)(fontSize + 4);
                    v_text.SetCurrentValue(Inline.FontWeightProperty, FontWeights.Bold);
                    v_text.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                    FlowDocument to = new FlowDocument();
                    MemoryStream stream = new MemoryStream();
                    TextRange range1 = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                    range1.Save(stream, DataFormats.Rtf);
                    range1 = new TextRange(to.ContentStart, to.ContentEnd);
                    range1.Load(stream, DataFormats.Rtf);
                    to = Question.ReturnIndexes(to);

                    if (chek.IsChecked == false)
                    {
                        foreach (Block b in to.Blocks)
                        {
                            if (b is Paragraph)
                            {
                                Paragraph par = b as Paragraph;
                                par.SetCurrentValue(Inline.FontWeightProperty, FontWeights.Bold);
                                par.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize + 4));
                                par.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                                par.TextAlignment = TextAlignment.Center;
                            }
                        }
                    }
                    else
                    {
                        foreach (Block b in to.Blocks)
                        {
                            if (b is Paragraph)
                            {
                                Paragraph par = b as Paragraph;
                                par.SetCurrentValue(Inline.FontWeightProperty, FontWeights.Normal);
                                par.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize + 2));
                                par.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                                par.TextAlignment = TextAlignment.Center;
                            }
                        }
                    }

                    thisVariant.Blocks.AddRange(to.Blocks.ToList());
                    thisVariant.Blocks.Add(v_text);
                    thisVariant.Blocks.Add(new Paragraph(new Run("")));
                    // variant\

                    // Answers
                    FlowDocument thisVariantAnswers = new FlowDocument();
                    if (setAnswers.IsChecked == true)
                    {
                        Paragraph v_text1 = new Paragraph(new Run("Ответы"));
                        v_text1.TextAlignment = TextAlignment.Center;
                        v_text1.FontSize = (double)(fontSize + 4);
                        v_text1.SetCurrentValue(Inline.FontWeightProperty, FontWeights.Bold);
                        v_text1.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                        v_text = new Paragraph(new Run("Вариант " + (v + 1).ToString()));
                        v_text.TextAlignment = TextAlignment.Center;
                        v_text.FontSize = (double)(fontSize + 4);
                        v_text.SetCurrentValue(Inline.FontWeightProperty, FontWeights.Bold);
                        v_text.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                        FlowDocument to2 = new FlowDocument();
                        MemoryStream stream11 = new MemoryStream();
                        TextRange range11 = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                        range11.Save(stream11, DataFormats.Rtf);
                        range11 = new TextRange(to2.ContentStart, to2.ContentEnd);
                        range11.Load(stream11, DataFormats.Rtf);
                        to2 = Question.ReturnIndexes(to2);

                        if (chek.IsChecked == false)
                        {
                            foreach (Block b in to2.Blocks)
                            {
                                if (b is Paragraph)
                                {
                                    Paragraph par = b as Paragraph;
                                    par.SetCurrentValue(Inline.FontWeightProperty, FontWeights.Bold);
                                    par.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize + 4));
                                    par.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                                    par.TextAlignment = TextAlignment.Center;
                                }
                            }
                        }
                        else
                        {
                            foreach (Block b in to2.Blocks)
                            {
                                if (b is Paragraph)
                                {
                                    Paragraph par = b as Paragraph;
                                    par.SetCurrentValue(Inline.FontWeightProperty, FontWeights.Normal);
                                    par.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize + 2));
                                    par.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                                    par.TextAlignment = TextAlignment.Center;
                                }
                            }
                        }
                        thisVariantAnswers.Blocks.AddRange(to2.Blocks.ToList());
                        thisVariantAnswers.Blocks.Add(v_text1);
                        thisVariantAnswers.Blocks.Add(v_text);
                        thisVariantAnswers.Blocks.Add(new Paragraph(new Run("")));
                    }
                    // answers \

                    int globalQ = 1;
                    for (int dif = 0; dif < 10; dif++)
                    {
                        for (int q = 0; q < quesOfDif[dif]; q++)
                        {
                            // Question\
                            FlowDocument currentFD = new FlowDocument();
                            TextRange range2 = new TextRange(newQuestions[dif][questionsID[dif]].ContentStart, newQuestions[dif][questionsID[dif]].ContentEnd);
                            MemoryStream sstream = new MemoryStream();
                            range2.Save(sstream, DataFormats.Rtf);
                            range2 = new TextRange(currentFD.ContentStart, currentFD.ContentEnd);
                            range2.Load(sstream, DataFormats.Rtf);
                            currentFD = Question.ReturnIndexes(currentFD);
                            sstream.Close();

                            List<Block> blocks = currentFD.Blocks.ToList();
                            foreach (Block b in blocks)
                            {
                                b.TextAlignment = TextAlignment.Left;
                                b.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize));
                                b.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                            }

                            string number = "    " + (globalQ).ToString() + ") ";

                       
                                 
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
                            // question\

                            // Answer
                            if(setAnswers.IsChecked == true)
                            {
                                FlowDocument currentAnswer = new FlowDocument();
                                TextRange range2a = new TextRange(newAnswers[dif][questionsID[dif]].ContentStart, newAnswers[dif][questionsID[dif]].ContentEnd);
                                MemoryStream sstreama = new MemoryStream();
                                range2a.Save(sstreama, DataFormats.Rtf);
                                range2a = new TextRange(currentAnswer.ContentStart, currentAnswer.ContentEnd);
                                range2a.Load(sstreama, DataFormats.Rtf);
                                currentAnswer = Question.ReturnIndexes(currentAnswer);
                                sstreama.Close();

                                List<Block> blocksa = currentAnswer.Blocks.ToList();
                                foreach (Block b in blocksa)
                                {
                                    b.TextAlignment = TextAlignment.Left;
                                    b.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize));
                                    b.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                                }

                                if (blocksa[0] is Paragraph)
                                {
                                    Paragraph par = blocksa[0] as Paragraph;

                                    par.Inlines.InsertBefore(par.Inlines.First(), new Run(number));
                                }
                                else
                                {
                                    Paragraph newPar = new Paragraph(new Run(number));
                                    newPar.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize));
                                    newPar.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                                    newPar.TextAlignment = TextAlignment.Left;
                                    thisVariantAnswers.Blocks.Add(newPar);
                                }

                                thisVariantAnswers.Blocks.AddRange(blocksa);
                                thisVariantAnswers.Blocks.Add(new Paragraph(new Run("")));
                            }
                            // answer\

                            globalQ++;
                            questionsID[dif]++;
                            if(questionsID[dif] > newQuestions[dif].Count - 1)
                            {
                                questionsID[dif] = 0;

                                List<FlowDocument> fdl = new List<FlowDocument>();
                                List<FlowDocument> fdla = new List<FlowDocument>();
                                foreach (FlowDocument ques in newQuestions[dif])
                                {
                                    int ind = GetNextRnd(0, fdl.Count);
                                    fdla.Insert(ind, newAnswers[dif][newQuestions[dif].IndexOf(ques)]);
                                    fdl.Insert(ind, ques);
                                }

                                newQuestions[dif] = fdl;
                                newAnswers[dif] = fdla;
                            }

                            //GPwin.ChangeValue(1);
                        }
                    }

                    if (chek.IsChecked == true)
                    {
                        to = new FlowDocument();
                        stream = new MemoryStream();
                        range1 = new TextRange(richTextBox2.Document.ContentStart, richTextBox2.Document.ContentEnd);
                        range1.Save(stream, DataFormats.Rtf);
                        range1 = new TextRange(to.ContentStart, to.ContentEnd);
                        range1.Load(stream, DataFormats.Rtf);
                        to = Question.ReturnIndexes(to);

                        foreach (Block b in to.Blocks)
                        {
                            if (b is Paragraph)
                            {
                                Paragraph par = b as Paragraph;
                                par.SetCurrentValue(Inline.FontWeightProperty, FontWeights.Normal);
                                par.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize + 2));
                                par.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                                par.TextAlignment = TextAlignment.Right;
                            }
                        }

                        thisVariant.Blocks.AddRange(to.Blocks.ToList());
                        thisVariant.Blocks.Add(new Paragraph(new Run("")));

                        if(setAnswers.IsChecked == true)
                        {
                            to = new FlowDocument();
                            stream = new MemoryStream();
                            range1 = new TextRange(richTextBox2.Document.ContentStart, richTextBox2.Document.ContentEnd);
                            range1.Save(stream, DataFormats.Rtf);
                            range1 = new TextRange(to.ContentStart, to.ContentEnd);
                            range1.Load(stream, DataFormats.Rtf);
                            to = Question.ReturnIndexes(to);

                            foreach (Block b in to.Blocks)
                            {
                                if (b is Paragraph)
                                {
                                    Paragraph par = b as Paragraph;
                                    par.SetCurrentValue(Inline.FontWeightProperty, FontWeights.Normal);
                                    par.SetCurrentValue(Inline.FontSizeProperty, (double)(fontSize + 2));
                                    par.SetCurrentValue(Inline.FontFamilyProperty, font); // need font
                                    par.TextAlignment = TextAlignment.Right;
                                }
                            }

                            thisVariantAnswers.Blocks.AddRange(to.Blocks.ToList());
                            thisVariantAnswers.Blocks.Add(new Paragraph(new Run("")));
                        }
                    }

                    // Variant
                    thisVariant.FontFamily = font;
                    object ffileName = "test" + v.ToString() + ".docx";
                    MemoryStream ms = new MemoryStream();
                    var rrrrange = new TextRange(thisVariant.ContentStart, thisVariant.ContentEnd);
                    var fffStream = new MemoryStream();
                    rrrrange.Save(fffStream, System.Windows.DataFormats.Rtf);
                    string t = (Encoding.UTF8.GetString(fffStream.ToArray())).Replace(@"\fs16", @"\fs" + ((int)(fontSize * 2 * 0.75f)).ToString() + @"\sub");
                    t = t.Replace(@"\fs17", @"\fs" + ((int)(fontSize * 2 * 0.75f)).ToString() + @"\super");
                    ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(t));
                    CreateWordDocument((string)ffileName, ms);
                    Variants.Add(System.IO.Path.GetFullPath((string)ffileName));
                    // variant\

                    // Answers
                    if(setAnswers.IsChecked == true)
                    {
                        thisVariantAnswers.FontFamily = font;
                        object ffileNamea = "testA" + v.ToString() + ".docx";
                        MemoryStream msa = new MemoryStream();
                        var rrrrangea = new TextRange(thisVariantAnswers.ContentStart, thisVariantAnswers.ContentEnd);
                        var fffStreama = new MemoryStream();
                        rrrrangea.Save(fffStreama, System.Windows.DataFormats.Rtf);
                        string ta = (Encoding.UTF8.GetString(fffStreama.ToArray())).Replace(@"\fs16", @"\fs" + ((int)(fontSize * 2 * 0.75f)).ToString() + @"\sub");
                        ta = ta.Replace(@"\fs17", @"\fs" + ((int)(fontSize * 2 * 0.75f)).ToString() + @"\super");
                        msa = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(ta));
                        CreateWordDocument((string)ffileNamea, msa);
                        VariantsAnswers.Add(System.IO.Path.GetFullPath((string)ffileNamea));
                    }
                    // answers\
                    //GPwin.ChangeValue(1);
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

                //GPwin.ChangeValue(1);
                
                // Variants
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

                    //GPwin.ChangeValue(1);
                }
                // variants\

                // Answers
                if(setAnswers.IsChecked == true)
                {
                    Microsoft.Office.Interop.Word.Range FinalDocRange2 = FinalDoc.Range(FinalDoc.Content.End - 1, FinalDoc.Content.End);

                    FinalDocRange2.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak);
                    FinalDocRange2 = FinalDoc.Range(FinalDoc.Content.End - 1, FinalDoc.Content.End);

                    foreach (string st in VariantsAnswers)
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
                    
                        wDoc.Range(wDoc.Content.Start, wDoc.Content.End).Copy();
                        FinalDocRange.Paste();

                        NotFinalDocRange = NotFinalDoc.Range(NotFinalDoc.Content.Start, NotFinalDoc.Content.End);
                        NotFinalDocRange.Delete();
                        FinalDocRange = FinalDoc.Range(FinalDoc.Content.Start, FinalDoc.Content.End);
                        FinalDocRange.Copy();
                        NotFinalDocRange.Paste();
                        NotFinalDoc.Save();
                        wDoc.Close();

                        //GPwin.ChangeValue(1);
                    }
                }
                // answers\

                NotFinalDoc.Save();
                NotFinalDoc.Close();
                FinalDoc.SaveAs2(ref aabsPath, ref mmissing, ref rreadOnly, ref mmissing, ref mmissing, ref mmissing, ref mmissing,
                                                             ref mmissing, ref mmissing, ref mmissing, ref mmissing, ref isVvisible);
                File.Delete((string)aabsPath2);
                foreach (string st in Variants)
                {
                    File.Delete(st);
                }

                //GPwin.ChangeValue(1);
                return FinalDoc;
        }
            catch
            {
                try
                {
                    //GPwin.Close();
                }
                catch { }
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

        private void Say_NO_to_letters(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789 ,".IndexOf(e.Text) < 0;
        }

        private void richTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            richTextBox.Document.Blocks.Clear();
            richTextBox.Document.Blocks.Add(new Paragraph(new Run("Самостоятельная работа")));
            richTextBox.Document.Blocks.Add(new Paragraph(new Run("по теме \"" + cre.choosedTheme.ThemeName + "\"")));
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (chek.IsChecked == true)

            {
                this.Width = this.Width + 325;
                richTextBox2.Visibility = Visibility.Visible;
                l2.Visibility = Visibility.Visible;
                lbl4.Content = "Шапка ОКР";
                richTextBox.Document.Blocks.Clear();
                richTextBox.Document.Blocks.Add(new Paragraph(new Run("Учреждение образования \"\"")));
                richTextBox.Document.Blocks.Add(new Paragraph(new Run("Задание на обязательную контрольную работу № по дисциплине \"\"")));
                richTextBox.Document.Blocks.Add(new Paragraph(new Run(cre.choosedTheme.ThemeName)));
                richTextBox2.Document.Blocks.Clear();
                richTextBox2.Document.Blocks.Add(new Paragraph(new Run("Составитель: ")));
                richTextBox2.Document.Blocks.Add(new Paragraph(new Run("Учреждение образования \"\"")));
                richTextBox2.Document.Blocks.Add(new Paragraph(new Run("Рассмотрено на заседании ЦК ")));
                richTextBox2.Document.Blocks.Add(new Paragraph(new Run("Рекомендовано к использованию")));
                richTextBox2.Document.Blocks.Add(new Paragraph(new Run("Протокол № __ от _____________")));
                richTextBox2.Document.Blocks.Add(new Paragraph(new Run("Председатель ЦК ")));

            }
            else
            {
                this.Width = this.Width - 325;
                richTextBox2.Visibility = Visibility.Collapsed;
                lbl4.Content = "Шапка работы";
                l2.Visibility = Visibility.Collapsed;
                richTextBox.Document.Blocks.Clear();
                richTextBox.Document.Blocks.Add(new Paragraph(new Run("Самостоятельная работа")));
                richTextBox.Document.Blocks.Add(new Paragraph(new Run("по теме \"" + cre.choosedTheme.ThemeName + "\"")));
            }


        }
    }
}
