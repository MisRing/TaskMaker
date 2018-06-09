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
<<<<<<< HEAD
        public bool Subscript = false, Supperscript = false, Bold = false, Cursive = false, BaseLine = false;

=======
        bool Subscript = false, Superscript = false;
        maths mat = new maths();
        spec sp = new spec();
>>>>>>> d1151383eea3f2bd76304606efcaca727b8d76e1
        BrushConverter converter = new System.Windows.Media.BrushConverter();
        Brush brush;

        public QuestionPage(Window win, TaskPage tp)
        {
            brush = (Brush)converter.ConvertFromString("#FFFCB93E");
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

<<<<<<< HEAD
        private void sup_Click(object sender, RoutedEventArgs e)
        {
            Supperscript = !Supperscript;
            Subscript = false;

            sub.Background = brush;
            sup.Background = Supperscript ? Brushes.LightCyan : brush;
        }

        private void sub_Click(object sender, RoutedEventArgs e)
        {
            Subscript = !Subscript;
            Supperscript = false;

            sup.Background = brush;
            sub.Background = Subscript ? Brushes.LightCyan : brush;
        }

        private void baseLine_Click(object sender, RoutedEventArgs e)
        {
            BaseLine = !BaseLine;
            baseLine.Background = BaseLine ? Brushes.LightCyan : brush;
        }

        private void cur_Click(object sender, RoutedEventArgs e)
        {
            Cursive = !Cursive;
            cur.Background = Cursive ? Brushes.LightCyan : brush;
        }

        private void bolt_Click(object sender, RoutedEventArgs e)
        {
            Bold = !Bold;
            bold.Background = Bold ? Brushes.LightCyan : brush;
=======
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
>>>>>>> d1151383eea3f2bd76304606efcaca727b8d76e1
        }

        private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
<<<<<<< HEAD
            if(Supperscript)
            {
                BaselineAlignment newAlignment = BaselineAlignment.Superscript;
                questionInput.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, newAlignment);
                questionInput.Selection.ApplyPropertyValue(Inline.FontSizeProperty, (double)10);
            }
            else if(Subscript)
            {
                BaselineAlignment newAlignment = BaselineAlignment.Subscript;
                questionInput.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, newAlignment);
                questionInput.Selection.ApplyPropertyValue(Inline.FontSizeProperty, (double)10);
=======
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
>>>>>>> d1151383eea3f2bd76304606efcaca727b8d76e1
            }
            else
            {
                BaselineAlignment newAlignment = BaselineAlignment.Baseline;
                questionInput.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, newAlignment);
<<<<<<< HEAD
                questionInput.Selection.ApplyPropertyValue(Inline.FontSizeProperty, (double)14);
            }

            if(Bold)
            {
                questionInput.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.ExtraBold);
            }
            else
            {
                questionInput.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            }

            if(Cursive)
            {
                questionInput.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            }
            else
            {
                questionInput.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
            }

            if(BaseLine)
                questionInput.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
                questionInput.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);

=======
            }
>>>>>>> d1151383eea3f2bd76304606efcaca727b8d76e1
        }
    }
}
