using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface IWechatActionHistoryService
    {
        ReturnValueModel AddActionHistory(RowNumModel<WechatActionHistory> model, WorkUser workUser);

        ReturnValueModel GetMagaZineList();
    }
}
