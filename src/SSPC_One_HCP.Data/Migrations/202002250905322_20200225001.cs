namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200225001 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PneumoniaBotOperationRecord", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.PneumoniaBotOperationRecord", "IslogIn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PneumoniaBotOperationRecord", "IslogIn", c => c.Int(nullable: false));
            DropColumn("dbo.PneumoniaBotOperationRecord", "Type");
        }
    }
}
