using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Linq;

namespace SSPC_One_HCP.Services.Implementations
{
    public class SystemService : ISystemService
    {
        private readonly IEfRepository _rep;
        private readonly ICommonService _commonService;

        public SystemService(IEfRepository rep, ICommonService commonService)
        {
            _rep = rep;
            _commonService = commonService;
        }

        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <param name="key">配置名称</param>
        /// <returns></returns>
        public string GetConfig(string key)
        {
            var item = _rep.FirstOrDefault<Configuration>(s => s.ConfigureName == key);
            return item?.ConfigureValue;
        }

        /// <summary>
        /// 保存系统配置
        /// </summary>
        /// <param name="key">配置名称</param>
        /// <param name="value">配置值</param>
        /// <param name="workUser">当前登录用户</param>
        public void SetConfig(string key, string value, WorkUser workUser)
        {
            var item = _rep.FirstOrDefault<Configuration>(s => s.ConfigureName == key);
            if (item == null)
            {
                item = new Configuration
                {
                    Id = Guid.NewGuid().ToString(),
                    ConfigureName = key,
                    ConfigureValue = value,
                    CreateTime = DateTime.Now,
                    UpdateUser = workUser.User.Id
                };
                _rep.Insert(item);
            }
            else
            {
                item.ConfigureValue = value;
                item.UpdateTime = DateTime.Now;
                item.UpdateUser = workUser.User.Id;
                _rep.Update(item);
            }
            _rep.SaveChanges();
        }

        /// <summary>
        /// 是否启用管理员审核功能
        /// </summary>
        public bool AdminApprovalEnabled
        {
            get
            {
                return GetConfig("AdminApprovalEnabled") == "1";
            }
        }

        /// <summary>
        /// 获取系统配置：是否启用管理员审核功能
        /// </summary>
        /// <param name="workUser">当前登录用户</param>
        /// <returns></returns>
        public ReturnValueModel GetAdminApprovalEnabled()
        {
            ReturnValueModel rvm = new ReturnValueModel();
            rvm.Success = true;
            rvm.Result = new
            {
                Enabled = AdminApprovalEnabled
            };
            return rvm;
        }

        /// <summary>
        /// 保存系统配置：是否启用管理员审核功能
        /// </summary>
        /// <param name="enabled">true:启用, false:禁用</param>
        /// <param name="workUser">当前登录用户</param>
        /// <returns></returns>
        public ReturnValueModel SetAdminApprovalEnabled(ConfigurationViewModel<bool?> model, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var isAdmin = _commonService.IsAdmin(workUser);
            if (!isAdmin)
            {
                rvm.Msg = "You have no administrator permission.";
                rvm.Success = false;
                return rvm;
            }

            if (model?.Value == null)
            {
                rvm.Msg = "Invalid parameters.";
                rvm.Success = false;
                return rvm;
            }

            try
            {
                bool value = (model?.Value ?? false);
                SetConfig("AdminApprovalEnabled", (value ? 1 : 0).ToString(), workUser);

                rvm.Success = true;
                rvm.Result = new
                {
                    Enabled = value
                };
            }
            catch (Exception ex)
            {
                rvm.Success = false;
                rvm.Msg = ex.Message;
            }
            return rvm;
        }

        /// <summary>
        /// 获取意见反馈列表
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetFeedbackList(RowNumModel<FeedbackListViewModel> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = from a in _rep.Where<Feedback>(s => s.IsDeleted != 1)
                       join b in _rep.Table<WxUserModel>() on a.CreateUser equals b.Id into ab
                       from b1 in ab.DefaultIfEmpty()
                       select new FeedbackListViewModel
                       {
                           Id = a.Id,
                           Content = a.Content,
                           CreateTime = a.CreateTime,
                           CreateUser = b1 == null ? "" : b1.UserName
                       };


            if (rowNum != null && rowNum.SearchParams != null)
            {
                if (!string.IsNullOrEmpty((rowNum.SearchParams.Content)))
                {
                    list = list.Where(s => s.Content.Contains(rowNum.SearchParams.Content));
                }
                if (!string.IsNullOrEmpty((rowNum.SearchParams.CreateUser)))
                {
                    list = list.Where(s => s.CreateUser.Contains(rowNum.SearchParams.CreateUser));
                }
            }

            var total = list.Count();
            var rows = list.OrderBy(s => s.CreateTime).ToPaginationList(rowNum?.PageIndex, rowNum?.PageSize).ToList();

            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                total,
                rows
            };
            return rvm;
        }
    }
}
