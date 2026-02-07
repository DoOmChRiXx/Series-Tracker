using Series_Tracker.Models;
using Series_Tracker.Resources;
using Series_Tracker.Services;
using Series_Tracker.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Series_Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(new WindowService());
        }

        private void EditorLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.Editor = (RichTextBox)sender;
            }
        }

        private void EditorControl_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm && sender is RichTextBox rtb)
            {
                vm.UpdateFormattingState(rtb);
            }
        }

        private void EditorControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is MainViewModel vm && sender is RichTextBox rtb)
            {
                vm.OnDocumentChanged(rtb);
            }
        }

        private void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EditorControl.Cut();
        }

        private void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (EditorControl == null)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = !EditorControl.Selection.IsEmpty;
        }

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EditorControl.Copy();
        }

        private void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (EditorControl == null)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = !EditorControl.Selection.IsEmpty;
        }

        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EditorControl.Paste();
        }

        private void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (EditorControl == null)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = Clipboard.ContainsText() || Clipboard.ContainsImage();
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (PubVars.IsNotSingleton == true) return;
            if (PubVars.RtbChanged == true)
            {
                if (MessageBox.Show("There are UNSAVED changes!\n" +
                    "Are you sure you want to exit.", "Confirm Exit", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}