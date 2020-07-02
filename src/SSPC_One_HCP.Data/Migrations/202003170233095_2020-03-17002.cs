namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200317002 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RongCloudContent", "WxName", c => c.String());
            AddColumn("dbo.RongCloudContent", "SignIn", c => c.String());
            AddColumn("dbo.RongCloudContent", "HospitalName", c => c.String());
            AddColumn("dbo.RongCloudContent", "Audit", c => c.Int(nullable: false));
            AddColumn("dbo.RongCloudContent", "Reason", c => c.String());
            AddColumn("dbo.RongCloudContent", "ReasonDateTime", c => c.DateTime());
            AddColumn("dbo.RongCloudContent", "ReasonUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RongCloudContent", "ReasonUser");
            DropColumn("dbo.RongCloudContent", "ReasonDateTime");
            DropColumn("dbo.RongCloudContent", "Reason");
            DropColumn("dbo.RongCloudContent", "Audit");
            DropColumn("dbo.RongCloudContent", "HospitalName");
            DropColumn("dbo.RongCloudContent", "SignIn");
            DropColumn("dbo.RongCloudContent", "WxName");
        }
    }
}
