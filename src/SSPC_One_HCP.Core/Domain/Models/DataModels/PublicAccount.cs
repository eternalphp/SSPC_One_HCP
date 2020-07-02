using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 公众号推广
    /// </summary>
    [DataContract]
    public class PublicAccount : BaseEntity
    {
        /// <summary>
        /// 小程序appid
        /// </summary>
        [DataMember]
        public string AppId { get; set; }
        /// <summary>
        /// 公众号名称
        /// </summary>
        [DataMember]
        public string PublicAccountName { get; set; }
        /// <summary>
        /// 跳转路径
        /// </summary>
        [DataMember]
        public string AppUrl { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        [DataMember]
        public int Iseffective { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        [DataMember]
        public string Dept { get; set; }
        /// <summary>
        /// 科室序列
        /// </summary>
        [DataMember]
        public string[] DeptList { get; set; }
        ///// <summary>
        ///// 排序
        ///// </summary>
        //[DataMember]
        //public string IsSort { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        [DataMember]
        public string ImageUrl { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        [DataMember]
        public string ImageName { get; set; }
    }
}
