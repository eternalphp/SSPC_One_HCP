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
        private readonly string tokenUrl = ConfigurationManager.AppSettings["tokenUrl"];


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hcpDataInfoService"></param>
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

            Message msg = new Message
            {
                SendType = 2,
                Receiver = mobile,
                SignCode = "FKSign0001",
                TemplateCode = "FKSMS0047",
                Content = "{\"code\":\"213421\"}"
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
                client_id = "42958C0C2F79F2C6A509",
                client_secret = "B6A1722A4BD88112B2F6618A9C85F8",
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

            DateTime datetime = Convert.ToDateTime(System.Web.HttpContext.Current.Application["datetime"]);
            AccessToken accessToken = System.Web.HttpContext.Current.Application["accessToken"] as AccessToken;

            if (accessToken != null && datetime.AddSeconds(accessToken.expires_in) > DateTime.Now)
            {

                return accessToken;
            }
            else
            {

                OAuthToken token = new OAuthToken
                {
                    client_id = "42958C0C2F79F2C6A509",
                    client_secret = "B6A1722A4BD88112B2F6618A9C85F8",
                    scope = "message_service",
                    grant_type = "client_credentials",
                    state = "123"
                };

                var postData = HttpUtils.ModelToUriParam(token);
                accessToken = HttpUtils.PostResponse<AccessToken>(tokenUrl, postData, "application/x-www-form-urlencoded");

                System.Web.HttpContext.Current.Application["accessToken"] = accessToken;
                System.Web.HttpContext.Current.Application["datetime"] = DateTime.Now;

                return accessToken;

            }

        }

    }
}
