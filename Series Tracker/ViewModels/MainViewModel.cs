using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Series_Tracker.Commands;
using Series_Tracker.Models;
using Series_Tracker.Resources;
using Series_Tracker.Services;

namespace Series_Tracker.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IRichTextService _richTextService;
        private readonly IPlatformFileService _platformFileService;
        private readonly IWindowService _windowService;
        private bool _isUpdatingFromSelection;
        private FontFamily _selectedFontFamily;
        //private bool rtbChanged = false;

        public FontFamily SelectedFontFamily
        {
            get => _selectedFontFamily;
            set
            { if (SetProperty(ref _selectedFontFamily, value))
                {
                    if (!_isUpdatingFromSelection)
                    ApplyFontFamily();
                }
            }
        }
        public string LastSavedStatus
        {
            get => _lastSavedStatus;
            set => SetProperty(ref _lastSavedStatus, value);
        }
        private string _lastSavedStatus = "Never";

        public string AppTitle { get; } = PubVars.ApplicationName;

        private Platform? _previousPlatform;
        public Platform? PreviousPlatform
        {
            get => _previousPlatform;
            set => _previousPlatform = value;
        }


        private Platform? _selectedPlatform;
        public Platform? SelectedPlatform
        {
            get => _selectedPlatform;
            set
            {
                if(_selectedPlatform == value) return;

                if (PubVars.RtbChanged && PreviousPlatform is Platform oldPlatform)
                {
                    // Optionally, prompt to save changes before switching platforms
                    MessageBoxResult result = MessageBox.Show("You have unsaved changes. Do you want to save before switching platforms?", "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        Save(oldPlatform);
                        PubVars.RtbChanged = false;
                    }
                }
                if(SetProperty(ref _selectedPlatform, value))
                {
                    PreviousPlatform = value;
                    LoadCreatePlatformFile();
                }
            }
        }


        private FlowDocument _document;
        public FlowDocument Document
        {
            get => _document;
            set => SetProperty(ref _document, value);
        }

        public List<double> FontSizes { get; } =
        [
            8,9,10,11,12,14,16,18,20,22,24,26,28,36,48,72
        ];

        private double _selectedFontSize;
        public double SelectedFontSize
        {
            get => _selectedFontSize;
            set
            {
                if (SetProperty(ref _selectedFontSize, value))
                {
                    if (!_isUpdatingFromSelection)
                        ApplyFontSize();
                }
            }
        }

        // Requiered to bind RichTextBox to formatting commands

        public RichTextBox Editor { get; set; }
        public ICommand BoldCommand { get; }
        public ICommand ItalicCommand { get; }
        public ICommand UnderlineCommand { get; }
        public ICommand StrikethroughCommand { get; }
        public ICommand IncreaseFontSizeCommand { get; }
        public ICommand DecreaseFontSizeCommand { get; }
        public ICommand ToggleBulletsCommand { get; }
        public ICommand OpenFolderCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ShowAboutCommand { get; }

        public MainViewModel(IWindowService windowService)
        {
            _richTextService = new RichTextService();
            _platformFileService = new PlatformFileService();
            _windowService = windowService;

            BoldCommand = new RelayCommand(_ => _richTextService.ToggleBold(Editor));
            ItalicCommand = new RelayCommand(_ => _richTextService.ToggleItalic(Editor));
            UnderlineCommand = new RelayCommand(_ => _richTextService.ToggleUnderline(Editor));
            StrikethroughCommand = new RelayCommand(_ => _richTextService.ToggleStrikethrough(Editor));
            IncreaseFontSizeCommand = new RelayCommand(_ => _richTextService.IncreaseFontSize(Editor));
            DecreaseFontSizeCommand = new RelayCommand(_ => _richTextService.DecreaseFontSize(Editor));
            ToggleBulletsCommand = new RelayCommand(_ => _richTextService.ToggleBullets(Editor));
            OpenFolderCommand = new RelayCommand(_ => OpenFolder());
            ShowAboutCommand = new RelayCommand(_ => _windowService.ShowAbout());
            SaveCommand = new RelayCommand(_ => Save());
        }

        private void LoadCreatePlatformFile()
        {
            if (SelectedPlatform is not Platform platform) return;


            Document = _platformFileService.Load(platform);
            if (Document.Blocks.Count == 0)
            {
                Document.FontFamily = new FontFamily("Segoe UI");
                Document.FontSize = 14;
                LastSavedStatus = $"List your series to generate a new {SelectedPlatform} document";
                PubVars.RtbChanged = false;
                Editor.Focus();
                return;
            }
            LastSavedStatus = $"Loaded {SelectedPlatform} at {System.DateTime.Now:t}";
            PubVars.RtbChanged = false;
        }

        private void Save(Platform platform)
        {
            _platformFileService.Save(platform, Document);
            PubVars.RtbChanged = false;
            LastSavedStatus = $"Saved {SelectedPlatform} at {System.DateTime.Now:t}";
        }

        private void ApplyFontFamily()
        {
            if (Editor != null && SelectedFontFamily != null)
            {
                _richTextService.ApplyFontFamily(Editor, SelectedFontFamily);
            }
        }

        private void ApplyFontSize()
        {
            if (Editor != null && SelectedFontSize > 0)
            {
                _richTextService.ApplyFontSize(Editor, SelectedFontSize);
            }
        }

        public void UpdateFormattingState(RichTextBox rtb)
        {
            _isUpdatingFromSelection = true;

            var selection = rtb.Selection;

            var fontFamily = selection.GetPropertyValue(TextElement.FontFamilyProperty);
            if (fontFamily != DependencyProperty.UnsetValue)
            {
                SelectedFontFamily = (FontFamily)fontFamily; 
            }

            var fontSize = selection.GetPropertyValue(TextElement.FontSizeProperty);
            if (fontSize != DependencyProperty.UnsetValue)
            {
                SelectedFontSize = (double)fontSize;
            }

            _isUpdatingFromSelection = false;
        }

        public void OnDocumentChanged(RichTextBox rtb)
        {
            PubVars.RtbChanged = true;
            LastSavedStatus = "Changes NOT saved";
        }

        public void OpenFolder()
        {
                _platformFileService.OpenFolder();
        }
        private void Save()
        {
            if (SelectedPlatform is Platform platform)
                Save(platform);
        }
    }
}