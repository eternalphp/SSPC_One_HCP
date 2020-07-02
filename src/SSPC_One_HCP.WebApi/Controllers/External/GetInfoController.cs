using SSPC_One_HCP.WebApi.CustomerAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.External
{
    public class GetInfoController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetInfo()
        {
            return Ok("1111");
        }
    }
}
