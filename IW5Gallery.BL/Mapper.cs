using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Gallery.DAL.Entities.Base;

namespace IW5Gallery.BL
{
    using IW5Gallery.DAL.Entities;
    using IW5Gallery.BL.Models;

    public class Mapper
    {
        public MiniatureModel MapImageEntityToMiniatureModel(Image image)
        {
            return new MiniatureModel
            {
                Id = image.Id,
                Name = image.Name,
                ThumbnailPath = image.ThumbnailPath
            };
        }

        public MiniatureModel MapAlbumEntityToMiniatureModel(Album album)
        {
            var thumbnailPath = string.Empty;
            if (album.CoverPhotoId != null)
            {
                thumbnailPath = album.CoverPhoto.ThumbnailPath;
            }

            return new MiniatureModel
            {
                Id = album.Id,
                Name = album.Name,
                ThumbnailPath = thumbnailPath
            };
        }

        public AlbumDetailModel MapAlbumEntityToAlbumDetailModel(Album album)
        {
            var albumImages = album.Images.Select(albumImage => MapImageEntityToMiniatureModel(albumImage.Image)).ToList();

            return new AlbumDetailModel
            {
                Id = album.Id,
                CoverPhotoId = album.CoverPhotoId,
                Name = album.Name,
                Images = albumImages
            };
        }

        public Album MapAlbumDetailModelToAlbumEntity(AlbumDetailModel album)
        {
            return new Album
            {
                Id = album.Id,
                CoverPhotoId = album.CoverPhotoId,
                Name = album.Name
            };
        }

        public ImageDetailModel MapImageEntityToImageDetailModel(Image image)
        {
            var imageAlbums = image.Albums.Select(a => MapAlbumEntityToMiniatureModel(a.Album)).ToList();
            var imageTags = image.Tags.Select(MapTagEntityToTagModel).ToList();

            return new ImageDetailModel
            {
                Id = image.Id,
                Albums = imageAlbums,
                DateAdded = image.DateAdded,
                DateTaken = image.DateTaken,
                Format = image.Format,
                Height = image.Height,
                Name = image.Name,
                Note = image.Note,
                Tags = imageTags,
                Path = image.Path,
                Width = image.Width
            };
        }

        public Image MapImageDetailModelToImageEntity(ImageDetailModel imageDetail)
        {
            return new Image
            {
                Id = imageDetail.Id,
                DateAdded = imageDetail.DateAdded,
                DateTaken = imageDetail.DateTaken,
                Format = imageDetail.Format,
                Height = imageDetail.Height,
                Name = imageDetail.Name,
                Note = imageDetail.Note,
                Path = imageDetail.Path,
                Width = imageDetail.Width
            };
        }

        public Person MapPersonDetailModelToPersonEntity(PersonDetailModel detail)
        {
            return new Person
            {
                Id = detail.Id,
                Name = detail.Name,
                Surname = detail.Surname
            };
        }

        public PersonDetailModel MapPersonEntityToPersonDetailModel(Person entity)
        {
            var images = entity.Tags.Select(t => MapImageEntityToMiniatureModel(t.Image)).ToList();
            return new PersonDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Surname = entity.Surname,
                Images = images
            };
        }

        public Thing MapThingDetailModelToThingEntity(ThingDetailModel detail)
        {
            return new Thing
            {
                Id = detail.Id,
                Name = detail.Name
            };
        }

        public ThingDetailModel MapThingEntityToThingDetailModel(Thing entity)
        {
            var images = entity.Tags.Select(t => MapImageEntityToMiniatureModel(t.Image)).ToList();
            return new ThingDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Images = images
            };
        }

        public MiniatureModel MapPersonEntityToMiniatureModel(Person entity)
        {
            return new MiniatureModel
            {
                Id = entity.Id,
                Name = entity.Name + " " + entity.Surname,
                ThumbnailPath = string.Empty
            };
        }

        public MiniatureModel MapThingEntityToMiniatureModel(Thing entity)
        {
            return new MiniatureModel
            {
                Id = entity.Id,
                Name = entity.Name,
                ThumbnailPath = string.Empty
            };
        }

        public TagModel MapTagEntityToTagModel(Tag tag)
        {
            var name = string.Empty;
            switch (tag.Taggable)
            {
                case Person person:
                    name = person.Name + " " + person.Surname;
                    break;
                case Thing thing:
                    name = thing.Name;
                    break;
            }

            return new TagModel
            {
                Id = tag.Id,
                Location = tag.Location,
                Name = name,
                TaggableId = tag.TaggableId,
                TagType = tag.TagType
            };
        }

        public Tag MapTagModelToTagEntity(TagModel tag)
        {
            return new Tag
            {
                Id = tag.Id,
                LocationId = tag.Location.Id,
                TaggableId = tag.TaggableId,
                TagType = tag.TagType
            };
        }
    }
}
