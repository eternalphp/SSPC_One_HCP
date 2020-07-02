namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200317001 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BotADWhiteName", "ChatAudit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BotADWhiteName", "ChatAudit");
        }
    }
}
