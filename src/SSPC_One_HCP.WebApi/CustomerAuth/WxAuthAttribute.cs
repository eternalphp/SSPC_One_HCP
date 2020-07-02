using Newtonsoft.Json;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Cache;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Services.Utils;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SSPC_One_HCP.WebApi.CustomerAuth
{
    /// <summary>
    /// 微信小程序用户身份认证自定义属性
    /// </summary>
    public class WxAuthAttribute : AuthorizeAttribute
    {
        enum ErrorCode
        {
            /// <summary>
            /// 发生异常
            /// </summary>
            ERROR,
            /// <summary>
            /// 未登录
            /// </summary>
            NOT_LOGIN,
            /// <summary>
            /// 用户未注册
            /// </summary>
            UNREGISTERED,
            /// <summary>
            /// 用户未验证
            /// </summary>
            UNVERIFIED
        }


        /// <summary>
        /// 微信小程序用户身份认证
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var attributes = actionContext?.ActionDescriptor?.GetCustomAttributes<AllowAnonymousAttribute>()?.OfType<AllowAnonymousAttribute>();
            if (attributes != null)
            {
                bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                //是否允许匿名访问
                if (isAnonymous)
                {
                    base.OnAuthorization(actionContext);
                    return;
                }
            }

            //获取不到用户名
            var userId = actionContext?.RequestContext?.Principal?.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                HandleUnauthorizedRequest(actionContext, ErrorCode.NOT_LOGIN, "No login id.");  //需要重新登录
                return;
            }

            //获取不到登录用户信息
            var cache = ContainerManager.Resolve<ICacheManager>();
            if (cache == null)
            {
                HandleUnauthorizedRequest(actionContext, ErrorCode.ERROR, "ICacheManager is null");
                return;
            }
            var workUser = cache.Get<WorkUser>(userId);
            if (workUser == null)
            {
                HandleUnauthorizedRequest(actionContext, ErrorCode.NOT_LOGIN, $"User [{userId}] not login.");  //需要重新登录
                return;
            }

            var rep = ContainerManager.Resolve<IEfRepository>();
            if (rep == null)
            {
                HandleUnauthorizedRequest(actionContext, ErrorCode.ERROR, "IEfRepository is null");
                return;
            }
            var wxUser = rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.Id == userId);
            if (wxUser == null)
            {
                HandleUnauthorizedRequest(actionContext, ErrorCode.NOT_LOGIN, $"User [{userId}] is not found, or has been deleted.");
                return;
            }
            workUser.WxUser = wxUser;
            cache.Set<WorkUser>(userId, workUser);

            //是否允许未注册用户访问
            bool allowUnregistered = actionContext?.ActionDescriptor?.GetCustomAttributes<AllowUnregisteredAttribute>().Any() ?? false;
            if (!allowUnregistered)
            {
                if (wxUser.IsCompleteRegister != 1)
                {
                    HandleUnauthorizedRequest(actionContext, ErrorCode.UNREGISTERED);
                    return;
                }
                ////用户认证状态
                ////新用户注册(未认证)，不可看详情:0
                ////注册成功后(认证中)，可看详情:5
                ////(认证拒绝)，不可看详情:2&3
                ////(申诉中)，不可看详情:4
                ////(申诉拒绝)，不可看详情:6
                ////(申诉通过)，可看详情:1
                ////(认证通过)，可看详情:1
                //if (wxUser.IsVerify != 1 && wxUser.IsVerify != 5)
                //{
                //    HandleUnauthorizedRequest(actionContext, ErrorCode.UNVERIFIED, $"Verify status of User [{unionId}] is {wxUser.IsVerify}");
                //    return;
                //}
            }

            base.OnAuthorization(actionContext);
        }

        /// <summary>
        /// 返回自定义的状态
        /// </summary>
        private void HandleUnauthorizedRequest(HttpActionContext filterContext, ErrorCode errCode, string msg = "")
        {
            base.HandleUnauthorizedRequest(filterContext);

            HttpStatusCode statusCode;
            switch (errCode)
            {
                case ErrorCode.ERROR:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
                default:
                    statusCode = HttpStatusCode.Unauthorized;
                    break;
            }

            var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
            response.StatusCode = statusCode;
            var content = new
            {
                success = false,
                errCode = errCode.ToString(),
                message = msg
            };
            response.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        }
    }
}