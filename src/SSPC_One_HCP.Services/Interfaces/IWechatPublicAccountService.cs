using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface IWechatPublicAccountService
    {
        /// <summary>
        /// 获取微信公众号列表
        /// </summary>
        /// <param name="model">分页搜索</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetWechatPublicAccountList(RowNumModel<WechatPublicAccount> model, WorkUser workUser);

        /// <summary>
        /// 获取单个微信公众号信息
        /// </summary>
        /// <param name="accountInfo">微信公众号信息</param>
        /// <returns></returns>
        ReturnValueModel GetWechatPublicAccountInfo(WechatPublicAccount accountInfo, WorkUser workUser);
        
        /// <summary>
        /// 新增或修改微信公众号记录
        /// </summary>
        /// <param name="accountInfo">微信公众号信息</param>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateWechatPublicAccount(WechatPublicAccount accountInfo, WorkUser workUser);

        /// <summary>
        /// 删除微信公众号记录
        /// </summary>
        /// <param name="accountInfo">微信公众号信息</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteWechatPublicAccount(WechatPublicAccount accountInfo, WorkUser workUser);
    }
}
