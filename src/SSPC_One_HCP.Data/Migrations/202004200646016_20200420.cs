namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200420 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BannerInfo", "BusinessTag", c => c.Int(nullable: false));
            AlterColumn("dbo.BannerInfo", "IsShow", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BannerInfo", "IsShow", c => c.Int(nullable: false));
            AlterColumn("dbo.BannerInfo", "BusinessTag", c => c.String(nullable: false, maxLength: 500));
        }
    }
}
