using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.CommonModels
{
    /// <summary>
    /// 分页基础
    /// </summary>
    [DataContract]
    public class BaseRowNumModel
    {        
        /// <summary>
        /// 当前页码（从1开始）
        /// </summary>
        [DataMember(Name = "pageindex")]
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        [DataMember(Name = "pagesize")]
        public int PageSize { get; set; }
        /// <summary>
        /// 排序字段名称
        /// </summary>
        [DataMember(Name = "ordername")]
        public string OrderName { get; set; }
        /// <summary>
        /// 排序（正序、倒序）
        /// </summary>
        [DataMember(Name = "sortorder")]
        public string SortOrder { get; set; }
    }
}