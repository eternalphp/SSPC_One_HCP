using SSPC_One_HCP.Core.Domain.Enums;
using SSPC_One_HCP.Core.Domain.ViewModels.ProductModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Services.Services.HCP.Dto
{

    /// <summary>
    /// 资料信息视图
    /// </summary>
    [DataContract]
    [NotMapped]
    public class HcpDataInfoOutDto
    {
      

        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [DataMember]
        public int IsDeleted { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [DataMember]
        public int IsEnabled { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [DataMember]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember]
        public string CreateUser { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        [DataMember]
        public string CreateUserID { get; set; }


        [DataMember]
        public string CreateUserADAccount { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [DataMember]
        public string UpdateUser { get; set; }
        /// <summary>
        /// 公司编码
        /// </summary>
        [DataMember]
        public string CompanyCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark { set; get; }

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
        /// 资料类别值
        /// </summary>
        [DataMember]
        public string DataTypeValue { get; set; }

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
        /// 博客封面图路径
        /// </summary>
        [DataMember]
        public string KnowImageUrl { get; set; }
        /// <summary>
        /// 博客封面名称
        /// </summary>
        [DataMember]
        public string KnowImageName { get; set; }

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
        /// 审批备注
        /// </summary>
        [DataMember]
        public string ApprovalNote { get; set; }

        /// <summary>
        /// 知识库对应的产品及科室
        /// </summary>
        [DataMember]
        public IEnumerable<ProductTypeViewModel> ProductAndDeps { get; set; }

        /// <summary>
        /// 是否精选
        /// 0、否
        /// 1、是
        /// </summary>
        [DataMember]
        public int? IsChoiceness { get; set; }

        /// <summary>
        /// 是否热门
        /// 0、否
        /// 1、是
        /// </summary>
        [DataMember]
        public int? IsHot { get; set; }

        /// <summary>
        /// 点击量
        /// </summary>
        [DataMember]
        public long? ClickVolume { get; set; }

        /// <summary>
        /// 赞同
        /// </summary>
        [DataMember]
        public int LikeCount { get; set; }

        /// <summary>
        /// 不赞同
        /// </summary>
        [DataMember]
        public int UNLikeCount { get; set; }

        [DataMember]
        public bool IsDownload;


        [DataMember]
        public string DownloadUrl { get; set; }
        [DataMember]
        public string ALastName { get;  set; }
        [DataMember]
        public List<string> ProIds { get; set; }
        [DataMember]
        public List<string> CatalogueNameList { get; set; }

        /// <summary>
        /// 是否新
        /// </summary>
        [DataMember]
        public bool IsNew { get; set; }
    }
}
