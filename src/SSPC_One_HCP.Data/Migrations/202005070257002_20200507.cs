namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class _20200507 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentManager",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ProductTypeInfoId = c.String(nullable: false),
                        Title = c.String(maxLength: 100),
                        DataContent = c.String(maxLength: 500),
                        DataType = c.String(maxLength: 36),
                        DataOrigin = c.Int(),
                        DataUrl = c.String(),
                        KnowImageUrl = c.String(maxLength: 500),
                        KnowImageName = c.String(maxLength: 500),
                        DataLink = c.String(),
                        IsRead = c.Int(),
                        IsSelected = c.Int(),
                        IsCopyRight = c.Int(),
                        MediaTime = c.String(maxLength: 500),
                        MediaType = c.Int(),
                        BuName = c.String(),
                        Dept = c.String(),
                        Product = c.String(),
                        Sort = c.Int(nullable: false),
                        IsCompleted = c.Int(),
                        OldId = c.String(),
                        ApprovalNote = c.String(),
                        IsPublic = c.Int(),
                        IsChoiceness = c.Int(),
                        ClickVolume = c.Long(),
                        Isdownload = c.Boolean(nullable: false),
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
            
            CreateTable(
                "dbo.DocumentManagerRel",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        BuName = c.String(maxLength: 50),
                        DeptId = c.String(maxLength: 50),
                        ProId = c.String(maxLength: 50),
                        DataInfoId = c.String(maxLength: 36),
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DocumentManagerRel",
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
            DropTable("dbo.DocumentManager",
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
