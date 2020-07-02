using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Services.MasterData
{
    public class MasterDataPush
    {
        public static List<UserInfo> UserInfos { get; set; } = new List<UserInfo>();
        public static List<Organization> Organizations { get; set; } = new List<Organization>();
        public static List<Position> Positions { get; set; } = new List<Position>();
    }
}
