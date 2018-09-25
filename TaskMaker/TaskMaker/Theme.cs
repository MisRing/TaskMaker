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

public class Theme : Window
{
    public Button theme_b;
    public Canvas can;
    public TextBox NameBox;
    public Button close;
    public CreateS cre;
    public StackPanel stac;
    public Theme(string imya, CreateS cre, StackPanel s)
    {
        stac = s;

        theme_b = new Button()
        {
            Width = 125,
            Height = 80,
            Margin = new Thickness(0, 5, 3, 0),
            BorderThickness = new Thickness(3, 3, 3, 3),
            Style = (Style)Application.Current.Resources["ButtonStyle1"],

        };
        Canvas can = new Canvas()
        {
            Width = 125,
            Height = 80,
        };
        TextBox NameBox = new TextBox()
        {
            Width = 117,
            Height = 60.5,
            Background = Brushes.Transparent,
            BorderThickness = new Thickness(0),
            Margin = new Thickness(0, 13, 3, 0),
            HorizontalContentAlignment = HorizontalAlignment.Center,
            TextWrapping = TextWrapping.Wrap,
            Text = imya,
        };
        //  NameBox.PreviewMouseDoubleClick += ChangeName;
        //  NameBox.PreviewMouseDown += ChoseThis;
        //  NameBox.LostFocus += ChangeName_close2;
        //  NameBox.KeyDown += new KeyEventHandler(OnKeyDownHandler);
        Image img = new Image();
        img.Source = new BitmapImage(new Uri(@"/cross.png", UriKind.Relative));
        Button close = new Button()
        {
            Content = img,
            Width = 15,
            Height = 15,
            Margin = new Thickness(104, 0, 0, 0),
            Background = Brushes.Transparent,
            BorderThickness = new Thickness(0),
            BorderBrush = Brushes.Transparent,
            Style = (Style)Application.Current.Resources["ButtonStyle1"],
        };
        theme_b.Content = can;
        can.Children.Add(NameBox);
        can.Children.Add(close);
        stac.Children.Remove(cre.CreateTheme_button);
        stac.Children.Add(theme_b);
        stac.Children.Add(cre.CreateTheme_button);
        close.Click += del_theme;
    }

    public void del_theme(object sender, EventArgs e)
    {
        cre.Themes.Remove(this);
        stac.Children.Remove(theme_b);
    }

    public void del_theme()
    {
        stac.Children.Remove(theme_b);
    }
}