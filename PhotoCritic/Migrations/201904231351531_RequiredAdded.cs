namespace PhotoCritic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredAdded : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Photos", "ImageName", c => c.String(nullable: false));
            AlterColumn("dbo.Photos", "ImagePath", c => c.String(nullable: false));
            AlterColumn("dbo.Photos", "Category", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Photos", "Category", c => c.String());
            AlterColumn("dbo.Photos", "ImagePath", c => c.String());
            AlterColumn("dbo.Photos", "ImageName", c => c.String());
        }
    }
}
