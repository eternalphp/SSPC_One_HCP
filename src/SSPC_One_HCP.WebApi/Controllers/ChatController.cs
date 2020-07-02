using SSPC_One_HCP.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    //public class ChatController : ApiController
    //{
    //    private static Dictionary<string, WebSocketHandler> _handlers = new Dictionary<string, WebSocketHandler>();

    //    [Route]
    //    [HttpGet]
    //    public async Task<HttpResponseMessage> Connect(string nickName)
    //    {
    //        if (string.IsNullOrEmpty(nickName))
    //        {
    //            throw new HttpResponseException(HttpStatusCode.BadRequest);
    //        }

    //        var webSocketHandler = new WebSocketHandler();
    //        if (_handlers.ContainsKey(nickName))
    //        {
    //            var origHandler = _handlers[nickName];
    //            // await origHandler.Close();
    //        }

    //        _handlers[nickName] = webSocketHandler;

    //        webSocketHandler.TextMessageReceived += ((sendor, msg) =>
    //        {
    //            BroadcastMessage(nickName, nickName + "说: " + msg);
    //        });

    //        webSocketHandler.Closed += (sendor, arg) =>
    //        {
    //            BroadcastMessage(nickName, nickName + " 断开!");
    //            _handlers.Remove(nickName);
    //        };

    //        webSocketHandler.Opened += (sendor, arg) =>
    //        {
    //            BroadcastMessage(nickName, nickName + " 连接!");
    //        };

    //        HttpContext.Current.AcceptWebSocketRequest(webSocketHandler);

    //        return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
    //    }

    //    private void BroadcastMessage(string sendorNickName, string message)
    //    {
    //        foreach (var handlerKvp in _handlers)
    //        {
    //            if (handlerKvp.Key != sendorNickName)
    //            {
    //                handlerKvp.Value.SendMessage(message).Wait();
    //            }
    //        }
    //    }
    //}
}
