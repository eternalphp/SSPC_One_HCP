using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.CommonModels
{
    [DataContract]
    public class RowNumModel<T> : BaseRowNumModel where T : class, new()
    {
        /// <summary>
        /// 搜索
        /// </summary>
        [DataMember(Name = "searchParams")]
        public T SearchParams { get; set; }
    }
}
