using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Services.HCP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.HCP.Implementations
{
    /// <summary>
    /// 横幅管理
    /// </summary>
    public class HcpBannerInfoService : IHcpBannerInfoService
    {
        private readonly IEfRepository _rep;
        public HcpBannerInfoService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 横幅管理-获取表列
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetList(RowNumModel<BannerInfo> rowNum, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var query = _rep.Where<BannerInfo>(o => o.IsDeleted != 1);
            if (rowNum?.SearchParams != null)
            {
                if (rowNum?.SearchParams?.Title != null)
                {
                    query = query.Where(o=>o.Title.Contains(rowNum.SearchParams.Title));
                }
                if (rowNum?.SearchParams?.Scene != null)
                {
                    query = query.Where(o => o.Scene== rowNum.SearchParams.Scene);
                }
            }
            var total = query.Count();
            var rows = query.OrderByDescending(o => o.CreateTime).ToPaginationList(rowNum.PageIndex, rowNum.PageSize);

            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                total = total,
                rows = rows,
            };

            return rvm;
        }
        /// <summary>
        /// 横幅管理- ID获取明细
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetById(string id, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }
            var bannerInfo = _rep.FirstOrDefault<BannerInfo>(o => o.Id == id && o.IsDeleted == 0);

            List<BannerInfoItem> bannerInfoItem = new List<BannerInfoItem>();
            if (bannerInfo != null)
            {
                bannerInfoItem = _rep.Where<BannerInfoItem>(o => o.BannerInfoId == bannerInfo.Id && o.IsDeleted == 0).OrderBy(o=>o.Sort).ToList();

            }

            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                bannerInfo,
                bannerInfoItem,
            };
            return rvm;
        }

        /// <summary>
        /// 横幅管理- 新增或修改
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdate(BannerInfo dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            BannerInfo bannerInfo = null;

            bannerInfo = _rep.FirstOrDefault<BannerInfo>(o => o.Id == dto.Id && o.IsDeleted == 0);
            if (bannerInfo == null)
            {
                var scene = _rep.Where<BannerInfo>(o => o.Scene == dto.Scene && o.IsDeleted == 0).Any();
                if (scene)
                {
                    rvm.Success = false;
                    rvm.Msg = $"{dto.Scene}已存在";
                    return rvm;
                }
                bannerInfo = new BannerInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = dto.Title,
                    Describe = dto.Describe,
                    Scene = dto.Scene,
                    IsShow = dto.IsShow,
                    Icon = dto.Icon,
                    CreateTime = DateTime.Now,
                    CreateUser = workUser.User.Id,
                };
                _rep.Insert(bannerInfo);
            }
            else
            {
                bannerInfo.Title = dto.Title;
                bannerInfo.Describe = dto.Describe;
                bannerInfo.Scene = dto.Scene;
                bannerInfo.IsShow = dto.IsShow;
                bannerInfo.Icon = dto.Icon;
                bannerInfo.UpdateTime = DateTime.UtcNow.AddHours(8);
                bannerInfo.UpdateUser = workUser.User.Id;
                _rep.Update(bannerInfo);
            }

            _rep.SaveChanges();

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = bannerInfo;
            return rvm;
        }


        public ReturnValueModel AddOrUpdateBannerInfoItem(BannerInfoItem dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            BannerInfoItem bannerInfoItem = null;

            bannerInfoItem = _rep.FirstOrDefault<BannerInfoItem>(o => o.Id == dto.Id && o.IsDeleted == 0);
            if (bannerInfoItem == null)
            {

                bannerInfoItem = new BannerInfoItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    BannerInfoId = dto.BannerInfoId,
                    Title = dto.Title,
                    Describe = dto.Describe,
                    Type = dto.Type,
                    ShowPlace = dto.ShowPlace,
                    Src = dto.Src,
                    Sort = dto.Sort,
                    CreateTime = DateTime.Now,
                    CreateUser = workUser.User.Id,
                };
                _rep.Insert(bannerInfoItem);
            }
            else
            {
                bannerInfoItem.BannerInfoId = dto.BannerInfoId;
                bannerInfoItem.Title = dto.Title;
                bannerInfoItem.Describe = dto.Describe;
                bannerInfoItem.Type = dto.Type;
                bannerInfoItem.ShowPlace = dto.ShowPlace;
                bannerInfoItem.Src = dto.Src;
                bannerInfoItem.Sort = dto.Sort;
                bannerInfoItem.UpdateTime = DateTime.UtcNow.AddHours(8);
                bannerInfoItem.UpdateUser = workUser.User.Id;
                _rep.Update(bannerInfoItem);
            }

            _rep.SaveChanges();

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = bannerInfoItem;
            return rvm;
        }

        /// <summary>
        /// 横幅管理- 删除
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel Delete(BannerInfo dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(dto?.Id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }
            var bannerInfo = _rep.FirstOrDefault<BannerInfo>(o => o.Id == dto.Id && o.IsDeleted == 0);
            bannerInfo.IsDeleted = 1;
            bannerInfo.UpdateTime = DateTime.UtcNow.AddHours(8);
            bannerInfo.UpdateUser = workUser.User.Id;
            _rep.Update(bannerInfo);
            _rep.SaveChanges();
            rvm.Success = true;
            rvm.Msg = "success";
            return rvm;
        }

    
        public ReturnValueModel DeleteBannerInfoItem(BannerInfoItem dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(dto?.Id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }
            var bannerInfoItem = _rep.FirstOrDefault<BannerInfoItem>(o => o.Id == dto.Id && o.IsDeleted == 0);
            bannerInfoItem.IsDeleted = 1;
            bannerInfoItem.UpdateTime = DateTime.UtcNow.AddHours(8);
            bannerInfoItem.UpdateUser = workUser.User.Id;
            _rep.Update(bannerInfoItem);
            _rep.SaveChanges();
            rvm.Success = true;
            rvm.Msg = "success";
            return rvm;
        }
    }
}
