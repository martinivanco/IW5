using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Gallery.DAL.Entities;

namespace IW5Gallery.DAL
{
    public class FileManager : IFileManager
    {
        public void AddImage(string path)
        {
            throw new NotImplementedException();
        }

        public void AddImagesFromLocation(string path)
        {
            throw new NotImplementedException();
        }

        public void CopyImages(string sourcePath, string destinationPath)
        {
            throw new NotImplementedException();
        }

        public void ExportImagesFromAlbum(Album sourceAlbum, string destinationPath)
        {
            throw new NotImplementedException();
        }

        public bool CheckExistenceOfImage(Image image)
        {
            throw new NotImplementedException();
        }

        private void AddImageToDatabase(Image image)
        {
            throw new NotImplementedException();
        }

        private ICollection<Image> GetImagesFromAlbum(Album album)
        {
            throw new NotImplementedException();
        }

        //Singleton
        private static FileManager _instance;

        public static FileManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FileManager();
                }
                return _instance;
            }
        }
    }
}
