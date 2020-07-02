using System;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Services.Services.WeChat.Dto
{
    public class HcpCatalogueManageOutDto
    {
        [DataMember]
        public string Id { get; set; }
        /// <summary>
        /// BU信息
        /// </summary>
        [DataMember]
        public string BuName { get; set; }
        /// <summary>
        /// 目录名称
        /// </summary>
        [DataMember]
        public string CatalogueName { get; set; }

        /// <summary>
        /// 是否新
        /// </summary>
        [DataMember]
        public bool IsNew { get; set; }
        [DataMember]
        public DateTime? CreateTime { get; internal set; }
    }
}
