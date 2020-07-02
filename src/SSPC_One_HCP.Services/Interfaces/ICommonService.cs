using SSPC_One_HCP.Core.Domain.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface ICommonService
    {
        /// <summary>
        /// 获取下拉框选项
        /// </summary>
        /// <param name="selectType">下拉框类型</param>
        /// <returns></returns>
        ReturnValueModel GetSelectList(string selectType);

        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetCompanyList();

        /// <summary>
        /// 审批状态获取
        /// </summary>
        /// <param name="status">审批状态key</param>
        /// <param name="assetsMainId">单据主键</param>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        string GetStatusStr(int? status, string assetsMainId, WorkUser workUser);
        
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="unifiedFileId">统一上传文件的主键ID</param>
        /// <returns></returns>
       // SystemFiles DownloadAssetFile(string unifiedFileId);

        /// <summary>
        /// 根据AD域账户获取公司
        /// </summary>
        /// <param name="accountName">AD域账户</param>
        /// <returns></returns>
        ReturnValueModel GetCompanyCodeByAccount(string accountName);

        /// <summary>
        /// 导入多语言
        /// </summary>
        /// <param name="httpRequest">请求内容</param>
        /// <returns></returns>
        ReturnValueModel UploadLanguageConfig(HttpRequest httpRequest);

        /// <summary>
        /// 导出语言包
        /// </summary>
        /// <returns></returns>
        ReturnValueModel ExportLanguageConfig();

        /// <summary>
        /// 获取admin权限
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        string GetAdminRoleId(string companyCode);

        /// <summary>
        /// 获取IT权限
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        string GetItRoleId(string companyCode);
        /// <summary>
        /// 判断是否为管理员
        /// </summary>
        /// <param name="companyCode">公司编号</param>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        bool IsAdmin(WorkUser workUser);
        /// <summary>
        /// 判断是否为BU管理员
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        bool IsBuAdmin(WorkUser workUser);

        void WriteLog(string Content);
    }
}
