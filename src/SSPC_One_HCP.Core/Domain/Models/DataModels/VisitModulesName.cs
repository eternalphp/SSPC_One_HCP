using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 模块访问编码
    /// </summary>
    public class VisitModulesName:BaseEntity
    {
        /// <summary>
        ///模块名称
        /// </summary>
        [DataMember]
        public string ModulesName { get; set; }

        /// <summary>
        /// 模块编号
        /// </summary>
        public string ModulesNo { get; set; }

        /// <summary>
        /// 模块对应小程序路径
        /// </summary>
        public string ModulesUrl { get; set; }
    }
}
