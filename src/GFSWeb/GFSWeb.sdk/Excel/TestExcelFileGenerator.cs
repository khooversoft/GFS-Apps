using ClosedXML.Excel;
using Toolbox.Types;

namespace GFSWeb.sdk.Excel;

public class TestExcelFileGenerator
{
    public static DataETag Generate()
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.AddWorksheet("Sample Sheet");
        worksheet.Cell("A1").Value = "Hello World!";
        worksheet.Cell("A2").FormulaA1 = "MID(A1, 7, 5)";

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return new DataETag(stream.ToArray());
    }
}
