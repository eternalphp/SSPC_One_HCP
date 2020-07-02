namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200518 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeetAndProAndDepRelation", "DepartmentType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MeetAndProAndDepRelation", "DepartmentType");
        }
    }
}
