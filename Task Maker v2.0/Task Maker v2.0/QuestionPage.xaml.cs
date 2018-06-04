using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для QuestionPage.xaml
    /// </summary>
    public partial class QuestionPage : Page
    {
        Window MW;
        TaskPage taskPage;
        bool Subscript = false, Superscript = false;
        maths mat = new maths();
        spec sp = new spec();
        BrushConverter converter = new System.Windows.Media.BrushConverter();
        Brush brush;

        public QuestionPage(Window win, TaskPage tp)
        {
            MW = win;
            taskPage = tp;
            brush = (Brush)converter.ConvertFromString("#FFFCB93E");

            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(questionInput);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            MW.Content = taskPage;
        }

        private void SetSuperscript(object sender, RoutedEventArgs e)
        {
            Superscript = !Superscript;
            Subscript = false;

            if (Superscript)
                up.Background = Brushes.LightCyan;
            else
                up.Background = brush;

            if (Subscript)
               down.Background = Brushes.LightCyan;
            else
                down.Background = brush;
        }

        private void SetSubscript(object sender, RoutedEventArgs e)
        {
            Subscript = !Subscript;
            Superscript = false;

            if (Superscript)
                up.Background = Brushes.LightCyan;
            else
                up.Background = brush;

            if (Subscript)
                down.Background = Brushes.LightCyan;
            else
                down.Background = brush;
        }

        private void math_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = mat;
        }

        private void symbols_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = sp;
        }

        private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Subscript)
            {
                BaselineAlignment newAlignment = BaselineAlignment.Subscript;
                questionInput.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, newAlignment);
                questionInput.Selection.ApplyPropertyValue(Inline.FontSizeProperty, 6);
            }
            else if (Superscript)
            {
                BaselineAlignment newAlignment = BaselineAlignment.Superscript;
                questionInput.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, newAlignment);
            }
            else
            {
                BaselineAlignment newAlignment = BaselineAlignment.Baseline;
                questionInput.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, newAlignment);
            }
        }
    }
}
