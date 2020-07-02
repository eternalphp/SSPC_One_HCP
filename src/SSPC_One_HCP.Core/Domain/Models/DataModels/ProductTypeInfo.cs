using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 资料类别信息
    /// </summary>
    [DataContract]
    public class ProductTypeInfo:BaseEntity
    {
        /// <summary>
        /// 1、产品资料
        /// 2、学术资料
        /// 3、播客
        /// </summary>
        [DataMember]
        public int? TypeId { get; set; }

        /// <summary>
        /// 资料组名
        /// </summary>
        [DataMember]
        public string SubTitle { get; set; }

        /// <summary>
        /// 资料类型
        /// </summary>
        [DataMember]
        public string ContentDepType { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        [DataMember]
        public string SubTypeUrl { get; set; }

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
    }
}
