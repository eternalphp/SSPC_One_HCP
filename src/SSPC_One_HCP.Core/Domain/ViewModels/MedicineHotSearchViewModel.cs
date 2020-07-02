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
    /// 用药热搜模型
    /// </summary>
    [NotMapped]
    [DataContract]
    public class MedicineHotSearchViewModel
    {
        /// <summary>
        /// 关键词
        /// </summary>
        [DataMember]
        public string KeyWord { get; set; }

        /// <summary>
        /// 搜索类别   1|2|3  药品|适应症|相互作用
        /// </summary>

        [DataMember]
        public string Type { get; set; }
    }
}
