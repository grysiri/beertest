namespace BeerTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Beer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Brewery = c.String(),
                        Country = c.String(),
                        Type = c.String(),
                        Percent = c.Double(nullable: false),
                        Sorting = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Rating",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        BeerID = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Beer", t => t.BeerID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.BeerID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Name = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rating", "UserID", "dbo.User");
            DropForeignKey("dbo.Rating", "BeerID", "dbo.Beer");
            DropIndex("dbo.Rating", new[] { "UserID" });
            DropIndex("dbo.Rating", new[] { "BeerID" });
            DropTable("dbo.User");
            DropTable("dbo.Rating");
            DropTable("dbo.Beer");
        }
    }
}
