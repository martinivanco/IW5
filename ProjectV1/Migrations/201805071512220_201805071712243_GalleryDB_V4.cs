namespace IW5Gallery.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201805071712243_GalleryDB_V4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Locations", "XCoordinate", c => c.Double(nullable: false));
            AlterColumn("dbo.Locations", "YCoordinate", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Locations", "YCoordinate", c => c.Int(nullable: false));
            AlterColumn("dbo.Locations", "XCoordinate", c => c.Int(nullable: false));
        }
    }
}
