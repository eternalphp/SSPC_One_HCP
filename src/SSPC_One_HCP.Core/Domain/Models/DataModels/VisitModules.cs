using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 医生各模块访问记录表
    /// </summary>
    public class VisitModules : BaseEntity
    {
        /// <summary>
        /// UnionId
        /// </summary>
        [DataMember]
        [DisplayName("UnionId")]
        public string UnionId { get; set; }


        /// <summary>
        /// 停留开始时间
        /// </summary>
        [DataMember]
        [DisplayName("VisitStart")]
        public DateTime? VisitStart { get; set; }


        /// <summary>
        /// 停留结束时间
        /// </summary>
        [DataMember]
        [DisplayName("VisitEnd")]
        public DateTime? VisitEnd { get; set; }
         

        /// <summary>
        /// 停留时间（秒）
        /// </summary>
        [DataMember]
        [DisplayName("StaySeconds")]
        public int StaySeconds { get; set; }

        /// <summary>
        /// 是否用户
        /// </summary>
        [DataMember]
        [DisplayName("Isvisitor")]
        public int Isvisitor { get; set; }

        /// <summary>
        /// 模块编号
        /// </summary>
        [DataMember]
        [DisplayName("ModuleNo")]
        public string ModuleNo { get; set; }

        /// <summary>
        /// 模块页面编号
        /// </summary>
        [DataMember]
        [DisplayName("ModulePageNo")]
        public string ModulePageNo { get; set; }

        /// <summary>
        /// 模块页面路径
        /// </summary>
        [DataMember]
        [DisplayName("ModulePageUrl")]
        public string ModulePageUrl { get; set; }


        /// <summary>
        /// WxUserid
        /// </summary>
        [DataMember]
        [DisplayName("WxUserid")]
        public string WxUserid { get; set; }


    }
}
