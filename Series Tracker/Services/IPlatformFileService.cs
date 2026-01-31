using System.Windows.Documents;
using Series_Tracker.Models;
namespace Series_Tracker.Services
{
    public interface IPlatformFileService
    {
        string GetFilePath(Platform platform);
        bool FileExists(Platform platform);
        FlowDocument Load(Platform platform);
        void Save(Platform platform, FlowDocument document);
        void OpenFolder();
    }
}
