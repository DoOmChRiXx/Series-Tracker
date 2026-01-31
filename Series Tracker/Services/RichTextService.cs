using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;

namespace Series_Tracker.Services
{
    public class RichTextService: IRichTextService
    {
        public void ToggleBold(RichTextBox rtb)
        {
            EditingCommands.ToggleBold.Execute(null, rtb);
        }
        public void ToggleItalic(RichTextBox rtb)
        {
            EditingCommands.ToggleItalic.Execute(null, rtb);
        }
        public void ToggleUnderline(RichTextBox rtb)
        {
            EditingCommands.ToggleUnderline.Execute(null, rtb);
        }
        public void IncreaseFontSize(RichTextBox rtb)
        {
            ChangeFontSize(rtb, +2);
        }
        public void DecreaseFontSize(RichTextBox rtb)
        {
            ChangeFontSize(rtb, -2); ;
        }

        public void ToggleBullets(RichTextBox rtb)
        {
            EditingCommands.ToggleBullets.Execute(null, rtb);
        }

        private void ChangeFontSize(RichTextBox rtb, double delta)
        {
            var selection = rtb.Selection;
            if (!selection.IsEmpty)
            {
                object currentSize = selection.GetPropertyValue(TextElement.FontSizeProperty);
                if (currentSize != DependencyProperty.UnsetValue && double.TryParse(currentSize.ToString(), out double size))
                {
                    double newSize = size + delta;
                    if (newSize < 1) newSize = 1; // Prevent font size from going below 1
                    selection.ApplyPropertyValue(TextElement.FontSizeProperty, newSize);
                }
            }
        }
        public void ToggleStrikethrough(RichTextBox rtb)
        {
            var selection = rtb.Selection;
            if (!selection.IsEmpty)
            {
                var current = selection.GetPropertyValue(Inline.TextDecorationsProperty);
                bool isActive = current != DependencyProperty.UnsetValue &&
                    current is TextDecorationCollection decorations &&
                    decorations.Any(d => d.Location == TextDecorationLocation.Strikethrough);

                if (isActive)
                {
                    // Remove strikethrough
                    selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
                }
                else
                {
                    // Add strikethrough
                    selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Strikethrough);
                }
            }
        }

        public void ApplyFontFamily(RichTextBox rtb, FontFamily fontFamily)
        {
            var selection = rtb.Selection;
            if (!selection.IsEmpty)
            {
                selection.ApplyPropertyValue(TextElement.FontFamilyProperty, fontFamily);
            }

            else
            {
                TextRange range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                range.ApplyPropertyValue(TextElement.FontFamilyProperty, fontFamily);
            }
        }

        public void ApplyFontSize(RichTextBox rtb, double fontSize)
        {
            var selection = rtb.Selection;
            if (!selection.IsEmpty)
            {
                selection.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize);
            }
            else
            {
                TextRange range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                range.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize);
            }
        }
    }
}
