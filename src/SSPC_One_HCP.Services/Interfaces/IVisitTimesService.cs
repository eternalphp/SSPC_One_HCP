using SSPC_One_HCP.Core.Domain.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.ViewModels;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface IVisitTimesService
    {
        ReturnValueModel AddVisitTimes(RowNumModel<VisitTimesViewModel> model, WorkUser workUser);
    }
}
