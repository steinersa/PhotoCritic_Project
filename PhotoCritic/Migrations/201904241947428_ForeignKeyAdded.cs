namespace PhotoCritic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeyAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OpinionatedIndividuals", "ApplicationId", c => c.String(maxLength: 128));
            CreateIndex("dbo.OpinionatedIndividuals", "ApplicationId");
            AddForeignKey("dbo.OpinionatedIndividuals", "ApplicationId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OpinionatedIndividuals", "ApplicationId", "dbo.AspNetUsers");
            DropIndex("dbo.OpinionatedIndividuals", new[] { "ApplicationId" });
            DropColumn("dbo.OpinionatedIndividuals", "ApplicationId");
        }
    }
}
