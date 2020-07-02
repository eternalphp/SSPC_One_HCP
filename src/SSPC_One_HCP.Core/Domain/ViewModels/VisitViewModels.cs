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
    /// 浏览记录 临时表
    /// </summary>
    [NotMapped]
    [DataContract]

    public class VisitViewModels
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public string DateBegin { get; set; }

        /// <summary>
        /// 学习时间
        /// </summary>
        public int?  Studytime { get; set; }

        /// <summary>
        /// 学习日期
        /// </summary>
        public string  dataday { get; set; }

        /// <summary>
        /// 学习标题
        /// </summary>
        public string Title { get; set; }


    }
}
