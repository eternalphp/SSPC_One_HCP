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
    /// 访问次数
    /// </summary>
    [NotMapped]
    [DataContract]
  
    public class VisitTimesViewModel
    {
        /// <summary>
        /// 打开开始时间
        /// </summary>
        [DataMember]
        public DateTime? VisitStart { get; set; }

        /// <summary>
        /// 打开结束时间
        /// </summary>
        [DataMember]
        public DateTime? VisitEnd { get; set; }
    }
}
