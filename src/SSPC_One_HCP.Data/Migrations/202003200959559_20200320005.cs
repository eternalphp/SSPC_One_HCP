namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200320005 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.RongCloudContent", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RongCloudContent", "UserId", c => c.String());
        }
    }
}
