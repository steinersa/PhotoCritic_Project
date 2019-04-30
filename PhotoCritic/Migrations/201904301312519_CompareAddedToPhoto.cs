namespace PhotoCritic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompareAddedToPhoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "Compare", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "Compare");
        }
    }
}
