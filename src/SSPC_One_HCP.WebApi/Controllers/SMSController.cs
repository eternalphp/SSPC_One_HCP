using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Aspose.Cells;
using Bot.Tool;
using SSPC_One_HCP.Core.Cache;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Core.Data;
using System.Configuration;
using Newtonsoft.Json;
using System.Runtime.Caching;

namespace SSPC_One_HCP.WebApi.Controllers
{
    /// <summary>
    /// 测试
    /// </summary>
    public class SMSController : ApiController
    {

        private readonly IADDoctorService _iADDoctorService;
        private readonly ICacheManager _cacheManager;

        private readonly IEfRepository _rep;
        private readonly string messageUrl = ConfigurationManager.AppSettings["messageUrl"];
        private readonly string messageSignCode = ConfigurationManager.AppSettings["messageSignCode"];
        private readonly string messageTemplateCode = ConfigurationManager.AppSettings["messageTemplateCode"];

        private readonly string tokenUrl = ConfigurationManager.AppSettings["tokenUrl"];
        private readonly string tokenClientId = ConfigurationManager.AppSettings["tokenClientId"];
        private readonly string tokenClientSecret = ConfigurationManager.AppSettings["tokenClientSecret"];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aDDoctorService"></param>
        /// <param name="cacheManager"></param>
        public SMSController(IADDoctorService aDDoctorService, ICacheManager cacheManager)
        {
            _iADDoctorService = aDDoctorService;
            _cacheManager = cacheManager;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult sendMessage([FromUri]string mobile)
        {

            AccessToken token = this.accessToken();
            Random random = new Random();

            var messageCode = new
            {
                code = random.Next(100000, 999999)
            };

            Message msg = new Message
            {
                SendType = 2,
                Receiver = mobile,
                SignCode = messageSignCode,
                TemplateCode = messageTemplateCode,
                Content = JsonConvert.SerializeObject(messageCode)
            };
            var postData = HttpUtils.ModelToUriParam(msg);
            var json = HttpUtils.PostResponse<MessageResult>(messageUrl, postData, token.access_token, "application/x-www-form-urlencoded");
            return Ok(json);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        public IHttpActionResult getAccessToken() {
            OAuthToken token = new OAuthToken
            {
                client_id = tokenClientId,
                client_secret = tokenClientSecret,
                scope = "message_service",
                grant_type = "client_credentials",
                state = "123"
            };

            var postData = HttpUtils.ModelToUriParam(token);
            var accessToken = HttpUtils.PostResponse<AccessToken>(tokenUrl, postData, "application/x-www-form-urlencoded");
            return Ok(accessToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AccessToken accessToken() {


            ObjectCache cache = MemoryCache.Default;//声明缓存类
            AccessToken accessToken = cache["access_token"] as AccessToken;

            if (accessToken != null)
            {
                return accessToken;
            }
            else {
                OAuthToken token = new OAuthToken
                {
                    client_id = tokenClientId,
                    client_secret = tokenClientSecret,
                    scope = "message_service",
                    grant_type = "client_credentials",
                    state = "123"
                };

                var postData = HttpUtils.ModelToUriParam(token);
                accessToken = HttpUtils.PostResponse<AccessToken>(tokenUrl, postData, "application/x-www-form-urlencoded");

                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTime.Now.AddSeconds(accessToken.expires_in);
                cache.Set("access_token", accessToken, policy);

                return accessToken;
            }

        }

    }
}
