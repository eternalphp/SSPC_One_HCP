using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Enums;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Bot.Dto;
using SSPC_One_HCP.Services.Services.WeChat.Dto;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace SSPC_One_HCP.Services.Services.WeChat.Implementations
{
    public class WcHcpDataInfoService : IWcHcpDataInfoService
    {
        private readonly IEfRepository _rep;
        public WcHcpDataInfoService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 获取目录名称
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetFileList(string buName, WorkUser workUser)
        {
            buName = string.IsNullOrEmpty(buName) ? "PN" : buName;
            ReturnValueModel rvm = new ReturnValueModel();
            var data = _rep.Where<HcpCatalogueManage>(s => s.BuName == buName && s.IsDeleted == 0);
            List<HcpCatalogueManageOutDto> list = new List<HcpCatalogueManageOutDto>();
            foreach (var item in data.ToList())
            {
                list.Add(new HcpCatalogueManageOutDto()
                {
                    Id = item.Id,
                    BuName = item.BuName,
                    CatalogueName = item.CatalogueName,
                    IsNew = GetIsNew(item.Id),
                    CreateTime = item.CreateTime,
                });
            }
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = list.OrderByDescending(o => o.CreateTime).ToList();
            return rvm;
        }
        /// <summary>
        /// 分页查询资料列表
        /// </summary>
        /// <param name="row"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetDataPageList(RowNumModel<WeChatHcpDataInfoInputDto> row, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            IQueryable<HcpDataCatalogueRel> dataCatalogueRel = _rep.Where<HcpDataCatalogueRel>(o => o.IsDeleted != 1);
            if (row.SearchParams != null && !string.IsNullOrEmpty(row.SearchParams.HcpCatalogueManageId))
            {
                dataCatalogueRel = dataCatalogueRel.Where(o => o.HcpCatalogueManageId == row.SearchParams.HcpCatalogueManageId);
            }
            var data = (from a in dataCatalogueRel
                        join b in _rep.All<HcpDataInfo>() on a.HcpDataInfoId equals b.Id
                        join c in _rep.Table<UserModel>() on a.CreateUser equals c.Id
                        into ab
                        from bb in ab.DefaultIfEmpty()
                        join d in _rep.All<DocumentType>() on b.DataType equals d.Id
                        where b.IsDeleted != 1
                        select new HcpDataInfoOutDto
                        {
                            Id = b.Id,
                            IsDeleted = b.IsDeleted,
                            IsEnabled = b.IsEnabled,
                            CreateTime = b.CreateTime,
                            UpdateTime = b.UpdateTime,
                            UpdateUser = b.UpdateUser,
                            CreateUser = bb.ChineseName,
                            CreateUserID = b.CreateUser,
                            CreateUserADAccount = bb.ADAccount,
                            CompanyCode = b.CompanyCode,
                            Remark = b.Remark,
                            ProductTypeInfoId = b.ProductTypeInfoId,
                            Title = b.Title,
                            DataContent = b.DataContent,
                            DataType = b.DataType,
                            DataTypeValue = d.TypeValue,
                            DataOrigin = b.DataOrigin,
                            DataLink = b.DataLink,
                            DataUrl = b.DataUrl,
                            IsRead = b.IsRead,
                            IsSelected = b.IsSelected,
                            IsCopyRight = b.IsCopyRight,
                            IsChoiceness = b.IsChoiceness,
                            IsHot = b.ClickVolume > 500 ? 1 : 0,
                            ClickVolume = b.ClickVolume,
                            BuName = b.BuName,
                            Dept = b.Dept,
                            Sort = b.Sort,
                            Product = b.Product,
                            MediaTime = b.MediaTime,
                            IsCompleted = b.IsCompleted ?? EnumComplete.AddedUnapproved,
                            ApprovalNote = b.ApprovalNote,
                            IsDownload = b.IsDownload,
                        });



            if (row.SearchParams != null && !string.IsNullOrEmpty(row.SearchParams.Title))
            {
                data = data.Where(o => o.Title.Contains(row.SearchParams.Title));
            }
            var total = data.Count();
            var rows = data.OrderBy(s => s.Sort).ThenByDescending(s => s.CreateTime).ToPaginationList(row.PageIndex, row.PageSize);
            var res = rows.ToList();
            string _host = ConfigurationManager.AppSettings["HostUrl"];


            var rowlist = res.Select(x => new HcpDataInfoOutDto
            {
                Id = x.Id,
                IsDeleted = x.IsDeleted,
                IsEnabled = x.IsEnabled,
                CreateTime = x.CreateTime.Value.AddHours(8),
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
                ALastName = x.DataUrl.Substring(x.DataUrl.LastIndexOf(".") + 1, (x.DataUrl.Length - x.DataUrl.LastIndexOf(".") - 1)), //扩展名
                DownloadUrl = $"{_host}{"/web/Download/Index?UserId="}{workUser.WxSaleUser.Id}{"&DataInfoId="}{x.Id}",
                LikeCount = _rep.Where<ProductInfoLike>(y => y.ProID.Equals(x.Id) && y.IsLike == 1).Count(),
                UNLikeCount = _rep.Where<ProductInfoLike>(y => y.ProID.Equals(x.Id) && y.IsLike == 2).Count(),
                IsNew = x.CreateTime.Value >= DateTime.Now.AddHours(-12) ? true : false,
            }).ToList();
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
        /// 根据ID查询数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetData(string id, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }
            var data = _rep.FirstOrDefault<HcpDataInfo>(o => o.Id == id && o.IsDeleted == 0);
            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = data;
            return rvm;
        }
        /// <summary>
        /// 预览PDF
        /// </summary>qy
        /// <returns></returns>
        public FileInfo PreviewPdf(string id)
        {
            var data = _rep.FirstOrDefault<HcpDataInfo>(o => o.Id == id && o.IsDeleted == 0);
            if (data != null)
            {
                var path = HostingEnvironment.MapPath("/" + data.DataUrl);
                return new FileInfo(path);
            }
            return null;
        }

        /// <summary>
        /// 获取文件Title
        /// </summary>
        /// <param name="row"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetTitle(string id)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }
            var data = _rep.FirstOrDefault<HcpDataInfo>(o => o.Id == id && o.IsDeleted == 0);
            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = data?.Title;
            return rvm;
        }
        /// <summary>
        /// 资料是否显示红点
        /// </summary>
        /// <param name="buName"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetRedDot(string buName, WorkUser workUser)
        {
            buName = string.IsNullOrEmpty(buName) ? "PN" : buName;
            ReturnValueModel rvm = new ReturnValueModel();
            var data = _rep.Where<HcpCatalogueManage>(s => s.BuName == buName && s.IsDeleted == 0);
            List<HcpCatalogueManageOutDto> list = new List<HcpCatalogueManageOutDto>();
            foreach (var item in data)
            {
                var isNew = GetIsNew(item.Id);
                if (isNew)
                {
                    rvm.Success = true;
                    rvm.Msg = "";
                    rvm.Result = true;
                    return rvm;
                }
            }
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = false;
            return rvm;
        }


        private bool GetIsNew(string hcpCatalogueManageId)
        {
            var dt = DateTime.Now.AddHours(-12);

            var data = (from a in _rep.All<HcpDataCatalogueRel>()
                        join b in _rep.All<HcpDataInfo>() on a.HcpDataInfoId equals b.Id
                        where a.HcpCatalogueManageId == hcpCatalogueManageId
                        && b.CreateTime.Value >= dt
                        && b.IsDeleted != 1
                        select new
                        {
                            a,
                            b
                        }).Any();

            return data;
        }
    }
}
