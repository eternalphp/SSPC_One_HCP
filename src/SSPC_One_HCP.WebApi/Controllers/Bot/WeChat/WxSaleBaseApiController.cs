using System.Web.Http;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.WebApi.CustomerAuth;


namespace SSPC_One_HCP.WebApi.Controllers.Bot
{
    /// <summary>
    /// 销售微信小程序基础控制器，用于扩展
    /// </summary>
    [WxSaleAuth]
    public class WxSaleBaseApiController : ApiController
    {
        /// <summary>
        /// 当前操作用户
        /// </summary>
        public WorkUser WorkUser
        {
            get
            {
                var userid = User.Identity.Name;
                if (string.IsNullOrEmpty(userid)) return null;

                var wxRegister = ContainerManager.Resolve<IWxRegisterService>();
                return wxRegister.GetWxSaleUser(userid);
            }
        }
    }
}
