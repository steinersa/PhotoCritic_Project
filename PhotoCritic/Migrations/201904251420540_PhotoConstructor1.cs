namespace PhotoCritic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotoConstructor1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Photos", "TotalLikes", c => c.Int(nullable: false));
            AlterColumn("dbo.Photos", "TotalDislikes", c => c.Int(nullable: false));
            AlterColumn("dbo.Photos", "TotalInteractions", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Photos", "TotalInteractions", c => c.Int());
            AlterColumn("dbo.Photos", "TotalDislikes", c => c.Int());
            AlterColumn("dbo.Photos", "TotalLikes", c => c.Int());
        }
    }
}
