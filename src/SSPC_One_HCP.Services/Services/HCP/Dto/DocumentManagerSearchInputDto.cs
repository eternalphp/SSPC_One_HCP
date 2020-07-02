using SSPC_One_HCP.Core.Domain.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 获取产品资料、播客的列表的查询条件的模型
    /// </summary>
    public class DocumentManagerSearchInputDto
    {
        /// <summary>
        /// 资料标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

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
        /// 图片路径
        /// </summary>
        [DataMember]
        public string KnowImageUrl { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        [DataMember]
        public string KnowImageName { get; set; }
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
        /// 是否完成审核(允许多选)
        /// 1、已审核
        /// 2、未审核
        /// 3、审核拒绝
        /// 4、已锁定
        /// 5、已作废
        /// 6、将要删除
        /// 7、已删除
        /// </summary>
        [DataMember]
        public List<EnumComplete?> IsCompletedList { get; set; }
    }
}
