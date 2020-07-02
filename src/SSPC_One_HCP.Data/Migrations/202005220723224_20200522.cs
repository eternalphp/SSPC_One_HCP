namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200522 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DoctorModel", "Age", c => c.String());
            AddColumn("dbo.DoctorModel", "IsBasicLevel", c => c.Int());
            AlterColumn("dbo.DoctorModel", "Gender", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DoctorModel", "Gender", c => c.String(maxLength: 5));
            DropColumn("dbo.DoctorModel", "IsBasicLevel");
            DropColumn("dbo.DoctorModel", "Age");
        }
    }
}
