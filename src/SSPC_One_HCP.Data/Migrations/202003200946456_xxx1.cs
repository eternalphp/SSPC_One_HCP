namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xxx1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RongCloudContent", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RongCloudContent", "UserId");
        }
    }
}
