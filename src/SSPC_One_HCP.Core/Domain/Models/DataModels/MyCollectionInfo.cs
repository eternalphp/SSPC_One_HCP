using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 个人收藏
    /// </summary>
    [DataContract]
    public class MyCollectionInfo:BaseEntity
    {
        /// <summary>
        /// UnionId
        /// </summary>
        [DataMember]
        public string UnionId { get; set; }

        /// <summary>
        /// 收藏的文件或会议的ID
        /// </summary>
        [DataMember]
        public string CollectionDataId { get; set; }

        /// <summary>
        /// 收藏的文档的类型
        /// --作废--
        /// 1.会议
        /// 2.播客
        /// 3.文档
        /// 4.视频
        /// --------
        /// 
        /// 1、文章
        /// 2、文档
        /// 3、播客
        /// 4、视频
        /// 5、会议
        /// </summary>
        [DataMember]
        public int? CollectionType { get; set; }

        /// <summary>
        /// 微信用户（医生）的Id
        /// --以后会取代 UnionId
        /// </summary>
        [DataMember]
        public string WxUserId { get; set; }
    }
}
