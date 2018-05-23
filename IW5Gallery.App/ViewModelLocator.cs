using System.Runtime.CompilerServices;
using IW5Gallery.App.ViewModels;
using IW5Gallery.BL;
using IW5Gallery.BL.Repositories;

namespace IW5Gallery.App
{
    public class ViewModelLocator
    {
        private readonly Messenger _messenger = new Messenger();
        private readonly TagRepository _tagRepository = new TagRepository();
        private readonly AlbumRepository _albumRepository = new AlbumRepository();
        private readonly ImageRepository _imageRepository = new ImageRepository();
        private FileManager _fileManager => new FileManager(_imageRepository, _messenger);

        public MainViewModel MainViewModel => CreateMainViewModel();
        public PhotosTabViewModel PhotosTabViewModel => CreatePhotosTabViewModel();
        public ImageListViewModel ImageListViewModel => CreateImageListViewModel();
        public ImageDetailViewModel ImageDetailViewModel => CreateImageDetailViewModel();
        public AlbumsTabViewModel AlbumsTabViewModel => CreateAlbumsTabViewModel();
        public AlbumListViewModel AlbumListViewModel => CreateAlbumListViewModel();
        public AlbumDetailViewModel AlbumDetailViewModel => CreateAlbumDetailViewModel();
        public TagsTabViewModel TagsTabViewModel => CreateTagsTabViewModel();
        public TagListViewModel TagListViewModel => CreateTagListViewModel();
        public TagDetailViewModel TagDetailViewModel => CreateTagDetailViewModel();

        private MainViewModel CreateMainViewModel()
        {
            return new MainViewModel(_messenger, _fileManager);
        }
        private PhotosTabViewModel CreatePhotosTabViewModel()
        {
            return new PhotosTabViewModel(_messenger);
        }
        private ImageListViewModel CreateImageListViewModel()
        {
            return new ImageListViewModel(_imageRepository, _messenger);
        }
        private ImageDetailViewModel CreateImageDetailViewModel()
        {
            return new ImageDetailViewModel(_imageRepository, _albumRepository, _tagRepository, _messenger);
        }
        private AlbumsTabViewModel CreateAlbumsTabViewModel()
        {
            return new AlbumsTabViewModel(_messenger);
        }
        private AlbumListViewModel CreateAlbumListViewModel()
        {
            return new AlbumListViewModel(_albumRepository, _messenger);
        }
        private AlbumDetailViewModel CreateAlbumDetailViewModel()
        {
            return new AlbumDetailViewModel(_albumRepository, _messenger, _fileManager);
        }
        private TagsTabViewModel CreateTagsTabViewModel()
        {
            return new TagsTabViewModel(_messenger);
        }
        private TagListViewModel CreateTagListViewModel()
        {
            return new TagListViewModel(_tagRepository, _messenger);
        }
        private TagDetailViewModel CreateTagDetailViewModel()
        {
            return  new TagDetailViewModel(_tagRepository, _messenger);
        }
    }
}