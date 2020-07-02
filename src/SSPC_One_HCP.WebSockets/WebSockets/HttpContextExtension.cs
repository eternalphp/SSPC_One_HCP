using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SSPC_One_HCP.WebSockets
{
    public static class HttpContextExtension
    {
        public static void AcceptWebSocketRequest(this HttpContext context, WebSocketHandler handler)
        {
            context.AcceptWebSocketRequest(handler.ProcessRequest);
        }
    }
}
