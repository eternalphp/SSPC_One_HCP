using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Services.WeChat.Dto;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;
using SSPC_One_HCP.WebApi.Controllers.WeChat;
using SSPC_One_HCP.WebApi.CustomerAuth;
using SSPC_One_HCP.WebSockets;
using SSPC_One_HCP.WebSockets.Dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>

    [RoutePrefix("WcChat")]
    public class WcChatController : ApiController
    {

        private static readonly List<RoomDto> roomDtos = new List<RoomDto>();
        static readonly object locker = new object();
        [Route]
        [HttpGet]
        [AllowUnregistered]
        public async Task<HttpResponseMessage> Connect(string room, string openId, string nickName, string nickPicture)
        {
            if (string.IsNullOrEmpty(room))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            try
            {
                lock (locker)
                {
                    var count = 0;
                    var webSocketHandler = new WebSocketHandler();
                    if (string.IsNullOrEmpty(nickName) || string.IsNullOrEmpty(nickPicture))
                    {
                        var files = GetFiles();
                        var tourist = GetRandomNumber(files);
                        string _host = ConfigurationManager.AppSettings["HostUrl"];
                        nickName = "游客";
                        nickPicture = $"{_host}/Content/images/Tourist/{tourist}";
                    }

                    count = GetPVCount(room);
                    var r = roomDtos.FirstOrDefault(o => o.RoomName == room && o.OpenId == openId);
                    if (r == null)
                        roomDtos.Add(new RoomDto
                        {
                            RoomName = room,
                            OpenId = openId,
                            WxName = nickName,
                            WxPicture = nickPicture,
                            CreateTime = DateTime.UtcNow.AddHours(8),
                            RoomWebSocket = webSocketHandler,

                        });//不存在，添加
                    else
                    {
                        if (r.RoomWebSocket != webSocketHandler)//当前对象不一致，更新
                        {
                            RemoveUser(room, openId);
                            roomDtos.Add(new RoomDto
                            {
                                RoomName = room,
                                OpenId = openId,
                                WxName = nickName,
                                WxPicture = nickPicture,
                                CreateTime = DateTime.UtcNow.AddHours(8),
                                RoomWebSocket = webSocketHandler,
                            });
                        }
                    }
                    AddLiveOnline(room, roomDtos.Count);



                    //if (_handlers.ContainsKey(room))
                    //{
                    //    var origHandler = _handlers[room];
                    //    // await origHandler.Close();
                    //}
                    //_handlers[room] = webSocketHandler;

                    webSocketHandler.TextMessageReceived += ((sendor, msg) =>
                    {
                        BroadcastMessage(room, openId, nickName + "说: " + msg);
                    });

                    webSocketHandler.Closed += (sendor, arg) =>
                    {
                        //BroadcastMessage(room, nickName, nickName + " 断开!");
                        //roomDtos.Remove(r);

                        BroadcastClosedMessage(room, count);
                        RemoveUser(room, openId);
                    };

                    webSocketHandler.Opened += (sendor, arg) =>
                    {

                        BroadcastOpeneMessage(room, /*count*/roomDtos.Count, openId);

                        //BroadcastMessage(room, nickName, nickName + " 连接!");
                    };

                    HttpContext.Current.AcceptWebSocketRequest(webSocketHandler);
                }
            }
            catch (Exception e)
            {

                throw;
            }
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);

        }

        private void BroadcastMessage(string room, string openId, string message)
        {
            
            foreach (var handlerKvp in roomDtos)
            {
                if (handlerKvp.RoomName == room && handlerKvp.OpenId != openId)
                {
                    handlerKvp.RoomWebSocket.SendMessage(message).Wait();
                }
            }

        }

        /// <summary>
        /// 连接 广播消息
        /// </summary>
        /// <param name="room"></param>
        /// <param name="count"></param>
        /// <param name="openId"></param>
        void BroadcastOpeneMessage(string room, int count, string openId = null)
        {
            //AddLiveOnline(room, roomDtos.Count);
            MessageDto messageDto = new MessageDto
            {
                PvCount = count,
                PvUsers = PvUsers(room)
            };
            string message = KBS.Helpers.Json.ToJson(messageDto);

            var rooms = roomDtos.Where(o => o.RoomName == room).ToList();
            message = "在线人数：" + count;
            foreach (var handlerKvp in rooms)
            {
                try
                {
                    //if (handlerKvp.OpenId == openId)
                    {
                        handlerKvp.RoomWebSocket.SendMessage(message).Wait();
                    }

                }
                catch
                {

                    RemoveUser(room, handlerKvp.OpenId);
                }
            }

        }

        /// <summary>
        /// 断开 广播消息
        /// </summary>
        /// <param name="room"></param>
        /// <param name="count"></param>
        void BroadcastClosedMessage(string room, int count)
        {
            AddLiveOnline(room, roomDtos.Count);
           
            string message = "有人断开连接，目前人数：" + roomDtos.Count;
            foreach (var handlerKvp in roomDtos)
            {
                if (handlerKvp.RoomName == room)
                {
                    handlerKvp.RoomWebSocket.SendMessage(message).Wait();
                }
            }
        }
        void RemoveUser(string room, string openId)
        {
            var r = roomDtos.FirstOrDefault(o => o.RoomName == room && o.OpenId == openId);
            if (r != null)
            {
                roomDtos.Remove(r);
            }

        }
        void AddLiveOnline(string meetInfoId, int count)
        {
            var rep = ContainerManager.Resolve<IEfRepository>();
            var model = new LiveOnline
            {
                Id = Guid.NewGuid().ToString(),
                MeetInfoId = meetInfoId,
                Total = count,
                CreateTime = DateTime.UtcNow.AddHours(8),
            };
            rep.Insert(model);
            rep.SaveChanges();
        }
        List<PvUserDto> PvUsers(string room)
        {
            List<PvUserDto> pvUsers = new List<PvUserDto>();
            var r = roomDtos.Where(o => o.RoomName == room).OrderByDescending(o => o.CreateTime).Skip(0).Take(3).ToList();
            foreach (var item in r)
            {
                pvUsers.Add(new PvUserDto
                {
                    OpenId = item.OpenId,
                    WxName = item.WxName,
                    WxPicture = item.WxPicture
                });
            }

            return pvUsers;
        }
      
        int GetPVCount(string room)
        {
            var rep = ContainerManager.Resolve<IEfRepository>();
            var meetInfodata = rep.FirstOrDefault<MeetInfo>(s => s.Id == room && s.IsDeleted != 1);
            var count = meetInfodata?.PVCount;
            return count.GetValueOrDefault();
        }
        string GetRandomNumber(FileInfo[] files)
        {
            if (files.Length <= 0)
            {
                return "Tourist.jpg";
            }
            Random rnd = new Random();
            int index = rnd.Next(files.Length);
            return files[index].Name;
        }

        FileInfo[] GetFiles()
        {
            var path = HostingEnvironment.MapPath("/Content/images/Tourist");
            DirectoryInfo root = new DirectoryInfo(path);
            return root.GetFiles();

        }
    }
}
