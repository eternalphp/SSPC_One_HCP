namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200306002 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PneumoniaBotOperationRecord", "LeaveTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PneumoniaBotOperationRecord", "LeaveTime", c => c.DateTime(nullable: false));
        }
    }
}
