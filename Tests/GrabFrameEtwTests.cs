using Text_Grab.Utilities;

namespace Tests;

public class GrabFrameEtwTests
{
    [Theory]
    [InlineData(true, false, true)]
    [InlineData(true, true, false)]
    [InlineData(false, false, false)]
    [InlineData(false, true, false)]
    public void ShouldOpenNewEtwInSpreadsheetMode_OnlyReturnsTrueForNewTableEtw(
        bool isTableModeSelected,
        bool hasExistingEditTextWindow,
        bool expected)
    {
        bool shouldUseSpreadsheetMode = WindowUtilities.ShouldOpenNewEtwInSpreadsheetMode(
            isTableModeSelected,
            hasExistingEditTextWindow);

        Assert.Equal(expected, shouldUseSpreadsheetMode);
    }
}
