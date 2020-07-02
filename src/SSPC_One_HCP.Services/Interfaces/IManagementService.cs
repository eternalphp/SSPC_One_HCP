using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels.Approval;

namespace SSPC_One_HCP.Services.Interfaces
{
    /// <summary>
    /// 短信管理
    /// </summary>
    public interface IManagementService
    {
        /// <summary>
        /// 新建或修改短信模板
        /// </summary>
        /// <param name="manage"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddorUpdateManagement(Management manage,WorkUser workUser);
        /// <summary>
        /// 删除短信模板
        /// </summary>
        /// <param name="manage"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteManagement(Management manage,WorkUser workUser);
        /// <summary>
        /// 获取所有短信模板
        /// </summary>
        /// <param name="manage"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetManagementList(RowNumModel<Management> manage, WorkUser workUser);
        /// <summary>
        /// 提交审核结果
        /// </summary>
        /// <param name="approvalResult"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel ManagementApproval(ApprovalResultViewModel approvalResult, WorkUser workUser);
        /// <summary>
        /// 撤销修改
        /// </summary>
        /// <param name="manage"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel RevokeUpdateManagement(Management manage,WorkUser workUser);
    }
}
