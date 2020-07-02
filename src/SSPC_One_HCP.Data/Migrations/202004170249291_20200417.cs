namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class _20200417 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SeriesCoursesMeetRel",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        SeriesCoursesId = c.String(nullable: false, maxLength: 36),
                        MeetInfoId = c.String(nullable: false, maxLength: 36),
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
            
            CreateTable(
                "dbo.SeriesCourses",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        CourseTitle = c.String(nullable: false, maxLength: 500),
                        Speaker = c.String(maxLength: 500),
                        Hospital = c.String(maxLength: 500),
                        IsHot = c.Int(nullable: false),
                        CourseCoverSmall = c.String(),
                        CourseCoverBig = c.String(),
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
            DropTable("dbo.SeriesCourses",
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
            DropTable("dbo.SeriesCoursesMeetRel",
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
