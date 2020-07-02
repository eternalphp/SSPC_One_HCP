using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.RongCloud.response;
using SSPC_One_HCP.RongCloud.util;
using SSPC_One_HCP.Services.RongCloud.Dto.YsWeChat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SSPC_One_HCP.Services.RongCloud.YsWeChat
{
    public class WxRongCloudService : IWxRongCloudService
    {
        private readonly IEfRepository _rep;
        public WxRongCloudService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 融云-获取Token
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ReturnValueModel GetToken(WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            StringBuilder sb = new StringBuilder();

            string tourist = $"{ConfigurationManager.AppSettings["HostUrl"]}/Content/images/Tourist.jpg";//游客头像
            if (!string.IsNullOrEmpty(workUser?.WxUser?.Id))
            {
                var doctor = _rep.FirstOrDefault<WxUserModel>(s => s != null && s.IsDeleted != 1 && s.Id == workUser.WxUser.Id);
                if (doctor == null)
                {
                    rvm.Msg = "获取用户失败";
                    rvm.Success = false;
                    rvm.Result = workUser.WxUser.Id;
                    return rvm;
                }
                var name = string.IsNullOrEmpty(doctor.WxName) ? "游客" : doctor.WxName;
                var portraitUri = string.IsNullOrEmpty(doctor.WxPicture) ? tourist : doctor.WxPicture;
                sb.Append("&userId=").Append(HttpUtility.UrlEncode(doctor.Id.ToString(), Encoding.UTF8));
                sb.Append("&name=").Append(HttpUtility.UrlEncode(name, Encoding.UTF8));
                sb.Append("&portraitUri=").Append(HttpUtility.UrlEncode(portraitUri, Encoding.UTF8));

            }
            else
            {
                sb.Append("&userId=").Append(HttpUtility.UrlEncode(Guid.NewGuid().ToString(), Encoding.UTF8));
                sb.Append("&name=").Append(HttpUtility.UrlEncode("游客", Encoding.UTF8));
                sb.Append("&portraitUri=").Append(HttpUtility.UrlEncode(tourist, Encoding.UTF8));
            }

            String body = sb.ToString();
            if (body.IndexOf("&") == 0)
                body = body.Substring(1, body.Length - 1);

            string appKey = ConfigurationManager.AppSettings["RongCloudAppKey"];
            string appSecret = ConfigurationManager.AppSettings["RongCloudAppSecret"];
            string _host = ConfigurationManager.AppSettings["RongCloudUrl"];
            string url = $"{_host}/user/getToken.json";

            string result = RongHttpClient.ExecutePost(appKey, appSecret, body, url, "application/x-www-form-urlencoded");

            var res = (TokenResult)RongJsonUtil.JsonStringToObj<TokenResult>(result);
            if (res.Code == 200)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = res;
                return rvm;
            }
            else
            {
                rvm.Msg = "获取Token失败";
                rvm.Success = false;
                rvm.Result = result;
                return rvm;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ChatRoomId"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel UserChatroom(string chatRoomId, WorkUser workUser)
        {

            ReturnValueModel rvm = new ReturnValueModel();
     
            var cloudChatroomStatus = _rep.Where<RongCloudChatroomStatus>(s => s != null && s.IsDeleted != 1 && s.ChatRoomId == chatRoomId && s.UserId == workUser.WxUser.Id)
                .OrderByDescending(o => o.CreateTime)
                .Skip(0).Take(1).ToList();
            var model = (cloudChatroomStatus != null && cloudChatroomStatus.Count > 0) ? cloudChatroomStatus[0] : new RongCloudChatroomStatus();
            if (model?.Status == "1" && model?.Type == "2")
            {
                rvm.Msg = "用户不在聊天室";
                rvm.Success = true;
                rvm.Result = false;
                return rvm;
            }
            else
            {
                rvm.Msg = "用户在聊天室";
                rvm.Success = true;
                rvm.Result = true;
                return rvm;
            }
        }
    }
}
