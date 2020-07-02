namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200410 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeetQAModel", "Sort", c => c.Int());
            DropColumn("dbo.QuestionModel", "Sort");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuestionModel", "Sort", c => c.Int());
            DropColumn("dbo.MeetQAModel", "Sort");
        }
    }
}
