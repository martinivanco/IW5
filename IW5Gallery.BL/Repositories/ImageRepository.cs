using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Gallery.BL.Models;
using IW5Gallery.DAL;
using IW5Gallery.DAL.Entities;
using IW5Gallery.DAL.Entities.Base;
using Image = IW5Gallery.DAL.Entities.Image;

namespace IW5Gallery.BL.Repositories
{
    public class ImageRepository
    {
        private readonly Mapper _mapper = new Mapper();

        public ImageDetailModel GetImageById(Guid id)
        {
            using (var context = new GalleryContext())
            {
                var imageEntity = context.Images.Include(i => i.Albums.Select(a => a.Album))
                    .Include(i => i.Tags).FirstOrDefault(i => i.Id == id);

                return imageEntity == null ? null : _mapper.MapImageEntityToImageDetailModel(imageEntity);
            }
        }

        public List<MiniatureModel> GetImagesByName(string name)
        {
            using (var context = new GalleryContext())
            {
                return context.Images.Select(_mapper.MapImageEntityToMiniatureModel)
                    .Where(i => i.Name.Contains(name)).ToList();
            }
        }

        public List<MiniatureModel> GetImagesByDateTaken(DateTime date)
        {
            using (var context = new GalleryContext())
            {
                return context.Images.Where(i => i.DateTaken <= date).AsEnumerable()
                    .Select(_mapper.MapImageEntityToMiniatureModel).ToList();
            }
        }

        public MiniatureModel GetImageMiniatureById(Guid id)
        {
            using (var context = new GalleryContext())
            {
                var imageEntity = context.Images.Include(i => i.Albums.Select(a => a.Album))
                    .Include(i => i.Tags).FirstOrDefault(i => i.Id == id);

                return imageEntity == null ? null : _mapper.MapImageEntityToMiniatureModel(imageEntity);
            }
        }

        public List<MiniatureModel> GetImagesByFormat(Format imageFormat)
        {
            using (var context = new GalleryContext())
            {
                return context.Images.Where(i => i.Format == imageFormat).AsEnumerable()
                    .Select(_mapper.MapImageEntityToMiniatureModel).ToList();
            }
        }

        public List<MiniatureModel> GetImagesByResolution(int width, int height)
        {
            using (var context = new GalleryContext())
            {
                return context.Images.Where(i => i.Width >= width && i.Height >= height).AsEnumerable()
                    .Select(_mapper.MapImageEntityToMiniatureModel).ToList();
            }
        }

        public List<MiniatureModel> GetAllImagesSortedByName()
        {
            using (var context = new GalleryContext())
            {
                return context.Images.Select(_mapper.MapImageEntityToMiniatureModel)
                    .OrderBy(i => i.Name).ToList();
            }
        }

        public List<MiniatureModel> GetAllImagesSortedByDate()
        {
            using (var context = new GalleryContext())
            {
                return context.Images.OrderBy(i => i.DateTaken)
                    .Select(_mapper.MapImageEntityToMiniatureModel).ToList();
            }
        }

        public ImageDetailModel InsertImage(ImageDetailModel image)
        {
            using (var context = new GalleryContext())
            {
                var imageEntity = _mapper.MapImageDetailModelToImageEntity(image);
                imageEntity.Id = Guid.NewGuid();
                imageEntity.ThumbnailPath = CreateThumbnail(imageEntity);
                context.Images.Add(imageEntity);
                context.SaveChanges();

                return _mapper.MapImageEntityToImageDetailModel(imageEntity);
            }
        }

        public void UpdateImageInfo(ImageDetailModel image)
        {
            using (var context = new GalleryContext())
            {
                var imageEntity = context.Images.FirstOrDefault(i => i.Id == image.Id);
                if (imageEntity == null) return;

                imageEntity.Name = image.Name;
                imageEntity.DateTaken = image.DateTaken;
                imageEntity.Note = image.Note;

                context.SaveChanges();
            }
        }

        public ImageDetailModel AddTagToImage(TagModel tag, Guid imageId)
        {
            using (var context = new GalleryContext())
            {
                if (tag.TagType == TagType.Person)
                {
                    var image = context.Images.FirstOrDefault(i => i.Id == imageId);
                    var person = context.Persons.FirstOrDefault(p => p.Id == tag.TaggableId);
                    if (person == null || image == null) return null;

                    tag.Location.Id = Guid.NewGuid();
                    context.Locations.Add(tag.Location);

                    var tagEntity = _mapper.MapTagModelToTagEntity(tag);
                    tagEntity.Id = Guid.NewGuid();
                    tagEntity.ImageId = imageId;
                    person.Tags.Add(tagEntity);
                    context.SaveChanges();
                }
                else
                {
                    var image = context.Images.FirstOrDefault(i => i.Id == imageId);
                    var thing = context.Things.FirstOrDefault(t => t.Id == tag.TaggableId);
                    if (thing == null || image == null) return null;

                    tag.Location.Id = Guid.NewGuid();
                    context.Locations.Add(tag.Location);

                    var tagEntity = _mapper.MapTagModelToTagEntity(tag);
                    tagEntity.Id = Guid.NewGuid();
                    tagEntity.ImageId = imageId;
                    thing.Tags.Add(tagEntity);
                    context.SaveChanges();
                }

                return GetImageById(imageId);
            }
        }

        public ImageDetailModel RemoveTagFromImage(Guid tagId)
        {
            using (var context = new GalleryContext())
            {
                var tag = context.Tags.FirstOrDefault(t => t.Id == tagId);
                if (tag == null) return null;
                var location = context.Locations.FirstOrDefault(l => l.Id == tag.LocationId);
                var imageId = tag.ImageId;

                context.Tags.Remove(tag);
                context.Locations.Remove(location);
                context.SaveChanges();

                return GetImageById(imageId);
            }
        }

        public void RemoveImage(Guid id)
        {
            using (var context = new GalleryContext())
            {
                var imageEntity = context.Images.Include(i => i.Albums).FirstOrDefault(i => i.Id == id);
                if (imageEntity == null) return;

                context.Albums.Where(a => a.CoverPhotoId == id).ToList().ForEach(a => a.CoverPhotoId = null);
                imageEntity.Albums.ToList().ForEach(a => context.AlbumImages.Remove(a));

                var locations = imageEntity.Tags.Select(t => t.Location).ToList();
                imageEntity.Tags.ToList().ForEach(t => context.Tags.Remove(t));
                locations.ForEach(l => context.Locations.Remove(l));

                context.Images.Remove(imageEntity);
                context.SaveChanges();
            }
        }

        private string CreateThumbnail(Image image)
        {
            var originalImage = new Bitmap(image.Path);

            int thumbnailHeight = 120, thumbnailWidth = 120;
            if (originalImage.Width > originalImage.Height)
                thumbnailHeight = (int) (originalImage.Height * (120.0 / originalImage.Width));
            else
                thumbnailWidth = (int) (originalImage.Width * (120.0 / originalImage.Height));

            var thumbnail = new Bitmap(thumbnailWidth, thumbnailHeight);
            var graph = Graphics.FromImage(thumbnail);
            graph.DrawImage(originalImage, 0, 0, thumbnailWidth, thumbnailHeight);

            var thumbnailPath = Path.Combine(AppContext.BaseDirectory, "thumbnails", image.Id + ".jpg");
            var directoryInfo = (new FileInfo(thumbnailPath)).Directory;
            directoryInfo?.Create();
            using (var memory = new MemoryStream())
            {
                using (var fs = new FileStream(thumbnailPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    thumbnail.Save(memory, ImageFormat.Jpeg);
                    var bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            return thumbnailPath;
        }


    }
}

