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

public class Question : Window
{
    public Canvas can;
    public Button close = new Button();
    public FlowDocumentScrollViewer text = new FlowDocumentScrollViewer();
    public string Text, ansText;
    public StackPanel qPan;
    public CreateS MW;
    public Window lol;
    public Theme theme;
    public int Dif = 0;

    public Question(string question_text, StackPanel stack, CreateS win, Theme t, int dif,  Window okno)
    {
        lol = okno;       
        Dif = dif;
        MW = win;
        theme = t;
        qPan = stack;
        can = new Canvas();
        qPan.Children.Add(can);
        can.Children.Add(close);
        can.Height = 100;
        can.Margin = new Thickness(10, 10, 10, 0);
        can.HorizontalAlignment = HorizontalAlignment.Stretch;
        can.Background = Brushes.White;
        if (Text != "" && Text != null)
            Text = question_text;

        can.Children.Add(text);
        text.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
        text.Margin = new Thickness(0, 0, 100, 0); //5 5 20 5
        text.Height = 100;
        text.Width = 755;
        text.HorizontalAlignment = HorizontalAlignment.Stretch;
        text.VerticalAlignment = VerticalAlignment.Stretch;
        close.BorderThickness = new Thickness(0, 0, 0, 0);
        Canvas.SetRight(close, 0);
        Canvas.SetTop(close, 0);
        close.Background = Brushes.Transparent;
        close.Height = 15;
        close.Width = 15;
        close.Click += DeliteQuestion;
        Image img = new Image();
        img.Source = new BitmapImage(new Uri(@"/cross.png", UriKind.Relative));
        close.Content = img;
        can.MouseUp += Redaktor;
        theme.Questions[Dif - 1].Add(this);
        text.MouseUp += Redaktor;
    }

    public Question() { }

    public void LoadQuestion(string question_text, string question_answer, StackPanel stack, CreateS win, Theme t, int dif, Window okno)
    {
        lol = okno;
        Dif = dif;
        MW = win;
        theme = t;
        qPan = stack;
        can = new Canvas();
        can.Children.Add(close);
        can.Height = 100;
        can.Margin = new Thickness(10, 10, 10, 0);
        can.HorizontalAlignment = HorizontalAlignment.Stretch;
        can.Background = Brushes.White;
        Text = question_text;
        ansText = question_answer;
        if (Text != null && Text != "")
        {
            var fStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(Text));
            text.Document = new FlowDocument();
            var range = new TextRange(text.Document.ContentStart, text.Document.ContentEnd);
            range.Load(fStream, DataFormats.Rtf);
            text.Document = ReturnIndexes(text.Document);
            fStream.Close();
        }
        can.Children.Add(text);
        text.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
        text.Margin = new Thickness(0, 0, 100, 0); //5 5 20 5
        text.Height = 100;
        text.Width = 770;
        text.HorizontalAlignment = HorizontalAlignment.Stretch;
        text.VerticalAlignment = VerticalAlignment.Stretch;
        close.BorderThickness = new Thickness(0, 0, 0, 0);
        Canvas.SetRight(close, 0);
        Canvas.SetTop(close, 0);
        close.Background = Brushes.Transparent;
        close.Height = 15;
        close.Width = 15;
        close.Click += DeliteQuestion;
        Image img = new Image();
        img.Source = new BitmapImage(new Uri(@"/cross.png", UriKind.Relative));
        close.Content = img;
        can.MouseUp += Redaktor;
        text.MouseUp += Redaktor;
        text.PreviewMouseUp += Redaktor;
    }

    public void Redaktor(object sender, RoutedEventArgs e)
    {
        QuestionPage quest = new QuestionPage(MW, lol, this);
        lol.Content = quest;
    }
    public void DeliteQuestion(object sender, RoutedEventArgs e)
    {
        theme.Questions[Dif - 1].Remove(this);
        qPan.Children.Remove(can);
        MW.SaveAll();
    }

    public static FlowDocument ReturnIndexes(FlowDocument fd)
    {
        foreach (Block b in fd.Blocks)
        {
            if (b is Paragraph)
            {
                Paragraph par = b as Paragraph;

                foreach (Inline line in par.Inlines)
                {
                    if (line is Span)
                    {
                        Span r = line as Span;
                        double fsize = Math.Round(double.Parse(r.GetValue(Inline.FontSizeProperty).ToString()), 1);
                        if (fsize == 10.7)
                        {
                            r.SetValue(Inline.BaselineAlignmentProperty, BaselineAlignment.Subscript);
                            r.SetValue(Inline.FontSizeProperty, 11.0);
                        }
                        else if (fsize == 11.3)
                        {
                            r.SetValue(Inline.BaselineAlignmentProperty, BaselineAlignment.Superscript);
                            r.SetValue(Inline.FontSizeProperty, 11.5);
                        }
                    }
                }
            }
        }
        return fd;
    }
}
