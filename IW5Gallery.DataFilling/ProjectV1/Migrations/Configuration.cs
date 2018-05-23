using System.Collections.Generic;
using IW5Gallery.DAL.Entities;

namespace IW5Gallery.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IW5Gallery.DAL.GalleryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IW5Gallery.DAL.GalleryContext context)
        {

        }
    }
}
