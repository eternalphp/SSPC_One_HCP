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
    /// 医生模型
    /// </summary>
    [DataContract]
    public class DoctorModel : BaseEntity
    {
        /// <summary>
        /// 开放平台中的唯一标识
        /// </summary>
        [DataMember]
        [DisplayName("UnionId")]
        public string UnionId { get; set; }

        /// <summary>
        ///是否为销售人员
        /// 0/null 医生 1:销售人员 2:内部员工
        /// </summary>
        [DataMember]
        public int? IsSalesPerson { get; set; }
        /// <summary>
        /// 小程序或公众号中的唯一标识
        /// </summary>
        [DataMember]
        [DisplayName("OpenId")]
        public string OpenId { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        [DataMember]
        [DisplayName("医生姓名")]
        public string UserName { get; set; }

        /// <summary>
        /// 医生电话
        /// </summary>
        [DataMember]
        [DisplayName("医生电话")]
        public string Mobile { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        [DataMember]
        [DisplayName("医院名称")]
        public string HospitalName { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        [DataMember]
        [DisplayName("科室名称")]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [DataMember]
        [DisplayName("省份")]
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [DataMember]
        [DisplayName("城市")]
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        [DataMember]
        [DisplayName("区")]
        public string Area { get; set; }

        /// <summary>
        /// 医院地址
        /// </summary>
        [DataMember]
        [DisplayName("医院地址")]
        public string HPAddress { get; set; }

        /// <summary>
        /// 毕业院校
        /// </summary>
        [DataMember]
        [DisplayName("毕业院校")]
        public string School { get; set; }

        /// <summary>
        /// 医生职称
        /// </summary>
        [DataMember]
        [DisplayName("医生职称")]
        public string Title { get; set; }

        /// <summary>
        /// 医生职务
        /// </summary>
        [DataMember]
        [DisplayName("医生职务")]
        public string DoctorPosition { get; set; }

        /// <summary>
        /// 是否已认证过
        /// 0、未认证
        /// 1、已认证
        /// 2、不确定
        /// 3、认证失败
        /// 4、申诉中
        /// 5、认证中
        /// 6、申诉拒绝
        /// </summary>
        [DataMember]
        [DisplayName("IsVerify")]
        public int IsVerify { get; set; }

        /// <summary>
        /// 是否完成注册
        /// 当用户走注册流程时，就先获取到他的UnionId，
        /// 然后发送验证码注册
        /// </summary>
        [DataMember]
        [DisplayName("IsCompleteRegister")]
        public int IsCompleteRegister { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        [DataMember]
        [DisplayName("微信昵称")]
        public string WxName { get; set; }

        /// <summary>
        /// 微信头像
        /// </summary>
        [DataMember]
        [DisplayName("微信头像")]
        public string WxPicture { get; set; }

        /// <summary>
        /// 微信性别
        /// </summary>
        [DataMember]
        [DisplayName("微信性别")]
        public string WxGender { get; set; }

        /// <summary>
        /// 所在国家
        /// </summary>
        [DataMember]
        [DisplayName("所在国家")]
        public string WxCountry { get; set; }

        /// <summary>
        /// 所在城市
        /// </summary>
        [DataMember]
        [DisplayName("所在城市")]
        public string WxCity { get; set; }
        /// <summary>
        /// 所在省份
        /// </summary>
        [DataMember]
        [DisplayName("所在省份")]
        public string WxProvince { get; set; }

        /// <summary>
        /// pvm平台的id
        /// </summary>
        [DataMember]
        [DisplayName("PVMId")]
        public string PVMId { get; set; }

        ///// <summary>
        ///// 云势平台的Id
        ///// (已作废，请使用yunshi_doctor_id)
        ///// </summary>
        //[DataMember]
        //[DisplayName("YSId")]
        //public string YSId { get; set; }

        /// <summary>
        /// 其他系统的ID
        /// </summary>
        [DataMember]
        [DisplayName("FCKId")]
        public string FCKId { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [DataMember]
        [DisplayName("验证码")]
        public string Code { get; set; }

        /// <summary>
        /// 验证码有效时间
        /// </summary>
        [DataMember]
        [DisplayName("验证码有效时间")]
        public DateTime? CodeTime { get; set; }
        /// <summary>
        /// 本系统医生注册后生成的Id
        /// </summary>
        [DataMember]
        public string OneHcpId { get; set; }
        /// <summary>
        /// 小程序创建人
        /// </summary>
        [DataMember]
        [DisplayName("小程序创建人")]
        public string creator { get; set; }
        /// <summary>
        /// 小程序创建时间
        /// </summary>
        [DataMember]
        [DisplayName("小程序创建时间")]
        public DateTime? creation_time { get; set; }
        /// <summary>
        /// 费森医院编码
        /// </summary>
        [DataMember]
        [DisplayName("费森医院编码")]
        public string hospital_code { get; set; }
        /// <summary>
        /// 费森医院编码
        /// </summary>
        [DataMember]
        [DisplayName("费森医院编码")]
        public string hospital_name { get; set; }
        /// <summary>
        /// 费森医生编码
        /// </summary>
        [DataMember]
        [DisplayName("费森医生编码")]
        public string doctor_code { get; set; }
        /// <summary>
        /// 费森医生姓名
        /// </summary>
        [DataMember]
        [DisplayName("费森医生姓名")]
        public string doctor_name { get; set; }
        /// <summary>
        /// 费森科室
        /// </summary>
        [DataMember]
        [DisplayName("费森科室")]
        public string department { get; set; }
        /// <summary>
        /// 费森职称
        /// </summary>
        [DataMember]
        [DisplayName("费森职称")]
        public string job_title_SFE { get; set; }
        /// <summary>
        /// 费森职务
        /// </summary>
        [DataMember]
        [DisplayName("费森职务")]
        public string position_SFE { get; set; }
        /// <summary>
        /// 费森是否信息客户
        /// </summary>
        [DataMember]
        [DisplayName("费森是否信息客户")]
        public bool is_infor_customer { get; set; }
        /// <summary>
        /// 云势线下HCP主键
        /// </summary>
        [DataMember]
        [DisplayName("云势线下HCP主键")]
        public string serial_number { get; set; }
        /// <summary>
        /// 云势状态(有效、无效、未确定)
        /// </summary>
        [DataMember]
        [DisplayName("云势状态(有效、无效、未确定)")]
        public string status { get; set; }
        /// <summary>
        /// 云势原因
        /// </summary>
        [DataMember]
        [DisplayName("云势原因")]
        public string reason { get; set; }
        /// <summary>
        /// 云势唯一医生ID
        /// </summary>
        [DataMember]
        [DisplayName("云势唯一医生ID")]
        public string unique_doctor_id { get; set; }
        /// <summary>
        /// 云势医院ID
        /// </summary>
        [DataMember]
        [DisplayName("云势医院ID")]
        public string yunshi_hospital_id { get; set; }
        /// <summary>
        /// 云势医院名称
        /// </summary>
        [DataMember]
        [DisplayName("云势医院名称")]
        public string yunshi_hospital_name { get; set; }
        /// <summary>
        /// 云势医生ID
        /// </summary>
        [DataMember]
        [DisplayName("云势医生ID")]
        public string yunshi_doctor_id { get; set; }
        /// <summary>
        /// 云势医生姓名 
        /// </summary>
        [DataMember]
        [DisplayName("云势医生姓名 ")]
        public string name { get; set; }

        /// <summary>
        /// 云势省份
        /// </summary>
        [DataMember]
        [DisplayName("云势省份")]
        public string yunshi_province { get; set; }

        /// <summary>
        /// 云势城市
        /// </summary>
        [DataMember]
        [DisplayName("云势城市")]
        public string yunshi_city { get; set; }

        /// <summary>
        /// 云势标准科室 
        /// </summary>
        [DataMember]
        [DisplayName("云势标准科室")]
        public string standard_department { get; set; }
        /// <summary>
        /// 云势专业
        /// </summary>
        [DataMember]
        [DisplayName("云势专业")]
        public string profession { get; set; }
        /// <summary>
        /// 云势性别
        /// </summary>
        [DataMember]
        [DisplayName("云势性别")]
        public string gender { get; set; }
        /// <summary>
        /// 云势职称 
        /// </summary>
        [DataMember]
        [DisplayName("云势职称")]
        public string job_title { get; set; }
        /// <summary>
        /// 云势职务
        /// </summary>
        [DataMember]
        [DisplayName("云势职务")]
        public string position { get; set; }
        /// <summary>
        /// 云势学术头衔
        /// </summary>
        [DataMember]
        [DisplayName("云势学术头衔")]
        public string academic_title { get; set; }
        /// <summary>
        /// 云势类型
        /// </summary>
        [DataMember]
        [DisplayName("云势类型")]
        public string type { get; set; }
        /// <summary>
        /// 云势类型
        /// </summary>
        [DataMember]
        [DisplayName("云势类型")]
        public string certificate_type { get; set; }
        /// <summary>
        /// 云势执业证书编码
        /// </summary>
        [DataMember]
        [DisplayName("云势执业证书编码")]
        public string certificate_code { get; set; }
        /// <summary>
        /// 云势学历
        /// </summary>
        [DataMember]
        [DisplayName("云势学历")]
        public string education { get; set; }
        /// <summary>
        /// 云势毕业院校
        /// </summary>
        [DataMember]
        [DisplayName("云势毕业院校")]
        public string graduated_school { get; set; }
        /// <summary>
        /// 云势毕业时间
        /// </summary>
        [DataMember]
        [DisplayName("云势毕业时间")]
        public string graduation_time { get; set; }
        /// <summary>
        /// 云势擅长
        /// </summary>
        [DataMember]
        [DisplayName("云势擅长")]
        public string specialty { get; set; }
        /// <summary>
        /// 云势医院简介
        /// </summary>
        [DataMember]
        [DisplayName("云势医院简介")]
        public string intro { get; set; }
        /// <summary>
        /// 云势科室电话
        /// </summary>
        [DataMember]
        [DisplayName("云势科室电话")]
        public string department_phone { get; set; }
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

        /// <summary>
        /// 医生来源AppId
        /// </summary>
        [DataMember]
        [DisplayName("医生来源")]
        public string SourceAppId { get; set; }

        /// <summary>
        /// 医生来源类型
        /// 0. 其它场景 
        /// 1. 会议二维码 
        /// 2. 名片二维码 
        /// 3. 第三方公众号二维码 
        /// 4. 推广二维码
        /// 5. 参会表单
        /// </summary>
        public string SourceType { get; set; }

        /// <summary>
        /// 医生来源微信场景ID
        /// </summary>
        public string WxSceneId { get; set; }

        #region 申诉

        /// <summary>
        /// 图片证明材料，内容可以是医院照片墙、排班表、工牌或胸卡、名片、挂号单，用|分隔
        /// </summary>
        [DataMember]
        [DisplayName("图片证明材料")]
        public string Pictures { get; set; }

        ///// <summary>
        ///// 医院照片墙
        ///// </summary>
        //[DataMember]
        //[DisplayName("医院照片墙")]
        //public string photowall { get; set; }

        ///// <summary>
        ///// 医院医生排班表
        ///// </summary>
        //[DataMember]
        //[DisplayName("医院医生排班表")]
        //public string doctor_working_schedule { get; set; }

        ///// <summary>
        ///// 医生工牌或胸卡
        ///// </summary>
        //[DataMember]
        //[DisplayName("医生工牌或胸卡")]
        //public string doctor_chest_card { get; set; }

        ///// <summary>
        ///// 医生名片
        ///// </summary>
        //[DataMember]
        //[DisplayName("医生名片")]
        //public string doctor_business_card { get; set; }

        ///// <summary>
        ///// 医院医生挂号单
        ///// </summary>
        //[DataMember]
        //[DisplayName("医院医生挂号单")]
        //public string doctor_registration { get; set; }

        /// <summary>
        /// 科室座机号码
        /// </summary>
        [DataMember]
        [DisplayName("科室座机号码")]
        public string doctor_office_tel { get; set; }
        #endregion

        /// <summary>
        /// 参会报名表 年龄
        /// </summary>
        [DataMember]
        [DisplayName("参会报名表年龄")]
        public string RegistrationAge { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        [DisplayName("参会报名表性别")]
        public string RegistrationGender { get; set; }
        /// <summary>
        /// 参会报名表是否基层
        /// </summary>
        [DataMember]
        [DisplayName("参会报名表是否基层")]
        public int? RegistrationIsBasicLevel { get; set; }
    }
}
