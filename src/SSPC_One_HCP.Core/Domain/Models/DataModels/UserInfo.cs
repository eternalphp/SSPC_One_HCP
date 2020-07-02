using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 后台用户信息
    /// </summary>
    [DataContract]
    public class UserInfo:BaseEntity
    {
        /// <summary>
        /// SAP编号
        /// </summary>
        [DataMember]
        public string Code { get; set; }
        /// <summary>
        /// 员工号
        /// </summary>
        [DataMember]
        public string EmployeeNo { get; set; }
        /// <summary>
        /// 域帐号
        /// </summary>
        [DataMember]
        public string ADAccount { get; set; }
        /// <summary>
        /// 中文名
        /// </summary>
        [DataMember]
        public string ChineseName { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        [DataMember]
        public string EnglishName { get; set; }
        /// <summary>
        /// 个人邮箱
        /// </summary>
        [DataMember]
        public string PersonalEmail { get; set; }
        /// <summary>
        /// 公司邮箱
        /// </summary>
        [DataMember]
        public string CompanyEmail { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [DataMember]
        public string MobileNo { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        [DataMember]
        public string IdCardNumber { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        [DataMember]
        public bool IsDisabled { get; set; }
        /// <summary>
        /// 汇报人Id
        /// </summary>
        [DataMember]
        public string ReporterId { get; set; }
        /// <summary>
        /// 组织架构Id
        /// </summary>
        [DataMember]
        public string OrganizationId { get; set; }
        /// <summary>
        /// 职位Id
        /// </summary>
        [DataMember]
        public string PositionId { get; set; }
        /// <summary>
        /// 成本中心
        /// </summary>
        [DataMember]
        public string CostCenter { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [DataMember]
        public string Password { get; set; }
        /// <summary>
        /// 部门经理
        /// </summary>
        [DataMember]
        public string ManagerId { get; set; }

    }
}
