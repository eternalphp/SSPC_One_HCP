using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 访问模块
    /// </summary>
    [NotMapped]
    [DataContract]
    
    public class VisitModulesViewModel
    {


        /// <summary>
        /// 模块编号
        ///1.发现
        ///2.会议
        ///3.知识库
        ///4.我的 
        ///5.公众号推广
        ///</summary>
        [DataMember]
        public string ModuleNo { get; set; }

        /// <summary>
        /// 模块页面编号
        /// </summary>
        [DataMember]
        public string ModulePageNo { get; set; }

        /// <summary>
        /// 模块页面Url
        /// </summary>
        [DataMember]
        public string ModulePageUrl { get; set; }

        /// <summary>
        /// 停留开始时间
        /// </summary>
        [DataMember]
        public DateTime? VisitStart { get; set; }

        /// <summary>
        /// 停留结束时间
        /// </summary>
        [DataMember]
        public DateTime? VisitEnd { get; set; }
    }
}
