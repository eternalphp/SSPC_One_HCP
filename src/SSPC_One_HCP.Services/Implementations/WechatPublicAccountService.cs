using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Linq;

namespace SSPC_One_HCP.Services.Implementations
{
    public class WechatPublicAccountService : IWechatPublicAccountService
    {
        private readonly IEfRepository _rep;

        public WechatPublicAccountService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// 新增或修改微信公众号记录
        /// </summary>
        /// <param name="accountInfo">微信公众号信息</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateWechatPublicAccount(WechatPublicAccount accountInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var item = _rep.FirstOrDefault<WechatPublicAccount>(s => s.Id == accountInfo.Id);
            if (item != null)
            {
                item.AppId = accountInfo.AppId;
                item.Name = accountInfo.Name;
                item.Summary = accountInfo.Summary;
                item.UpdateTime = DateTime.Now;
                item.UpdateUser = workUser.User.Id;
                _rep.Update(item);
                _rep.SaveChanges();
            }
            else
            {
                accountInfo.Id = Guid.NewGuid().ToString();
                accountInfo.ClickVolume = 0;
                accountInfo.CreateTime = DateTime.Now;
                accountInfo.CreateUser = workUser.User.Id;
                _rep.Insert(accountInfo);
                _rep.SaveChanges();
            }
            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = accountInfo;

            return rvm;
        }

        /// <summary>
        /// 获取微信公众号列表
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetWechatPublicAccountList(RowNumModel<WechatPublicAccount> rowNum, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var wx = _rep.Where<WechatPublicAccount>(s => s.IsDeleted != 1);

            if (rowNum != null && rowNum.SearchParams != null)
            {
                if (!string.IsNullOrEmpty(rowNum.SearchParams.Name))
                {
                    wx = wx.Where(s => !string.IsNullOrEmpty(s.Name) && s.Name.Contains(rowNum.SearchParams.Name));
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.Summary))
                {
                    wx = wx.Where(s => !string.IsNullOrEmpty(s.Summary) && s.Name.Contains(rowNum.SearchParams.Summary));
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.AppId))
                {
                    wx = wx.Where(s => !string.IsNullOrEmpty(s.AppId) && s.AppId == rowNum.SearchParams.AppId);
                }
                if (rowNum.SearchParams.ClickVolume.HasValue && rowNum.SearchParams.ClickVolume > 0)
                {
                    wx = wx.Where(s => s.ClickVolume.HasValue && s.ClickVolume >= rowNum.SearchParams.ClickVolume);
                }
            }

            var list = from a in wx
                       select new
                       {
                           Id = a.Id,
                           AppId = a.AppId,
                           Name = a.Name,
                           ClickVolume = a.ClickVolume,
                       };

            var total = list.Count();
            list = list.OrderBy(s => s.Name).ToPaginationList(rowNum.PageIndex, rowNum.PageSize);

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total,
                list
            };

            return rvm;
        }

        /// <summary>
        /// 获取单个微信公众号信息
        /// </summary>
        /// <param name="accountInfo">微信公众号信息</param>
        /// <returns></returns>
        public ReturnValueModel GetWechatPublicAccountInfo(WechatPublicAccount accountInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var account = _rep.FirstOrDefault<WechatPublicAccount>(s => s.Id == accountInfo.Id);

            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = account;

            return rvm;
        }

        /// <summary>
        /// 删除微信公众号记录
        /// </summary>
        /// <param name="accountInfo">微信公众号信息</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel DeleteWechatPublicAccount(WechatPublicAccount accountInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (accountInfo == null)
            { 
                rvm.Success = false;
                rvm.Msg = "Invalid parameters.";
                return rvm;   
            }

            var account = _rep.FirstOrDefault<WechatPublicAccount>(s => s.Id == accountInfo.Id);
            if (account == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid Id.";
                return rvm;
            }

            account.IsDeleted = 1;
            _rep.Update(account);

            _rep.SaveChanges();
            rvm.Success = true;
            rvm.Msg = "";

            return rvm;
        }
    }
}
