using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
namespace SSPC_One_HCP.Core.Domain.ViewModels.StaticModels
{
    [NotMapped]
    [DataContract]
    public class OverViewModel
    {
        /// <summary>
        /// 总访问人数
        /// </summary>
        [DataMember]
        public int allvisitcount { get; set; }


        /// <summary>
        /// 已授权人数
        /// </summary>
        [DataMember]
        public int hasauthListcount { get; set; }

        /// <summary>
        /// 已注册人数
        /// </summary>
        [DataMember]
        public int hasregistercount { get; set; }

        /// <summary>
        /// 总验证通过
        /// </summary>
        [DataMember]
        public int hascheckcount { get; set; }


        /// <summary>
        /// 总验证未通过
        /// </summary>
        [DataMember]
        public int notpasscount { get; set; }


        /// <summary>
        /// 总验证未定人数
        /// </summary>
        [DataMember]
        public int notsurecount { get; set; }

        /// <summary>
        /// 总待验证人数
        /// </summary>
        [DataMember]
        public int waitcheckcount { get; set; }
    }
}
