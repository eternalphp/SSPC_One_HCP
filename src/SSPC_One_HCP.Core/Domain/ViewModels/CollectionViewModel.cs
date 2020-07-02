using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 收藏
    /// </summary>
    [DataContract]
    [NotMapped]
    public class CollectionViewModel
    {
        /// <summary>
        /// 收藏信息
        /// </summary>
        [DataMember]
        public MyCollectionInfo MyCollectionInfo { get; set; }
        /// <summary>
        /// 具体数据
        /// </summary>
        [DataMember]
        public DataInfo DataInfo { get; set; }
        /// <summary>
        /// 会议
        /// </summary>
        [DataMember]
        public MeetInfo MeetInfo { get; set; }
        /// <summary>
        /// 搜索
        /// </summary>
        [DataMember]
        public string SearchTitle { get; set; }
    }
}