namespace PhotoCritic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelsEstablished : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Educations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IncomeLevels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MaritalStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OpinionatedIndividualPhotoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LikeDislike = c.Boolean(nullable: false),
                        Comment = c.String(),
                        Reason1 = c.String(),
                        Reason2 = c.String(),
                        PhotoId = c.Int(nullable: false),
                        OpinionatedIndividualId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OpinionatedIndividuals", t => t.OpinionatedIndividualId, cascadeDelete: true)
                .ForeignKey("dbo.Photos", t => t.PhotoId, cascadeDelete: true)
                .Index(t => t.PhotoId)
                .Index(t => t.OpinionatedIndividualId);
            
            CreateTable(
                "dbo.OpinionatedIndividuals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AgeId = c.Int(nullable: false),
                        SexId = c.Int(nullable: false),
                        RaceId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                        EducationId = c.Int(nullable: false),
                        ProfessionId = c.Int(nullable: false),
                        MaritalStatusId = c.Int(nullable: false),
                        IncomeLevelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ages", t => t.AgeId, cascadeDelete: true)
                .ForeignKey("dbo.Educations", t => t.EducationId, cascadeDelete: true)
                .ForeignKey("dbo.IncomeLevels", t => t.IncomeLevelId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.MaritalStatus", t => t.MaritalStatusId, cascadeDelete: true)
                .ForeignKey("dbo.Professions", t => t.ProfessionId, cascadeDelete: true)
                .ForeignKey("dbo.Races", t => t.RaceId, cascadeDelete: true)
                .ForeignKey("dbo.Sexes", t => t.SexId, cascadeDelete: true)
                .Index(t => t.AgeId)
                .Index(t => t.SexId)
                .Index(t => t.RaceId)
                .Index(t => t.LocationId)
                .Index(t => t.EducationId)
                .Index(t => t.ProfessionId)
                .Index(t => t.MaritalStatusId)
                .Index(t => t.IncomeLevelId);
            
            CreateTable(
                "dbo.Professions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Races",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sexes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageName = c.String(),
                        ImagePath = c.String(),
                        Category = c.String(),
                        Hidden = c.Boolean(nullable: false),
                        CommentsEnabled = c.Boolean(nullable: false),
                        TotalLikes = c.Int(),
                        TotalDislikes = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Uploaders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Uploaders", "ApplicationId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.OpinionatedIndividualPhotoes", "PhotoId", "dbo.Photos");
            DropForeignKey("dbo.OpinionatedIndividualPhotoes", "OpinionatedIndividualId", "dbo.OpinionatedIndividuals");
            DropForeignKey("dbo.OpinionatedIndividuals", "SexId", "dbo.Sexes");
            DropForeignKey("dbo.OpinionatedIndividuals", "RaceId", "dbo.Races");
            DropForeignKey("dbo.OpinionatedIndividuals", "ProfessionId", "dbo.Professions");
            DropForeignKey("dbo.OpinionatedIndividuals", "MaritalStatusId", "dbo.MaritalStatus");
            DropForeignKey("dbo.OpinionatedIndividuals", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.OpinionatedIndividuals", "IncomeLevelId", "dbo.IncomeLevels");
            DropForeignKey("dbo.OpinionatedIndividuals", "EducationId", "dbo.Educations");
            DropForeignKey("dbo.OpinionatedIndividuals", "AgeId", "dbo.Ages");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Uploaders", new[] { "ApplicationId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.OpinionatedIndividuals", new[] { "IncomeLevelId" });
            DropIndex("dbo.OpinionatedIndividuals", new[] { "MaritalStatusId" });
            DropIndex("dbo.OpinionatedIndividuals", new[] { "ProfessionId" });
            DropIndex("dbo.OpinionatedIndividuals", new[] { "EducationId" });
            DropIndex("dbo.OpinionatedIndividuals", new[] { "LocationId" });
            DropIndex("dbo.OpinionatedIndividuals", new[] { "RaceId" });
            DropIndex("dbo.OpinionatedIndividuals", new[] { "SexId" });
            DropIndex("dbo.OpinionatedIndividuals", new[] { "AgeId" });
            DropIndex("dbo.OpinionatedIndividualPhotoes", new[] { "OpinionatedIndividualId" });
            DropIndex("dbo.OpinionatedIndividualPhotoes", new[] { "PhotoId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Uploaders");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Photos");
            DropTable("dbo.Sexes");
            DropTable("dbo.Races");
            DropTable("dbo.Professions");
            DropTable("dbo.OpinionatedIndividuals");
            DropTable("dbo.OpinionatedIndividualPhotoes");
            DropTable("dbo.MaritalStatus");
            DropTable("dbo.Locations");
            DropTable("dbo.IncomeLevels");
            DropTable("dbo.Educations");
            DropTable("dbo.Ages");
        }
    }
}
