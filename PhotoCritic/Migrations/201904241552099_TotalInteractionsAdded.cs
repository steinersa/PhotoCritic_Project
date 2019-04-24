namespace PhotoCritic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TotalInteractionsAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "TotalInteractions", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "TotalInteractions");
        }
    }
}
