using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Enums;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Services.HCP.Dto;
using SSPC_One_HCP.Services.Services.HCP.Interfaces;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace SSPC_One_HCP.Services.Services.HCP.Implementations
{
    public class DocumentManagerService : IDocumentManagerService
    {
        private readonly IEfRepository _rep;
        private readonly ISystemService _systemService;
        private readonly ICommonService _commonService;
        private readonly string DownloadUrl = ConfigurationManager.AppSettings["dLoadUrl"];
        private readonly string IsSendMail = ConfigurationManager.AppSettings["IsSendMail"];
        public DocumentManagerService(IEfRepository rep, ICommonService commonService, ISystemService systemService)
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
        public ReturnValueModel GetList(RowNumModel<DocumentManagerSearchInputDto> rowNum, WorkUser workUser)
        {
            var isAdmin = _commonService.IsAdmin(workUser);

            ReturnValueModel rvm = new ReturnValueModel();
            var list = (from a in _rep.Where<DocumentManager>(s => s.IsDeleted != 1 && s.MediaType == 1
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
                        select new HcpDataInfoOutDto
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
                            ApprovalNote = a.ApprovalNote,
                            IsDownload = a.IsDownload,
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

            var rowlist = rows.ToList().Select(x => new DocumentManagerOutDto
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
                IsDownload = x.IsDownload,
                LikeCount = _rep.Where<ProductInfoLike>(y => y.ProID.Equals(x.Id) && y.IsLike == 1).Count(),
                UNLikeCount = _rep.Where<ProductInfoLike>(y => y.ProID.Equals(x.Id) && y.IsLike == 2).Count(),
                ProIds = _rep.Where<DocumentManagerRel>(y => y.DataInfoId == x.Id && y.IsDeleted == 0).Select(y => y.ProId).Distinct().ToList(),
                //CatalogueNameList = _rep.Where<HcpDataCatalogueRel>(y => y.HcpDataInfoId == x.Id).Select(y => y.HcpCatalogueManageName).ToList()
            });
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
        /// 新增或修改资料信息
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdate(DocumentManagerInputDto productInfoView, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var data = _rep.FirstOrDefault<DocumentManager>(s => s.Id == productInfoView.dataInfo.Id);

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
                var dataTitle = _rep.FirstOrDefault<DocumentManager>(s => s.Title == productInfoView.dataInfo.Title && s.IsDeleted == 0);
                if (dataTitle != null)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Msg = $"已存在【{productInfoView.dataInfo.Title}】的资料";
                    return rvm;
                }

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
                            DocumentManagerRel documentManagerRel;
                            foreach (var buName in productInfoView.ProductAndDeps.BuNameList)
                            {
                                foreach (var pro in productInfoView.ProductAndDeps.Products)
                                {
                                    foreach (var dep in productInfoView.ProductAndDeps.Departments)
                                    {
                                        documentManagerRel = new DocumentManagerRel();
                                        documentManagerRel.Id = Guid.NewGuid().ToString();
                                        documentManagerRel.DataInfoId = productInfoView.dataInfo.Id;
                                        documentManagerRel.ProId = pro.ProId;
                                        documentManagerRel.DeptId = dep.DeptId;
                                        documentManagerRel.BuName = buName;
                                        documentManagerRel.CreateTime = DateTime.Now;
                                        documentManagerRel.CreateUser = workUser.User.Id;


                                        _rep.Insert(documentManagerRel);
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

                var dataTitle = _rep.FirstOrDefault<DocumentManager>(s => s.Title == productInfoView.dataInfo.Title && s.IsDeleted == 0 && s.Id != productInfoView.dataInfo.Id);
                if (dataTitle != null)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    rvm.Msg = $"已存在【{productInfoView.dataInfo.Title}】的资料";
                    return rvm;
                }

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
                        data.IsDownload = productInfoView.dataInfo.IsDownload;
                        _rep.Update(data);
                        _rep.SaveChanges();


                        if (productInfoView.ProductAndDeps != null
                            && productInfoView.ProductAndDeps.BuNameList != null
                            && productInfoView.ProductAndDeps.Products != null
                            && productInfoView.ProductAndDeps.Departments != null)
                        {
                            var mediaRelList = _rep.Where<DocumentManagerRel>(s => s.DataInfoId == productInfoView.dataInfo.Id);

                            foreach (var media in mediaRelList)
                            {
                                _rep.Delete(media);
                            }
                            DocumentManagerRel mediaDataRel;
                            foreach (var buName in productInfoView.ProductAndDeps.BuNameList)
                            {
                                foreach (var pro in productInfoView.ProductAndDeps.Products)
                                {
                                    foreach (var dep in productInfoView.ProductAndDeps.Departments)
                                    {
                                        mediaDataRel = new DocumentManagerRel();
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
            //if (workUser != null && IsSendMail == "1")
            //{
            //    LoggerHelper.WarnInTimeTest("[产品资料 SendMeetMail开始]：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

            //    MailUtil.SendMeetMail(workUser?.User.ChineseName ?? "", "产品资料", productInfoView.dataInfo?.Title ?? "")
            //                .ContinueWith((previousTask) =>
            //                {
            //                    bool rCount = previousTask.Result;
            //                });
            //    //MailUtil.SendMeetMail(workUser?.User.ChineseName ?? "", "产品资料", productInfoView.dataInfo?.Title ?? "");
            //    LoggerHelper.WarnInTimeTest("[产品资料 SendMeetMail结束]：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            //}

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
        public ReturnValueModel Delete(DocumentManager documentManager, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var data = _rep.FirstOrDefault<DocumentManager>(s => s.Id == documentManager.Id);
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
                        if (!DocumentManagerDeletion(data, workUser))
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
                if (!DocumentManagerDeletion(data, workUser))
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
        bool DocumentManagerDeletion(DocumentManager data, WorkUser workUser)
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
                        bool hasApprovingCopy = _rep.Table<DocumentManager>().Any(s => s.IsDeleted != 1 && s.Id != data.Id && s.OldId == data.OldId);
                        if (!hasApprovingCopy)
                        {
                            //解锁原始数据
                            var originalDataInfo = _rep.FirstOrDefault<DocumentManager>(s => s.IsDeleted != 1 && s.Id == data.OldId && s.IsCompleted == EnumComplete.Locked);
                            if (originalDataInfo != null)
                            {
                                originalDataInfo.IsCompleted = EnumComplete.Approved;
                                _rep.Update(originalDataInfo);
                            }
                        }
                    }

                    _rep.Update(data);

                    var mediaRelList = _rep.Where<DocumentManagerRel>(s => s.DataInfoId == data.Id);

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
        /// 预览PDF
        /// </summary>
        /// <returns></returns>
        public FileInfo PreviewPdf(string id)
        {
            var data = _rep.FirstOrDefault<DocumentManager>(o => o.Id == id && o.IsDeleted == 0);
            if (data != null)
            {
                var path = HostingEnvironment.MapPath("/" + data.DataUrl);
                return new FileInfo(path);
            }
            return null;
        }
    }
}
