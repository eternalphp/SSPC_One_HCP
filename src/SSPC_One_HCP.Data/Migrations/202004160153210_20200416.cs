namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200416 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeetInfo", "LivePicture", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MeetInfo", "LivePicture");
        }
    }
}
