using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Reflection;

namespace Editor
{

    public partial class MainWindow : Window
    {
        string version = Assembly.GetEntryAssembly().GetName().Version.ToString();

        public MainWindow()
        {
            InitializeComponent();
            mathToolBar.CommandCompleted += (x, y) => { editor.Focus(); };
            AddHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(MainWindow_MouseDown), true);
        }

        public void HandleToolBarCommand(CommandDetails commandDetails)
        {
            editor.HandleUserCommand(commandDetails);
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.DirectlyOver != null)
            {
                if (Mouse.DirectlyOver.GetType() == typeof(EditorToolBarButton))
                {
                    return;
                }
                else if (editor.IsMouseOver)
                {
                    editor.Focus();
                }
                mathToolBar.HideVisiblePanel();
            }
        }

        private void Window_TextInput(object sender, TextCompositionEventArgs e)
        {
            if (!editor.IsFocused)
            {
                editor.Focus();
                editor.ConsumeText(e.Text);
                mathToolBar.HideVisiblePanel();
            }
        }


        
           

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
        }



      
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!editor.IsFocused)
            {
                editor.Focus();
                mathToolBar.HideVisiblePanel();
            }
        }


        private void scrollViwer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            editor.InvalidateVisual();
        }

     

        private void button_Click(object sender, RoutedEventArgs e)
        {
            editor.ExportImage(@"\img.bmp", logic.Logic.text);
            this.Close();
        }
    }
}
