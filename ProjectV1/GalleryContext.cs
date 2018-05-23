using IW5Gallery.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Gallery.DAL.Entities;

namespace IW5Gallery.DAL
{
    public class GalleryContext : DbContext
    {
        public IDbSet<Image> Images { get; set; }
        public IDbSet<Album> Albums { get; set; }
        public IDbSet<Person> Persons { get; set; }
        public IDbSet<Thing> Things { get; set; }
        public IDbSet<AlbumImage> AlbumImages { get; set; }
        public IDbSet<Tag> Tags { get; set; }
        public IDbSet<Location> Locations { get; set; }

        public GalleryContext() : base("GalleryContext")
        {
        }
    }
}
