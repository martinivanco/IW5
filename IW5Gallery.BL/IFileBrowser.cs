using System.Collections.Generic;
using System.IO;

namespace IW5Gallery.BL
{
    public interface IFileBrowser
    {
        List<FileInfo> OpenFileInfos();
        string GetTargetDirectory();
        bool CheckFileExistence(string path);
        void CopyFile(string sourceFilePath, string targetFilePath);
    }
}