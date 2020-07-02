namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200303001 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeetInfo", "WithinExternalType", c => c.Int(nullable: false));
            AddColumn("dbo.MeetInfo", "WatchType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MeetInfo", "WatchType");
            DropColumn("dbo.MeetInfo", "WithinExternalType");
        }
    }
}
