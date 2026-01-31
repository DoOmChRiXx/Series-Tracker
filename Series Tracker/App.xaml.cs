using Series_Tracker.Resources;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Series_Tracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex? mutex;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "SeriesTracker_SingleInstance";
            bool createdNew;
            mutex = new Mutex(true, appName, out createdNew);
            if (!createdNew)
            {
                // Application is already running
                PubVars.IsNotSingleton = true;
                MessageBox.Show("Another instance of the application is already running.", "Application Already Running", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();
                PubVars.IsNotSingleton = false;
                return;
            }
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (mutex != null)
            {
                mutex?.ReleaseMutex();
                mutex?.Dispose();
            }
            base.OnExit(e);
        }
    }

}
