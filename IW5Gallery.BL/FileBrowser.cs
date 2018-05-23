using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Win32;
using WPFFolderBrowser;


namespace IW5Gallery.BL
{
    public class FileBrowser: IFileBrowser
    {
        public List<FileInfo> OpenFileInfos()
        {
            var openFileDialog = InitializeOpenFileDialog();
            var fileInfos = new List<FileInfo>();
            if (openFileDialog.ShowDialog() != true) return fileInfos;
            fileInfos.AddRange(openFileDialog.FileNames.Select(fileName => new FileInfo(fileName)));
            return fileInfos;
        }

        public string GetTargetDirectory()
        {
            var folderBrowser = new WPFFolderBrowserDialog {Title = "Choose a directory to extract your album to."};
            if (folderBrowser.ShowDialog() != true) return string.Empty;
            return folderBrowser.FileName;

        }

        public void CopyFile(string sourceFilePath, string targetFilePath)
        {
            string targetDirectory;
            if ((targetDirectory = Path.GetDirectoryName(targetFilePath)) == null) return;
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            try
            {
                File.Copy(sourceFilePath, targetFilePath, true);
            }
            catch (IOException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool CheckFileExistence(string path)
        {
            return File.Exists(path);
        }

        public void CheckThumbnailsDirectoryExistence()
        {
            var thumbnailDir = Path.Combine(AppContext.BaseDirectory, "thumbnails");
            if (!Directory.Exists(thumbnailDir))
            {
                Directory.CreateDirectory(thumbnailDir);
            }

        }
        private static OpenFileDialog InitializeOpenFileDialog()
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Images (*.BMP;*.JPG;*.GIF;*.PNG;*.TIF;)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIF;|" +
                         "All files (*.*)|*.*",
                Title = "Select images or folder"
            };
            return openFileDialog;
        }
    }
}