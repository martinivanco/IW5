namespace IW5Gallery.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GalleryDB_V3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlbumImages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AlbumId = c.Guid(nullable: false),
                        ImageId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Images", t => t.ImageId, cascadeDelete: true)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .Index(t => t.AlbumId)
                .Index(t => t.ImageId);
            
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        CoverPhotoId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Images", t => t.CoverPhotoId)
                .Index(t => t.CoverPhotoId);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        DateTaken = c.DateTime(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        Format = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Note = c.String(),
                        Path = c.String(nullable: false),
                        ThumbnailPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TaggableId = c.Guid(nullable: false),
                        TagType = c.Int(nullable: false),
                        LocationId = c.Guid(nullable: false),
                        ImageId = c.Guid(nullable: false),
                        Person_Id = c.Guid(),
                        Thing_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Images", t => t.ImageId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.TaggableBases", t => t.Person_Id)
                .ForeignKey("dbo.TaggableBases", t => t.Thing_Id)
                .ForeignKey("dbo.TaggableBases", t => t.TaggableId, cascadeDelete: true)
                .Index(t => t.TaggableId)
                .Index(t => t.LocationId)
                .Index(t => t.ImageId)
                .Index(t => t.Person_Id)
                .Index(t => t.Thing_Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        XCoordinate = c.Int(nullable: false),
                        YCoordinate = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaggableBases",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Surname = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AlbumImages", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.Albums", "CoverPhotoId", "dbo.Images");
            DropForeignKey("dbo.Tags", "TaggableId", "dbo.TaggableBases");
            DropForeignKey("dbo.Tags", "Thing_Id", "dbo.TaggableBases");
            DropForeignKey("dbo.Tags", "Person_Id", "dbo.TaggableBases");
            DropForeignKey("dbo.Tags", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Tags", "ImageId", "dbo.Images");
            DropForeignKey("dbo.AlbumImages", "ImageId", "dbo.Images");
            DropIndex("dbo.Tags", new[] { "Thing_Id" });
            DropIndex("dbo.Tags", new[] { "Person_Id" });
            DropIndex("dbo.Tags", new[] { "ImageId" });
            DropIndex("dbo.Tags", new[] { "LocationId" });
            DropIndex("dbo.Tags", new[] { "TaggableId" });
            DropIndex("dbo.Albums", new[] { "CoverPhotoId" });
            DropIndex("dbo.AlbumImages", new[] { "ImageId" });
            DropIndex("dbo.AlbumImages", new[] { "AlbumId" });
            DropTable("dbo.TaggableBases");
            DropTable("dbo.Locations");
            DropTable("dbo.Tags");
            DropTable("dbo.Images");
            DropTable("dbo.Albums");
            DropTable("dbo.AlbumImages");
        }
    }
}
