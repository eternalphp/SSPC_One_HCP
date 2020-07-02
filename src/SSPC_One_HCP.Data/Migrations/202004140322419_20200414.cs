namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200414 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeetSubscribe", "TemplateId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MeetSubscribe", "TemplateId");
        }
    }
}
