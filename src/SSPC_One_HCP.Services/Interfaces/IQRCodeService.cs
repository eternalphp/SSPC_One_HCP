using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Services.Interfaces
{
    /// <summary>
    /// 二维码相关
    /// </summary>
    public interface IQRCodeService
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        string GenerateQRCode(string content);

        /// <summary>
        /// 新增或修改推广二维码信息
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdateAdQRCode(AdQRCode viewModel, WorkUser workUser);

        /// <summary>
        /// 删除推广二维码信息
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteAdQRCode(AdQRCode viewModel, WorkUser workUser);

        /// <summary>
        /// 解析推广二维码, 获取推广的公众号或小程序的相关信息, 同时二维码访问次数+1
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        ReturnValueModel AnalyzeAdQRCode(string id);

        /// <summary>
        /// 获取推广二维码的列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ReturnValueModel GetAdQRCodeList(RowNumModel<AdQRCode> rowNum);
    }
}
