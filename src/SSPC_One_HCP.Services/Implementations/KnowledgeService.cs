using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Enums;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.Approval;
using SSPC_One_HCP.Core.Domain.ViewModels.ProductInfoModels;
using SSPC_One_HCP.Core.Domain.ViewModels.ProductModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SSPC_One_HCP.Services.Implementations
{
    /// <summary>
    /// 知识库管理
    /// </summary>
    public class KnowledgeService : IKnowledgeService
    {
        private readonly IEfRepository _rep;
        private readonly ICommonService _commonService;
        private readonly ISystemService _systemService;
        private readonly string DownloadUrl = ConfigurationManager.AppSettings["dLoadUrl"];
        private readonly string IsSendMail = ConfigurationManager.AppSettings["IsSendMail"];
        public KnowledgeService(IEfRepository rep, ICommonService commonService, ISystemService systemService)
        {
            _rep = rep;
            _commonService = commonService;
            _systemService = systemService;
        }

        /// <summary>
        /// 获取资料详情列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetProductInfoList(RowNumModel<DataInfoSearchViewModel> rowNum, WorkUser workUser)
        {
            var isAdmin = _commonService.IsAdmin(workUser);

            ReturnValueModel rvm = new ReturnValueModel();
            var list = (from a in _rep.Where<DataInfo>(s => s.IsDeleted != 1 && s.MediaType == 1
                        && (s.IsCompleted == null
                            || s.IsCompleted == EnumComplete.Approved
                            || s.IsCompleted == EnumComplete.Reject
                            || s.IsCompleted == EnumComplete.AddedUnapproved
                            || s.IsCompleted == EnumComplete.UpdatedUnapproved
                            || s.IsCompleted == EnumComplete.WillDelete)
                        && (string.IsNullOrEmpty(rowNum.SearchParams.Title) || s.Title.Contains(rowNum.SearchParams.Title))
                        && (string.IsNullOrEmpty(rowNum.SearchParams.BuName) || s.BuName.Contains(rowNum.SearchParams.BuName))
                        && (string.IsNullOrEmpty(rowNum.SearchParams.Dept) || s.Dept.Contains(rowNum.SearchParams.Dept))
                        && (string.IsNullOrEmpty(rowNum.SearchParams.Product) || s.Product.Contains(rowNum.SearchParams.Product))
                        && (string.IsNullOrEmpty(rowNum.SearchParams.DataType) || s.DataType.Contains(rowNum.SearchParams.DataType))
                        && (!rowNum.SearchParams.IsChoiceness.HasValue || s.IsChoiceness == rowNum.SearchParams.IsChoiceness)
                        && (!rowNum.SearchParams.ClickVolume.HasValue || s.ClickVolume >= rowNum.SearchParams.ClickVolume)
                        && (string.IsNullOrEmpty(rowNum.SearchParams.DataType) || s.DataType.Contains(rowNum.SearchParams.DataType))
                        && (!rowNum.SearchParams.IsCompleted.HasValue || s.IsCompleted == rowNum.SearchParams.IsCompleted))
                        join d in _rep.All<DocumentType>() on a.DataType equals d.Id
                        join b in _rep.Table<UserModel>() on a.CreateUser equals b.Id
                        into ab
                        from bb in ab.DefaultIfEmpty()
                        select new DataInfoViewModel
                        {
                            Id = a.Id,
                            IsDeleted = a.IsDeleted,
                            IsEnabled = a.IsEnabled,
                            CreateTime = a.CreateTime,
                            UpdateTime = a.UpdateTime,
                            UpdateUser = a.UpdateUser,
                            CreateUser = bb.ChineseName,
                            CreateUserID = a.CreateUser,
                            CreateUserADAccount = bb.ADAccount,
                            CompanyCode = a.CompanyCode,
                            Remark = a.Remark,
                            ProductTypeInfoId = a.ProductTypeInfoId,
                            Title = a.Title,
                            DataContent = a.DataContent,
                            DataType = a.DataType,
                            DataTypeValue = d.TypeValue,
                            DataOrigin = a.DataOrigin,
                            DataLink = a.DataLink,
                            DataUrl = a.DataUrl,
                            IsRead = a.IsRead,
                            IsSelected = a.IsSelected,
                            IsCopyRight = a.IsCopyRight,
                            IsChoiceness = a.IsChoiceness,
                            IsHot = a.ClickVolume > 500 ? 1 : 0,
                            ClickVolume = a.ClickVolume,
                            BuName = a.BuName,
                            Dept = a.Dept,
                            Sort = a.Sort,
                            Product = a.Product,
                            MediaTime = a.MediaTime,
                            IsCompleted = a.IsCompleted ?? EnumComplete.AddedUnapproved,
                            ApprovalNote = a.ApprovalNote
                        });

            if (rowNum.SearchParams.IsCompletedList != null && rowNum.SearchParams.IsCompletedList.Count > 0)
            {
                //审批状态
                list = list.Where(s => rowNum.SearchParams.IsCompletedList.Contains(s.IsCompleted));
            }

            foreach (var item in list)
            {
                //如果不是外部链接
                if (!item.DataUrl.Contains("http"))
                {
                    item.DataUrl = DownloadUrl + "?url=" + item.DataUrl;
                }
            }

            if (!isAdmin)
            {
                //BU管理员筛选本BU数据
                if (_commonService.IsBuAdmin(workUser))
                {
                    list = list.Where(s => s.BuName == workUser.Organization.BuName);
                }
                else
                {
                    //代表筛选本人数据
                    list = list.Where(s => s.CreateUserID == workUser.User.Id);
                }
            }
            var total = list.Count();
            //先按照Sort字段升序排列，再按照创建时间倒序排列
            var rows = list.OrderBy(s => s.Sort).ThenByDescending(s => s.CreateTime).ToPaginationList(rowNum.PageIndex, rowNum.PageSize);

           var rowlist= rows.ToList().Select(x => new DataInfoViewModel
            {
                Id = x.Id,
                IsDeleted = x.IsDeleted,
                IsEnabled = x.IsEnabled,
                CreateTime = x.CreateTime,
                UpdateTime = x.UpdateTime,
                UpdateUser = x.UpdateUser,
                CreateUser = x.CreateUser,
                CreateUserID = x.CreateUser,
                CreateUserADAccount = x.CreateUserADAccount,
                CompanyCode = x.CompanyCode,
                Remark = x.Remark,
                ProductTypeInfoId = x.ProductTypeInfoId,
                Title = x.Title,
                DataContent = x.DataContent,
                DataType = x.DataType,
                DataTypeValue = x.DataTypeValue,
                DataOrigin = x.DataOrigin,
                DataLink = x.DataLink,
                DataUrl = x.DataUrl,
                IsRead = x.IsRead,
                IsSelected = x.IsSelected,
                IsCopyRight = x.IsCopyRight,
                IsChoiceness = x.IsChoiceness,
                IsHot = x.IsHot,
                ClickVolume = x.ClickVolume,
                BuName = x.BuName,
                Dept = x.Dept,
                Sort = x.Sort,
                Product = x.Product,
                MediaTime = x.MediaTime,
                IsCompleted = x.IsCompleted,
                ApprovalNote = x.ApprovalNote,
                LikeCount = _rep.Where<ProductInfoLike>(y => y.ProID.Equals(x.Id) && y.IsLike == 1).Count(),
                UNLikeCount = _rep.Where<ProductInfoLike>(y => y.ProID.Equals(x.Id) && y.IsLike == 2).Count()
            }) ;
            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total = total,
                rows = rowlist
            };

            return rvm;
        }

        /// <summary>
        /// 获取学术知识列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetAcademicList(RowNumModel<DataInfoSearchViewModel> rowNum, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var list = (from a in _rep.Where<DataInfo>(s => s.IsDeleted != 1 && s.MediaType == 2).Where(
                        s => (string.IsNullOrEmpty(rowNum.SearchParams.Title) || s.Title.Contains(rowNum.SearchParams.Title))
                        && (string.IsNullOrEmpty(rowNum.SearchParams.BuName) || s.BuName.Contains(rowNum.SearchParams.BuName))
                        && (string.IsNullOrEmpty(rowNum.SearchParams.Dept) || s.Dept.Contains(rowNum.SearchParams.Dept))
                        && (string.IsNullOrEmpty(rowNum.SearchParams.Product) || s.Product.Contains(rowNum.SearchParams.Product))
                        && (string.IsNullOrEmpty(rowNum.SearchParams.DataType) || s.DataType.Contains(rowNum.SearchParams.DataType)))
                        join d in _rep.All<DocumentType>() on a.DataType equals d.Id
                        select new DataInfoViewModel
                        {
                            Id = a.Id,
                            IsDeleted = a.IsDeleted,
                            IsEnabled = a.IsEnabled,
                            CreateTime = a.CreateTime,
                            UpdateTime = a.UpdateTime,
                            UpdateUser = a.UpdateUser,
                            CreateUser = a.CreateUser,
                            CreateUserID = a.CreateUser,
                            CompanyCode = a.CompanyCode,
                            Remark = a.Remark,
                            ProductTypeInfoId = a.ProductTypeInfoId,
                            Title = a.Title,
                            DataContent = a.DataContent,
                            DataType = a.DataType,
                            DataTypeValue = d.TypeValue,
                            DataOrigin = a.DataOrigin,
                            DataLink = a.DataLink,
                            DataUrl = a.DataUrl,
                            IsRead = a.IsRead,
                            IsSelected = a.IsSelected,
                            IsCopyRight = a.IsCopyRight,
                            IsChoiceness = a.IsChoiceness,
                            IsHot = a.ClickVolume > 500 ? 1 : 0,
                            ClickVolume = a.ClickVolume,
                            BuName = a.BuName,
                            Dept = a.Dept,
                            Sort = a.Sort,
                            Product = a.Product,
                            MediaTime = a.MediaTime
                        });


            foreach (var item in list)
            {
                //如果不是外部链接
                if (!item.DataUrl.Contains("http"))
                {
                    item.DataUrl = DownloadUrl + "?url=" + item.DataUrl;
                }
            }

            var total = list.Count();
            var rows = list.OrderByDescending(s => s.CreateTime).ToPaginationList(rowNum.PageIndex, rowNum.PageSize);

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total = total,
                rows = rows
            };

            return rvm;
        }

        /// <summary>
        /// 获取播客列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetPodcastList(RowNumModel<DataInfoSearchViewModel> rowNum, WorkUser workUser)
        {
            var isAdmin = _commonService.IsAdmin(workUser);

            ReturnValueModel rvm = new ReturnValueModel();
            var list = (from a in _rep.Where<DataInfo>(s => s.IsDeleted != 1 && s.MediaType == 3
                        && (s.IsCompleted == null
                            || s.IsCompleted == EnumComplete.Approved
                            || s.IsCompleted == EnumComplete.Reject
                            || s.IsCompleted == EnumComplete.AddedUnapproved
                            || s.IsCompleted == EnumComplete.UpdatedUnapproved
                            || s.IsCompleted == EnumComplete.WillDelete)
                        && (string.IsNullOrEmpty(rowNum.SearchParams.Title) || s.Title.Contains(rowNum.SearchParams.Title))
                        && (string.IsNullOrEmpty(rowNum.SearchParams.BuName) || s.BuName.Contains(rowNum.SearchParams.BuName))
                        && (string.IsNullOrEmpty(rowNum.SearchParams.Dept) || s.Dept.Contains(rowNum.SearchParams.Dept))
                        && (string.IsNullOrEmpty(rowNum.SearchParams.Product) || s.Product.Contains(rowNum.SearchParams.Product))
                        && (string.IsNullOrEmpty(rowNum.SearchParams.DataType) || s.DataType.Contains(rowNum.SearchParams.DataType))
                        && (!rowNum.SearchParams.IsCompleted.HasValue || s.IsCompleted == rowNum.SearchParams.IsCompleted))
                        join d in _rep.All<DocumentType>() on a.DataType equals d.Id
                        join b in _rep.Table<UserModel>() on a.CreateUser equals b.Id
                        into ab
                        from bb in ab.DefaultIfEmpty()
                        select new DataInfoViewModel
                        {
                            Id = a.Id,
                            IsDeleted = a.IsDeleted,
                            IsEnabled = a.IsEnabled,
                            CreateTime = a.CreateTime,
                            UpdateTime = a.UpdateTime,
                            UpdateUser = a.UpdateUser,
                            CreateUser = bb.ChineseName,
                            CreateUserID = a.CreateUser,
                            CreateUserADAccount = bb.ADAccount,
                            CompanyCode = a.CompanyCode,
                            Remark = a.Remark,
                            ProductTypeInfoId = a.ProductTypeInfoId,
                            Title = a.Title,
                            DataContent = a.DataContent,
                            DataType = a.DataType,
                            DataTypeValue = d.TypeValue,
                            DataOrigin = a.DataOrigin,
                            DataLink = a.DataLink,
                            DataUrl = a.DataUrl,
                            IsRead = a.IsRead,
                            IsSelected = a.IsSelected,
                            IsCopyRight = a.IsCopyRight,
                            IsChoiceness = a.IsChoiceness,
                            IsHot = a.ClickVolume > 500 ? 1 : 0,
                            ClickVolume = a.ClickVolume,
                            BuName = a.BuName,
                            Dept = a.Dept,
                            Product = a.Product,
                            Sort = a.Sort,
                            MediaTime = a.MediaTime,
                            IsCompleted = a.IsCompleted ?? EnumComplete.AddedUnapproved,
                            ApprovalNote = a.ApprovalNote,
                            KnowImageUrl = a.KnowImageUrl,
                            KnowImageName = a.KnowImageName
                        });

            if (rowNum.SearchParams.IsCompletedList != null && rowNum.SearchParams.IsCompletedList.Count > 0)
            {
                //审批状态
                list = list.Where(s => rowNum.SearchParams.IsCompletedList.Contains(s.IsCompleted));
            }

            foreach (var item in list)
            {
                //如果不是外部链接
                if (!item.DataUrl.Contains("http"))
                {
                    item.DataUrl = DownloadUrl + "?url=" + item.DataUrl;
                }
            }
            if (!isAdmin)
            {

                //BU管理员筛选本BU数据
                if (_commonService.IsBuAdmin(workUser))
                {
                    list = list.Where(s => s.BuName==workUser.Organization.BuName);
                }
                else
                {
                    //代表筛选本人数据
                    list = list.Where(s => s.CreateUserID == workUser.User.Id);
                }

            }
            var total = list.Count();
            //先按照Sort字段升序排列，再按照创建时间倒序排列
            var rows = list.OrderBy(s => s.Sort).ThenByDescending(s => s.CreateTime).ToPaginationList(rowNum.PageIndex, rowNum.PageSize);

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total = total,
                rows = rows
            };

            return rvm;
        }
        /// <summary>
        ///获取临床指南列表
        /// </summary>
        /// <param name="fsysArtice"></param>
        /// <returns></returns>
        public ReturnValueModel GetClinicalguidelines(RowNumModel<FsysArticle> fsysArtice)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var Clinicalguidelines = _rep.Where<FsysArticle>(s => s.IsDeleted != 1);
            if (Clinicalguidelines == null)
            {
                rvm.Success = false;
                rvm.Msg = "No data available for the time being!";
            }
            else
            {
                    if (fsysArtice.SearchParams.ArticleTitle != "" && fsysArtice.SearchParams.DepartmentId!="" && fsysArtice.SearchParams.DepartmentId!=null && fsysArtice.SearchParams.ArticleTitle!=null)
                    {
                        var DeptName = _rep.FirstOrDefault<DepartmentInfo>(s => s.Id == fsysArtice.SearchParams.DepartmentId && s.IsDeleted != 1).DepartmentName;
                        Clinicalguidelines = Clinicalguidelines.Where(s=>s.IsDeleted!=1 && s.ArticleTitle.Contains(fsysArtice.SearchParams.ArticleTitle) && s.DepartmentId.Contains(DeptName));
                       
                    }
                    if ((fsysArtice.SearchParams.DepartmentId!=""&& fsysArtice.SearchParams.DepartmentId != null) && (fsysArtice.SearchParams.ArticleTitle==null || fsysArtice.SearchParams.ArticleTitle==""))
                    {
                        //通过传递的科室id查询科室名称
                        var DeptName = _rep.FirstOrDefault<DepartmentInfo>(s=>s.Id==fsysArtice.SearchParams.DepartmentId && s.IsDeleted!=1).DepartmentName;
                        Clinicalguidelines = Clinicalguidelines.Where(s=>s.DepartmentId.Contains(DeptName));
                    }
                    if ((fsysArtice.SearchParams.ArticleTitle != "" && fsysArtice.SearchParams.ArticleTitle != null) &&(fsysArtice.SearchParams.DepartmentId==null || fsysArtice
                    .SearchParams.DepartmentId=="")) {
                        Clinicalguidelines = Clinicalguidelines.Where(s => s.IsDeleted != 1 && s.ArticleTitle.Contains(fsysArtice.SearchParams.ArticleTitle));
                    }
                    var total = Clinicalguidelines.Count();
                    var rows = Clinicalguidelines.OrderByDescending(s => s.CreateTime).ThenBy(x=>x.ArticleSort).ToPaginationList(fsysArtice.PageIndex, fsysArtice.PageSize);
                    rvm.Success = true;
                    rvm.Msg = "Success";
                    rvm.Result = new
                    {
                        total = total,
                        rows = rows
                    };
               
            }
            return rvm;
        }
        ///// <summary>
        ///// 获取产品资料列表（作废）
        ///// </summary>
        ///// <param name="rowNum"></param>
        ///// <param name="workUser"></param>
        ///// <returns></returns>
        //public ReturnValueModel GetProductList(WorkUser workUser)
        //{
        //    ReturnValueModel rvm = new ReturnValueModel();

        //    var list = from a in _rep.Where<ProductTypeInfo>(s => s.TypeId == 1 && s.IsDeleted != 1)
        //               join b in _rep.All<ProAndDepRelation>() on a.Id equals b.ProductId
        //               into ab
        //               from bb in ab.DefaultIfEmpty()
        //               join c in _rep.All<DepartmentInfo>() on bb.DepartmentId equals c.Id
        //               group new { c } by new { a }
        //               into j
        //               select new ProductViewModel
        //               {
        //                   Departments = j.Select(s => s.c),
        //                   ProductInfo = j.Key.a
        //               };



        //    //不是管理员，只能看到自己创建的
        //    //if (!workUser.Roles.FirstOrDefault().RoleName.Equals("Admin"))
        //    //{
        //    //    list = list.Where(s => s.ProductInfo.CreateUser == workUser.User.Id);
        //    //}

        //    rvm.Msg = "success";
        //    rvm.Success = true;
        //    rvm.Result = new
        //    {
        //        list = list
        //    };

        //    return rvm;
        //}



        /// <summary>
        /// 新增或修改产品（作废）
        /// </summary>
        /// <param name="product"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateProduct(ProductTypeViewModel product, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var productinfo = _rep.FirstOrDefault<ProductTypeInfo>(s => s.Id == product.ProductInfo.Id);
            //var IsNeedCompleted = _rep.FirstOrDefault<Core.Domain.Models.DataModels.Configuration>(s => s.ConfigureName == "Examine" && s.IsDeleted != 1).ConfigureValue == 1 ? true : false;
            if (productinfo == null)
            {
                using (var tran = _rep.Database.BeginTransaction())
                {
                    try
                    {
                        product.ProductInfo.Id = Guid.NewGuid().ToString();
                        product.ProductInfo.CreateTime = DateTime.Now;
                        product.ProductInfo.CreateUser = workUser.User.Id;
                        _rep.Insert(product.ProductInfo);
                        _rep.SaveChanges();

                        ProAndDepRelation departmentRelation;
                        foreach (var item in product.Departments)
                        {
                            departmentRelation = new ProAndDepRelation();
                            departmentRelation.Id = Guid.NewGuid().ToString();
                            departmentRelation.CreateTime = DateTime.Now;
                            departmentRelation.CreateUser = workUser.User.Id;
                            departmentRelation.DepartmentId = item.Id;
                            departmentRelation.ProductId = product.ProductInfo.Id;
                            _rep.Insert(departmentRelation);
                            _rep.SaveChanges();
                        }

                        tran.Commit();

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                    }
                }
            }
            else
            {
                using (var tran = _rep.Database.BeginTransaction())
                {
                    try
                    {
                        productinfo.SubTitle = product.ProductInfo.SubTitle;
                        productinfo.SubTypeUrl = product.ProductInfo.SubTypeUrl;
                        productinfo.TypeId = product.ProductInfo.TypeId;
                        productinfo.IsCompleted = product.ProductInfo.IsCompleted;
                        productinfo.ContentDepType = product.ProductInfo.ContentDepType;
                        productinfo.Remark = product.ProductInfo.Remark;
                        productinfo.UpdateTime = DateTime.Now;
                        productinfo.UpdateUser = workUser.User.Id;
                        _rep.Update(productinfo);
                        _rep.SaveChanges();

                        var list = _rep.Where<ProAndDepRelation>(s => s.ProductId == productinfo.Id);
                        _rep.DeleteList(list);
                        _rep.SaveChanges();

                        ProAndDepRelation departmentRelation;
                        foreach (var item in product.Departments)
                        {
                            departmentRelation = new ProAndDepRelation();
                            departmentRelation.Id = Guid.NewGuid().ToString();
                            departmentRelation.CreateTime = DateTime.Now;
                            departmentRelation.CreateUser = workUser.User.Id;
                            departmentRelation.DepartmentId = item.Id;
                            departmentRelation.ProductId = productinfo.Id;
                            _rep.Insert(departmentRelation);
                            _rep.SaveChanges();
                        }

                        tran.Commit();

                    }
                    catch (Exception ex)
                    {
                        rvm.Msg = ex.Message;
                        rvm.Success = false;
                        tran.Rollback();
                        return rvm;
                    }
                }
            }

            rvm.Msg = "success";
            rvm.Success = true;

            return rvm;
        }

        /// <summary>
        /// 新增或修改资料信息
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public   ReturnValueModel  AddOrUpdateProductInfo(ProductInfoViewModel productInfoView, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var data = _rep.FirstOrDefault<DataInfo>(s => s.Id == productInfoView.dataInfo.Id);
           
            var approvalEnabled = _systemService.AdminApprovalEnabled; //是否启用审核

            if (data == null)
            {
                productInfoView.dataInfo.CreateTime = DateTime.Now;
                productInfoView.dataInfo.CreateUser = workUser.User.Id;
                productInfoView.dataInfo.IsCompleted = approvalEnabled ? EnumComplete.AddedUnapproved : EnumComplete.Approved;
            }
            else
            {
                if (data.IsDeleted == 1)
                {
                    rvm.Msg = "This DataInfo has been deleted.";
                    rvm.Success = false;
                    return rvm;
                }
                switch (data.IsCompleted ?? EnumComplete.AddedUnapproved)
                {
                    case EnumComplete.Approved:
                        if (approvalEnabled)
                        {
                            data.IsCompleted = EnumComplete.Locked;
                            _rep.Update(data);
                            _rep.SaveChanges();

                            //复制一条新数据
                            productInfoView.dataInfo.CreateTime = data.CreateTime;
                            productInfoView.dataInfo.CreateUser = data.CreateUser;
                            productInfoView.dataInfo.UpdateTime = DateTime.Now;
                            productInfoView.dataInfo.UpdateUser = workUser.User.Id;
                            productInfoView.dataInfo.IsCompleted = EnumComplete.UpdatedUnapproved;
                            productInfoView.dataInfo.OldId = data.Id;
                            data = null;
                        }
                        break;
                    case EnumComplete.Reject:
                    case EnumComplete.AddedUnapproved:
                    case EnumComplete.UpdatedUnapproved:
                        data.IsCompleted = string.IsNullOrEmpty(data.OldId) ? EnumComplete.AddedUnapproved : EnumComplete.UpdatedUnapproved;
                        break;
                    default:
                        rvm.Msg = "This DataInfo can not be changed at this time.";
                        rvm.Success = false;
                        return rvm;
                }
            }

            if (data == null)
            {
                using (var tran = _rep.Database.BeginTransaction())
                {
                    try
                    {
                        productInfoView.dataInfo.Id = Guid.NewGuid().ToString();
                        productInfoView.dataInfo.MediaType = 1;
                        productInfoView.dataInfo.ProductTypeInfoId = ""; 
                        _rep.Insert(productInfoView.dataInfo);
                        _rep.SaveChanges();

                        if (productInfoView.ProductAndDeps != null
                            && productInfoView.ProductAndDeps.BuNameList != null
                            && productInfoView.ProductAndDeps.Products != null
                            && productInfoView.ProductAndDeps.Departments != null)
                        {
                            MediaDataRel mediaDataRel;
                            foreach (var buName in productInfoView.ProductAndDeps.BuNameList)
                            {
                                foreach (var pro in productInfoView.ProductAndDeps.Products)
                                {
                                    foreach (var dep in productInfoView.ProductAndDeps.Departments)
                                    {
                                        mediaDataRel = new MediaDataRel();
                                        mediaDataRel.Id = Guid.NewGuid().ToString();
                                        mediaDataRel.DataInfoId = productInfoView.dataInfo.Id;
                                        mediaDataRel.ProId = pro.ProId;
                                        mediaDataRel.DeptId = dep.DeptId;
                                        mediaDataRel.BuName = buName;
                                        mediaDataRel.CreateTime = DateTime.Now;
                                        mediaDataRel.CreateUser = workUser.User.Id;
                                        

                                        _rep.Insert(mediaDataRel);
                                    }
                                }
                            }
                            _rep.SaveChanges();
                        }
                        tran.Commit();

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        rvm.Msg = ex.Message;
                        rvm.Success = false;
                        return rvm;
                    }
                }

            }
            else
            {
                using (var tran = _rep.Database.BeginTransaction())
                {
                    try
                    {
                        data.ProductTypeInfoId = "";
                        data.Title = productInfoView.dataInfo.Title;
                        data.DataContent = productInfoView.dataInfo.DataContent;
                        data.CompanyCode = productInfoView.dataInfo.CompanyCode;
                        data.DataOrigin = productInfoView.dataInfo.DataOrigin;
                        data.DataLink = productInfoView.dataInfo.DataLink;
                        data.DataType = productInfoView.dataInfo.DataType;
                        data.DataUrl = productInfoView.dataInfo.DataUrl;
                        data.IsRead = productInfoView.dataInfo.IsRead;
                        data.IsSelected = productInfoView.dataInfo.IsSelected;
                        data.MediaTime = productInfoView.dataInfo.MediaTime;
                        data.BuName = productInfoView.dataInfo.BuName;
                        data.Dept = productInfoView.dataInfo.Dept;
                        data.Product = productInfoView.dataInfo.Product;
                        data.Sort = productInfoView.dataInfo.Sort;
                        data.IsCopyRight = productInfoView.dataInfo.IsCopyRight;
                        data.IsChoiceness = productInfoView.dataInfo.IsChoiceness;
                        //data.ClickVolume = productInfoView.dataInfo.ClickVolume;
                        data.Sort = productInfoView.dataInfo.Sort;
                        data.UpdateTime = DateTime.Now;
                        data.UpdateUser = workUser.User.Id;
                        data.KnowImageUrl = productInfoView.dataInfo.KnowImageUrl;
                        data.KnowImageName = productInfoView.dataInfo.KnowImageName;
                        data.Remark = productInfoView.dataInfo.Remark;
                        _rep.Update(data);
                        _rep.SaveChanges();

                        if (productInfoView.ProductAndDeps != null
                            && productInfoView.ProductAndDeps.BuNameList != null
                            && productInfoView.ProductAndDeps.Products != null
                            && productInfoView.ProductAndDeps.Departments != null)
                        {
                            var mediaRelList = _rep.Where<MediaDataRel>(s => s.DataInfoId == productInfoView.dataInfo.Id);

                            foreach (var media in mediaRelList)
                            {
                                media.IsDeleted = 1;
                                _rep.Update(media);
                            }
                            MediaDataRel mediaDataRel;
                            foreach (var buName in productInfoView.ProductAndDeps.BuNameList)
                            {
                                foreach (var pro in productInfoView.ProductAndDeps.Products)
                                {
                                    foreach (var dep in productInfoView.ProductAndDeps.Departments)
                                    {
                                        mediaDataRel = new MediaDataRel();
                                        mediaDataRel.Id = Guid.NewGuid().ToString();
                                        mediaDataRel.DataInfoId = productInfoView.dataInfo.Id;
                                        mediaDataRel.ProId = pro.ProId;
                                        mediaDataRel.DeptId = dep.DeptId;
                                        mediaDataRel.BuName = buName;
                                        mediaDataRel.CreateTime = DateTime.Now;
                                        mediaDataRel.CreateUser = workUser.User.Id;

                                        _rep.Insert(mediaDataRel);
                                    }
                                }
                            }
                            _rep.SaveChanges();
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        rvm.Msg = "fail";
                        rvm.Success = false;
                        return rvm;
                    }
                }
            }

            //新增或变更 邮件发送
            if (workUser != null && IsSendMail == "1")
            {
                LoggerHelper.WarnInTimeTest("[产品资料 SendMeetMail开始]：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

                MailUtil.SendMeetMail(workUser?.User.ChineseName ?? "", "产品资料", productInfoView.dataInfo?.Title ?? "")
                            .ContinueWith((previousTask) =>
                            {
                                bool rCount = previousTask.Result;
                            });
                //MailUtil.SendMeetMail(workUser?.User.ChineseName ?? "", "产品资料", productInfoView.dataInfo?.Title ?? "");
                LoggerHelper.WarnInTimeTest("[产品资料 SendMeetMail结束]：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            }

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = data ?? productInfoView.dataInfo;

            return rvm;
        }

        /// <summary>
        /// 新增或修改学术知识
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateAcademic(DataInfo dataInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var data = _rep.FirstOrDefault<DataInfo>(s => s.Id == dataInfo.Id);
            if (data == null)
            {
                dataInfo.Id = Guid.NewGuid().ToString();
                dataInfo.DataType = "-1";
                dataInfo.CreateTime = DateTime.Now;

                dataInfo.CreateUser = workUser.User.Id;
                _rep.Insert(dataInfo);
                _rep.SaveChanges();
            }
            else
            {
                data.ProductTypeInfoId = dataInfo.ProductTypeInfoId;
                data.Title = dataInfo.Title;
                data.DataContent = dataInfo.DataContent;
                data.CompanyCode = dataInfo.CompanyCode;
                data.DataOrigin = dataInfo.DataOrigin;
                data.DataLink = dataInfo.DataLink;
                data.DataType = "-1";
                data.DataUrl = dataInfo.DataUrl;
                data.IsRead = dataInfo.IsRead;
                data.IsSelected = dataInfo.IsSelected;
                data.IsCopyRight = dataInfo.IsCopyRight;
                data.IsChoiceness = dataInfo.IsChoiceness;
                //data.ClickVolume = dataInfo.ClickVolume;
                data.MediaTime = dataInfo.MediaTime;
                data.UpdateTime = DateTime.Now;
                data.UpdateUser = workUser.User.Id;
                data.Sort = dataInfo.Sort;
                _rep.Update(data);
                _rep.SaveChanges();
            }
            rvm.Msg = "success";
            rvm.Success = true;

            return rvm;
        }
        /// <summary>
        /// 新增或修改临床指南
        /// </summary>
        /// <param name="fsysArticle"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateClinicalguidelines(FsysArticle fsysArticle, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var item = _rep.FirstOrDefault<FsysArticle>(s => s.Id == fsysArticle.Id && s.IsDeleted != 1);
            var deptNameList = _rep.Where<DepartmentInfo>(s => fsysArticle.DeptList.Contains(s.Id) && s.IsDeleted != 1).Select(x => x.DepartmentName).ToList();
                      
            //新增
            if (item == null)
            {
                fsysArticle.Id = Guid.NewGuid().ToString();
                fsysArticle.DepartmentId = string.Join(",", deptNameList);
                fsysArticle.CreateTime = DateTime.Now;
                fsysArticle.CreateUser = workUser.User.Id;
                _rep.Insert(fsysArticle);
            }
            else
            {
                item.ArticleIsHot = fsysArticle.ArticleIsHot;
                item.ArticleSort = fsysArticle.ArticleSort;
                item.ArticleSource = fsysArticle.ArticleSource;
                item.ArticleTitle= fsysArticle.ArticleTitle;
                item.ArticleUrl = fsysArticle.ArticleUrl;
                item.DepartmentId = string.Join(",", deptNameList);
                item.UpdateTime = DateTime.Now;
                item.UpdateUser = workUser.User.Id;
                _rep.Update(item);
            }
            var result= _rep.SaveChanges();
            if (result > 0)
            {
                rvm.Success = true;
                rvm.Msg = "success";
                rvm.Result = item ?? fsysArticle;
            }
            else
            {
                rvm.Success = false;
                rvm.Msg = "Failure to modify";
            }
            return rvm;
        }
        /// <summary>
        /// 新增或修改播客
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdatePodcast(ProductInfoViewModel productInfoView, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var data = _rep.FirstOrDefault<DataInfo>(s => s.Id == productInfoView.dataInfo.Id);
            var approvalEnabled = _systemService.AdminApprovalEnabled; //是否启用审核

            if (data == null)
            {
                productInfoView.dataInfo.CreateTime = DateTime.Now;
                productInfoView.dataInfo.CreateUser = workUser.User.Id;
                productInfoView.dataInfo.IsCompleted = approvalEnabled ? EnumComplete.AddedUnapproved : EnumComplete.Approved;
            }
            else
            {
                if (data.IsDeleted == 1)
                {
                    rvm.Msg = "This Podcast has been deleted.";
                    rvm.Success = false;
                    return rvm;
                }
                switch (data.IsCompleted ?? EnumComplete.AddedUnapproved)
                {
                    case EnumComplete.Approved:
                        if (approvalEnabled)
                        {
                            data.IsCompleted = EnumComplete.Locked;
                            _rep.Update(data);
                            _rep.SaveChanges();

                            //复制一条新数据
                            productInfoView.dataInfo.CreateTime = data.CreateTime;
                            productInfoView.dataInfo.CreateUser = data.CreateUser;
                            productInfoView.dataInfo.UpdateTime = DateTime.Now;
                            productInfoView.dataInfo.UpdateUser = workUser.User.Id;
                            productInfoView.dataInfo.OldId = data.Id;
                            productInfoView.dataInfo.IsCompleted = EnumComplete.UpdatedUnapproved;
                            data = null;
                        }
                        break;
                    case EnumComplete.Reject:
                    case EnumComplete.AddedUnapproved:
                    case EnumComplete.UpdatedUnapproved:
                        data.IsCompleted = string.IsNullOrEmpty(data.OldId) ? EnumComplete.AddedUnapproved : EnumComplete.UpdatedUnapproved;
                        break;
                    default:
                        rvm.Msg = "This Podcast can not be changed at this time.";
                        rvm.Success = false;
                        return rvm;
                }
            }

            if (data == null)
            {
                using (var tran = _rep.Database.BeginTransaction())
                {
                    try
                    {
                        productInfoView.dataInfo.Id = Guid.NewGuid().ToString();
                        productInfoView.dataInfo.MediaType = 3;
                        productInfoView.dataInfo.ProductTypeInfoId = "";

                      
                        _rep.Insert(productInfoView.dataInfo);
                        _rep.SaveChanges();

                        //添加知识库和产品和科室关系表
                        if (productInfoView.ProductAndDeps != null
                            && productInfoView.ProductAndDeps.BuNameList != null
                            && productInfoView.ProductAndDeps.Products != null
                            && productInfoView.ProductAndDeps.Departments != null)
                        {
                            var mediaRelList = _rep.Where<MediaDataRel>(s => s.DataInfoId == productInfoView.dataInfo.Id);

                            foreach (var media in mediaRelList)
                            {
                                media.IsDeleted = 1;
                                _rep.Update(media);
                            }
                            MediaDataRel mediaDataRel;
                            foreach (var buName in productInfoView.ProductAndDeps.BuNameList)
                            {
                                foreach (var pro in productInfoView.ProductAndDeps.Products)
                                {
                                    foreach (var dep in productInfoView.ProductAndDeps.Departments)
                                    {
                                        mediaDataRel = new MediaDataRel();
                                        mediaDataRel.Id = Guid.NewGuid().ToString();
                                        mediaDataRel.DataInfoId = productInfoView.dataInfo.Id;
                                        mediaDataRel.ProId = pro.ProId;
                                        mediaDataRel.DeptId = dep.DeptId;
                                        mediaDataRel.BuName = buName;
                                        mediaDataRel.CreateTime = DateTime.Now;
                                        mediaDataRel.CreateUser = workUser.User.Id;

                                        _rep.Insert(mediaDataRel);
                                    }
                                }
                            }
                            _rep.SaveChanges();
                        }
                        tran.Commit();

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        rvm.Msg = "fail";
                        rvm.Success = false;
                        return rvm;
                    }
                }

            }
            else
            {
                using (var tran = _rep.Database.BeginTransaction())
                {
                    try
                    {
                        data.ProductTypeInfoId = "";
                        data.Title = productInfoView.dataInfo.Title;
                        data.DataContent = productInfoView.dataInfo.DataContent;
                        data.CompanyCode = productInfoView.dataInfo.CompanyCode;
                        data.DataOrigin = productInfoView.dataInfo.DataOrigin;
                        data.DataLink = productInfoView.dataInfo.DataLink;
                        data.DataType = productInfoView.dataInfo.DataType;
                        data.DataUrl = productInfoView.dataInfo.DataUrl;
                        data.IsRead = productInfoView.dataInfo.IsRead;
                        data.IsSelected = productInfoView.dataInfo.IsSelected;
                        data.MediaTime = productInfoView.dataInfo.MediaTime;
                        data.BuName = productInfoView.dataInfo.BuName;
                        data.Dept = productInfoView.dataInfo.Dept;
                        data.Sort = productInfoView.dataInfo.Sort;
                        data.Product = productInfoView.dataInfo.Product;
                        data.IsCopyRight = productInfoView.dataInfo.IsCopyRight;
                        data.IsChoiceness = productInfoView.dataInfo.IsChoiceness;
                        //data.ClickVolume = productInfoView.dataInfo.ClickVolume;
                        data.KnowImageUrl = productInfoView.dataInfo.KnowImageUrl;
                        data.KnowImageName = productInfoView.dataInfo.KnowImageName;
                        
                        data.UpdateTime = DateTime.Now;
                        data.UpdateUser = workUser.User.Id;
                        data.Remark = productInfoView.dataInfo.Remark;
                        _rep.Update(data);
                        _rep.SaveChanges();
                        if (productInfoView.ProductAndDeps != null
                            && productInfoView.ProductAndDeps.BuNameList != null
                            && productInfoView.ProductAndDeps.Products != null
                            && productInfoView.ProductAndDeps.Departments != null)
                        {
                            var mediaRelList = _rep.Where<MediaDataRel>(s => s.DataInfoId == productInfoView.dataInfo.Id);

                            foreach (var media in mediaRelList)
                            {
                                media.IsDeleted = 1;
                                _rep.Update(media);
                            }
                            MediaDataRel mediaDataRel;
                            foreach (var buName in productInfoView.ProductAndDeps.BuNameList)
                            {
                                foreach (var pro in productInfoView.ProductAndDeps.Products)
                                {
                                    foreach (var dep in productInfoView.ProductAndDeps.Departments)
                                    {
                                        mediaDataRel = new MediaDataRel();
                                        mediaDataRel.Id = Guid.NewGuid().ToString();
                                        mediaDataRel.DataInfoId = productInfoView.dataInfo.Id;
                                        mediaDataRel.ProId = pro.ProId;
                                        mediaDataRel.DeptId = dep.DeptId;
                                        mediaDataRel.BuName = buName;
                                        mediaDataRel.CreateTime = DateTime.Now;
                                        mediaDataRel.CreateUser = workUser.User.Id;

                                        _rep.Insert(mediaDataRel);
                                    }
                                }
                            }
                            _rep.SaveChanges();
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        rvm.Msg = "fail";
                        rvm.Success = false;
                        return rvm;
                    }
                }
            }

            //新增或变更 邮件发送
            if (workUser != null && IsSendMail == "1")
            {
                MailUtil.SendMeetMail(workUser?.User.ChineseName ?? "", "播客资料", productInfoView.dataInfo?.Title ?? "")
                           .ContinueWith((previousTask) =>
                           {
                               bool rCount = previousTask.Result;
                           });

                //MailUtil.SendMeetMail(workUser?.User.ChineseName ?? "", "播客资料", productInfoView.dataInfo?.Title ?? "");
            }
            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = data ?? productInfoView.dataInfo;

            return rvm;
        }

        /// <summary>
        /// 删除资料
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel DeleteDataInfo(DataInfo dataInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var data = _rep.FirstOrDefault<DataInfo>(s => s.Id == dataInfo.Id);
            if (data == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid Id";
                return rvm;
            }

            if (data.IsDeleted == 1)
            {
                rvm.Msg = "This DataInfo has been deleted.";
                rvm.Success = true;
                return rvm;
            }

            var approvalEnabled = _systemService.AdminApprovalEnabled; //是否启用审核
            if (approvalEnabled && (data.MediaType == 1 || data.MediaType == 3))
            {
                switch (data.IsCompleted ?? EnumComplete.AddedUnapproved)
                {
                    case EnumComplete.Locked:
                        rvm.Success = false;
                        return rvm;
                    case EnumComplete.Approved:
                        data.IsCompleted = EnumComplete.WillDelete;  //将要删除（等待审核）
                        _rep.Update(data);
                        _rep.SaveChanges();
                        //待审核发送邮件提醒
                       
                        if (IsSendMail == "1")
                        {
                            MailUtil.SendMeetMail(workUser?.User.ChineseName ?? "", "资料", data.Title)
                                       .ContinueWith((previousTask) =>
                                       {
                                           bool rCount = previousTask.Result;
                                       });

                            //MailUtil.SendMeetMail(workUser?.User.ChineseName ?? "", "资料", data.Title);
                        }
                        break;
                    case EnumComplete.Reject:
                    case EnumComplete.AddedUnapproved:
                    case EnumComplete.UpdatedUnapproved:
                        //删除数据
                        if (!DoDataInfoDeletion(data, workUser))
                        {
                            rvm.Msg = "fail";
                            rvm.Success = false;
                            return rvm;
                        }
                        break;
                    default:
                        rvm.Msg = "This DataInfo can not be deleted at this time.";
                        rvm.Success = false;
                        return rvm;
                }
            }
            else
            {
                //删除数据
                if (!DoDataInfoDeletion(data, workUser))
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    return rvm;
                }
            }

            rvm.Msg = "success";
            rvm.Success = true;
            return rvm;
        }

        /// <summary>
        /// 从数据库删除数据 （逻辑删除）
        /// </summary>
        /// <param name="meetInfo"></param>
        /// <param name="workUser"></param>
        private bool DoDataInfoDeletion(DataInfo data, WorkUser workUser)
        {
            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {

                    data.IsDeleted = 1;
                    data.UpdateTime = DateTime.Now;
                    data.UpdateUser = workUser.User.Id;

                    if (!string.IsNullOrEmpty(data.OldId))
                    {
                        data.IsCompleted = EnumComplete.CanceledUpdate; //修改后的副本变为取消修改状态

                        //是否存在需要审核的其它记录
                        bool hasApprovingCopy = _rep.Table<DataInfo>().Any(s => s.IsDeleted != 1 && s.Id != data.Id && s.OldId == data.OldId);
                        if (!hasApprovingCopy)
                        {
                            //解锁原始数据
                            var originalDataInfo = _rep.FirstOrDefault<DataInfo>(s => s.IsDeleted != 1 && s.Id == data.OldId && s.IsCompleted == EnumComplete.Locked);
                            if (originalDataInfo != null)
                            {
                                originalDataInfo.IsCompleted = EnumComplete.Approved;
                                _rep.Update(originalDataInfo);
                            }
                        }
                    }

                    _rep.Update(data);

                    var mediaRelList = _rep.Where<MediaDataRel>(s => s.DataInfoId == data.Id);

                    foreach (var media in mediaRelList)
                    {
                        media.IsDeleted = 1;
                        _rep.Update(media);
                    }

                    _rep.SaveChanges();
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return false;
                }
            }
        }

        /// <summary>
        /// 删除产品、学术、播客（作废）
        /// </summary>
        /// <param name="product"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel DeleteProduct(ProductTypeInfo product, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var productinfo = _rep.FirstOrDefault<ProductTypeInfo>(s => s.Id == product.Id);
            var IsNeedCompleted = _rep.FirstOrDefault<Core.Domain.Models.DataModels.Configuration>(s => s.ConfigureName == "Examine" && s.IsDeleted != 1).ConfigureValue == "1" ? true : false;

            if (productinfo != null)
            {
                productinfo.IsDeleted = 1;
                productinfo.IsCompleted = 1;
                if (IsNeedCompleted)
                {
                    productinfo.IsCompleted = 2;
                    //string formatterMail = string.Format(MailUtil.Notice, "陈荣春", "IT201812210001", "汪赟", "test");
                    //MailUtil.SendMail(formatterMail, "测试邮件发送功能", "wangyun@deewing.com");
                }

                _rep.Update(productinfo);
                _rep.SaveChanges();
            }
            _rep.CommitAndRefreshChanges();
            rvm.Msg = "success";
            rvm.Success = true;

            return rvm;
        }

        /// <summary>
        /// 获取文档类型
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel GetDocumentType(RowNumModel<DocumentType> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = _rep.Where<DocumentType>(s => s.IsDeleted != 1).Where(rowNum.SearchParams);

            var total = list.Count();
            var rows = list.OrderByDescending(s => s.CreateTime).ToPaginationList(rowNum.PageIndex, rowNum.PageSize);

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total = total,
                rows = rows
            };

            return rvm;
        }

        /// <summary>
        /// 获取文档类型详情
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public ReturnValueModel GetDocumentInfoType(DocumentType document)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var item = _rep.FirstOrDefault<DocumentType>(s => s.Id == document.Id);
            if (item != null)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    item = item
                };
            }
            else
            {
                rvm.Msg = "fail";
                rvm.Success = false;
            }

            return rvm;
        }


        /// <summary>
        /// 新增或更新文档类型
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateDocumentType(DocumentType documentType, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var doc = _rep.FirstOrDefault<DocumentType>(s => s.Id == documentType.Id);
            if (doc == null)
            {
                documentType.Id = Guid.NewGuid().ToString();
                documentType.CreateTime = DateTime.Now;
                documentType.CreateUser = workUser.User.Id;
                _rep.Insert(documentType);
                _rep.SaveChanges();
            }
            else
            {
                doc.ImgUrl = documentType.ImgUrl;
                doc.TypeValue = documentType.TypeValue;
                doc.UpdateTime = DateTime.Now;
                doc.UpdateUser = workUser.User.Id;

                _rep.Update(doc);
                _rep.SaveChanges();
            }
            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                doc = doc
            };
            return rvm;
        }

        /// <summary>
        /// 删除文档类型
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel DeleteDocumentType(DocumentType documentType, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var doc = _rep.FirstOrDefault<DocumentType>(s => s.Id == documentType.Id);
            if (doc != null)
            {
                //doc.IsDeleted = 1;
                //doc.UpdateTime = DateTime.Now;
                //doc.UpdateUser = workUser.User.Id;
                //_rep.Update(doc);
                _rep.Delete(doc);
                _rep.SaveChanges();
            }

            rvm.Msg = "success";
            rvm.Success = true;

            return rvm;
        }
        /// <summary>
        /// 删除临床指南
        /// </summary>
        /// <param name="fsysArticle"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel DeleteClinicalguidelines(FsysArticle fsysArticle, WorkUser workUser) {
            ReturnValueModel rvm = new ReturnValueModel();
            var doc = _rep.FirstOrDefault<FsysArticle>(s => s.Id == fsysArticle.Id && s.IsDeleted != 1);
            if (doc != null)
            {
                doc.IsDeleted = 1;
                _rep.Update(doc);
              var send=_rep.SaveChanges();
                //判断是否删除
                if (send > 0)
                {
                    rvm.Success = true;
                    rvm.Msg = "success";
                    rvm.Result = doc;
                }
                else {
                    rvm.Success = false;
                    rvm.Msg = "Delete failed";
                }
                
            }
            else {
                rvm.Success = false;
                rvm.Msg = "Invalid data to be deleted";
            }
            return rvm;
        }
        /// <summary>
        /// 提交播客的审核结果
        /// </summary>
        /// <param name="approvalResult">审核结果</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel PodcastApproval(ApprovalResultViewModel approvalResult, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            //var isAdmin = workUser.Roles.FirstOrDefault().RoleName.Contains("Admin");
            var isAdmin = _commonService.IsAdmin(workUser);
            if (!isAdmin)
            {
                rvm.Msg = "You have no administrator permission.";
                rvm.Success = false;
                return rvm;
            }

            if (approvalResult == null || !approvalResult.Approved.HasValue || string.IsNullOrEmpty(approvalResult.Id))
            {
                rvm.Msg = "Invalid parameter.";
                rvm.Success = false;
                return rvm;
            }

            DataInfo modifiedDataInfo = null, originalDataInfo = null;
            modifiedDataInfo = _rep.FirstOrDefault<DataInfo>(s => s.Id == approvalResult.Id);
            if (modifiedDataInfo == null)
            {
                rvm.Msg = $"Invalid Podcast Id: {approvalResult.Id}";
                rvm.Success = false;
                return rvm;
            }

            switch (modifiedDataInfo.IsCompleted ?? EnumComplete.AddedUnapproved)
            {
                case EnumComplete.AddedUnapproved:
                case EnumComplete.UpdatedUnapproved:
                case EnumComplete.Reject:
                case EnumComplete.WillDelete:
                    break;
                default:
                    rvm.Msg = $"This Podcast is not unapproved.";
                    rvm.Success = false;
                    return rvm;
            }

            if (!string.IsNullOrEmpty(modifiedDataInfo.OldId))
            {
                originalDataInfo = _rep.FirstOrDefault<DataInfo>(s => s.Id == modifiedDataInfo.OldId);
                if (originalDataInfo == null)
                {
                    rvm.Msg = "Data is broken. Invalid original Podcast Id.";
                    rvm.Success = false;
                    return rvm;
                }
            }

            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {
                    switch (modifiedDataInfo.IsCompleted ?? EnumComplete.AddedUnapproved)
                    {
                        case EnumComplete.AddedUnapproved:
                        case EnumComplete.UpdatedUnapproved:
                        case EnumComplete.Reject:
                            if (approvalResult.Approved ?? false)
                            {
                                if (originalDataInfo == null)
                                {
                                    modifiedDataInfo.IsCompleted = EnumComplete.Approved; //审核通过
                                    _rep.Update(modifiedDataInfo);
                                }
                                else
                                {
                                    originalDataInfo.ProductTypeInfoId = "";
                                    originalDataInfo.Title = modifiedDataInfo.Title;
                                    originalDataInfo.DataContent = modifiedDataInfo.DataContent;
                                    originalDataInfo.CompanyCode = modifiedDataInfo.CompanyCode;
                                    originalDataInfo.DataOrigin = modifiedDataInfo.DataOrigin;
                                    originalDataInfo.DataLink = modifiedDataInfo.DataLink;
                                    originalDataInfo.DataType = modifiedDataInfo.DataType;
                                    originalDataInfo.DataUrl = modifiedDataInfo.DataUrl;
                                    originalDataInfo.IsRead = modifiedDataInfo.IsRead;
                                    originalDataInfo.IsSelected = modifiedDataInfo.IsSelected;
                                    originalDataInfo.MediaTime = modifiedDataInfo.MediaTime;
                                    originalDataInfo.BuName = modifiedDataInfo.BuName;
                                    originalDataInfo.Dept = modifiedDataInfo.Dept;
                                    originalDataInfo.Product = modifiedDataInfo.Product;
                                    originalDataInfo.Sort = modifiedDataInfo.Sort;
                                    originalDataInfo.IsCopyRight = modifiedDataInfo.IsCopyRight;
                                    originalDataInfo.IsChoiceness = modifiedDataInfo.IsChoiceness;
                                    originalDataInfo.KnowImageUrl= modifiedDataInfo.KnowImageUrl;
                                    //originalDataInfo.ClickVolume = modifiedDataInfo.ClickVolume;
                                    originalDataInfo.Sort = modifiedDataInfo.Sort;
                                    originalDataInfo.Remark = modifiedDataInfo.Remark;

                                    //转移子表关联 

                                    var mediaRelList = _rep.Where<MediaDataRel>(s => s.DataInfoId == originalDataInfo.Id && s.IsDeleted != 1);

                                    foreach (var media in mediaRelList)
                                    {
                                        media.IsDeleted = 1;
                                        _rep.Update(media);
                                    }

                                    mediaRelList = _rep.Where<MediaDataRel>(s => s.DataInfoId == modifiedDataInfo.Id && s.IsDeleted != 1);

                                    foreach (var item in mediaRelList)
                                    {
                                        item.DataInfoId = originalDataInfo.Id;
                                    }

                                    _rep.UpdateList(mediaRelList);

                                    modifiedDataInfo.IsCompleted = EnumComplete.Obsolete;  //已作废 
                                    modifiedDataInfo.IsDeleted = 1;
                                    _rep.Update(modifiedDataInfo);

                                    originalDataInfo.IsCompleted = EnumComplete.Approved; //审核通过
                                    originalDataInfo.UpdateUser = modifiedDataInfo.UpdateUser;
                                    originalDataInfo.UpdateTime = modifiedDataInfo.UpdateTime;
                                    _rep.Update(originalDataInfo);
                                }
                            }
                            else
                            {
                                modifiedDataInfo.IsCompleted = EnumComplete.Reject;  //审核拒绝
                                modifiedDataInfo.ApprovalNote = approvalResult.Note;
                                _rep.Update(modifiedDataInfo);
                            }
                            break;
                        case EnumComplete.WillDelete:
                            if (originalDataInfo == null)
                            {
                                if (approvalResult.Approved ?? false)
                                {
                                    //同意删除
                                    modifiedDataInfo.IsDeleted = 1;
                                    modifiedDataInfo.IsCompleted = EnumComplete.Deleted;
                                }
                                else
                                {
                                    //拒绝删除
                                    modifiedDataInfo.IsCompleted = EnumComplete.Approved;
                                }
                                _rep.Update(modifiedDataInfo);
                            }
                            break;
                    }

                    _rep.SaveChanges();

                    tran.Commit();
                    rvm.Msg = "success";
                    rvm.Success = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    rvm.Msg = "fail";
                    rvm.Success = false;
                }
            }
            return rvm;
        }

        /// <summary>
        /// 提交产品资料的审核结果
        /// </summary>
        /// <param name="approvalResult">审核结果</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel ProductDataApproval(ApprovalResultViewModel approvalResult, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            //var isAdmin = workUser.Roles.FirstOrDefault().RoleName.Contains("Admin");
            var isAdmin = _commonService.IsAdmin(workUser);
            if (!isAdmin)
            {
                rvm.Msg = "You have no administrator permission.";
                rvm.Success = false;
                return rvm;
            }

            if (approvalResult == null || !approvalResult.Approved.HasValue || string.IsNullOrEmpty(approvalResult.Id))
            {
                rvm.Msg = "Invalid parameter.";
                rvm.Success = false;
                return rvm;
            }

            DataInfo modifiedDataInfo = null, originalDataInfo = null;
            modifiedDataInfo = _rep.FirstOrDefault<DataInfo>(s => s.Id == approvalResult.Id);
            if (modifiedDataInfo == null)
            {
                rvm.Msg = $"Invalid DataInfo Id: {approvalResult.Id}";
                rvm.Success = false;
                return rvm;
            }

            switch (modifiedDataInfo.IsCompleted ?? EnumComplete.AddedUnapproved)
            {
                case EnumComplete.AddedUnapproved:
                case EnumComplete.UpdatedUnapproved:
                case EnumComplete.Reject:
                case EnumComplete.WillDelete:
                    break;
                default:
                    rvm.Msg = $"This DataInfo is not unapproved.";
                    rvm.Success = false;
                    return rvm;
            }

            

            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {

                    switch (modifiedDataInfo.IsCompleted ?? EnumComplete.AddedUnapproved)
                    {
                        case EnumComplete.AddedUnapproved:
                        case EnumComplete.UpdatedUnapproved:
                        case EnumComplete.Reject:
                            if (approvalResult.Approved ?? false)
                            {
                                if (originalDataInfo == null)
                                {
                                    modifiedDataInfo.IsCompleted = EnumComplete.Approved; //审核通过
                                    _rep.Update(modifiedDataInfo);
                                }
                                else
                                {
                                    originalDataInfo.ProductTypeInfoId = "";
                                    originalDataInfo.Title = modifiedDataInfo.Title;
                                    originalDataInfo.DataContent = modifiedDataInfo.DataContent;
                                    originalDataInfo.CompanyCode = modifiedDataInfo.CompanyCode;
                                    originalDataInfo.DataOrigin = modifiedDataInfo.DataOrigin;
                                    originalDataInfo.DataLink = modifiedDataInfo.DataLink;
                                    originalDataInfo.DataType = modifiedDataInfo.DataType;
                                    originalDataInfo.DataUrl = modifiedDataInfo.DataUrl;
                                    originalDataInfo.IsRead = modifiedDataInfo.IsRead;
                                    originalDataInfo.IsSelected = modifiedDataInfo.IsSelected;
                                    originalDataInfo.MediaTime = modifiedDataInfo.MediaTime;
                                    originalDataInfo.BuName = modifiedDataInfo.BuName;
                                    originalDataInfo.Dept = modifiedDataInfo.Dept;
                                    originalDataInfo.Product = modifiedDataInfo.Product;
                                    originalDataInfo.Sort = modifiedDataInfo.Sort;
                                    originalDataInfo.IsCopyRight = modifiedDataInfo.IsCopyRight;
                                    originalDataInfo.IsChoiceness = modifiedDataInfo.IsChoiceness;
                                    //originalDataInfo.ClickVolume = modifiedDataInfo.ClickVolume;
                                    originalDataInfo.Sort = modifiedDataInfo.Sort;
                                    originalDataInfo.Remark = modifiedDataInfo.Remark;

                                    //转移子表关联 
                                    var mediaRelList = _rep.Where<MediaDataRel>(s => s.DataInfoId == originalDataInfo.Id && s.IsDeleted != 1);

                                    foreach (var media in mediaRelList)
                                    {
                                        media.IsDeleted = 1;
                                        _rep.Update(media);
                                    }

                                    mediaRelList = _rep.Where<MediaDataRel>(s => s.DataInfoId == modifiedDataInfo.Id && s.IsDeleted != 1);

                                    foreach (var item in mediaRelList)
                                    {
                                        item.DataInfoId = originalDataInfo.Id;
                                    }

                                    _rep.UpdateList(mediaRelList);

                                    modifiedDataInfo.IsCompleted = EnumComplete.Obsolete;  //已作废 
                                    modifiedDataInfo.IsDeleted = 1;
                                    _rep.Update(modifiedDataInfo);

                                    originalDataInfo.IsCompleted = EnumComplete.Approved; //审核通过
                                    originalDataInfo.UpdateUser = modifiedDataInfo.UpdateUser;
                                    originalDataInfo.UpdateTime = modifiedDataInfo.UpdateTime;
                                    _rep.Update(originalDataInfo);
                                }
                            }
                            else
                            {
                                modifiedDataInfo.IsCompleted = EnumComplete.Reject;  //审核拒绝
                                modifiedDataInfo.ApprovalNote = approvalResult.Note;
                                _rep.Update(modifiedDataInfo);
                            }
                            break;
                        case EnumComplete.WillDelete:
                            if (originalDataInfo == null)
                            {
                                if (approvalResult.Approved ?? false)
                                {
                                    //同意删除
                                    modifiedDataInfo.IsDeleted = 1;
                                    modifiedDataInfo.IsCompleted = EnumComplete.Deleted;
                                }
                                else
                                {
                                    //拒绝删除
                                    modifiedDataInfo.IsCompleted = EnumComplete.Approved;
                                }
                                _rep.Update(modifiedDataInfo);
                            }
                            break;
                    }

                    _rep.SaveChanges();

                    tran.Commit();
                    rvm.Msg = "success";
                    rvm.Success = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    rvm.Msg = "fail";
                    rvm.Success = false;
                }
            }
            return rvm;
        }

        /// <summary>
        /// [分页]赞同不赞同 人员列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel ApproveProductList(RowNumModel<ProductInfoLike> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var list = _rep.Where<ProductInfoLike>(x => x.ProID == rowNum.SearchParams.ProID && x.IsLike == rowNum.SearchParams.IsLike);
            var total = list.Count();
            var rows = list.OrderByDescending(s => s.CreateTime).ToPaginationList(rowNum.PageIndex, rowNum.PageSize);
            var data = rows.Join(_rep.Where<WxUserModel>(x => true), x => x.CreateUser, y => y.UnionId, (x, y) => new {y.UnionId, y.WxName, y.UserName, x.CreateTime }).ToList();
            rvm.Success = true;
            rvm.Result = new
            {
                total,
                rows = data.GroupBy(x => x.UnionId).Select(x=>x.FirstOrDefault()).ToList()
            };
            return rvm;
        }
    }
}
