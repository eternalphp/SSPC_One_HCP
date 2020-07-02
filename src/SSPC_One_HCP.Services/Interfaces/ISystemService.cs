using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.ViewModels;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface ISystemService
    {
        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <param name="key">配置名称</param>
        /// <returns></returns>
        string GetConfig(string key);

        /// <summary>
        /// 保存系统配置
        /// </summary>
        /// <param name="key">配置名称</param>
        /// <param name="value">配置值</param>
        /// <param name="workUser">当前登录用户</param>
        void SetConfig(string key, string value, WorkUser workUser);

        /// <summary>
        /// 是否启用管理员审核功能
        /// </summary>
        bool AdminApprovalEnabled { get; }

        /// <summary>
        /// 获取系统配置：是否启用管理员审核功能
        /// </summary>
        /// <param name="workUser">当前登录用户</param>
        /// <returns></returns>
        ReturnValueModel GetAdminApprovalEnabled();

        /// <summary>
        /// 保存系统配置：是否启用管理员审核功能
        /// </summary>
        /// <param name="enabled">true:启用, false:禁用</param>
        /// <param name="workUser">当前登录用户</param>
        /// <returns></returns>
        ReturnValueModel SetAdminApprovalEnabled(ConfigurationViewModel<bool?> model, WorkUser workUser);

        /// <summary>
        /// 获取意见反馈列表
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetFeedbackList(RowNumModel<FeedbackListViewModel> rowNum);
    }
}
