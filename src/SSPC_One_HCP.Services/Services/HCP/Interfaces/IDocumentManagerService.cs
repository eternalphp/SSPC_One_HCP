using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Services.HCP.Dto;
using System.IO;

namespace SSPC_One_HCP.Services.Services.HCP.Interfaces
{
    public interface IDocumentManagerService
    {
        /// <summary>
        /// 获取资料详情列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetList(RowNumModel<DocumentManagerSearchInputDto> rowNum, WorkUser workUser);
        /// <summary>
        /// 新增或修改资料信息
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddOrUpdate(DocumentManagerInputDto productInfoView, WorkUser workUser);
        /// <summary>
        /// 删除资料
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel Delete(DocumentManager dataInfo, WorkUser workUser);

        /// <summary>
        /// 预览PDF
        /// </summary>qy
        /// <returns></returns>
        FileInfo PreviewPdf(string id);
    }
}
