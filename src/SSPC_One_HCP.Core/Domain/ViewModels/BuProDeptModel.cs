using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 前端使用（保存）
    /// </summary>
    [DataContract]
    public class BuProDeptModel
    {
        /// <summary>
        /// bu名称
        /// </summary>
        [DataMember]
        public string BuName { get; set; }
        /// <summary>
        /// 产品Id
        /// </summary>
        [DataMember]
        public string ProId { get; set; }
        /// <summary>
        /// bu名称原来
        /// </summary>
        [DataMember]
        public string BuNameOld { get; set; }
        /// <summary>
        /// 产品Id原来
        /// </summary>
        [DataMember]
        public string ProIdOld { get; set; }
        /// <summary>
        /// 科室Id集合
        /// </summary>
        [DataMember]
        public List<string> DeptIds { get; set; }
    }
}
