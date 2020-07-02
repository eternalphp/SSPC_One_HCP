using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 添加新名片模型
    /// </summary>
    public class AddBusinessCardViewModel
    {
        ///// <summary>
        ///// 对方医生的UnionId, 和OwnerWxUserId两者间只能选一个
        ///// </summary>
        //[DataMember]
        //public string ObjectUnionId { get; set; }

        /// <summary>
        /// 对方医生的Id
        /// </summary>
        [DataMember]
        public string OwnerWxUserId { get; set; }
    }
}
