using System;
using System.Collections.Generic;
using System.Text;
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

        File.WriteAllBytes(@"E:\\work\test.xlsx", [..blobData.Data]);
    }
}
