namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200421 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BannerInfo", "BannerIcon", c => c.String());
            AddColumn("dbo.BannerInfo", "Ascription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BannerInfo", "Ascription");
            DropColumn("dbo.BannerInfo", "BannerIcon");
        }
    }
}
