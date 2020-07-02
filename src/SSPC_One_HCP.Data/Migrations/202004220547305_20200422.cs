namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class _20200422 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BannerInfoItem",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        BannerInfoId = c.String(),
                        Title = c.String(),
                        Describe = c.String(),
                        Type = c.Int(nullable: false),
                        ShowPlace = c.String(),
                        Src = c.String(),
                        Sort = c.Int(nullable: false),
                        IsDeleted = c.Int(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "-1")
                                },
                            }),
                        IsEnabled = c.Int(nullable: false),
                        CreateTime = c.DateTime(),
                        UpdateTime = c.DateTime(),
                        CreateUser = c.String(maxLength: 36),
                        UpdateUser = c.String(maxLength: 36),
                        CompanyCode = c.String(maxLength: 50),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.BannerInfo", "Title", c => c.String());
            AddColumn("dbo.BannerInfo", "Icon", c => c.String());
            AddColumn("dbo.BannerInfo", "Scene", c => c.String());
            AddColumn("dbo.BannerInfo", "Describe", c => c.String());
            DropColumn("dbo.BannerInfo", "BannerTitle");
            DropColumn("dbo.BannerInfo", "BannerIcon");
            DropColumn("dbo.BannerInfo", "Ascription");
            DropColumn("dbo.BannerInfo", "BannerDescribe");
            DropColumn("dbo.BannerInfo", "BusinessTag");
            DropColumn("dbo.BannerInfo", "ShowPlace");
            DropColumn("dbo.BannerInfo", "Src");
            DropColumn("dbo.BannerInfo", "Sort");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BannerInfo", "Sort", c => c.Int(nullable: false));
            AddColumn("dbo.BannerInfo", "Src", c => c.String(nullable: false, maxLength: 500));
            AddColumn("dbo.BannerInfo", "ShowPlace", c => c.String(nullable: false, maxLength: 500));
            AddColumn("dbo.BannerInfo", "BusinessTag", c => c.Int(nullable: false));
            AddColumn("dbo.BannerInfo", "BannerDescribe", c => c.String());
            AddColumn("dbo.BannerInfo", "Ascription", c => c.String());
            AddColumn("dbo.BannerInfo", "BannerIcon", c => c.String());
            AddColumn("dbo.BannerInfo", "BannerTitle", c => c.String(nullable: false, maxLength: 500));
            DropColumn("dbo.BannerInfo", "Describe");
            DropColumn("dbo.BannerInfo", "Scene");
            DropColumn("dbo.BannerInfo", "Icon");
            DropColumn("dbo.BannerInfo", "Title");
            DropTable("dbo.BannerInfoItem",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "IsDeleted",
                        new Dictionary<string, object>
                        {
                            { "Default", "-1" },
                        }
                    },
                });
        }
    }
}
