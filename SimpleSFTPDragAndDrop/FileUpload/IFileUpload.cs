using System.Collections.Generic;

namespace SimpleSFTPDragAndDrop.FileUpload;

public interface IFileUpload
{
    public void UploadFiles(IEnumerable<string> filePaths, string? targetDirectory);
}