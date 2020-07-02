namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200310001 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeetInfo", "PVCount", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MeetInfo", "PVCount");
        }
    }
}
