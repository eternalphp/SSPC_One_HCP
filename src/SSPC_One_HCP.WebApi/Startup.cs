using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using SSPC_One_HCP.OAuth.Providers;
using System.Web.Http;
using log4net;
using Microsoft.Owin.Security.Infrastructure;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Services.MasterData;

[assembly: OwinStartup(typeof(SSPC_One_HCP.WebApi.Startup))]

namespace SSPC_One_HCP.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private readonly ILog _errorLog = LogManager.GetLogger("ErrorFileLogger");
        private readonly string _clientId = ConfigurationManager.AppSettings["clientId"];
        private readonly string _clientSecret = ConfigurationManager.AppSettings["clientSecret"];
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            
            ConfigureOAuth(app);
            HttpConfiguration config = new HttpConfiguration();
            //配置api信息
            app.UseWebApi(config);
            app.UseAutofacWebApi(config);
            log4net.Config.XmlConfigurator.Configure();
            app.UseAutofacMiddleware(ContainerManager.Container);
            //WebApiConfig.Register(config);
            //使用跨域
            app.UseCors(CorsOptions.AllowAll);

            #region 接受主数据
            MDS.Api.PushOptions pushOptions = new MDS.Api.PushOptions
            {
                ClientId = _clientId,
                ClientKey = _clientSecret
            };
            pushOptions.UsePushListener<OrganizationPushListener>(MDS.Api.DataCategory.Organization);
            pushOptions.UsePushListener<UserPushListener>(MDS.Api.DataCategory.User);
            pushOptions.UsePushListener<PositionPushListener>(MDS.Api.DataCategory.Position);
            pushOptions.OnException += PushOptions_OnException;
            app.UseMasterDataPushServer(pushOptions);
            #endregion
          

            //开始后台工作
            Jobs.JobScheduler.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        private void ConfigureOAuth(IAppBuilder app)
        {
            // 使应用程序可以使用 Cookie 来存储已登录用户的信息
            // 并使用 Cookie 来临时存储有关使用第三方登录提供程序登录的用户的信息
            //app.UseCookieAuthentication(new CookieAuthenticationOptions());
            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/auth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(12),
                Provider = new CustomOAuthorizationServerProvider(),
                RefreshTokenProvider = new CustomRefreshTokenProvider()
            };
            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);

            OAuthAuthorizationServerOptions OAuthServerOptionsWx = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/auth/token/Wx"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(12),
                Provider = new WxOAuthorizationServerProvider(),
                RefreshTokenProvider = new CustomRefreshTokenProvider(),
                //AuthorizeEndpointPath = new PathString("/auth/code"),
                //AuthorizationCodeProvider = new AuthenticationTokenProvider()
            };
            app.UseOAuthAuthorizationServer(OAuthServerOptionsWx);


            OAuthAuthorizationServerOptions OAuthServerOptionsSaleWx = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/auth/token/WxSale"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(12),
                Provider = new WxSaleOAuthorizationServerProvider(),
                //RefreshTokenProvider = new CustomRefreshTokenProvider(),
                //AuthorizeEndpointPath = new PathString("/auth/code"),
                //AuthorizationCodeProvider = new AuthenticationTokenProvider()
            };
            app.UseOAuthAuthorizationServer(OAuthServerOptionsSaleWx);


            OAuthAuthorizationServerOptions OAuthServerOptionsYs = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/auth/token/Ys"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(12),
                Provider = new YsOAuthorizationServerProvider(),
                //RefreshTokenProvider = new CustomRefreshTokenProvider(),
                //AuthorizeEndpointPath = new PathString("/auth/code"),
                //AuthorizationCodeProvider = new AuthenticationTokenProvider()
            };
            app.UseOAuthAuthorizationServer(OAuthServerOptionsYs);

            //OAuthAuthorizationServerOptions OAuthServerOptionsExternal = new OAuthAuthorizationServerOptions()
            //{
            //    AllowInsecureHttp = true,
            //    TokenEndpointPath = new PathString("/auth/token/External"),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromHours(12),
            //    Provider = new WxOAuthorizationServerProvider(),
            //    RefreshTokenProvider = new CustomRefreshTokenProvider()
            //};

            //app.UseOAuthAuthorizationServer(OAuthServerOptionsWx);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            // 取消注释以下行可允许使用第三方登录提供程序登录
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

        /// <summary>
        /// 主数据接收异常
        /// </summary>
        /// <param name="exception">错误</param>
        private void PushOptions_OnException(Exception exception)
        {
            //发生异常后的处理
            _errorLog.Error("主数据接收异常----------------------->BEGIN");
            _errorLog.Error("错误消息:" + exception.Message);
            _errorLog.Error(exception.Source);
            _errorLog.Error(exception.StackTrace);
            _errorLog.Error("主数据接收异常----------------------->END");
        }
    }
}
