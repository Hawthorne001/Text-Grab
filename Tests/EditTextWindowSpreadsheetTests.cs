using System.Data;
using Text_Grab;

namespace Tests;

public class EditTextWindowSpreadsheetTests
{
    [Fact]
    public void ClearSpreadsheetCellValues_ClearsOnlyRequestedCells()
    {
        DataTable dataTable = new();
        dataTable.Columns.Add("A", typeof(string));
        dataTable.Columns.Add("B", typeof(string));
        dataTable.Columns.Add("C", typeof(string));
        dataTable.Rows.Add("a1", "b1", "c1");
        dataTable.Rows.Add("a2", "b2", "c2");

        EditTextWindow.ClearSpreadsheetCellValues(
            dataTable,
            [
                (0, 0),
                (1, 2),
                (1, 2),
                (-1, 1),
                (5, 0),
                (0, 5)
            ]);

        Assert.Equal(string.Empty, dataTable.Rows[0][0]);
        Assert.Equal("b1", dataTable.Rows[0][1]);
        Assert.Equal("c1", dataTable.Rows[0][2]);
        Assert.Equal("a2", dataTable.Rows[1][0]);
        Assert.Equal("b2", dataTable.Rows[1][1]);
        Assert.Equal(string.Empty, dataTable.Rows[1][2]);
    }
}
