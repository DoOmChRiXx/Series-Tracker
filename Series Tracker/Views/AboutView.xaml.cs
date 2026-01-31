using Series_Tracker.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace Series_Tracker.Views
{
    /// <summary>
    /// Interaction logic for ViewAbout.xaml
    /// </summary>
    public partial class ViewAbout : Window
    {
        public ViewAbout()
        {
            InitializeComponent();
            DataContext = new AboutViewModel();
        }

        private void OnOkClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true
            });

            e.Handled = true;
        }

    }
}
