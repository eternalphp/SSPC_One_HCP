using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    /// <summary>
    /// 医生信息导出模型
    /// </summary>
    [DataContract]
    [NotMapped]
    public class DoctorViewModel
    {
        /// <summary>
        /// 平台自有Id
        /// </summary>
        [DataMember]
        [DisplayName("HcpId")]
        public string Id { get; set; }

        /// <summary>
        /// pvm平台的id
        /// </summary>
        [DataMember]
        [DisplayName("PVMId")]
        public string PVMId { get; set; }

        /// <summary>
        /// 云势平台的Id
        /// </summary>
        [DataMember]
        [DisplayName("YSId")]
        public string YSId { get; set; }

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

        [DataMember]
        [DisplayName("科室名称Id")]
        public string DepartmentID{ get; set; }

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
        /// 是否已认证过
        /// 1、已认证
        /// 2、不确定
        /// 3、认证失败
        /// 4、申诉中
        /// 5、认证中
        /// 6、申诉拒绝
        /// </summary>
        [DataMember]
        [DisplayName("状态")]
        public string IsVerify { get; set; }

        /// <summary>
        /// 用户创建时间
        /// </summary>
        [DataMember]
        [DisplayName("注册时间")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 用户创建时间
        /// </summary>
        [DataMember]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 微信头像
        /// </summary>
        [DataMember]
        public string WxPicture { get; set; }

        /// <summary>
        /// 微信名
        /// </summary>
        [DataMember]
        public string WxName { get; set; }


        //#region 申诉
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
        //public string doctor_work { get; set; }

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
        /// 性别
        /// </summary>
        [DataMember]
        [DisplayName("性别")]
        public string gender { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [DataMember]
        [DisplayName("标签")]
        public IList<string> DocTags { get; set; }

        /// <summary>
        /// 医生来源类型
        /// 0. 绑定的小程序公众号
        /// 1. 费卡文库扫码 / 文章分享
        /// 2. 平台生成的二维码
        /// 3. 其他小程序场景
        /// </summary>
        [DataMember]
        [DisplayName("数据来源")]
        public string DataSource { get; set; }
        /// <summary>
        /// 微信来源
        /// </summary>
        [DataMember]
        [DisplayName("微信场景值")]
        public string WxScene { get; set; }

        /// <summary>
        /// 是否H5注册用户
        /// </summary>
        [DataMember]
        [DisplayName("是否H5注册用户")]
        public int? IsH5 { get; set; }
        [DataMember]
        [DisplayName("是否基层")]
        public string BasicLevelName { get; set; }
        [DataMember]
        public int? RegistrationIsBasicLevel { get; set; }
        [DataMember]
        [DisplayName("年龄")]
        public string RegistrationAgea { get; set; }
        [DataMember]
        [DisplayName("性别")]
        public string RegistrationGender { get; set; }
        [DataMember]
        [DisplayName("UnionId")]
        public string UnionId { get; set; }
        ///// <summary>
        ///// 科室座机号码
        ///// </summary>
        //[DataMember]
        //[DisplayName("科室座机号码")]
        //public string room_mobile { get; set; }
        //#endregion

    }
}
