using log4net;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Services.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services
{
    public class WxSaleUserService : IWxSaleUserService
    {
        private readonly ILog _errLogger = LogManager.GetLogger("ErrorFileLogger");
        private readonly IEfRepository _efRepositiry;
        public WxSaleUserService(IEfRepository efRepositiry)
        {
            _efRepositiry = efRepositiry;
        }

        public async Task<WxSaleUserModel> FindSaleUser(string id)
        {
            try
            {
                var user = await _efRepositiry.FirstOrDefaultAsync<WxSaleUserModel>(s => s.IsDeleted != 1 && s.Id == id && s.ADAccount != "" && s.ADAccount != null);
                return user;
            }
            catch (Exception ex)
            {
                _errLogger.Error($"--------------------------------------------------------------------------------");
                _errLogger.Error($"[MSG]:{ex.Message};\n");
                _errLogger.Error($"[Source]:{ex.Source}\n");
                _errLogger.Error($"[StackTrace]:{ex.StackTrace}\n");
                _errLogger.Error($"[StackTrace]:{ex.TargetSite.Name}\n");
                _errLogger.Error($"[Method]: AuthRepository.FindWxUser \n");
                _errLogger.Error($"[IEfRepository]: _efRepositiry is null? {_efRepositiry == null} \n");
                _errLogger.Error($"[UnionId]: {id} \n");
                _errLogger.Error($"--------------------------------------------------------------------------------");
            }
            return null;
        }
    }
}
