using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using SSPC_One_HCP.Core.Domain.Enums;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 短信类
    /// </summary>
   public class Management:BaseEntity
    {
        /// <summary>
        /// 短信编号
        /// </summary>
        [DataMember]
        public string ManagementId { get; set; }
        /// <summary>
        /// 短信内容
        /// </summary>
        [DataMember]
        public string ManagementWord { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        [DataMember]
        public EnumComplete? IsCompleted { get; set; }
        /// <summary>
        /// 原本的Id
        /// </summary>
        [DataMember]
        public string OldManagementId { get; set; }
        /// <summary>
        /// 查询审核信息
        /// </summary>
        [DataMember]
        public  int [] IsCompletedList { get; set; }
    }
}
