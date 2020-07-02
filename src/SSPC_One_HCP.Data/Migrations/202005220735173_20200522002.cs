namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200522002 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DoctorModel", "RegistrationAge", c => c.String());
            AddColumn("dbo.DoctorModel", "RegistrationGender", c => c.String());
            AddColumn("dbo.DoctorModel", "RegistrationIsBasicLevel", c => c.Int());
            AlterColumn("dbo.DoctorModel", "gender", c => c.String(maxLength: 5));
            DropColumn("dbo.DoctorModel", "Age");
            DropColumn("dbo.DoctorModel", "IsBasicLevel");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DoctorModel", "IsBasicLevel", c => c.Int());
            AddColumn("dbo.DoctorModel", "Age", c => c.String());
            AlterColumn("dbo.DoctorModel", "gender", c => c.String());
            DropColumn("dbo.DoctorModel", "RegistrationIsBasicLevel");
            DropColumn("dbo.DoctorModel", "RegistrationGender");
            DropColumn("dbo.DoctorModel", "RegistrationAge");
        }
    }
}
