using Toolbox.Tools;

namespace GFSWeb.sdk.Store.Azure;

public record ReportFileDetail
{
    public ReportFileDetail(string fullPath, DateTime createdDate, long contentLength)
    {
        fullPath.NotEmpty();

        var (userEmail, packageId, fileName) = UserDatalakeStoreTool.ParsePath(fullPath);

        UserEmail = userEmail;
        PackageId = packageId;
        FileName = fileName;
        CreatedDate = createdDate;
        ContentLength = contentLength;
    }

    public string UserEmail { get; }
    public string PackageId { get; }
    public string FileName { get; }
    public DateTime CreatedDate { get; }
    public long ContentLength { get; }

    public string SizeK => $"{(int)(ContentLength / 1024)}K";
}
