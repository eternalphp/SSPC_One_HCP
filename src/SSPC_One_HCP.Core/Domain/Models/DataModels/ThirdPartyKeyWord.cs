using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 第三方查询关键字
    /// </summary>
    public class ThirdPartyKeyWord : BaseEntity
    {
        /// <summary>
        /// 关键字内容
        /// </summary>
        [DataMember]
        public string KeyWordContent { get; set; }

        /// <summary>
        /// 关键字类型(1,用药查询 2,期刊影响)
        /// </summary>
        [DataMember]
        public int KeyWordType { get; set; }

        /// <summary>
        /// 关键字排序
        /// </summary>
        [DataMember]
        public int KeyWordSort { get; set; }
    }
}
