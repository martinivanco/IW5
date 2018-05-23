using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Gallery.DAL;
using IW5Gallery.DAL.Entities;

namespace IW5Gallery.DataFilling
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new GalleryContext())
            {
                var kekImage = new Image();
                kekImage.Format = Format.gif;
                kekImage.DateAdded = DateTime.Now;
                kekImage.DateTaken = DateTime.Now;
                kekImage.Height = 1920;
                kekImage.Width = 1080;
                kekImage.Name = "Kekino";
                kekImage.Path = "bam";

                context.Entry(kekImage).State = EntityState.Added;
                context.SaveChanges();

                // context.Albums.Add(new Album());
                //var kek = new Album();
                //kek.Name = "kekekke";

                ////context.Albums.Add(kek);
                ////context.Albums.Create(kek);
                //context.Entry(kek).State = EntityState.Added;
                //context.SaveChanges();
            }
        }
    }
}
