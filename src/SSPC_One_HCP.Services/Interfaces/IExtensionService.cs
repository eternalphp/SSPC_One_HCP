using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;


namespace SSPC_One_HCP.Services.Interfaces
{
   public interface IExtensionService
    {
        /// <summary>
        /// 获取公众号推广列表
        /// </summary>
        /// <param name="publicaccount"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetPublicAccount(RowNumModel<PublicAccount> publicaccount,WorkUser workUser);
        /// <summary>
        /// 新增或更新公众号信息
        /// </summary>
        /// <param name="publicaccount"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddorUpdatePublicAccount(PublicAccount publicaccount,WorkUser workUser);
        /// <summary>
        /// 删除公众号推广信息
        /// </summary>
        /// <param name="publicAccount"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeletePublicAccount(PublicAccount publicaccount, WorkUser workUser);
        /// <summary>
        /// 添加二维码推广信息
        /// </summary>
        /// <param name="qrCodeExtension"></param>
        /// <param name="work"></param>
        /// <returns></returns>
        ReturnValueModel AddorUpdateQRCodeExtension(QRcodeExtension qrCodeExtension, WorkUser work);


    }
}
