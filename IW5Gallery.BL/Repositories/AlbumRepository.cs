using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Gallery.BL.Models;
using IW5Gallery.DAL;
using IW5Gallery.DAL.Entities;

namespace IW5Gallery.BL.Repositories
{
    public class AlbumRepository
    {
        private readonly Mapper _mapper = new Mapper();

        public AlbumDetailModel GetAlbumById(Guid id)
        {
            using (var context = new GalleryContext())
            {
                var albumEntity = context.Albums.Include(a => a.Images.Select(i => i.Image))
                    .FirstOrDefault(a => a.Id == id);

                return albumEntity == null ? null : _mapper.MapAlbumEntityToAlbumDetailModel(albumEntity);
            }
        }

        public bool ContainsAlbumName(string name)
        {
            using (var context = new GalleryContext())
            {
                var result = context.Albums.FirstOrDefault(x => x.Name.Equals(name));
                return result != null;
            }
        }

        public List<MiniatureModel> GetAllAlbums()
        {
            using (var context = new GalleryContext())
            {
                return context.Albums.Select(_mapper.MapAlbumEntityToMiniatureModel).ToList();
            }
        }

        public AlbumDetailModel InsertAlbum(AlbumDetailModel detail)
        {
            using (var context = new GalleryContext())
            {
                var entity = _mapper.MapAlbumDetailModelToAlbumEntity(detail);

                entity.Id = Guid.NewGuid();

                context.Albums.AddOrUpdate(entity);
                context.SaveChanges();

                return _mapper.MapAlbumEntityToAlbumDetailModel(entity);
            }
        }

        public void RemoveAlbum(Guid id)
        {
            using (var context = new GalleryContext())
            {
                var album = context.Albums.FirstOrDefault(a => a.Id == id);
                if (album == null)
                    return;

                album.Images.ToList().ForEach(p => context.AlbumImages.Remove(p));
                context.Entry(album).State = EntityState.Deleted;

                context.SaveChanges();
            }
        }

        public void UpdateAlbumInfo(AlbumDetailModel detail)
        {
            using (var context = new GalleryContext())
            {
                var entity = context.Albums.First(a => a.Id == detail.Id);

                entity.Name = detail.Name;
                entity.CoverPhotoId = detail.CoverPhotoId;

                context.SaveChanges();
            }
        }

        public void AddImageToAlbum(Guid imageId, Guid albumId)
        {
            using (var context = new GalleryContext())
            {
                var album = context.Albums.FirstOrDefault(a => a.Id == albumId);
                var image = context.Images.FirstOrDefault(i => i.Id == imageId);
                if (album == null || image == null)
                    return;

                album.Images.Add(new AlbumImage()
                {
                    AlbumId = album.Id,
                    ImageId = image.Id
                });

                context.SaveChanges();
            }
        }

        public void RemoveImageFromAlbum(Guid imageId, Guid albumId)
        {
            using (var context = new GalleryContext())
            {
                var album = context.Albums.FirstOrDefault(a => a.Id == albumId);
                if (album == null)
                    return;

                foreach (var image in album.Images)
                {
                    if (image.ImageId == imageId)
                    {
                        context.AlbumImages.Remove(image);
                        break;
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
