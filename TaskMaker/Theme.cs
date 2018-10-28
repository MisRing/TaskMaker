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
    public string ThemeName;
    public List<Question>[] Questions = new List<Question>[10];

    public Theme(string imya, CreateS _cre, StackPanel s)
    {
        for(int i = 0; i < 10; i++)
        {
            Questions[i] = new List<Question>();
        }
        stac = s;
        cre = _cre;
        ThemeName = imya;
        theme_b = new Button()
        {
            Width = 125,
            Height = 80,
            Margin = new Thickness(0, 5, 3, 0),
            BorderThickness = new Thickness(3, 3, 3, 3),
            Style = (Style)Application.Current.Resources["ButtonStyle1"],
        };
        theme_b.Click += ThemeOpen;
        can = new Canvas()
        {
            Width = 125,
            Height = 80,
        };
        NameBox = new TextBox()
        {
            Width = 117,
            Height = 60.5,
            Background = Brushes.Transparent,
            BorderThickness = new Thickness(0),
            Margin = new Thickness(0, 13, 3, 0),
            HorizontalContentAlignment = HorizontalAlignment.Center,
            TextWrapping = TextWrapping.Wrap,
            Text = ThemeName,
            IsReadOnly = true,
            Style = (Style)Application.Current.Resources["Arrow_Cursor"],
    };
        NameBox.TextChanged += NameChanges;
        NameBox.PreviewMouseDoubleClick += ChangeName;
        NameBox.LostFocus += ChangeName_close2;
        NameBox.PreviewMouseDown += ThemeOpen;
        NameBox.KeyDown += new KeyEventHandler(OnKeyDownHandler);


        Image img = new Image();
        img.Source = new BitmapImage(new Uri(@"/cross.png", UriKind.Relative));
        close = new Button()
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

    public Theme() { }

    public void LoadThisTheme(CreateS _cre, StackPanel s, List<Question>[] questions)
    {
        Questions = questions;
        stac = s;
        cre = _cre;
        theme_b = new Button()
        {
            Width = 125,
            Height = 80,
            Margin = new Thickness(0, 5, 3, 0),
            BorderThickness = new Thickness(3, 3, 3, 3),
            Style = (Style)Application.Current.Resources["ButtonStyle1"],
        };
        theme_b.Click += ThemeOpen;
        can = new Canvas()
        {
            Width = 125,
            Height = 80,
        };
        NameBox = new TextBox()
        {
            Width = 117,
            Height = 60.5,
            Background = Brushes.Transparent,
            BorderThickness = new Thickness(0),
            Margin = new Thickness(0, 13, 3, 0),
            HorizontalContentAlignment = HorizontalAlignment.Center,
            TextWrapping = TextWrapping.Wrap,
            Text = ThemeName,
            IsReadOnly = true,
            Style = (Style)Application.Current.Resources["Arrow_Cursor"],
        };
        NameBox.TextChanged += NameChanges;
        NameBox.PreviewMouseDoubleClick += ChangeName;
        NameBox.LostFocus += ChangeName_close2;
        NameBox.PreviewMouseDown += ThemeOpen;
        NameBox.KeyDown += new KeyEventHandler(OnKeyDownHandler);


        Image img = new Image();
        img.Source = new BitmapImage(new Uri(@"/cross.png", UriKind.Relative));
        close = new Button()
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

    public void ThemeOpen(object sender, EventArgs e)
    {
        cre.ChooseTheme(this);
    }

    public void NameChanges(object sender, EventArgs e)
    {
        ThemeName = NameBox.Text;
        cre.SaveAll();
    }

    public void ChangeName_close2(object sender, EventArgs e)
    {
        NameBox.IsReadOnly = true;
        NameBox.Style = (Style)Application.Current.Resources["Arrow_Cursor"];
        Keyboard.Focus(null);
        theme_b.Focus();
    }

    public void OnKeyDownHandler(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return)
        {
            NameBox.IsReadOnly = true;
            NameBox.Style = (Style)Application.Current.Resources["Arrow_Cursor"];
            Keyboard.Focus(null);
            try
            {
                theme_b.Focus();
            }
            catch { }
        }
    }

    public void ChangeName(object sender, MouseButtonEventArgs e)
    {
        NameBox.IsReadOnly = false;
        NameBox.Style = (Style)Application.Current.Resources["I_Cursor"];
        Keyboard.Focus(NameBox);
        NameBox.SelectAll();
    }

    public void del_theme(object sender, EventArgs e)
    {
        MessageBoxResult dialogResult = MessageBox.Show("Вы действительно хотите удалить эту тему?", "Удалить тему?", MessageBoxButton.YesNo);

        if (dialogResult == MessageBoxResult.Yes)
        {
            if (cre.choosedTheme == this)
                cre.ChooseTheme(null);
            cre.Themes.Remove(this);
            stac.Children.Remove(theme_b);
            cre.SaveAll();
        }
    }

    public void del_theme()
    {
        if (cre.choosedTheme == this)
            cre.ChooseTheme(null);
        stac.Children.Remove(theme_b);
    }
}