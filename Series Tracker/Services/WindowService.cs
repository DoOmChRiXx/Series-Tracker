using Series_Tracker.Views;


namespace Series_Tracker.Services
{
    public class WindowService : IWindowService
    {
        private ViewAbout? _aboutWindow;
        public void ShowAbout()
        {
            if (_aboutWindow == null)
            {
                _aboutWindow = new ViewAbout();
                _aboutWindow.Closed += (s, e) => _aboutWindow = null;
                _aboutWindow.ShowDialog();
            }
            else
            {
                _aboutWindow.Activate();
            }
        }
    }
}
