namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class _20200226001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PneumoniaBotForward",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        UserId = c.String(),
                        UnionId = c.String(),
                        OpenId = c.String(),
                        PageName = c.String(),
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
            DropTable("dbo.PneumoniaBotForward",
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
