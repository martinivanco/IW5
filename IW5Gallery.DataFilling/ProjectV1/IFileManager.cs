using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Gallery.DAL.Entities;

namespace IW5Gallery.DAL
{
    public interface IFileManager
    {
        void AddImage(string path);
        void AddImagesFromLocation(string path);
        void CopyImages(string sourcePath, string destinationPath);
        void ExportImagesFromAlbum(Album sourceAlbum, string destinationPath);
        bool CheckExistenceOfImage(Image image);
    }
}
