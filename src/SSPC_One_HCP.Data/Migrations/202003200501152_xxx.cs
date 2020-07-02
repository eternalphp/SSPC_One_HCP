namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xxx : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RongCloudContent", "ToUserId", c => c.String());
            AddColumn("dbo.RongCloudContent", "ObjectName", c => c.String());
            AddColumn("dbo.RongCloudContent", "MsgUID", c => c.String());
            AddColumn("dbo.RongCloudContent", "SensitiveType", c => c.String());
            AddColumn("dbo.RongCloudContent", "Source", c => c.String());
            AddColumn("dbo.RongCloudContent", "GroupUserIds", c => c.String());
            AddColumn("dbo.RongCloudContent", "WxPicture", c => c.String());
            DropColumn("dbo.RongCloudContent", "AppKey");
            DropColumn("dbo.RongCloudContent", "TargetId");
            DropColumn("dbo.RongCloudContent", "MsgType");
            DropColumn("dbo.RongCloudContent", "MessageId");
            DropColumn("dbo.RongCloudContent", "SignIn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RongCloudContent", "SignIn", c => c.String());
            AddColumn("dbo.RongCloudContent", "MessageId", c => c.String());
            AddColumn("dbo.RongCloudContent", "MsgType", c => c.String());
            AddColumn("dbo.RongCloudContent", "TargetId", c => c.String());
            AddColumn("dbo.RongCloudContent", "AppKey", c => c.String());
            DropColumn("dbo.RongCloudContent", "WxPicture");
            DropColumn("dbo.RongCloudContent", "GroupUserIds");
            DropColumn("dbo.RongCloudContent", "Source");
            DropColumn("dbo.RongCloudContent", "SensitiveType");
            DropColumn("dbo.RongCloudContent", "MsgUID");
            DropColumn("dbo.RongCloudContent", "ObjectName");
            DropColumn("dbo.RongCloudContent", "ToUserId");
        }
    }
}
