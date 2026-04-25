using System.Reflection;
using Text_Grab;
using Text_Grab.Models;

namespace Tests;

public class EditTextWindowActionCatalogTests
{
    private readonly record struct ExpectedButtonAction(string ButtonText, string? Command = null, string? ClickEvent = null);

    [Fact]
    public void AllButtons_UsesResolvableEditTextCommandsAndClickEvents()
    {
        HashSet<string> commandNames = [.. EditTextWindow.GetRoutedCommands().Keys];
        HashSet<string> methodNames = [.. typeof(EditTextWindow)
            .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
            .Select(method => method.Name)];

        foreach (ButtonInfo button in ButtonInfo.AllButtons)
        {
            if (!string.IsNullOrWhiteSpace(button.Command))
                Assert.Contains(button.Command, commandNames);

            if (!string.IsNullOrWhiteSpace(button.ClickEvent))
                Assert.Contains(button.ClickEvent, methodNames);
        }
    }

    [Fact]
    public void AllButtons_ContainsExpectedEditTextActions()
    {
        ExpectedButtonAction[] expectedButtons =
        [
            new("OCR Paste", Command: "OcrPasteCommand"),
            new("Write .txt File For Each Image", ClickEvent: "ToggleWriteTxtFileForEachImage_Click"),
            new("Close", ClickEvent: "CloseMenuItem_Click"),
            new("Correct Common GUID/UUID Errors", ClickEvent: "CorrectGuid_Click"),
            new("Transpose Table", Command: "TransposeTableCmd"),
            new("Add Spreadsheet Row", ClickEvent: "AddSpreadsheetRowMenuItem_Click"),
            new("Add Spreadsheet Column", ClickEvent: "AddSpreadsheetColumnMenuItem_Click"),
            new("Copy Selected Spreadsheet Cells", ClickEvent: "CopySpreadsheetSelectionMenuItem_Click"),
            new("Copy Selected Spreadsheet Rows", ClickEvent: "CopySpreadsheetRowsMenuItem_Click"),
            new("Copy Current Spreadsheet Column", ClickEvent: "CopySpreadsheetColumnMenuItem_Click"),
            new("Move Spreadsheet Row Up", ClickEvent: "MoveSpreadsheetRowUpMenuItem_Click"),
            new("Move Spreadsheet Row Down", ClickEvent: "MoveSpreadsheetRowDownMenuItem_Click"),
            new("Delete Spreadsheet Row", ClickEvent: "DeleteSpreadsheetRowMenuItem_Click"),
            new("Move Spreadsheet Column Left", ClickEvent: "MoveSpreadsheetColumnLeftMenuItem_Click"),
            new("Move Spreadsheet Column Right", ClickEvent: "MoveSpreadsheetColumnRightMenuItem_Click"),
            new("Delete Spreadsheet Column", ClickEvent: "DeleteSpreadsheetColumnMenuItem_Click"),
            new("Enter Raw Text Mode", ClickEvent: "EnterRawTextMode_Click"),
            new("Enter Spreadsheet Mode", ClickEvent: "EnterSpreadsheetMode_Click"),
            new("Enter Markdown Mode", ClickEvent: "EnterMarkdownMode_Click"),
            new("Toggle Show Math Errors", ClickEvent: "ToggleShowMathErrors_Click"),
            new("Toggle Calculation Pane", ClickEvent: "CalcToggleButton_Click"),
            new("Copy All Calculation Results", ClickEvent: "CalcCopyAllButton_Click"),
            new("Calculation Pane Help", ClickEvent: "CalcInfoButton_Click"),
            new("Toggle Always On Top", ClickEvent: "ToggleAlwaysOnTop_Click"),
            new("Toggle Hide Bottom Bar", ClickEvent: "ToggleHideBottomBar_Click"),
            new("Toggle Fullscreen on Launch", ClickEvent: "ToggleLaunchFullscreenOnLoad_Click"),
            new("Toggle Restore Window Position", ClickEvent: "ToggleRestorePosition_Click"),
            new("Restore This Window Position", ClickEvent: "RestoreThisPosition_Click"),
            new("Toggle Margins", ClickEvent: "ToggleMargins_Click"),
            new("Toggle Wrap Text", ClickEvent: "ToggleWrapText_Click"),
            new("Font...", ClickEvent: "FontMenuItem_Click"),
            new("Grab Previous Region", ClickEvent: "PreviousRegion_Click"),
            new("Edit Last Grab", ClickEvent: "OpenLastAsGrabFrameMenuItem_Click"),
            new("Contact Developer", ClickEvent: "ContactMenuItem_Click"),
            new("Rate and Review", ClickEvent: "RateAndReview_Click"),
            new("Feedback...", ClickEvent: "FeedbackMenuItem_Click"),
            new("About", ClickEvent: "AboutMenuItem_Click"),
            new("Select All", ClickEvent: "SelectAllMenuItem_Click"),
            new("Select None", ClickEvent: "SelectNoneMenuItem_Click"),
            new("Delete Selected Text", ClickEvent: "DeleteSelectedTextMenuItem_Click"),
            new("Show Character Details", ClickEvent: "CharDetailsButton_Click"),
            new("Find Similar Matches", ClickEvent: "SimilarMatchesButton_Click"),
            new("Open Regex Pattern Search", ClickEvent: "RegexPatternButton_Click"),
            new("Save Regex Pattern", ClickEvent: "SavePatternMenuItem_Click"),
            new("Explain Regex Pattern", ClickEvent: "ExplainPatternMenuItem_Click"),
            new("Summarize Paragraph", ClickEvent: "SummarizeMenuItem_Click"),
            new("Rewrite with Local AI", ClickEvent: "RewriteMenuItem_Click"),
            new("Convert to Table", ClickEvent: "ConvertTableMenuItem_Click"),
            new("Translate to System Language", ClickEvent: "TranslateToSystemLanguageMenuItem_Click"),
            new("Translate to English", ClickEvent: "TranslateToEnglish_Click"),
            new("Translate to Spanish", ClickEvent: "TranslateToSpanish_Click"),
            new("Translate to French", ClickEvent: "TranslateToFrench_Click"),
            new("Translate to German", ClickEvent: "TranslateToGerman_Click"),
            new("Translate to Italian", ClickEvent: "TranslateToItalian_Click"),
            new("Translate to Portuguese", ClickEvent: "TranslateToPortuguese_Click"),
            new("Translate to Russian", ClickEvent: "TranslateToRussian_Click"),
            new("Translate to Japanese", ClickEvent: "TranslateToJapanese_Click"),
            new("Translate to Chinese (Simplified)", ClickEvent: "TranslateToChineseSimplified_Click"),
            new("Translate to Korean", ClickEvent: "TranslateToKorean_Click"),
            new("Translate to Arabic", ClickEvent: "TranslateToArabic_Click"),
            new("Translate to Hindi", ClickEvent: "TranslateToHindi_Click"),
            new("Extract RegEx", ClickEvent: "ExtractRegexMenuItem_Click"),
            new("Learn About Local AI Features...", ClickEvent: "LearnAiMenuItem_Click"),
        ];

        foreach (ExpectedButtonAction expected in expectedButtons)
        {
            ButtonInfo button = Assert.Single(ButtonInfo.AllButtons, button => button.ButtonText == expected.ButtonText);

            if (expected.Command is not null)
                Assert.Equal(expected.Command, button.Command);

            if (expected.ClickEvent is not null)
                Assert.Equal(expected.ClickEvent, button.ClickEvent);
        }
    }
}
