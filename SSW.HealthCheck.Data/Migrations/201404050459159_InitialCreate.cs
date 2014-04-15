namespace SSW.HealthCheck.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientApplications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 500),
                        DateCreated = c.DateTime(nullable: false),
                        UserCreated = c.String(nullable: false, maxLength: 200),
                        DateModified = c.DateTime(nullable: false),
                        UserModified = c.String(nullable: false, maxLength: 200),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TestRuns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TestId = c.Int(nullable: false),
                        TestResult = c.Int(nullable: false),
                        Message = c.String(nullable: false),
                        RunningTime = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        UserCreated = c.String(nullable: false, maxLength: 200),
                        DateModified = c.DateTime(nullable: false),
                        UserModified = c.String(nullable: false, maxLength: 200),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tests", t => t.TestId, cascadeDelete: true)
                .Index(t => t.TestId);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientApplicationId = c.Int(nullable: false),
                        Key = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        ShortDescription = c.String(maxLength: 500),
                        Name = c.String(nullable: false, maxLength: 500),
                        DateCreated = c.DateTime(nullable: false),
                        UserCreated = c.String(nullable: false, maxLength: 200),
                        DateModified = c.DateTime(nullable: false),
                        UserModified = c.String(nullable: false, maxLength: 200),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientApplications", t => t.ClientApplicationId)
                .Index(t => t.ClientApplicationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestRuns", "TestId", "dbo.Tests");
            DropForeignKey("dbo.Tests", "ClientApplicationId", "dbo.ClientApplications");
            DropIndex("dbo.Tests", new[] { "ClientApplicationId" });
            DropIndex("dbo.TestRuns", new[] { "TestId" });
            DropTable("dbo.Tests");
            DropTable("dbo.TestRuns");
            DropTable("dbo.ClientApplications");
        }
    }
}
