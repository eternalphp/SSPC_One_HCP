using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Cache;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.OAuth.Interfaces;
using SSPC_One_HCP.Services.Bot;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.OAuth.Providers
{
    /// <summary>
    /// 小程序 销售用户授权
    /// </summary>
    public class WxSaleOAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            AppClientInfo client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Validated();
                //context.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }


            client = ContainerManager.Resolve<IAuthRepository>().FindClient(context.ClientId);


            if (client == null)
            {
                context.SetError("invalid_clientId", $"Client '{context.ClientId}' is not registered in the system.");
                context.SetError("res_msg", "登录失败");
                context.SetError("res_code", "60007");
                context.SetError("ERROR_LOGIN", "登录失败");
                return Task.FromResult<object>(null);
            }

            if (!(client.ApplicationType == 1))
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    context.SetError("res_msg", "登录失败");
                    context.SetError("res_code", "60007");
                    context.SetError("ERROR_LOGIN", "登录失败");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != StringEncryptionHelper.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid.");
                        context.SetError("res_msg", "登录失败");
                        context.SetError("res_code", "60007");
                        context.SetError("ERROR_LOGIN", "登录失败");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                context.SetError("res_msg", "登录失败");
                context.SetError("res_code", "60007");
                context.SetError("ERROR_LOGIN", "登录失败");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());
            context.OwinContext.Set<string>("companyCode", context.Parameters["company_code"]);
            context.OwinContext.Set<string>("culture", context.Parameters["culture"]);
            context.Request.Set("culture", context.Parameters["culture"]);
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            //IAuthRepository authRepository = new AuthRepository();
            //var companyCode = context.OwinContext.Environment["companyCode"].ToString();
            //var culture = context.OwinContext.Environment["culture"].ToString();
            if (string.IsNullOrEmpty(context.UserName))
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                context.SetError("res_code", "60005");
                context.SetError("res_msg", "登录失败");
                context.SetError("ERROR_LOGIN", "登录失败");
                return;
            }

            var rep = ContainerManager.Resolve<IEfRepository>();

            var user = await rep.FirstOrDefaultAsync<WxSaleUserModel>((s => s.IsDeleted != 1 && s.Id == context.UserName));
           // var user = await ContainerManager.Resolve<IWxSaleUserService>().FindSaleUser(context.UserName);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                context.SetError("res_code", "60005");
                context.SetError("res_msg", "登录失败");
                context.SetError("ERROR_LOGIN", "登录失败");
                return;
            }
            var wxRegisterService = ContainerManager.Resolve<IWxRegisterService>();
            wxRegisterService.CacheWxSaleUser(user);

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Wxuser"));
            //加入client字典用于刷新token
            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {
                    "as:client_id", context.ClientId ?? string.Empty
                },
                {
                    "userName", context.UserName
                },
                {
                    "name", user.Id
                },
                {
                    "res_code","6000"
                },
                {
                    "res_msg","登录成功"
                }
            });
            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

        }
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;
            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                context.SetError("res_msg", "登录失败");
                context.SetError("res_code", "60007");
                context.SetError("ERROR_LOGIN", "登录失败");
                return Task.FromResult<object>(null);
            }


            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newClaim = newIdentity.Claims.FirstOrDefault(c => c.Type == "newClaim");
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            #region 当前用户信息
            //var rep = ContainerManager.Resolve<IEfRepository>();
            //var cache = ContainerManager.Resolve<ICacheManager>();
            //var companyCode = context.OwinContext.Environment["companyCode"].ToString();
            //var culture = context.OwinContext.Environment["culture"].ToString();
            //var user = ContainerManager.Resolve<IAuthRepository>().FindUser(context.Ticket.Identity.Name, companyCode);
            //cache.Clear();
            //var workUser = new WorkUser
            //{
            //    User = user,
            //    Culture = culture,
            //    RoleCodeList = rep.All<T_SysRoleUser>().Where(c => c.IsDeleted != 1 && c.UserID == user.Id && c.CompanyCode == user.CompanyCode).Select(s => s.RoleCode).ToList(),
            //    Position = (from p in rep.All<T_SysPosition>()
            //                join u in rep.All<T_SysOrgUser>() on p.Id equals u.PosID
            //                where u.UserID == user.Id && p.CompanyCode == user.CompanyCode
            //                select p).FirstOrDefault() ?? new T_SysPosition(),
            //    Orgnation = (from o in rep.All<T_SysOrgnation>()
            //                 join u in rep.All<T_SysOrgUser>() on o.OrgCode equals u.OrgCode
            //                 where u.UserID == user.Id && o.CompanyCode == user.CompanyCode
            //                 select o).FirstOrDefault() ?? new T_SysOrgnation(),
            //    CompanyCode = companyCode
            //};
            //cache.Set("CurrentUser", workUser, 12);
            #endregion
            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(context);
        }

    }
}
