namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200318004 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MeetInfo", "IsForbiddenWords", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MeetInfo", "IsForbiddenWords", c => c.Int());
        }
    }
}
