namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200403 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeetSchedule", "Hospital", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MeetSchedule", "Hospital");
        }
    }
}
