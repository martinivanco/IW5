using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using IW5Gallery.DAL.Entities;
using System.IO;
using System.Windows;
using IW5Gallery.BL;
using IW5Gallery.BL.Messages;
using IW5Gallery.BL.Models;
using IW5Gallery.BL.Repositories;
using Microsoft.Win32;

namespace IW5Gallery.App
{
    public class FileManager : IFileManager
    {
        private readonly ImageRepository _imageRepository;
        private readonly IMessenger _messenger;

        public FileManager(ImageRepository imageRepository, IMessenger messenger)
        {
            _imageRepository = imageRepository;
            _messenger = messenger;
        }

        public void BrowseFiles()
        {
           var browser = new FileBrowser();
            browser.CheckThumbnailsDirectoryExistence();
            var fileInfos = browser.OpenFileInfos();
            foreach (var fileInfo in fileInfos)
            {
                AddImageToDatabase(CreateImage(fileInfo));
            }
        }

        private static ImageDetailModel CreateImage(FileInfo file)
        {
            var img = System.Drawing.Image.FromFile(file.FullName);
            var image = new ImageDetailModel
            {
                Id = Guid.NewGuid(),
                DateTaken = file.CreationTime,
                Name = Path.GetFileNameWithoutExtension(file.FullName),
                Format = (Format)Enum.Parse(typeof(Format), file.Extension.ToLower().Remove(0,1)),
                Height = img.Height,
                Width = img.Width,
                DateAdded = DateTime.Now,
                Note = "No note has been specified.",
                Path = file.FullName
            };

            return image;
        }

        public void AddImage(string path)
        {
            var fileInfo = new FileInfo(path);

            if (!fileInfo.Exists || !CheckFileExtension(fileInfo)) return;
            var imageDetail = CreateImage(fileInfo);
            AddImageToDatabase(imageDetail);
        }

        public void ExportImagesFromAlbum(AlbumDetailModel sourceAlbum)
        {
            var browser = new FileBrowser();
            var targetPath = Path.Combine(browser.GetTargetDirectory(), sourceAlbum.Name);
            foreach (var image in sourceAlbum.Images)
            {
                var imageDetail = _imageRepository.GetImageById(image.Id);
                try
                {
                    browser.CopyFile(imageDetail.Path, Path.Combine(targetPath, Path.GetFileName(imageDetail.Path) ?? throw new InvalidOperationException()));
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to get target path. " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        public bool CheckExistenceOfImageFile(ImageDetailModel image)
        {
            var browser = new FileBrowser();
            return browser.CheckFileExistence(image.Path);
        }

        private void AddImageToDatabase(ImageDetailModel image)
        {
            _imageRepository.InsertImage(image);
            _messenger.Send(new NewImageAddedMessage(image.Id));
        }

        private static bool CheckFileExtension(FileSystemInfo file)
        {
            try
            {
                var extension = Enum.Parse(typeof(Format), file.Extension, true);
                return true;
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Unsupported file extension.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

    }
}
