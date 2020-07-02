using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 产品信息
    /// </summary>
    [DataContract]
    public class ProductInfo : BaseEntity
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        [DataMember]
        public string ProductName { get; set; }
        /// <summary>
        /// 产品描述
        /// </summary>
        [DataMember]
        public string ProductDesc { get; set; }
        /// <summary>
        /// 产品图片路径
        /// </summary>
        [DataMember]
        public string ProductUrl { get; set; }
        /// <summary>
        /// 产品图片名称
        /// </summary>
        [DataMember]
        public string ProductPicName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public int? Sort { get; set; }

        /// <summary>
        /// 是否完成审核
        /// 1、已审核
        /// 2、新增未审核
        /// 3、审核拒绝
        /// 4、已锁定
        /// 5、已作废
        /// 6、删除未审核
        /// 7、已删除
        /// 8、编辑未审核
        /// </summary>
        [DataMember]
        public int? IsCompleted { get; set; }
        
        /// <summary>
        /// 修改前原来的记录Id
        /// </summary>
        [DataMember]
        public string OldId { get; set; }

        /// <summary>
        /// 审批备注
        /// </summary>
        [DataMember]
        public string ApprovalNote { get; set; }

    }
}
