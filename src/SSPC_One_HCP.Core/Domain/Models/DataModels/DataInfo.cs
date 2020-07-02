using SSPC_One_HCP.Core.Domain.Enums;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 资料信息
    /// 1、播客
    /// 2、文章
    /// 3、视频
    /// </summary>
    [DataContract]
    public class DataInfo : BaseEntity
    {
        /// <summary>
        /// 资料所在资源组的ID
        /// </summary>
        [DataMember]
        public string ProductTypeInfoId { get; set; }

        /// <summary>
        /// 资料标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 资料描述
        /// </summary>
        [DataMember]
        public string DataContent { get; set; }

        /// <summary>
        /// 资料类别
        /// 1、PDF
        /// 2、PPT
        /// 3、视频
        /// 4、音频
        /// </summary>
        [DataMember]
        public string DataType { get; set; }

        /// <summary>
        /// 文件来源
        /// 1、附件上传
        /// 2、外部链接
        /// </summary>
        [DataMember]
        public int? DataOrigin { get; set; }

        /// <summary>
        /// 资料所在路径 附件上传
        /// </summary>
        [DataMember]
        public string DataUrl { get; set; }
        /// <summary>
        /// 封面路径
        /// </summary>
        [DataMember]
        public string KnowImageUrl { get; set; }
        /// <summary>
        /// 封面名称
        /// </summary>
        [DataMember]
        public string KnowImageName { get; set; }

        /// <summary>
        /// 外部链接
        /// </summary>
        [DataMember]
        public string DataLink { get; set; }

        /// <summary>
        /// 是否已读/已听
        /// 1.已读，
        /// 2.未读
        /// </summary>
        [DataMember]
        public int? IsRead { get; set; }

        /// <summary>
        /// 是否是精选
        /// 0-非精选
        /// 1-精选
        /// </summary>
        [DataMember]
        public int? IsSelected { get; set; }

        /// <summary>
        /// 是否有版权，关系到是否可以下载
        /// 0.没有版权
        /// 1.有版权
        /// </summary>
        [DataMember]
        public int? IsCopyRight { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        [DataMember]
        public string MediaTime { get; set; }
        /// <summary>
        /// 资料类型
        /// 1、产品资料
        /// 2、学术资料
        /// 3、播客
        /// </summary>
        [DataMember]
        public int? MediaType { get; set; }
        /// <summary>
        /// Bu
        /// </summary>
        [DataMember]
        public string BuName { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        [DataMember]
        public string Dept { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        [DataMember]
        public string Product { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public int Sort { get; set; }

        /// <summary>
        /// 是否完成审核
        /// 1、已审核
        /// 2、新增未审核
        /// 3、审核拒绝
        /// 4、已锁定
        /// 5、已作废
        /// 6、删除未审核
        /// 7、已删除
        /// 8、编辑未审核
        /// </summary>
        [DataMember]
        public EnumComplete? IsCompleted { get; set; }

        /// <summary>
        /// 修改前原来的记录Id
        /// </summary>
        [DataMember]
        public string OldId { get; set; }

        /// <summary>
        /// 审批备注
        /// </summary>
        [DataMember]
        public string ApprovalNote { get; set; }

        /// <summary>
        /// 是否公开给未登录用户访问
        /// </summary>
        [DataMember]
        public int? IsPublic { get; set; }

        /// <summary>
        /// 是否精选
        /// 0、否
        /// 1、是
        /// </summary>
        [DataMember]
        public int? IsChoiceness { get; set; }

        /// <summary>
        /// 点击量
        /// </summary>
        [DataMember]
        public long? ClickVolume { get; set; }
    }
}
