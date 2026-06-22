using GFSWeb.sdk.Excel;
using Toolbox.Tools;

namespace GFSWeb.sdk.test.Excel;

public class ExcelGenerationTest
{
    [Fact]
    public void GenerateExcelFile_ShouldReturnBlobData()
    {
        // Act
        var blobData = TestExcelFileGenerator.Generate();

        // Assert
        blobData.Data.Length.Assert(x => x > 0, "BlobData.Data should not be empty.");
        blobData.ETag.NotEmpty();

        string tempFile = Path.GetTempFileName() + ".xlsx";
        File.WriteAllBytes(tempFile, [.. blobData.Data]);
    }
}
