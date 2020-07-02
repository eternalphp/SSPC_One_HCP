namespace SSPC_One_HCP.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class _20200221001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountLoginInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 36),
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
                "dbo.AppClientInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        AppName = c.String(nullable: false, maxLength: 36),
                        Secret = c.String(nullable: false, maxLength: 1000),
                        ApplicationType = c.Int(nullable: false),
                        AppClientName = c.String(nullable: false, maxLength: 200),
                        Active = c.Boolean(nullable: false),
                        RefreshTokenLifeTime = c.Int(nullable: false),
                        AllowedOrigin = c.String(maxLength: 200),
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
                "dbo.RefreshTokenInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        AppId = c.String(maxLength: 36),
                        TokenId = c.String(),
                        Subject = c.String(nullable: false, maxLength: 200),
                        AppClientId = c.String(nullable: false, maxLength: 36),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpiresUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(nullable: false, maxLength: 1000),
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
                "dbo.AdQRCode",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        BuName = c.String(maxLength: 50),
                        AppName = c.String(maxLength: 100),
                        AppUrl = c.String(maxLength: 500),
                        QRCodePicUrl = c.String(maxLength: 500),
                        VisitAmount = c.Int(nullable: false),
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
                "dbo.AnalysisDailyVisitTrend",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ref_date = c.String(maxLength: 100),
                        session_cnt = c.Int(nullable: false),
                        visit_pv = c.Int(nullable: false),
                        visit_uv = c.Int(nullable: false),
                        visit_uv_new = c.Int(nullable: false),
                        stay_time_uv = c.Single(nullable: false),
                        stay_time_session = c.Single(nullable: false),
                        visit_depth = c.Single(nullable: false),
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
                "dbo.AnswerModel",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        QuestionId = c.String(maxLength: 36),
                        AnswerContent = c.String(maxLength: 500),
                        Sort = c.String(maxLength: 11),
                        IsRight = c.Boolean(),
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
                "dbo.ApprovalRecord",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        AssetsMainId = c.String(maxLength: 200),
                        OperationUser = c.String(maxLength: 50),
                        OperationAction = c.String(),
                        OperationDate = c.DateTime(),
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
                "dbo.BotMedalBusinessConfigure",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        KBSBotId = c.String(maxLength: 36),
                        KBSBotName = c.String(),
                        FaqPackageId = c.String(maxLength: 36),
                        FaqPackageName = c.String(),
                        MedalName = c.String(maxLength: 500),
                        MedalYSrc = c.String(maxLength: 2000),
                        MedalNSrc = c.String(maxLength: 2000),
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
                "dbo.BotSaleConfigure",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        KBSBotId = c.String(maxLength: 36),
                        BotName = c.String(maxLength: 500),
                        AppId = c.String(),
                        AppSecret = c.String(),
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
                "dbo.BotMedalStandardConfigure",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        KBSBotId = c.String(maxLength: 36),
                        KBSBotName = c.String(),
                        MedalName = c.String(maxLength: 500),
                        Ruletotal = c.Int(nullable: false),
                        MedalYSrc = c.String(maxLength: 2000),
                        MedalNSrc = c.String(maxLength: 2000),
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
                "dbo.BotADWhiteName",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ADAccount = c.String(),
                        Type = c.Int(nullable: false),
                        BuName = c.String(),
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
                "dbo.HcpDataCatalogueRel",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        HcpCatalogueManageId = c.String(),
                        HcpDataInfoId = c.String(),
                        HcpCatalogueManageName = c.String(),
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
                "dbo.HcpDataInfo",
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
                "dbo.BotSaleUserTotalRecord",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        SaleUserId = c.String(maxLength: 36),
                        Total = c.Int(nullable: false),
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
                "dbo.BotSaleUserMedalInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        BotMedalRuleId = c.String(maxLength: 36),
                        SaleUserId = c.String(maxLength: 36),
                        MedalSrc = c.String(maxLength: 2000),
                        MedalType = c.Int(nullable: false),
                        MedalName = c.String(),
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
                "dbo.BotSaleUserInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ADAccount = c.String(),
                        UnionId = c.String(),
                        OpenId = c.String(),
                        WxName = c.String(),
                        WxPicture = c.String(),
                        WxGender = c.String(),
                        WxCountry = c.String(),
                        WxCity = c.String(),
                        WxProvince = c.String(),
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
                        UserName = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BuInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        BuName = c.String(maxLength: 500),
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
                "dbo.BuProDeptRel",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        BuName = c.String(maxLength: 50),
                        ProId = c.String(maxLength: 36),
                        DeptId = c.String(maxLength: 36),
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
                "dbo.BusinessCard",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        WxUserId = c.String(maxLength: 50),
                        OwnerWxUserId = c.String(maxLength: 50),
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
                "dbo.CompanyInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        CompanyName = c.String(maxLength: 50),
                        CompanyNum = c.String(maxLength: 200),
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
                "dbo.CompetingProduct",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        CompeteProductId = c.Int(nullable: false),
                        ProductName = c.String(maxLength: 200),
                        MedicineName = c.String(maxLength: 200),
                        MedicineSource = c.String(maxLength: 200),
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
                "dbo.Configuration",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ConfigureName = c.String(maxLength: 20),
                        ConfigureValue = c.String(maxLength: 200),
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
                "dbo.DataInfo",
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
                        BuName = c.String(maxLength: 50),
                        Dept = c.String(),
                        Product = c.String(maxLength: 500),
                        Sort = c.Int(nullable: false),
                        IsCompleted = c.Int(),
                        OldId = c.String(),
                        ApprovalNote = c.String(),
                        IsPublic = c.Int(),
                        IsChoiceness = c.Int(),
                        ClickVolume = c.Long(),
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
                "dbo.DepartmentInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        DepartmentName = c.String(),
                        DepartmentType = c.Int(),
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
                "dbo.DocGroup",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        DocId = c.String(maxLength: 36),
                        GroupId = c.String(maxLength: 36),
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
                "dbo.DocTag",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        DocId = c.String(maxLength: 36),
                        TagId = c.String(maxLength: 36),
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
                "dbo.DoctorMeeting",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        MeetingID = c.String(maxLength: 36),
                        DoctorID = c.String(maxLength: 36),
                        TagGroupID = c.String(maxLength: 36),
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
                "dbo.DoctorModel",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        UnionId = c.String(),
                        IsSalesPerson = c.Int(),
                        OpenId = c.String(maxLength: 128),
                        UserName = c.String(maxLength: 50),
                        Mobile = c.String(maxLength: 20),
                        HospitalName = c.String(maxLength: 100),
                        DepartmentName = c.String(maxLength: 100),
                        Province = c.String(maxLength: 20),
                        City = c.String(maxLength: 20),
                        Area = c.String(maxLength: 20),
                        HPAddress = c.String(maxLength: 50),
                        School = c.String(maxLength: 50),
                        Title = c.String(maxLength: 200),
                        DoctorPosition = c.String(maxLength: 200),
                        IsVerify = c.Int(nullable: false),
                        IsCompleteRegister = c.Int(nullable: false),
                        WxName = c.String(maxLength: 128),
                        WxPicture = c.String(maxLength: 256),
                        WxGender = c.String(maxLength: 10),
                        WxCountry = c.String(maxLength: 256),
                        WxCity = c.String(maxLength: 128),
                        WxProvince = c.String(maxLength: 128),
                        PVMId = c.String(maxLength: 128),
                        FCKId = c.String(maxLength: 128),
                        Code = c.String(maxLength: 11),
                        CodeTime = c.DateTime(),
                        OneHcpId = c.String(maxLength: 36),
                        creator = c.String(),
                        creation_time = c.DateTime(),
                        hospital_code = c.String(maxLength: 100),
                        hospital_name = c.String(maxLength: 500),
                        doctor_code = c.String(maxLength: 100),
                        doctor_name = c.String(maxLength: 200),
                        department = c.String(maxLength: 500),
                        job_title_SFE = c.String(maxLength: 200),
                        position_SFE = c.String(maxLength: 200),
                        is_infor_customer = c.Boolean(nullable: false),
                        serial_number = c.String(maxLength: 100),
                        status = c.String(maxLength: 50),
                        reason = c.String(maxLength: 200),
                        unique_doctor_id = c.String(maxLength: 100),
                        yunshi_hospital_id = c.String(maxLength: 100),
                        yunshi_hospital_name = c.String(maxLength: 500),
                        yunshi_doctor_id = c.String(maxLength: 100),
                        name = c.String(maxLength: 200),
                        yunshi_province = c.String(maxLength: 200),
                        yunshi_city = c.String(maxLength: 200),
                        standard_department = c.String(maxLength: 500),
                        profession = c.String(maxLength: 200),
                        gender = c.String(maxLength: 5),
                        job_title = c.String(maxLength: 200),
                        position = c.String(maxLength: 200),
                        academic_title = c.String(maxLength: 200),
                        type = c.String(maxLength: 200),
                        certificate_type = c.String(maxLength: 50),
                        certificate_code = c.String(maxLength: 100),
                        education = c.String(maxLength: 200),
                        graduated_school = c.String(maxLength: 500),
                        graduation_time = c.String(maxLength: 20),
                        specialty = c.String(maxLength: 1000),
                        intro = c.String(),
                        department_phone = c.String(maxLength: 100),
                        modifier = c.String(maxLength: 500),
                        change_time = c.DateTime(),
                        data_update_type = c.String(maxLength: 500),
                        SourceAppId = c.String(maxLength: 100),
                        SourceType = c.String(maxLength: 20),
                        WxSceneId = c.String(maxLength: 20),
                        Pictures = c.String(),
                        doctor_office_tel = c.String(maxLength: 500),
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
                "dbo.DocumentType",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ImgUrl = c.String(),
                        TypeValue = c.String(maxLength: 100),
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
                "dbo.DropDownConfig",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        DropDownValue = c.String(maxLength: 50),
                        DorpDownText = c.String(maxLength: 50),
                        DropDownType = c.String(maxLength: 20),
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
                "dbo.EdaCheckInRecord",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        AppId = c.String(maxLength: 200),
                        UnionId = c.String(maxLength: 200),
                        OpenId = c.String(maxLength: 200),
                        UserName = c.String(maxLength: 100),
                        WxName = c.String(maxLength: 200),
                        ActivityID = c.String(maxLength: 50),
                        VisitTime = c.DateTime(),
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
                "dbo.Feekback",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        Content = c.String(),
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
                "dbo.FsysArticle",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        DepartmentId = c.String(),
                        ArticleTitle = c.String(maxLength: 200),
                        ArticleUrl = c.String(maxLength: 256),
                        ArticleSort = c.Int(nullable: false),
                        ArticleIsHot = c.Int(nullable: false),
                        ArticleSource = c.String(maxLength: 200),
                        PublishedDate = c.DateTime(),
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
                "dbo.GroupInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        GroupName = c.String(maxLength: 500),
                        GroupType = c.String(maxLength: 500),
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
                "dbo.GroupTagRel",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        TagGroupId = c.String(maxLength: 500),
                        TagId = c.String(name: "TagId ", maxLength: 500),
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
                "dbo.GuidVisit",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        userid = c.String(maxLength: 100),
                        ActionType = c.String(maxLength: 100),
                        GuideId = c.Int(nullable: false),
                        GuideType = c.String(),
                        GuideName = c.String(maxLength: 500),
                        Email = c.String(maxLength: 100),
                        Keyword = c.String(maxLength: 100),
                        VisitStart = c.DateTime(),
                        VisitEnd = c.DateTime(),
                        StaySeconds = c.Int(),
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
                "dbo.HcpDownloadInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        Sender = c.String(),
                        DownloadPeople = c.String(),
                        HcpDataInfoId = c.String(),
                        DownloadFileName = c.String(),
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
                "dbo.HcpCatalogueManage",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        BuName = c.String(),
                        CatalogueName = c.String(),
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
                "dbo.HospitalInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        HospitalId = c.String(maxLength: 100),
                        HospitalName = c.String(maxLength: 500),
                        CustomerType = c.Int(),
                        HospitalCode = c.String(maxLength: 100),
                        AreaId = c.String(maxLength: 100),
                        Address = c.String(maxLength: 500),
                        NetAddress = c.String(),
                        PhoneNum = c.String(maxLength: 200),
                        ZipCode = c.String(maxLength: 100),
                        HospitalTypeFlag = c.Int(),
                        Email = c.String(maxLength: 500),
                        HospitalType = c.String(maxLength: 100),
                        PyFull = c.String(maxLength: 500),
                        PyShort = c.String(maxLength: 500),
                        ComeFrom = c.String(maxLength: 100),
                        IsVerify = c.Int(),
                        hospital_code = c.String(maxLength: 200),
                        hospital_name_SFE = c.String(maxLength: 500),
                        serial_number = c.String(maxLength: 100),
                        status = c.String(maxLength: 50),
                        reason = c.String(maxLength: 200),
                        unique_hospital_id = c.String(maxLength: 100),
                        yunshi_hospital_id = c.String(maxLength: 100),
                        hospital_name = c.String(maxLength: 500),
                        hospital_alias = c.String(maxLength: 500),
                        hospital_level = c.String(maxLength: 100),
                        hospital_category = c.String(maxLength: 50),
                        hospital_nature = c.String(maxLength: 20),
                        hospital_organization_code = c.String(maxLength: 100),
                        province = c.String(maxLength: 50),
                        city = c.String(maxLength: 20),
                        area = c.String(maxLength: 100),
                        YsAddress = c.String(maxLength: 500),
                        post_code = c.String(maxLength: 10),
                        hospital_phone = c.String(maxLength: 50),
                        website = c.String(maxLength: 100),
                        number_of_beds = c.Int(nullable: false),
                        number_of_outpatient = c.Int(nullable: false),
                        hospitalization = c.Int(nullable: false),
                        hospital_intro = c.String(),
                        number_of_employees = c.Int(nullable: false),
                        modifier = c.String(maxLength: 50),
                        change_time = c.DateTime(),
                        data_update_type = c.String(maxLength: 500),
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
                "dbo.LanguageConfig",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        LanKey = c.String(maxLength: 200),
                        LanValue = c.String(maxLength: 200),
                        LanType = c.String(maxLength: 200),
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
                "dbo.PneumoniaBotOperationRecord",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        UnionId = c.String(),
                        OpenId = c.String(),
                        WxName = c.String(),
                        WxPicture = c.String(),
                        WxGender = c.String(),
                        WxCountry = c.String(),
                        WxCity = c.String(),
                        WxProvince = c.String(),
                        ModulesClicked = c.String(),
                        ControlId = c.String(),
                        ControlName = c.String(),
                        ClickingTime = c.DateTime(nullable: false),
                        ResidenceTime = c.String(),
                        PlayTime = c.String(),
                        IslogIn = c.Int(nullable: false),
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
                "dbo.Management",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ManagementId = c.String(maxLength: 50),
                        ManagementWord = c.String(),
                        IsCompleted = c.Int(),
                        OldManagementId = c.String(),
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
                "dbo.HcpMediaDataRel",
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
            
            CreateTable(
                "dbo.MediaDataRel",
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
            
            CreateTable(
                "dbo.MedicineHotSearch",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        KeyWord = c.String(maxLength: 200),
                        Type = c.String(maxLength: 50),
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
                "dbo.MedicineSearchHistory",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        KeyWord = c.String(maxLength: 200),
                        Type = c.String(maxLength: 50),
                        Wxuserid = c.String(maxLength: 100),
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
                "dbo.MeetAndProAndDepRelation",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        MeetId = c.String(maxLength: 36),
                        ProductId = c.String(maxLength: 36),
                        DepartmentId = c.String(),
                        BuName = c.String(maxLength: 50),
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
                "dbo.MeetQAResult",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        MeetId = c.String(maxLength: 36),
                        MeetQAId = c.String(maxLength: 36),
                        SignUpUserId = c.String(maxLength: 36),
                        UserAnswerId = c.String(maxLength: 36),
                        UserAnswer = c.String(maxLength: 500),
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
                "dbo.ProductInfoLike",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ProID = c.String(maxLength: 64),
                        IsLike = c.Int(nullable: false),
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
                "dbo.PublicAccount",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        AppId = c.String(maxLength: 70),
                        PublicAccountName = c.String(maxLength: 100),
                        AppUrl = c.String(),
                        Iseffective = c.Int(nullable: false),
                        Dept = c.String(),
                        ImageUrl = c.String(maxLength: 200),
                        ImageName = c.String(maxLength: 100),
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
                "dbo.QRcodeExtension",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        AppId = c.String(maxLength: 150),
                        AppName = c.String(maxLength: 150),
                        AppType = c.String(maxLength: 150),
                        AppUrl = c.String(maxLength: 300),
                        AppimangeUrl = c.String(maxLength: 300),
                        AppimangeName = c.String(maxLength: 300),
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
                "dbo.QRcodeRecord",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        AppId = c.String(maxLength: 200),
                        UnionId = c.String(maxLength: 200),
                        Isregistered = c.Int(nullable: false),
                        SourceType = c.String(),
                        WxSceneId = c.String(),
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
                "dbo.SendRate",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        DoctorId = c.String(maxLength: 36),
                        SendCycleType = c.Int(),
                        SendNumber = c.Int(),
                        IsDefault = c.Boolean(),
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
                "dbo.SpreadQRCode",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        SpreadAppId = c.String(maxLength: 50),
                        SpreadName = c.String(maxLength: 100),
                        SpreadQRType = c.Int(nullable: false),
                        SpreadQRCodeUrl = c.String(maxLength: 256),
                        RegisteredCount = c.Int(nullable: false),
                        VisitorsCount = c.Int(nullable: false),
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
                "dbo.TagGroup",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        TagGroupName = c.String(maxLength: 500),
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
                "dbo.TemplateForm",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        FormID = c.String(maxLength: 36),
                        SendTime = c.DateTime(),
                        Page = c.String(maxLength: 128),
                        OpenID = c.String(maxLength: 36),
                        MsgID = c.Int(nullable: false),
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
                "dbo.ThirdPartyKeyWord",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        KeyWordContent = c.String(maxLength: 100),
                        KeyWordType = c.Int(nullable: false),
                        KeyWordSort = c.Int(nullable: false),
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
                "dbo.VisitModules",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        UnionId = c.String(maxLength: 100),
                        VisitStart = c.DateTime(),
                        VisitEnd = c.DateTime(),
                        StaySeconds = c.Int(nullable: false),
                        Isvisitor = c.Int(nullable: false),
                        ModuleNo = c.String(maxLength: 100),
                        ModulePageNo = c.String(maxLength: 100),
                        ModulePageUrl = c.String(maxLength: 100),
                        WxUserid = c.String(),
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
                "dbo.VisitModulesName",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ModulesName = c.String(maxLength: 100),
                        ModulesNo = c.String(maxLength: 100),
                        ModulesUrl = c.String(maxLength: 100),
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
                "dbo.VisitTimes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        UnionId = c.String(maxLength: 100),
                        WxuserId = c.String(maxLength: 100),
                        VisitStart = c.DateTime(),
                        VisitEnd = c.DateTime(),
                        StaySeconds = c.Int(nullable: false),
                        Isvisitor = c.Int(nullable: false),
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
                "dbo.WechatActionHistory",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ActionType = c.Int(nullable: false),
                        Content = c.String(maxLength: 500),
                        ContentId = c.String(maxLength: 100),
                        UnionId = c.String(maxLength: 100),
                        WxuserId = c.String(maxLength: 100),
                        StaySeconds = c.Int(),
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
                "dbo.WechatPublicAccount",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        AppId = c.String(maxLength: 200),
                        Name = c.String(maxLength: 500),
                        Summary = c.String(),
                        ClickVolume = c.Long(),
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
                "dbo.MeetFile",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        MeetId = c.String(maxLength: 36),
                        Title = c.String(maxLength: 100),
                        FileName = c.String(maxLength: 500),
                        FilePath = c.String(),
                        FileSize = c.Int(nullable: false),
                        FileType = c.String(maxLength: 500),
                        IsCopyRight = c.Int(),
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
                "dbo.MeetInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        MeetTitle = c.String(maxLength: 100),
                        MeetSubject = c.String(maxLength: 500),
                        MeetType = c.Int(),
                        MeetDep = c.String(maxLength: 100),
                        MeetIntroduction = c.String(maxLength: 500),
                        MeetStartTime = c.DateTime(),
                        MeetEndTime = c.DateTime(),
                        MeetDate = c.DateTime(),
                        Speaker = c.String(maxLength: 36),
                        SpeakerDetail = c.String(),
                        MeetAddress = c.String(maxLength: 1000),
                        ReplayAddress = c.String(maxLength: 1000),
                        MeetData = c.String(maxLength: 100),
                        MeetCodeUrl = c.String(),
                        MeetCity = c.String(maxLength: 500),
                        MeetingNumber = c.Int(nullable: false),
                        MeetSite = c.String(),
                        MeetCoverSmall = c.String(),
                        MeetCoverBig = c.String(),
                        IsCompleted = c.Int(),
                        IsChoiceness = c.Int(),
                        IsHot = c.Int(),
                        OldId = c.String(maxLength: 36),
                        ApprovalNote = c.String(maxLength: 500),
                        Source = c.String(maxLength: 50),
                        SourceId = c.String(maxLength: 50),
                        SourceHospital = c.String(maxLength: 500),
                        SourceDepartment = c.String(),
                        HasReminded = c.Int(nullable: false),
                        InvitationDetail = c.String(),
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
                "dbo.MeetPic",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        MeetId = c.String(maxLength: 36),
                        MeetPicName = c.String(maxLength: 200),
                        MeetPicType = c.String(maxLength: 200),
                        MeetPicUrl = c.String(),
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
                "dbo.MeetQAModel",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        MeetId = c.String(maxLength: 36),
                        QAType = c.Int(),
                        QuestionId = c.String(maxLength: 36),
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
                "dbo.MeetSchedule",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        MeetId = c.String(maxLength: 36),
                        ScheduleStart = c.DateTime(),
                        ScheduleEnd = c.DateTime(),
                        ScheduleContent = c.String(maxLength: 500),
                        MeetSpeakerId = c.String(maxLength: 36),
                        AMPM = c.Int(nullable: false),
                        Sort = c.Int(nullable: false),
                        Topic = c.String(maxLength: 100),
                        Speaker = c.String(maxLength: 50),
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
                "dbo.MeetSignUp",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        MeetId = c.String(maxLength: 36),
                        SignUpUserId = c.String(maxLength: 36),
                        IsSignIn = c.Int(),
                        SignInTime = c.DateTime(),
                        IsKnewDetail = c.Int(),
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
                "dbo.MeetSpeaker",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        MeetId = c.String(maxLength: 36),
                        SpeakerName = c.String(maxLength: 200),
                        SpeakerDetail = c.String(),
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
                "dbo.MeetTag",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        MeetId = c.String(maxLength: 36),
                        TagId = c.String(maxLength: 36),
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
                "dbo.MenuInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        MenuName = c.String(maxLength: 50),
                        MenuIcons = c.String(maxLength: 50),
                        MenuPath = c.String(maxLength: 500),
                        LinkPath = c.String(maxLength: 500),
                        ParentId = c.String(maxLength: 36),
                        Component = c.String(maxLength: 500),
                        Hidden = c.Boolean(),
                        Leaf = c.Boolean(),
                        Sort = c.Int(),
                        Props = c.String(),
                        IsNormal = c.Boolean(),
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
                "dbo.MyBrowseRecord",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        UnionId = c.String(maxLength: 50),
                        DataInfoId = c.String(maxLength: 36),
                        IsRead = c.Int(),
                        WxUserId = c.String(maxLength: 36),
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
                "dbo.MyCollectionInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        UnionId = c.String(),
                        CollectionDataId = c.String(),
                        CollectionType = c.Int(),
                        WxUserId = c.String(maxLength: 36),
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
                "dbo.MyLRecord",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        UnionId = c.String(),
                        LObjectId = c.String(maxLength: 36),
                        LObjectType = c.Int(),
                        LDate = c.DateTime(nullable: false),
                        LDateStart = c.DateTime(),
                        LDateEnd = c.DateTime(),
                        LObjectDate = c.Int(),
                        IsRead = c.Int(),
                        WxUserId = c.String(maxLength: 36),
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
                "dbo.MyMeetOrder",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        UnionId = c.String(),
                        MeetId = c.String(maxLength: 36),
                        IsRemind = c.Int(nullable: false),
                        HasReminded = c.Int(nullable: false),
                        RemindTime = c.DateTime(),
                        RemindOffsetMinutes = c.Int(nullable: false),
                        JoinInMeetTime = c.DateTime(),
                        WxUserId = c.String(maxLength: 36),
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
                "dbo.Organization",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        Code = c.String(maxLength: 50),
                        Name = c.String(maxLength: 500),
                        ParentId = c.String(maxLength: 50),
                        IsDisabled = c.Boolean(nullable: false),
                        ManagerId = c.String(maxLength: 50),
                        Level = c.Int(nullable: false),
                        Path = c.String(maxLength: 500),
                        BuName = c.String(),
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
                "dbo.Position",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        Code = c.String(maxLength: 50),
                        Name = c.String(maxLength: 200),
                        IsDisabled = c.Boolean(nullable: false),
                        OrganizationId = c.String(maxLength: 50),
                        ReporterId = c.String(maxLength: 50),
                        HolderId = c.String(maxLength: 50),
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
                "dbo.ProAndDepRelation",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ProductId = c.String(maxLength: 36),
                        DepartmentId = c.String(maxLength: 36),
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
                "dbo.ProductInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ProductName = c.String(maxLength: 500),
                        ProductDesc = c.String(),
                        ProductUrl = c.String(),
                        ProductPicName = c.String(),
                        Sort = c.Int(),
                        IsCompleted = c.Int(),
                        OldId = c.String(maxLength: 36),
                        ApprovalNote = c.String(maxLength: 500),
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
                "dbo.ProductTypeInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        TypeId = c.Int(),
                        SubTitle = c.String(maxLength: 20),
                        ContentDepType = c.String(maxLength: 100),
                        SubTypeUrl = c.String(),
                        IsCompleted = c.Int(),
                        OldId = c.String(),
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
                "dbo.ProtocolModel",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ProctocolName = c.String(maxLength: 50),
                        ProctocolType = c.Int(),
                        ProctocolUrl = c.String(),
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
                "dbo.QuestionModel",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        QuestionType = c.Int(),
                        QuestionContent = c.String(maxLength: 500),
                        QuestionOfA = c.String(maxLength: 200),
                        MeetId = c.String(maxLength: 36),
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
                "dbo.RegisterModel",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        UnionId = c.String(maxLength: 50),
                        SignUpName = c.String(),
                        WxUserId = c.String(maxLength: 36),
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
                "dbo.RoleInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        RoleName = c.String(maxLength: 50),
                        RoleDesc = c.String(maxLength: 200),
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
                "dbo.RoleMenu",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        MenuId = c.String(),
                        RoleId = c.String(),
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
                "dbo.TagInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        TagName = c.String(maxLength: 500),
                        TagType = c.String(maxLength: 500),
                        TagRule = c.String(),
                        TextKey = c.String(maxLength: 500),
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
                "dbo.UserInfo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        Code = c.String(maxLength: 50),
                        EmployeeNo = c.String(maxLength: 50),
                        ADAccount = c.String(maxLength: 200),
                        ChineseName = c.String(maxLength: 200),
                        EnglishName = c.String(maxLength: 200),
                        PersonalEmail = c.String(maxLength: 200),
                        CompanyEmail = c.String(maxLength: 200),
                        MobileNo = c.String(maxLength: 200),
                        IdCardNumber = c.String(maxLength: 200),
                        IsDisabled = c.Boolean(nullable: false),
                        ReporterId = c.String(maxLength: 50),
                        OrganizationId = c.String(maxLength: 50),
                        PositionId = c.String(maxLength: 50),
                        CostCenter = c.String(maxLength: 50),
                        Password = c.String(maxLength: 500),
                        ManagerId = c.String(maxLength: 50),
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
                "dbo.UserRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        SapCode = c.String(maxLength: 50),
                        UserId = c.String(maxLength: 36),
                        RoleId = c.String(maxLength: 36),
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
                "dbo.WordBlackList",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        Words = c.String(),
                        Type = c.String(maxLength: 50),
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
            DropTable("dbo.WordBlackList",
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
            DropTable("dbo.UserRole",
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
            DropTable("dbo.UserInfo",
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
            DropTable("dbo.TagInfo",
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
            DropTable("dbo.RoleMenu",
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
            DropTable("dbo.RoleInfo",
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
            DropTable("dbo.RegisterModel",
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
            DropTable("dbo.QuestionModel",
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
            DropTable("dbo.ProtocolModel",
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
            DropTable("dbo.ProductTypeInfo",
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
            DropTable("dbo.ProductInfo",
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
            DropTable("dbo.ProAndDepRelation",
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
            DropTable("dbo.Position",
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
            DropTable("dbo.Organization",
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
            DropTable("dbo.MyMeetOrder",
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
            DropTable("dbo.MyLRecord",
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
            DropTable("dbo.MyCollectionInfo",
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
            DropTable("dbo.MyBrowseRecord",
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
            DropTable("dbo.MenuInfo",
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
            DropTable("dbo.MeetTag",
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
            DropTable("dbo.MeetSpeaker",
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
            DropTable("dbo.MeetSignUp",
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
            DropTable("dbo.MeetSchedule",
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
            DropTable("dbo.MeetQAModel",
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
            DropTable("dbo.MeetPic",
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
            DropTable("dbo.MeetInfo",
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
            DropTable("dbo.MeetFile",
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
            DropTable("dbo.WechatPublicAccount",
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
            DropTable("dbo.WechatActionHistory",
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
            DropTable("dbo.VisitTimes",
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
            DropTable("dbo.VisitModulesName",
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
            DropTable("dbo.VisitModules",
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
            DropTable("dbo.ThirdPartyKeyWord",
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
            DropTable("dbo.TemplateForm",
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
            DropTable("dbo.TagGroup",
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
            DropTable("dbo.SpreadQRCode",
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
            DropTable("dbo.SendRate",
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
            DropTable("dbo.QRcodeRecord",
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
            DropTable("dbo.QRcodeExtension",
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
            DropTable("dbo.PublicAccount",
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
            DropTable("dbo.ProductInfoLike",
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
            DropTable("dbo.MeetQAResult",
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
            DropTable("dbo.MeetAndProAndDepRelation",
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
            DropTable("dbo.MedicineSearchHistory",
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
            DropTable("dbo.MedicineHotSearch",
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
            DropTable("dbo.MediaDataRel",
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
            DropTable("dbo.HcpMediaDataRel",
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
            DropTable("dbo.Management",
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
            DropTable("dbo.PneumoniaBotOperationRecord",
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
            DropTable("dbo.LanguageConfig",
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
            DropTable("dbo.HospitalInfo",
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
            DropTable("dbo.HcpCatalogueManage",
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
            DropTable("dbo.HcpDownloadInfo",
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
            DropTable("dbo.GuidVisit",
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
            DropTable("dbo.GroupTagRel",
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
            DropTable("dbo.GroupInfo",
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
            DropTable("dbo.FsysArticle",
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
            DropTable("dbo.Feekback",
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
            DropTable("dbo.EdaCheckInRecord",
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
            DropTable("dbo.DropDownConfig",
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
            DropTable("dbo.DocumentType",
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
            DropTable("dbo.DoctorModel",
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
            DropTable("dbo.DoctorMeeting",
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
            DropTable("dbo.DocTag",
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
            DropTable("dbo.DocGroup",
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
            DropTable("dbo.DepartmentInfo",
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
            DropTable("dbo.DataInfo",
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
            DropTable("dbo.Configuration",
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
            DropTable("dbo.CompetingProduct",
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
            DropTable("dbo.CompanyInfo",
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
            DropTable("dbo.BusinessCard",
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
            DropTable("dbo.BuProDeptRel",
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
            DropTable("dbo.BuInfo",
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
            DropTable("dbo.BotSaleUserInfo",
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
            DropTable("dbo.BotSaleUserMedalInfo",
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
            DropTable("dbo.BotSaleUserTotalRecord",
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
            DropTable("dbo.HcpDataInfo",
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
            DropTable("dbo.HcpDataCatalogueRel",
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
            DropTable("dbo.BotADWhiteName",
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
            DropTable("dbo.BotMedalStandardConfigure",
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
            DropTable("dbo.BotSaleConfigure",
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
            DropTable("dbo.BotMedalBusinessConfigure",
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
            DropTable("dbo.ApprovalRecord",
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
            DropTable("dbo.AnswerModel",
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
            DropTable("dbo.AnalysisDailyVisitTrend",
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
            DropTable("dbo.AdQRCode",
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
            DropTable("dbo.RefreshTokenInfo",
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
            DropTable("dbo.AppClientInfo",
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
            DropTable("dbo.AccountLoginInfo",
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
