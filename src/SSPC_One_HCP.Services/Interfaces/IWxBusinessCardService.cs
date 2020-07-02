using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface IWxBusinessCardService
    {
        /// <summary>
        /// 添加名片夹
        /// </summary>
        ReturnValueModel AddBusinessCard(AddBusinessCardViewModel businessCard, WorkUser workUser);

        /// <summary>
        /// 我的名片夹
        /// </summary>
        ReturnValueModel GetBusinessCardList(RowNumModel<BusinessCard> businessCard, WorkUser workUser);
    }
}
