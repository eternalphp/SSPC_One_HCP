using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.RongCloud.models;
using SSPC_One_HCP.RongCloud.response;
using SSPC_One_HCP.RongCloud.util;
using SSPC_One_HCP.Services.RongCloud.Dto;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.IO;

namespace SSPC_One_HCP.Services.RongCloud
{
    public class RongCloudService : IRongCloudService
    {
        private readonly IEfRepository _rep;
        public RongCloudService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// 融云-消息路由
        /// </summary>
        public dynamic TemplateRouteCallback(TemplateRouteInputDto inputDto)
        {

            var form = HttpContext.Current?.Request?.Form;
            var items = form.AllKeys.SelectMany(form.GetValues, (k, v) => new { key = k, value = v });

            var fromUserId = items.FirstOrDefault(o => o.key.ToUpper() == "FROMUSERID")?.value;
            var toUserId = items.FirstOrDefault(o => o.key.ToUpper() == "TOUSERID")?.value;
            var objectName = items.FirstOrDefault(o => o.key.ToUpper() == "OBJECTNAME")?.value;
            var content = items.FirstOrDefault(o => o.key.ToUpper() == "CONTENT")?.value;
            var channelType = items.FirstOrDefault(o => o.key.ToUpper() == "CHANNELTYPE")?.value;
            var msgTimestamp = items.FirstOrDefault(o => o.key.ToUpper() == "MSGTIMESTAMP")?.value;
            var msgUID = items.FirstOrDefault(o => o.key.ToUpper() == "MSGUID")?.value;
            var sensitiveType = items.FirstOrDefault(o => o.key.ToUpper() == "SENSITIVETYPE")?.value;
            var source = items.FirstOrDefault(o => o.key.ToUpper() == "SOURCE")?.value;
            var groupUserIds = items.FirstOrDefault(o => o.key.ToUpper() == "GROUPUSERIDS")?.value;


            string appKey = ConfigurationManager.AppSettings["RongCloudAppKey"];
            string appSecret = ConfigurationManager.AppSettings["RongCloudAppSecret"];

            string signature = RongHttpClient.GetHash(appSecret + inputDto.Nonce + inputDto.SignTimestamp);
            if (signature.ToUpper() != inputDto.Signature.ToUpper())
            {
                var massage = string.Format("[RouteCallback]:Error {0}==>{1},{2}", inputDto.Signature, signature, RongJsonUtil.ObjToJsonString(inputDto));
                LoggerHelper.Error(massage);
                return new { pass = "0" };
            }


            var rongCloudContent = new RongCloudContent
            {
                Id = Guid.NewGuid().ToString(),
                CreateTime = DateTime.UtcNow.AddHours(8),

                FromUserId = fromUserId,
                ToUserId = toUserId,//此ID对应会议ID
                ObjectName = objectName,
                ChannelType = channelType,
                MsgTimeStamp = msgTimestamp,
                MsgUID = msgUID,
                SensitiveType = sensitiveType,
                Source = source,
                Audit = 0,
                Remark = RongJsonUtil.ObjToJsonString(items),

            };
            if (objectName == "RC:TxtMsg" && !string.IsNullOrEmpty(content))
            {
                var res = (ContentInputDto)RongJsonUtil.JsonStringToObj<ContentInputDto>(content);

                //var doctor = _rep.FirstOrDefault<WxUserModel>(s => s != null && s.IsDeleted != 1 && s.Id == toUserId);

                rongCloudContent.WxName = res?.User?.Name;
                rongCloudContent.WxPicture = res?.User?.Avatar;
                rongCloudContent.Content = res.Content;
            }
            else
            {
                rongCloudContent.Content = content;
            }

            rongCloudContent.GroupUserIds = groupUserIds;

            _rep.Insert<RongCloudContent>(rongCloudContent);
            _rep.SaveChanges();

            return new { pass = "1" };
        }
        /// <summary>
        /// 融云-聊天室 状态同步
        /// </summary>
        public dynamic ChatroomStatusSync(ChatroomStatusSyncDto inputDto, string body)
        {

            var bodys = RongJsonUtil.JsonStringToObj<List<RongCloudChatroomStatusDto>>(body);
            string appKey = ConfigurationManager.AppSettings["RongCloudAppKey"];
            string appSecret = ConfigurationManager.AppSettings["RongCloudAppSecret"];

            string signature = RongHttpClient.GetHash(appSecret + inputDto.Nonce + inputDto.SignTimestamp);
            if (signature.ToUpper() != inputDto.Signature.ToUpper())
            {
                var massage = string.Format("[ChatroomStatusSync]:Error {0}==>{1},{2}", inputDto.Signature, signature, RongJsonUtil.ObjToJsonString(inputDto));
                LoggerHelper.Error(massage);
                return new { pass = "0" };
            }
            List<RongCloudChatroomStatus> rongCloudChatroomStatus = new List<RongCloudChatroomStatus>();
            foreach (var item in bodys)
            {
                if (item.UserIds.Count > 0)
                {
                    foreach (var user in item.UserIds)
                    {
                        var doctor = _rep.FirstOrDefault<WxUserModel>(s => s != null && s.IsDeleted != 1 && s.Id == user);
                        if (doctor != null)
                        {
                            rongCloudChatroomStatus.Add(new RongCloudChatroomStatus
                            {
                                Id = Guid.NewGuid().ToString(),
                                CreateTime = DateTime.UtcNow.AddHours(8),
                                ChatRoomId = item.ChatRoomId,
                                UserId = doctor.Id,
                                Status = item.Status,
                                Type = item.Type,
                                Time = item.Time,
                                UserName = doctor.UserName,
                                HospitalName = doctor.HospitalName,
                            });
                        }

                    }
                }
                else
                {
                    rongCloudChatroomStatus.Add(new RongCloudChatroomStatus
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreateTime = DateTime.UtcNow.AddHours(8),
                        ChatRoomId = item.ChatRoomId,
                        Status = item.Status,
                        Type = item.Type,
                        Time = item.Time,
                        //UserName = doctor.UserName,
                        //HospitalName = doctor.HospitalName,
                    });
                }

            }

