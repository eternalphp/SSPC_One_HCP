using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    [DataContract]
   public class MyFixedDate:BaseEntity
    {
        /// <summary>
        /// 数据序号
        /// </summary>
        [DataMember]
        public int Sort { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        [DataMember]
        public string Type { get; set; }
        /// <summary>
        /// 数据内容
        /// </summary>
        [DataMember]
        public string Text { get; set; }
       
       
    }
}
