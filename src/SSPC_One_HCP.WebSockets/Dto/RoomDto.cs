using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.WebSockets.Dto
{
    public class RoomDto
    {
        public string RoomName { get; set; }
        public string OpenId { get; set; }
        public string WxName { get; set; }
        public string WxPicture { get; set; }
        public DateTime CreateTime { get; set; }
        public WebSocketHandler RoomWebSocket { get; set; }
    }

    public class RoomUserDto
    {
        public string WxName { get; set; }
        public string WxPicture { get; set; }
        public DateTime CreateTime { get; set; }
        public WebSocketHandler RoomWebSocket { get; set; }
    }
}
