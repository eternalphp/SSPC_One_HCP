using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Services.Services.WeChat.Dto;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.WeChat.Interfaces
{
    public interface IWcSaleAdService
    {

        /// <summary>
        /// 多福-销售登录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        Task<ReturnValueModel> Login(LoginInputDto dto, WorkUser workUser);
    }
}
