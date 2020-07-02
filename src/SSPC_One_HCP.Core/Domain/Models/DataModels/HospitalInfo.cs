using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{
    /// <summary>
    /// 医院信息
    /// </summary>
    [DataContract]
    public class HospitalInfo : BaseEntity
    {
        /// <summary>
        /// 医院原有系统ID
        /// </summary>
        [DataMember]
        public string HospitalId { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        [DataMember]
        public string HospitalName { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>
        [DataMember]
        public int? CustomerType { get; set; }
        /// <summary>
        /// 医院编码
        /// </summary>
        [DataMember]
        public string HospitalCode { get; set; }
        /// <summary>
        /// 区域Id
        /// </summary>
        [DataMember]
        public string AreaId { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [DataMember]
        public string Address { get; set; }
        /// <summary>
        /// 网址
        /// </summary>
        [DataMember]
        public string NetAddress { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [DataMember]
        public string PhoneNum { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        [DataMember]
        public string ZipCode { get; set; }
        /// <summary>
        /// 医院类型标识
        /// </summary>
        [DataMember]
        public int? HospitalTypeFlag { get; set; }
        /// <summary>
        /// 邮件
        /// </summary>
        [DataMember]
        public string Email { get; set; }
        /// <summary>
        /// 医院类型
        /// </summary>
        [DataMember]
        public string HospitalType { get; set; }
        /// <summary>
        /// 拼音全
        /// </summary>
        [DataMember]
        public string PyFull { get; set; }
        /// <summary>
        /// 拼音简称
        /// </summary>
        [DataMember]
        public string PyShort { get; set; }
        /// <summary>
        /// 来自
        /// </summary>
        [DataMember]
        public string ComeFrom { get; set; }
        ///// <summary>
        ///// YsId
        ///// (已作废，请使用yunshi_hospital_id)
        ///// </summary>
        //[DataMember]
        //public string YsId { get; set; }
        /// <summary>
        /// 是否已验证
        /// 0、未认证
        /// 1、已认证
        /// 2、不确定
        /// 3、认证失败
        /// </summary>
        [DataMember]
        public int? IsVerify { get; set; }
        /// <summary>
        /// 费森医院编码
        /// </summary>
        [DataMember]
        [DisplayName("费森医院编码")]
        public string hospital_code { get; set; }
        /// <summary>
        /// 费森医院名称
        /// </summary>
        [DataMember]
        [DisplayName("费森医院名称")]
        public string hospital_name_SFE { get; set; }
        /// <summary>
        /// 云势序号
        /// </summary>
        [DataMember]
        [DisplayName("云势序号")]
        public string serial_number { get; set; }

        /// <summary>
        /// 云势状态
        /// </summary>
        [DataMember]
        [DisplayName("云势状态")]
        public string status { get; set; }
        /// <summary>
        /// 云势原因
        /// </summary>
        [DataMember]
        [DisplayName("云势原因")]
        public string reason { get; set; }
        /// <summary>
        /// 云势唯一医院ID
        /// </summary>
        [DataMember]
        [DisplayName("云势唯一医院ID")]
        public string unique_hospital_id { get; set; }
        /// <summary>
        /// 云势医院ID
        /// </summary>
        [DataMember]
        [DisplayName("云势医院ID")]
        public string yunshi_hospital_id { get; set; }
        /// <summary>
        /// 云势正确医院名称
        /// </summary>
        [DataMember]
        [DisplayName("云势正确医院名称")]
        public string hospital_name { get; set; }
        /// <summary>
        /// 云势正确医院别名（曾用名）
        /// </summary>
        [DataMember]
        [DisplayName("云势正确医院别名（曾用名）")]
        public string hospital_alias { get; set; }
        /// <summary>
        /// 云势医院等级
        /// </summary>
        [DataMember]
        [DisplayName("云势医院等级")]
        public string hospital_level { get; set; }
        /// <summary>
        /// 云势正确医院类别
        /// </summary>
        [DataMember]
        [DisplayName("云势正确医院类别")]
        public string hospital_category { get; set; }
        /// <summary>
        /// 云势医院性质
        /// </summary>
        [DataMember]
        [DisplayName("云势医院性质")]
        public string hospital_nature { get; set; }
        /// <summary>
        /// 云势医院组织机构代码
        /// </summary>
        [DataMember]
        [DisplayName("云势医院组织机构代码")]
        public string hospital_organization_code { get; set; }
        /// <summary>
        /// 云势正确省份
        /// </summary>
        [DataMember]
        [DisplayName("云势正确省份")]
        public string province { get; set; }
        /// <summary>
        /// 云势正确城市
        /// </summary>
        [DataMember]
        [DisplayName("云势正确城市")]
        public string city { get; set; }
        /// <summary>
        /// 云势正确地区
        /// </summary>
        [DataMember]
        [DisplayName("云势正确地区")]
        public string area { get; set; }
        /// <summary>
        /// 云势正确地址
        /// </summary>
        [DataMember]
        [DisplayName("云势正确地址")]
        public string YsAddress { get; set; }
        /// <summary>
        /// 云势邮编
        /// </summary>
        [DataMember]
        [DisplayName("云势邮编")]
        public string post_code { get; set; }
        /// <summary>
        /// 云势医院电话
        /// </summary>
        [DataMember]
        [DisplayName("云势医院电话")]
        public string hospital_phone { get; set; }
        /// <summary>
        /// 云势官网
        /// </summary>
        [DataMember]
        [DisplayName("云势官网")]
        public string website { get; set; }
        /// <summary>
        /// 云势床位数
        /// </summary>
        [DataMember]
        [DisplayName("云势床位数")]
        public int number_of_beds { get; set; }
        /// <summary>
        /// 云势门诊量
        /// </summary>
        [DataMember]
        [DisplayName("云势门诊量")]
        public int number_of_outpatient { get; set; }
        /// <summary>
        /// 云势住院量
        /// </summary>
        [DataMember]
        [DisplayName("云势住院量")]
        public int hospitalization { get; set; }
        /// <summary>
        /// 云势医院简介
        /// </summary>
        [DataMember]
        [DisplayName("云势医院简介")]
        public string hospital_intro { get; set; }
        /// <summary>
        /// 云势职工人数
        /// </summary>
        [DataMember]
        [DisplayName("云势职工人数")]
        public int number_of_employees { get; set; }
        /// <summary>
        /// 云势修改人
        /// </summary>
        [DataMember]
        [DisplayName("云势修改人")]
        public string modifier { get; set; }
        /// <summary>
        /// 云势修改时间
        /// </summary>
        [DataMember]
        [DisplayName("云势修改时间")]
        public DateTime? change_time { get; set; }
        /// <summary>
        /// 云势数据更新类型
        /// </summary>
        [DataMember]
        [DisplayName("云势数据更新类型")]
        public string data_update_type { get; set; }
    }
}
