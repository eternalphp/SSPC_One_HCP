using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.WeChat.Dto
{
    public class PvCacheDto
    {
        public string Count { get; set; }
        public List<PvCacheWxDto> CacheWxs { get; set; } = new List<PvCacheWxDto>();
    }

    public class PvCacheWxDto
    {
        public string OpenId { get; set; }
        public string WxName { get; set; }
        public string WxPicture { get; set; }
        public DateTime CreateTime { get; set; }
    }


}
