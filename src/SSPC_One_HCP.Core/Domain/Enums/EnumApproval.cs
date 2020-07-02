using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Enums
{
    /// <summary>
    /// 审批相关
    /// </summary>
    public enum EnumApproval
    {
        /// <summary>
        /// 草稿
        /// </summary>
        Draft = 1,
        /// <summary>
        /// 待审批
        /// </summary>
        Pending = 2,
        /// <summary>
        /// 驳回
        /// </summary>
        Reject = 3,
        /// <summary>
        /// 审批完成
        /// </summary>
        CompletionApproval = 4,
        /// <summary>
        /// 用户收货
        /// </summary>
        Receiving = 5,
        /// <summary>
        /// 完成
        /// </summary>
        Complete = 6,
        /// <summary>
        /// 作废
        /// </summary>
        Obsolete = 7,
        /// <summary>
        /// 已审批
        /// </summary>
        Approved = 8,
        /// <summary>
        /// PS数据发送失败
        /// </summary>
        PsSendError = 9
    }
}
