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
        can.Height = 100;
        can.Margin = new Thickness(10, 10, 10, 0);
        can.HorizontalAlignment = HorizontalAlignment.Stretch;
        can.Background = Brushes.White;
        q_text = question_text;
        text.Text = q_text;
        can.Children.Add(text);
        text.TextWrapping = TextWrapping.Wrap;
        text.Margin = new Thickness(5, 5, 20, 5);
        text.HorizontalAlignment = HorizontalAlignment.Stretch;
        text.VerticalAlignment = VerticalAlignment.Stretch;
        close.BorderThickness = new Thickness(0, 0, 0, 0);
        Canvas.SetRight(close, 0);
        Canvas.SetTop(close, 0);
        close.Background = Brushes.White;
        close.Height = 15;
        close.Width = 15;
        close.Click += DeliteQuestion;
        Image img = new Image();
        img.Source = new BitmapImage(new Uri(@"/cross.png", UriKind.Relative));
        close.Content = img;
        can.MouseUp += OpenText;

        theme.Questions[Dif - 1].Add(this);
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

    void SaveQ(object sender, RoutedEventArgs e)
    {
        // MainWindow.SaveQuestions();
    }

    public void SaveQuestion()
    {
        //text.Text = q_text.Replace("@:n:@", "\n");
        //MainWindow.SaveQuestions();
    }
}
