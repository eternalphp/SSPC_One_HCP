using SSPC_One_HCP.Core.Domain.Enums;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels.ProductModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.MeetModels
{
    /// <summary>
    /// 会议列表查询ViewModel
    /// </summary>
    [NotMapped]
    [DataContract]
    public class MeetSearchViewModel
    {
        /// <summary>
        /// 会议信息
        /// </summary>
        [DataMember]
        public MeetInfo Meet { get; set; }

        /// <summary>
        /// 会议开始时间
        /// </summary>
        [DataMember]
        public DateTime? Meet_Start { get; set; }

        /// <summary>
        /// 会议结束时间
        /// </summary>
        [DataMember]
        public DateTime? Meet_End { get; set; }
        
        /// <summary>
        /// 会议状态
        /// 0、未开始
        /// 1、进行中
        /// 2、已完成
        /// </summary>
        [DataMember]
        public int? IsMeetEnd { get; set; }
        
        /// <summary>
        /// 是否推荐
        /// 0、否
        /// 1、是
        /// </summary>
        [DataMember]
        public int? IsChoiceness { get; set; }

        /// <summary>
        /// 是否精选
        /// 0、否
        /// 1、是
        /// </summary>
        [DataMember]
        public int? IsHot { get; set; }

        /// <summary>
        /// Bu名称
        /// </summary>
        [DataMember]
        public string BuName { get; set; }

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

        /// <summary>
        /// 是否获取待审核列表
        /// </summary>
        [DataMember]
        public bool IsGettingApprovalList { get; set; }

        ///// <summary>
        ///// 产品名称
        ///// </summary>
        //[DataMember]
        //public string ProductName { get; set; }

        ///// <summary>
        ///// 科室名称
        ///// </summary>
        //[DataMember]
        //public string DepartmentName { get; set; }
    }
}
