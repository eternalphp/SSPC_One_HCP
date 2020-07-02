using SSPC_One_HCP.Core.Domain.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.ViewModels;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface IVisitModulesService
    {
        /// <summary>
        /// 新增用户访问模块记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ReturnValueModel AddVisitModules(RowNumModel<VisitModulesViewModel> model,WorkUser workUser);
       
    }
}
