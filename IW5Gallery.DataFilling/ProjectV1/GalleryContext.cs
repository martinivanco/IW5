using IW5Gallery.DAL.Entities;
using IW5Gallery.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Gallery.DAL
{
    public class GalleryContext : DbContext
    {
        public DbSet<Image> Images { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<PersonTag> PersonTags { get; set; }
        public DbSet<ThingTag> ThingTags { get; set; }

        public GalleryContext() : base("GalleryContex")
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<GalleryContext>());
        }

        //public void DropDB()
        //{
        //    using (var context = new GalleryContext())
        //    {
        //        context.Database.ExecuteSqlCommand("TRUNCATE TABLE Albums");

        //    }
        //}

        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Images");
            });

            modelBuilder.Entity<Album>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Albums");
            });

            modelBuilder.Entity<PersonTag>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("PersonTags");
            });

            modelBuilder.Entity<ThingTag>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("ThingTags");
            });
        }*/
    }
}
