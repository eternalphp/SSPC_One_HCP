namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200306001 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PneumoniaBotOperationRecord", "ClickTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.PneumoniaBotOperationRecord", "LeaveTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.PneumoniaBotOperationRecord", "ClickingTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PneumoniaBotOperationRecord", "ClickingTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.PneumoniaBotOperationRecord", "LeaveTime");
            DropColumn("dbo.PneumoniaBotOperationRecord", "ClickTime");
        }
    }
}
