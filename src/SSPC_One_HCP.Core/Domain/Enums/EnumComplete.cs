namespace SSPC_One_HCP.Core.Domain.Enums
{
    /// <summary>
    /// 审批状态枚举
    /// </summary>
    public enum EnumComplete
    {
        /// <summary>
        /// 已审核
        /// </summary>
        Approved = 1,

        /// <summary>
        /// 未审核
        /// </summary>
        AddedUnapproved = 2,

        /// <summary>
        /// 审核拒绝
        /// </summary>
        Reject = 3,

        /// <summary>
        /// 已锁定
        /// </summary>
        Locked = 4,

        /// <summary>
        /// 已作废
        /// </summary>
        Obsolete = 5,

        /// <summary>
        /// 将要删除
        /// </summary>
        WillDelete = 6,

        /// <summary>
        /// 已删除
        /// </summary>
        Deleted = 7,

        /// <summary>
        /// 修改后未审核
        /// </summary>
        UpdatedUnapproved = 8,

        /// <summary>
        /// 取消修改
        /// </summary>
        CanceledUpdate = 9,
    }
}
