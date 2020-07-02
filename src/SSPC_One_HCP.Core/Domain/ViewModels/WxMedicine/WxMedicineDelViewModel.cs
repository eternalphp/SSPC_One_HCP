using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.WxMedicine
{
    /// <summary>
    /// 个人查询记录 删除
    /// </summary>
    [DataContract]
    public class WxMedicineDelViewModel
    {
        /// <summary>
        /// 个人搜索记录id
        /// </summary>
        [DataMember]
        public List<string> historyId { get; set; }
    }
}
