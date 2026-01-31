using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Text;
using System.Windows.Documents;
using System.Diagnostics;
using Series_Tracker.Models;

namespace Series_Tracker.Services
{
    public class PlatformFileService : IPlatformFileService
    {
        private readonly string _baseFolder;
        //public string BaseFolder => _baseFolder;
        public PlatformFileService()
        {
            _baseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "Series Tracker");
            if (!Directory.Exists(_baseFolder))
            {
                Directory.CreateDirectory(_baseFolder);
            }

        }

        public string GetBaseFolder()
        {
            return _baseFolder;
        }
        public string GetFilePath(Platform platform)
        {
            return Path.Combine(_baseFolder, $"{platform}.rtf");
        }
        public bool FileExists(Platform platform)
        {
            return File.Exists(GetFilePath(platform));
        }


        public FlowDocument Load(Platform platform)
        {
            var path = GetFilePath(platform);

            if (!FileExists(platform)) return new FlowDocument();
            try
            {
                using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                FlowDocument doc = new FlowDocument();
                TextRange textRange = new TextRange(doc.ContentStart, doc.ContentEnd);

                textRange.Load(fs, DataFormats.Rtf);
                return doc;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading file for {platform}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new FlowDocument();
            }
        }
        public void Save(Platform platform, FlowDocument document)
        {
            var path = GetFilePath(platform);

            try
            {
                TextRange textRange = new TextRange(document.ContentStart, document.ContentEnd);
                string text = textRange.Text;
                if (string.IsNullOrWhiteSpace(text))
                {
                    MessageBoxResult boxResult = MessageBox.Show($"No content to save for {platform}.\nDelete existing file if any?", "Info", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (boxResult == MessageBoxResult.No) return;
                    
                    if (FileExists(platform))
                    {
                        File.Delete(path);
                    }
                    return;
                }
                using FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                textRange.Save(fs, DataFormats.Rtf);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file for {platform}.\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OpenFolder()
        {
            try
            {
                if (Directory.Exists(_baseFolder))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = _baseFolder,
                        UseShellExecute = true,
                    });
                }
                else
                {
                    MessageBox.Show($"Folder does not exist: {_baseFolder}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error opening folder: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
