namespace SiteServer.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PVSite : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PVs", "Site", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PVs", "Site");
        }
    }
}
