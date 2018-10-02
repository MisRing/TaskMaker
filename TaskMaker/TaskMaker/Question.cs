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
    public TextBlock text = new TextBlock();
    public string q_text;
    public StackPanel qPan;
    //public List<System.Drawing.Image> im = new List<System.Drawing.Image>();
    public CreateS MW;
    public Theme theme;
    public int Dif = 0;

    public Question(string question_text, StackPanel stack, CreateS win, Theme t, int dif, bool create = true)
    {
        Dif = dif;
        MW = win;
        theme = t;
        qPan = stack;
        can = new Canvas();
        if (create)
            qPan.Children.Add(can);
        can.Children.Add(close);
        can.Width = 650;
        can.Height = 60;
        can.Margin = new Thickness(30, 6, 0, 0);
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
        close.Margin = new Thickness(635, 0, 0, 30);
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
        //MW.SaveThemes();
    }

    public void OpenText(object sender, RoutedEventArgs e)
    {
        //QuestionPage qp = new QuestionPage();
        //MW.MW.Content = qp;
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
