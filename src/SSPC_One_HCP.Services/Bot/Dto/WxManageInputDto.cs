using SSPC_One_HCP.Core.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Bot.Dto
{
    public class WxManageInputDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string code { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string encryptedData { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string iv { set; get; }


        public string AppId { get; set; }

        public string WxSceneId { get; set; }

        public DecodedUserInfoModel userInfo { get; set; }
    }

}
