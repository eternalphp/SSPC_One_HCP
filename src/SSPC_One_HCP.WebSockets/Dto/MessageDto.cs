using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.WebSockets.Dto
{
    public class MessageDto
    {
        public int PvCount { get; set; }
        public List<PvUserDto> PvUsers { get; set; } = new List<PvUserDto>();
    }

    public class PvUserDto
    {
        public string OpenId { get; set; }
        public string WxName { get; set; }
        public string WxPicture { get; set; }
    }
}
