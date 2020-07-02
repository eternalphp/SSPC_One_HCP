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
    /// 日期折线图 增长趋势
    /// </summary>
    [DataContract]
    [NotMapped]
    public class DayList
    {
        /// <summary>
        /// 日期 中文
        /// </summary>
        public string Day { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 日期 排序时间
        /// </summary>
        public DateTime DayTime { get; set; }
    }
}
