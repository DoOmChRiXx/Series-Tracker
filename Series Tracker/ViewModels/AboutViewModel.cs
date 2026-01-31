using Series_Tracker.Resources;
namespace Series_Tracker.ViewModels
{
    public class AboutViewModel: BaseViewModel
    {
        public string AppVersionAndBuild => $"{PubVars.Version} - Built on {PubVars.BuildDate}";
        public string ApplicationName { get; } = PubVars.ApplicationName;
        public string Version { get; } = PubVars.Version;
        public string Author { get; } = "Christian Trubbo";
        public string Description { get; } = "An application to track your favorite TV series.";
        public string Description2 { get; } = "This application allows you to keep track of the series you watch\n" +
            "and manage your watchlist to stay updated with new episodes.";
        public string OSVersion => PubVars.OSVersion;
        public string ImageIcon { get; } = "pack://Application:,,,/Assets/img/TV256.ico";
        public string GitHubLink { get; } = "https://github.com/DoOmChRiXx";
        public AboutViewModel()
        {
            
        }
    }

}
