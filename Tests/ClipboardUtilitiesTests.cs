using Text_Grab.Utilities;

namespace Tests;

public class ClipboardUtilitiesTests
{
    private const string SampleCfHtml = """
        Version:1.0
        StartHTML:00000097
        EndHTML:00002353
        StartFragment:00000153
        EndFragment:00002320
        <!DOCTYPE><HTML><HEAD></HEAD><BODY><!--StartFragment --><html>
            <body>
                <table>
                    <tr>
                        <td>Month</td>
                        <td>Int</td>
                        <td>Season</td>
                    </tr>
                    <tr>
                        <td>January</td>
                        <td>1</td>
                        <td>Winter</td>
                    </tr>
                    <tr>
                        <td>February</td>
                        <td>2</td>
                        <td>Winter</td>
                    </tr>
                </table>
            </body>
        </html><!--EndFragment --></BODY></HTML>
        """;

    [Fact]
    public void ConvertHtmlToTabSeparated_ParsesBasicTable()
    {
        string result = ClipboardUtilities.ConvertHtmlToTabSeparated(SampleCfHtml);

        string[] lines = result.Split('\n');
        Assert.Equal(3, lines.Length);
        Assert.Equal("Month\tInt\tSeason", lines[0]);
        Assert.Equal("January\t1\tWinter", lines[1]);
        Assert.Equal("February\t2\tWinter", lines[2]);
    }

    [Fact]
    public void ConvertHtmlToTabSeparated_HandlesBrTag()
    {
        string html = """
            <!--StartFragment--><table>
                <tr><td>4<br/>A</td><td>Spring</td></tr>
            </table><!--EndFragment-->
            """;

        string result = ClipboardUtilities.ConvertHtmlToTabSeparated(html);

        Assert.Equal("4 A\tSpring", result);
    }

    [Fact]
    public void ConvertHtmlToTabSeparated_ReturnsEmptyWhenNoTable()
    {
        string html = "<!--StartFragment--><p>No table here</p><!--EndFragment-->";
        string result = ClipboardUtilities.ConvertHtmlToTabSeparated(html);
        Assert.Empty(result);
    }

    [Fact]
    public void ConvertHtmlToTabSeparated_DecodesHtmlEntities()
    {
        string html = """
            <!--StartFragment--><table>
                <tr><td>A &amp; B</td><td>&lt;tag&gt;</td></tr>
            </table><!--EndFragment-->
            """;

        string result = ClipboardUtilities.ConvertHtmlToTabSeparated(html);

        Assert.Equal("A & B\t<tag>", result);
    }

    [Fact]
    public void ConvertHtmlToTabSeparated_HandlesThElements()
    {
        string html = """
            <!--StartFragment--><table>
                <tr><th>Name</th><th>Value</th></tr>
                <tr><td>Foo</td><td>42</td></tr>
            </table><!--EndFragment-->
            """;

        string result = ClipboardUtilities.ConvertHtmlToTabSeparated(html);

        string[] lines = result.Split('\n');
        Assert.Equal(2, lines.Length);
        Assert.Equal("Name\tValue", lines[0]);
        Assert.Equal("Foo\t42", lines[1]);
    }
}
