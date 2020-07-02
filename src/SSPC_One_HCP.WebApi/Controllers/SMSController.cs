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
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult send([FromUri]string secret)
        {
            var ret = secret.GetHash();
            return Ok(ret);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult getAccessToken() {
            OAuthToken token = new OAuthToken
            {
                client_id = "client_id",
                client_secret = "client_secret ",
                scope = "scope ",
                grant_type = "client_credentials",
                state = "123"
            };
            var postData = JsonConvert.SerializeObject(token);
            var accessToken = HttpUtils.PostResponse<AccessToken>(tokenUrl, postData);
            return Ok(accessToken);
        }
    }
}
