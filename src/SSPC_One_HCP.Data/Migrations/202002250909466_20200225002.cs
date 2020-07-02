namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200225002 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PneumoniaBotOperationRecord", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PneumoniaBotOperationRecord", "UserId");
        }
    }
}
