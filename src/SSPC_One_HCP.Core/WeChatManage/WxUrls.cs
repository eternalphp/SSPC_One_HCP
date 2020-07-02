using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.WeChatManage
{
    /// <summary>
    /// 微信API地址管理
    /// </summary>
    public static class WxUrls
    {
        /// <summary>
        /// 基础地址
        /// </summary>
        public static string BaseUrl { get; } = @"https://api.weixin.qq.com/";
        /// <summary>
        /// 小程序获取openid的地址
        /// param {0}:appid 微信小程序appid
        /// param {1}:secret 微信小程序appsecret
        /// param {2}:js_code 小程序登录的code
        /// </summary>
        public static string OpenIdUrl { get; } = BaseUrl + @"sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";
        /// <summary>
        /// 小程序获取unionid的地址
        /// param {0}:appid 微信小程序appid
        /// param {1}:secret 微信小程序appsecret
        /// param {2}:js_code 小程序登录的code
        /// </summary>
        public static string UnionIdUrl { get; } = BaseUrl + @"sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";

        /// <summary>
        /// 公众号-通过code换取网页授权access_token
        /// param {0}:appid 公众号的唯一标识
        /// param {1}:secret 公众号的appsecret
        /// param {2}:code 填写第一步获取的code参数
        /// grant_type 填写为authorization_code
        /// </summary>
        public static string AccessTokenUrl { get; } = BaseUrl + @"sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
        /// <summary>
        /// 公众号-拉取用户信息(需scope为 snsapi_userinfo)
        /// param {0}:access_token 网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// param {1}:openid 用户的唯一标识
        /// lang 返回国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语 
        /// </summary>
        public static string UserinfoUrl { get; } = BaseUrl + @"sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";
    }
}
