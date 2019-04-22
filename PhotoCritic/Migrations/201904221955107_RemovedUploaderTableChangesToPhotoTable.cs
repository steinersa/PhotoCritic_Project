namespace PhotoCritic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedUploaderTableChangesToPhotoTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Uploaders", "ApplicationId", "dbo.AspNetUsers");
            DropIndex("dbo.Uploaders", new[] { "ApplicationId" });
            AddColumn("dbo.Photos", "ApplicationId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Photos", "ApplicationId");
            AddForeignKey("dbo.Photos", "ApplicationId", "dbo.AspNetUsers", "Id");
            DropTable("dbo.Uploaders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Uploaders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Photos", "ApplicationId", "dbo.AspNetUsers");
            DropIndex("dbo.Photos", new[] { "ApplicationId" });
            DropColumn("dbo.Photos", "ApplicationId");
            CreateIndex("dbo.Uploaders", "ApplicationId");
            AddForeignKey("dbo.Uploaders", "ApplicationId", "dbo.AspNetUsers", "Id");
        }
    }
}
