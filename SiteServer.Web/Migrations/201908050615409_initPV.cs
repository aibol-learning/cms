namespace SiteServer.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initPV : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PVs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PVs");
        }
    }
}
