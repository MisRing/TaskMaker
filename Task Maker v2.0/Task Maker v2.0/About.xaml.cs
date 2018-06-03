﻿using System;
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

namespace Task_Maker_v2._0
{
    /// <summary>
    /// Логика взаимодействия для About.xaml
    /// </summary>
    public partial class About : Page
    {
        Window MW = new Window();
        public About(Window win)
        {
            MW = win;
            InitializeComponent();
        }

        private void back(object sender, RoutedEventArgs e)
        {
            MainPage main = new MainPage(MW);
            MW.Content = main;
        }
    }
}