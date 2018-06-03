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
        Window MW = new Window();
        TaskPage taskPage;

        public QuestionPage(Window win, TaskPage tp)
        {
            MW = win;
            taskPage = tp;
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

        private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string ch = ((System.Windows.Forms.Keys)(KeyInterop.VirtualKeyFromKey(e.Key))).ToString();

            Run p = new Run(ch);
            p.Typography.Variants = FontVariants.Superscript;
            //MessageBox.Show("1" + p.Text);
            questionInput.Document.Blocks.Add(new Paragraph(p));
                string clipText = System.Windows.Forms.Clipboard.GetText();
                System.Windows.Forms.Clipboard.SetDataObject(p);
                questionInput.Paste();
                System.Windows.Forms.Clipboard.Clear();
                questionInput.IsReadOnly = true;
                System.Windows.Forms.Clipboard.SetText(clipText);
        }
    }
}
