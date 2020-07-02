using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 小程序模板消息FormID 保存
    /// </summary>
    [DataContract]
    public class TemplateForm:BaseEntity
    {
        /// <summary>
        /// 用于发送模板消息
        /// </summary>
        [DataMember]
        public string FormID { get; set; }
        /// <summary>
        /// 用于发送模板时间
        /// </summary>
        [DataMember]
        public DateTime? SendTime { get; set; }

        /// <summary>
        /// 跳转页面
        /// </summary>
        [DataMember]
        public string Page { get; set; }
        /// <summary>
        /// 接受者
        /// </summary>
        [DataMember]
        public string OpenID { get; set; }

        /// <summary>
        /// 消息编号
        /// </summary>
        [DataMember]
        public int MsgID { get; set; }

    }
}
