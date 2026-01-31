using System;
using System.Windows.Media;
using System.Windows.Controls;

namespace Series_Tracker.Services
{
    public interface IRichTextService
    {
        void ToggleBold(RichTextBox rtb);
        void ToggleItalic(RichTextBox rtb);
        void ToggleUnderline(RichTextBox rtb);
        void ToggleStrikethrough(RichTextBox rtb);
        void IncreaseFontSize(RichTextBox rtb);
        void DecreaseFontSize(RichTextBox rtb);
        void ToggleBullets(RichTextBox rtb);
        void ApplyFontFamily(RichTextBox rtb, FontFamily fontFamily);
        void ApplyFontSize(RichTextBox rtb, double fontSize);
         
    }
}
