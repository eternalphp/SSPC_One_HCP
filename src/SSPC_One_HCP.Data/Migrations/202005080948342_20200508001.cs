namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200508001 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LiveOnline", "Total", c => c.Int(nullable: false));
            DropColumn("dbo.LiveOnline", "Count");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LiveOnline", "Count", c => c.Int(nullable: false));
            DropColumn("dbo.LiveOnline", "Total");
        }
    }
}
