using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Gallery.BL;
using IW5Gallery.BL.Models;
using IW5Gallery.BL.Repositories;
using IW5Gallery.DAL;
using IW5Gallery.DAL.Entities;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Gallery.Tests
{
    [CollectionDefinition("DbCollection")]
    public class GalleryDbCollection : ICollectionFixture<TestBase> { }

    [Collection("DbCollection")]
    public class GalleryDbContextTests
    {

        private readonly ImageRepository _imageRepository = new ImageRepository();
        private readonly AlbumRepository _albumRepository = new AlbumRepository();
        private readonly TagRepository _tagRepository = new TagRepository();

        [Fact]
        public void GetAllImages_18Images_true()
        {
            var images = _imageRepository.GetAllImagesSortedByName();
            Assert.True(images.Count == 18);
        }

        [Fact]
        public void GetAllAlbums_1Albums_true()
        {
            ICollection<MiniatureModel> albums = _albumRepository.GetAllAlbums();
            Assert.True(albums.Any());
        }

        [Fact]
        // Test ci dobre cuca many to many vztah Album Image
        public void GetAllAlbumsRaw_2Album_true()
        {
            using (var context = new GalleryContext())
            {
                var albums = context.Albums.ToList();
                Assert.True(albums.Any());
            }
        }

        [Fact]
        // Test ci dobre cuca many to many vztah Album Image
        public void GetImagesRaw_1Image_true()
        {
            using (var context = new GalleryContext())
            {
                var image = context.Images.ToList().FirstOrDefault();
                Assert.True(image != null);
            }
        }


        [Fact]
        // Test ci dobre cuca many to many vztah Album Image
        public void GetPersonTagsRaw_1Tag_true()
        {
            using (var context = new GalleryContext())
            {
                var image = context.Persons.ToList().FirstOrDefault();
                Assert.True(image != null);
            }
        }

        [Fact]
        public void GetImageById_Correct_ImageDetailModel()
        {
            var image = _imageRepository.GetImageById(new Guid("1abdfee1-c970-4afd-aff8-aa3cfef8b1ac"));
            Assert.NotNull(image);
            Assert.Equal("(1)jpg", image.Name);
            Assert.Equal(Format.jpg, image.Format);
            Assert.NotEmpty(image.Albums);
            Assert.NotEmpty(image.Tags);
        }

        [Fact]
        public void GetImagesByName_AllPNGs()
        {
            var images = _imageRepository.GetImagesByName("png");
            Assert.NotEmpty(images);
            Assert.True(images.Count == 9);
        }

        [Fact]
        public void GetImagesByDateTaken_Zero()
        {
            var images = _imageRepository.GetImagesByDateTaken(new DateTime(1992, 4, 2));
            Assert.Empty(images);
        }

        [Fact]
        public void GetImagesByFormat_9JPEGs()
        {
            var images = _imageRepository.GetImagesByFormat(Format.jpg);
            Assert.NotEmpty(images);
            Assert.True(images.Count == 9);
        }

        [Fact]
        public void GetImagesByResolution_All18()
        {
            var images = _imageRepository.GetImagesByResolution(1080, 1920);
            Assert.NotEmpty(images);
            Assert.True(images.Count == 18);
        }

        [Fact]
        public void Simple_InsertUpdateRemoveImage()
        {
            var imageModel = new ImageDetailModel()
            {
                Name = "A Simple Test Image",
                DateAdded = DateTime.Now,
                DateTaken = DateTime.Now.AddDays(-2),
                Format = Format.jpg,
                Height = 1920,
                Width = 1080,
                Path = "..\\..\\..\\Gallery.Tests\\TestImages\\Lake.jpg"
            };

            var image = _imageRepository.InsertImage(imageModel);
            var test = _imageRepository.GetImageById(image.Id);
            Assert.NotNull(test);

            image.Name = "A New Simple Name";
            _imageRepository.UpdateImageInfo(image);
            test = _imageRepository.GetImageById(image.Id);
            Assert.NotNull(test);
            Assert.Equal(image.Name, test.Name);

            _imageRepository.RemoveImage(image.Id);
            test = _imageRepository.GetImageById(image.Id);
            Assert.Null(test);
        }

        [Fact]
        public void CreateAlbum_test()
        {
            var album = new AlbumDetailModel()
            {
                Name = "new Album test",
                CoverPhotoId = null
            };
            album = _albumRepository.InsertAlbum(album);

            var test = _albumRepository.GetAlbumById(album.Id);
            Assert.True(test.Name == "new Album test");

            album.Name = "new Album test update";
            _albumRepository.UpdateAlbumInfo(album);

            test = _albumRepository.GetAlbumById(album.Id);
            Assert.True(test.Name == "new Album test update");

            _albumRepository.RemoveAlbum(album.Id);
            Assert.Null(_albumRepository.GetAlbumById(album.Id));
        }

        [Fact]
        public void InsertImageToAlbum_RemoveImageFromAlbum()
        {
            var imageId = new Guid("aabdfee1-c970-4afd-aff8-aa3cfef8b1ac");
            var albumId = new Guid("ab8db9b3-799c-4ef2-9d85-ce32a9ffa843");

            var image = _imageRepository.GetImageById(imageId);
            Assert.NotNull(image);
            Assert.True(image.Albums.Count == 1);

            var album = _albumRepository.GetAlbumById(albumId);
            Assert.NotNull(album);
            Assert.True(album.Images.Count == 4);

            _albumRepository.AddImageToAlbum(imageId, albumId);

            image = _imageRepository.GetImageById(imageId);
            Assert.True(image.Albums.Count == 2);
            album = _albumRepository.GetAlbumById(albumId);
            Assert.True(album.Images.Count == 5);

            _albumRepository.RemoveImageFromAlbum(imageId, albumId);

            image = _imageRepository.GetImageById(imageId);
            Assert.True(image.Albums.Count == 1);
            album = _albumRepository.GetAlbumById(albumId);
            Assert.True(album.Images.Count == 4);
        }

        [Fact]
        public void AddTagToImage_RemoveTagFromImage()
        {
            var imageId = new Guid("13bdfee1-c970-4afd-aff8-aa3cfef8b1ac");
            var personId = new Guid("7a3aaaaa-799c-4ef2-9d85-ce32a9ffa843");

            var image = _imageRepository.GetImageById(imageId);
            Assert.NotNull(image);
            Assert.True(image.Tags.Count == 0);

            var person = _tagRepository.GetPersonById(personId);
            Assert.NotNull(person);
            Assert.True(person.Images.Count == 3);

            var tag = new TagModel
            {
                Name = "Pepe Master",
                TaggableId = personId,
                TagType = TagType.Person,
                Location = new Location
                {
                    Height = 20,
                    Width = 20,
                    XCoordinate = 50,
                    YCoordinate = 95
                }
            };

            image = _imageRepository.AddTagToImage(tag, imageId);
            Assert.True(image.Tags.Count == 1);
            person = _tagRepository.GetPersonById(personId);
            Assert.True(person.Images.Count == 4);

            image = _imageRepository.RemoveTagFromImage(image.Tags.First(t => t.Name == "Pepe Master").Id);
            Assert.True(image.Tags.Count == 0);
            person = _tagRepository.GetPersonById(personId);
            Assert.True(person.Images.Count == 3);
        }

        [Fact]
        public void RemoveBindedImage()
        {
            var imageModel = new ImageDetailModel()
            {
                Name = "A Simple Test Image",
                DateAdded = DateTime.Now,
                DateTaken = DateTime.Now.AddDays(-2),
                Format = Format.jpg,
                Height = 1920,
                Width = 1080,
                Path = "..\\..\\..\\Gallery.Tests\\TestImages\\Sky.jpg"
            };

            var image = _imageRepository.InsertImage(imageModel);
            var album = _albumRepository.GetAlbumById(new Guid("ab8db9b3-799c-4ef2-9d85-ce32a9ffa843"));
            Assert.NotNull(album);
            var person = _tagRepository.GetPersonById(new Guid("7a3aaaaa-799c-4ef2-9d85-ce32a9ffa843"));
            Assert.NotNull(person);

            _albumRepository.AddImageToAlbum(image.Id, album.Id);
            image = _imageRepository.GetImageById(image.Id);
            Assert.True(image.Albums.Count == 1);
            album = _albumRepository.GetAlbumById(album.Id);
            Assert.True(album.Images.Count == 5);

            var tag = new TagModel
            {
                Name = "Pepe Master",
                TaggableId = person.Id,
                TagType = TagType.Person,
                Location = new Location
                {
                    Height = 20,
                    Width = 20,
                    XCoordinate = 50,
                    YCoordinate = 95
                }
            };
            image = _imageRepository.AddTagToImage(tag, image.Id);
            Assert.True(image.Tags.Count == 1);
            person = _tagRepository.GetPersonById(person.Id);
            Assert.True(person.Images.Count == 4);

            _imageRepository.RemoveImage(image.Id);

            image = _imageRepository.GetImageById(image.Id);
            Assert.Null(image);
            album = _albumRepository.GetAlbumById(album.Id);
            Assert.True(album.Images.Count == 4);
            person = _tagRepository.GetPersonById(person.Id);
            Assert.True(person.Images.Count == 3);
        }

        [Fact]
        public void Create_AddImage_Remove_AlbumDetailModel_test()
        {
            var newAlbum = new AlbumDetailModel
            {
                Id = new Guid(),
                Name = "AlbumDetailModel test",
                CoverPhotoId = new Guid("2abdfee1-c970-4afd-aff8-aa3cfef8b1ac")
            };

            var album = _albumRepository.InsertAlbum(newAlbum);

            Assert.True(album.Name == "AlbumDetailModel test");

            album.Name = "AlbumDetailModel test update";

            _albumRepository.UpdateAlbumInfo(album);

            album = _albumRepository.GetAlbumById(album.Id);
            Assert.True(album.Name == "AlbumDetailModel test update");

            var imageId = new Guid("aabdfee1-c970-4afd-aff8-aa3cfef8b1ac");
            _albumRepository.AddImageToAlbum(imageId, album.Id);

            imageId = new Guid("3abdfee1-c970-4afd-aff8-aa3cfef8b1ac");
            _albumRepository.AddImageToAlbum(imageId, album.Id);

            _albumRepository.RemoveAlbum(album.Id);

            Assert.Null(_albumRepository.GetAlbumById(album.Id));
        }

        [Fact]
        public void GetAllPersonTags()
        {
            var persons = _tagRepository.GetAllPersons();
            Assert.True(persons.Any());
        }

    }
}
