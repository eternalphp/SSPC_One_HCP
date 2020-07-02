using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.OAuth.Interfaces;

namespace SSPC_One_HCP.OAuth.Providers
{
    public class YsOAuthorizationServerProvider : OAuthAuthorizationServerProvider
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
                context.SetError("res_code", "60004");
                context.SetError("res_msg", "登录失败");
                return Task.FromResult<object>(null);
            }

            if (!(client.ApplicationType == 1))
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    context.SetError("res_code", "60003");
                    context.SetError("res_msg", "登录失败");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != StringEncryptionHelper.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid.");
                        context.SetError("res_code", "60002");
                        context.SetError("res_msg", "登录失败");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                context.SetError("res_code", "60001");
                context.SetError("res_msg", "登录失败");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());
            context.Validated();
            return base.ValidateClientAuthentication(context);
        }
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            if (string.IsNullOrEmpty(context?.ClientId))
            {
                return base.GrantClientCredentials(context);
            }
             
            var client = ContainerManager.Resolve<IAuthRepository>().FindClient(context.ClientId);
            if (client == null)
            {
                return base.GrantClientCredentials(context);
            }

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { client.AllowedOrigin });
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, context.ClientId));
            var ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties());
            context.Validated(ticket);

            return base.GrantClientCredentials(context);
        }

    }
}