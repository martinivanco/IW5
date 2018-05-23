using IW5Gallery.BL.Models;
using IW5Gallery.DAL.Entities;

namespace IW5Gallery.App
{
    public interface IFileManager
    {
        void AddImage(string path);
        void ExportImagesFromAlbum(AlbumDetailModel sourceAlbum);
        bool CheckExistenceOfImageFile(ImageDetailModel image);
    }
}
