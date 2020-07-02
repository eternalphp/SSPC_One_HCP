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
    /// 获取用户访问小程序数据日趋势
    /// </summary>
    public class AnalysisDailyVisitTrend : BaseEntity
    {
        /// <summary>
        ///  日期
        /// </summary>
        [DataMember]
        [DisplayName("ref_date")]
        public string ref_date { get; set; }

        /// <summary>
        ///  打开次数 
        /// </summary>
        [DataMember]
        [DisplayName("session_cnt")]
        public int session_cnt { get; set; }

        /// <summary>
        ///  访问次数
        /// </summary>
        [DataMember]
        [DisplayName("visit_pv")]
        public int visit_pv { get; set; }

        /// <summary>
        ///  访问人数
        /// </summary>
        [DataMember]
        [DisplayName("visit_uv")]
        public int visit_uv { get; set; }
        /// <summary>
        ///  新用户数
        /// </summary>
        [DataMember]
        [DisplayName("visit_uv_new")]
        public int visit_uv_new { get; set; }


        /// <summary>
        ///  人均停留时长 (浮点型，单位：秒)
        /// </summary>
        [DataMember]
        [DisplayName("stay_time_uv")]
        public float stay_time_uv { get; set; }



        /// <summary>
        ///  次均停留时长 (浮点型，单位：秒)
        /// </summary>
        [DataMember]
        [DisplayName("stay_time_session")]
        public float stay_time_session { get; set; }



        /// <summary>
        ///  平均访问深度 (浮点型)
        /// </summary>
        [DataMember]
        [DisplayName("visit_depth")]
        public float visit_depth { get; set; }
    }
}
