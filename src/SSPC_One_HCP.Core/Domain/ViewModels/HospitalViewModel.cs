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
   public class HospitalViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public string Id { get; set; }
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
        public string CustomerType { get; set; }
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
        public string HospitalTypeFlag { get; set; }
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
        /// <summary>
        /// YsId
        /// </summary>
        [DataMember]
        public string YsId { get; set; }
        /// <summary>
        /// 是否已验证
        /// 1.是
        /// 2.否
        /// </summary>
        [DataMember]
        //public string IsVerify { get; set; }
        public int? IsVerify { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime? CreateTime { get; set; }
    }
}
