using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Bot
{
    public interface IWxSaleUserService
    {
        Task<WxSaleUserModel> FindSaleUser(string id);
    }
}
