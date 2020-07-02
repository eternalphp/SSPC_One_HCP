using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Services.Bot.Dto;
using System.IO;

namespace SSPC_One_HCP.Services.Services.WeChat.Interfaces
{
    public interface IWcHcpDataInfoService
    {
        /// <summary>
        /// 获取目录名称
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetFileList(string buName, WorkUser workUser);
        /// <summary>
        /// 资料列表
        /// </summary>
        /// <param name="row"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetDataPageList(RowNumModel<WeChatHcpDataInfoInputDto> row, WorkUser workUser);
        /// <summary>
        /// 根据ID查询数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetData(string id, WorkUser workUser);
        /// <summary>
        /// 预览PDF
        /// </summary>
        /// <returns></returns>
        FileInfo PreviewPdf(string id);
        /// <summary>
        /// 获取文件Title
        /// </summary>
        /// <param name="row"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetTitle(string id);
        /// <summary>
        /// 资料是否显示红点
        /// </summary>
        /// <param name="buName"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetRedDot(string buName, WorkUser workUser);

    }
}