            //var doctor = _rep.FirstOrDefault<WxUserModel>(s => s != null && s.IsDeleted != 1 && s.Id == toUserId);


            _rep.InsertList<RongCloudChatroomStatus>(rongCloudChatroomStatus);
            _rep.SaveChanges();

            return new { pass = "1" };
        }

        /// <summary>
        /// 融云-消息撤回
        /// </summary>
        public ReturnValueModel MessageRecall(RecallMessageInputDto inputDto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            string appKey = ConfigurationManager.AppSettings["RongCloudAppKey"];
            string appSecret = ConfigurationManager.AppSettings["RongCloudAppSecret"];
            string _host = ConfigurationManager.AppSettings["RongCloudUrl"];
            string url = $"{_host}/message/recall.json";

            StringBuilder sb = new StringBuilder();
            sb.Append("&conversationType=").Append(HttpUtility.UrlEncode("1", Encoding.UTF8));
            sb.Append("&fromUserId=").Append(HttpUtility.UrlEncode(inputDto.SenderId.ToString(), Encoding.UTF8));
            sb.Append("&targetId=").Append(HttpUtility.UrlEncode(inputDto.TargetId.ToString(), Encoding.UTF8));
            sb.Append("&messageUID=").Append(HttpUtility.UrlEncode(inputDto.UId.ToString(), Encoding.UTF8));
            sb.Append("&sentTime=").Append(HttpUtility.UrlEncode(inputDto.SentTime.ToString(), Encoding.UTF8));
            String body = sb.ToString();
            if (body.IndexOf("&") == 0)
            {
                body = body.Substring(1, body.Length - 1);
            }

            string result = RongHttpClient.ExecutePost(appKey, appSecret, body, url, "application/x-www-form-urlencoded");

            var res = (ResponseResult)RongJsonUtil.JsonStringToObj<ResponseResult>(result);
            if (res.Code == 200)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = res;
                return rvm;
            }
            else
            {
                rvm.Msg = "消息撤回失败";
                rvm.Success = false;
                rvm.Result = result;
                return rvm;
            }
        }
        /// <summary>
        /// 以发送聊天室消息方法实现：业务处理聊天室禁言与解禁
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel ChatroomSend(ChatroomSendInputDto dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var meetInfo = _rep.FirstOrDefault<MeetInfo>(s => s != null && s.IsDeleted != 1 && s.Id == dto.Id);
            if (meetInfo == null)
            {
                rvm.Msg = "获取会议失败";
                rvm.Success = false;
                rvm.Result = "";
                return rvm;
            }

            string appKey = ConfigurationManager.AppSettings["RongCloudAppKey"];
            string appSecret = ConfigurationManager.AppSettings["RongCloudAppSecret"];
            string _host = ConfigurationManager.AppSettings["RongCloudUrl"];
            string url = $"{_host}/message/chatroom/publish.json";

            StringBuilder sb = new StringBuilder();

            var content = RongJsonUtil.ObjToJsonString(new
            {
                content = dto.Content,
                messageName = "TextMessage",
                extra = "extra"
                //user = new
                //{
                //    id = workUser.User.Id,
                //    name = "系统管理员",
                //    icon = "http://example.com/p1.png",
                //    extra = "extra"
                //}
            });
            var objectName = $"RCFK:{dto.Content}";
            sb.Append("&fromUserId=").Append(HttpUtility.UrlEncode(workUser.User.Id, Encoding.UTF8));//发送人用户 Id
            sb.Append("&toChatroomId=").Append(HttpUtility.UrlEncode(dto.Id, Encoding.UTF8));//接收聊天室 Id
            sb.Append("&objectName=").Append(HttpUtility.UrlEncode(objectName, Encoding.UTF8));
            sb.Append("&content=").Append(HttpUtility.UrlEncode(content, Encoding.UTF8));
            String body = sb.ToString();
            if (body.IndexOf("&") == 0)
                body = body.Substring(1, body.Length - 1);
            string result = RongHttpClient.ExecutePost(appKey, appSecret, body, url, "application/x-www-form-urlencoded");
            var res = (ResponseResult)RongJsonUtil.JsonStringToObj<ResponseResult>(result);
            if (res.Code == 200)
            {

                meetInfo.UpdateTime = DateTime.Now;
                meetInfo.UpdateUser = workUser.User.Id;
                meetInfo.IsForbiddenWords = dto.Content == "Close" ? 1 : 0;
                _rep.Update(meetInfo);
                _rep.Insert<RongCloudChatRoomHandle>(new RongCloudChatRoomHandle
                {
                    Id = Guid.NewGuid().ToString(),
                    CreateUser = workUser.User.Id,
                    CreateTime = DateTime.UtcNow.AddHours(8),
                    ChatRoomId = dto.Id,
                    Event = dto.Content,
                });
                _rep.SaveChanges();

                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = res;
                return rvm;
            }
            else
            {
                rvm.Msg = "消息失败";
                rvm.Success = false;
                rvm.Result = result;
                return rvm;
            }
        }
        /// <summary>
        /// 消息历史记录下载地址获取
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ReturnValueModel MessageHistory(string date)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            string appKey = ConfigurationManager.AppSettings["RongCloudAppKey"];
            string appSecret = ConfigurationManager.AppSettings["RongCloudAppSecret"];
            string _host = ConfigurationManager.AppSettings["RongCloudUrl"];
            string url = $"{_host}/message/history.json";

            StringBuilder sb = new StringBuilder();
            sb.Append("&date=").Append(HttpUtility.UrlEncode(date.ToString(), Encoding.UTF8));
            String body = sb.ToString();
            if (body.IndexOf("&") == 0)
            {
                body = body.Substring(1, body.Length - 1);
            }

            string result = RongHttpClient.ExecutePost(appKey, appSecret, body, url, "application/x-www-form-urlencoded");
            var res = (HistoryMessageResult)RongJsonUtil.JsonStringToObj<HistoryMessageResult>(result);
            if (res.Code == 200)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = res;
                return rvm;
            }
            else
            {
                rvm.Msg = "创建聊天室失败";
                rvm.Success = false;
                rvm.Result = result;
                return rvm;
            }

        }

        #region 聊天室
        /// <summary>
        /// 融云-创建聊天室
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel ChatroomCreate(ChatroomInputDto inputDto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            string body = $"chatroom[{inputDto.Id}]={HttpUtility.UrlEncode(inputDto.Name, Encoding.UTF8)}";

            string appKey = ConfigurationManager.AppSettings["RongCloudAppKey"];
            string appSecret = ConfigurationManager.AppSettings["RongCloudAppSecret"];
            string _host = ConfigurationManager.AppSettings["RongCloudUrl"];
            string url = $"{_host}/chatroom/create.json";

            string result = RongHttpClient.ExecutePost(appKey, appSecret, body, url, "application/x-www-form-urlencoded");

            var res = (ResponseResult)RongJsonUtil.JsonStringToObj<ResponseResult>(result);
            if (res.Code == 200)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = res.Code;
                return rvm;
            }
            else
            {
                rvm.Msg = "创建聊天室失败";
                rvm.Success = false;
                rvm.Result = result;
                return rvm;
            }


        }

        /// <summary>
        /// 融云-查询聊天室
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel ChatroomQuery(string chatroomId, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            string body = $"chatroomId={HttpUtility.UrlEncode(chatroomId, Encoding.UTF8)}";

            string appKey = ConfigurationManager.AppSettings["RongCloudAppKey"];
            string appSecret = ConfigurationManager.AppSettings["RongCloudAppSecret"];
            string _host = ConfigurationManager.AppSettings["RongCloudUrl"];
            string url = $"{_host}/chatroom/query.json";

            string result = RongHttpClient.ExecutePost(appKey, appSecret, body, url, "application/x-www-form-urlencoded");

            var res = (ChatroomQueryResult)RongJsonUtil.JsonStringToObj<ChatroomQueryResult>(result);

            if (res.Code == 200)
            {
                var rongCloudChatRoomHandle = _rep.Where<RongCloudChatRoomHandle>(s => s != null && s.IsDeleted != 1 && s.ChatRoomId == chatroomId)
                                 .OrderByDescending(o => o.CreateTime).FirstOrDefault();


                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    ChatroomQuery = (res?.ChatRooms != null && res?.ChatRooms?.Count > 0) ? res.ChatRooms[0] : new ChatroomQuery(),
                    Event = rongCloudChatRoomHandle == null ? "Open" : rongCloudChatRoomHandle.Event,
                };
                return rvm;
            }
            else
            {
                rvm.Msg = "查询聊天室失败";
                rvm.Success = false;
                rvm.Result = result;
                return rvm;
            }

        }
        /// <summary>
        /// 融云-销毁聊天室
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel ChatroomDestroy(ChatroomInputDto inputDto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            //String message = CommonUtil.CheckFiled(inputDto, "chatroom", CheckMethod.DESTORY);
            //if (null != message)
            //{
            //    rvm.Msg = "success";
            //    rvm.Success = false;
            //    rvm.Result = (ResponseResult)RongJsonUtil.JsonStringToObj<ResponseResult>(message);
            //    return rvm;
            //}
            string body = $"chatroomId={HttpUtility.UrlEncode(inputDto.Id, Encoding.UTF8)}";

            string appKey = ConfigurationManager.AppSettings["RongCloudAppKey"];
            string appSecret = ConfigurationManager.AppSettings["RongCloudAppSecret"];
            string _host = ConfigurationManager.AppSettings["RongCloudUrl"];
            string url = $"{_host}/chatroom/destroy.json";

            String result = RongHttpClient.ExecutePost(appKey, appSecret, body, url, "application/x-www-form-urlencoded");

            var res = (ResponseResult)RongJsonUtil.JsonStringToObj<ResponseResult>(result);
            if (res.Code == 200)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = res;
                return rvm;
            }
            else
            {
                rvm.Msg = "销毁聊天室失败";
                rvm.Success = false;
                rvm.Result = result;
                return rvm;
            }
        }
        /// <summary>
        /// 融云-添加-聊天室保活服务
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel ChatroomKeepaliveAdd(ChatroomInputDto inputDto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            //String message = CommonUtil.CheckFiled(inputDto, "", CheckMethod.ADD);

            string body = $"chatroomId={HttpUtility.UrlEncode(inputDto.Id, Encoding.UTF8)}";

            string appKey = ConfigurationManager.AppSettings["RongCloudAppKey"];
            string appSecret = ConfigurationManager.AppSettings["RongCloudAppSecret"];
            string _host = ConfigurationManager.AppSettings["RongCloudUrl"];
            string url = $"{_host}/chatroom/keepalive/add.json";

            String result = RongHttpClient.ExecutePost(appKey, appSecret, body, url, "application/x-www-form-urlencoded");
            var res = (ResponseResult)RongJsonUtil.JsonStringToObj<ResponseResult>(result);
            if (res.Code == 200)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = res;
                return rvm;
            }
            else
            {
                rvm.Msg = "聊天室保活失败";
                rvm.Success = false;
                rvm.Result = result;
                return rvm;
            }
        }
        /// <summary>
        /// 融云-删除-聊天室保活服务
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel ChatroomKeepaliveRemove(ChatroomInputDto inputDto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            string body = $"chatroomId={HttpUtility.UrlEncode(inputDto.Id, Encoding.UTF8)}";

            string appKey = ConfigurationManager.AppSettings["RongCloudAppKey"];
            string appSecret = ConfigurationManager.AppSettings["RongCloudAppSecret"];
            string _host = ConfigurationManager.AppSettings["RongCloudUrl"];
            string url = $"{_host}/chatroom/keepalive/remove.json";
            String result = RongHttpClient.ExecutePost(appKey, appSecret, body, url, "application/x-www-form-urlencoded");


            var res = (ResponseResult)RongJsonUtil.JsonStringToObj<ResponseResult>(result);

            if (res.Code == 200)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = res;
                return rvm;
            }
            else
            {
                rvm.Msg = "删除聊天室保活失败";
                rvm.Success = false;
                rvm.Result = result;
                return rvm;
            }
        }
        /// <summary>
        /// 融云-获取聊天室保活
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel ChatroomKeepaliveGetList()
        {
            ReturnValueModel rvm = new ReturnValueModel();
            string appKey = ConfigurationManager.AppSettings["RongCloudAppKey"];
            string appSecret = ConfigurationManager.AppSettings["RongCloudAppSecret"];
            string _host = ConfigurationManager.AppSettings["RongCloudUrl"];
            string url = $"{_host}/chatroom/keepalive/query.json";

            String result = RongHttpClient.ExecutePost(appKey, appSecret, "", url, "application/x-www-form-urlencoded");

            var res = (ChatroomKeepaliveResult)RongJsonUtil.JsonStringToObj<ChatroomKeepaliveResult>(result);
            if (res.Code == 200)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = res;
                return rvm;
            }
            else
            {
                rvm.Msg = "success";
                rvm.Success = false;
                rvm.Result = result;
                return rvm;
            }
        }
        /// <summary>
        /// 融云-聊天室成员禁言
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel ChatroomUserGagAdd(ChatroomUserGagAddDto dto)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            string appKey = ConfigurationManager.AppSettings["RongCloudAppKey"];
            string appSecret = ConfigurationManager.AppSettings["RongCloudAppSecret"];
            string _host = ConfigurationManager.AppSettings["RongCloudUrl"];
            string url = $"{_host}/chatroom/user/gag/add.json";

            StringBuilder sb = new StringBuilder();
            foreach (var member in dto.UserIds)
            {
                sb.Append("&userId=").Append(HttpUtility.UrlEncode(member, Encoding.UTF8));
            }
            sb.Append("&chatroomId=").Append(HttpUtility.UrlEncode(dto.ChatroomId.ToString(), Encoding.UTF8));
            sb.Append("&minute=").Append(HttpUtility.UrlEncode(dto.Minute.ToString(), Encoding.UTF8));
            String body = sb.ToString();
            if (body.IndexOf("&") == 0)
            {
                body = body.Substring(1, body.Length - 1);
            }
            String result = RongHttpClient.ExecutePost(appKey, appSecret, body, url, "application/x-www-form-urlencoded");

            var res = (ResponseResult)RongJsonUtil.JsonStringToObj<ResponseResult>(result);
            if (res.Code == 200)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = res;
                return rvm;
            }
            else
            {
                rvm.Msg = "success";
                rvm.Success = false;
                rvm.Result = result;
                return rvm;
            }

        }
        /// <summary>
        /// 融云-聊天室成员禁言 移除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ReturnValueModel ChatroomUserGagRollback(ChatroomUserGagAddDto dto)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            string appKey = ConfigurationManager.AppSettings["RongCloudAppKey"];
            string appSecret = ConfigurationManager.AppSettings["RongCloudAppSecret"];
            string _host = ConfigurationManager.AppSettings["RongCloudUrl"];
            string url = $"{_host}/chatroom/user/gag/rollback.json";

            StringBuilder sb = new StringBuilder();
            foreach (var member in dto.UserIds)
            {
                sb.Append("&userId=").Append(HttpUtility.UrlEncode(member, Encoding.UTF8));
            }
            sb.Append("&chatroomId=").Append(HttpUtility.UrlEncode(dto.ChatroomId.ToString(), Encoding.UTF8));
            String body = sb.ToString();
            if (body.IndexOf("&") == 0)
            {
                body = body.Substring(1, body.Length - 1);
            }
            String result = RongHttpClient.ExecutePost(appKey, appSecret, body, url, "application/x-www-form-urlencoded");

            var res = (ResponseResult)RongJsonUtil.JsonStringToObj<ResponseResult>(result);
            if (res.Code == 200)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = res;
                return rvm;
            }
            else
            {
                rvm.Msg = "success";
                rvm.Success = false;
                rvm.Result = result;
                return rvm;
            }

        }

        /// <summary>
        /// 融云-查询 聊天室成员禁言
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ReturnValueModel ChatroomUserGagList(string chatroomId)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            string appKey = ConfigurationManager.AppSettings["RongCloudAppKey"];
            string appSecret = ConfigurationManager.AppSettings["RongCloudAppSecret"];
            string _host = ConfigurationManager.AppSettings["RongCloudUrl"];
            string url = $"{_host}/chatroom/user/gag/list.json";

            StringBuilder sb = new StringBuilder();
            sb.Append("&chatroomId=").Append(HttpUtility.UrlEncode(chatroomId.ToString(), Encoding.UTF8));
            String body = sb.ToString();
            if (body.IndexOf("&") == 0)
            {
                body = body.Substring(1, body.Length - 1);
            }
            String result = RongHttpClient.ExecutePost(appKey, appSecret, body, url, "application/x-www-form-urlencoded");

            var res = (ListGagChatroomUserResult)RongJsonUtil.JsonStringToObj<ListGagChatroomUserResult>(result);
            if (res.Code == 200)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = res;
                return rvm;
            }
            else
            {
                rvm.Msg = "success";
                rvm.Success = false;
                rvm.Result = result;
                return rvm;
            }
        }


        #endregion
    }
}
