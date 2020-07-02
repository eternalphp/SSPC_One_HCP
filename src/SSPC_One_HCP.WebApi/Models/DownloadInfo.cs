using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSPC_One_HCP.WebApi.Models
{
    public class DownloadInfo
    {
        public string UserId { get; set; }
        public string DataInfoId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}