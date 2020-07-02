using SSPC_One_HCP.Core.Domain.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface IDiscoveryService
    {
        ReturnValueModel GetDiscoveryHome(WorkUser workUser);
    }
}
