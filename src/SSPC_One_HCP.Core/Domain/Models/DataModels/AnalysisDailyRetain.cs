using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 日存留 获取用户访问小程序日留存
    /// </summary>
    [DataContract]
    public class AnalysisDailyRetain : BaseEntity
    {
        /// <summary>
        /// 日期
        /// </summary>
        [DataMember]
        public string ref_date { get; set; }

        /// <summary>
        /// 新增用户留存
        /// </summary>
        [DataMember]
        public string visit_uv_new { get; set; }


        /// <summary>
        /// 活跃用户存留
        /// </summary>
        [DataMember]
        public string visit_uv { get; set; }
    }
}
