namespace PhotoCritic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryModelCreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Photos", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Photos", "CategoryId");
            AddForeignKey("dbo.Photos", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
            DropColumn("dbo.Photos", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Photos", "Category", c => c.String(nullable: false));
            DropForeignKey("dbo.Photos", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Photos", new[] { "CategoryId" });
            DropColumn("dbo.Photos", "CategoryId");
            DropTable("dbo.Categories");
        }
    }
}
