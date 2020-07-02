namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xxx2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionModel", "Sort", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionModel", "Sort");
        }
    }
}
