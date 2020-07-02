using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    /// <summary>
    /// 医院表映射
    /// </summary>
    public class HospitalInfoMap : BaseEntityTypeConfiguration<HospitalInfo>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HospitalInfoMap()
        {
            // 设置表名
            this.ToTable("HospitalInfo");
            // 医院原有系统ID
            this.Property(t => t.HospitalId)
                .HasMaxLength(100)
                .HasColumnName("HospitalId");
            // 医院名称
            this.Property(t => t.HospitalName)
                .HasMaxLength(500)
                .HasColumnName("HospitalName");
            // 客户类型
            this.Property(t => t.CustomerType)
                .HasColumnName("CustomerType");
            // 医院编码
            this.Property(t => t.HospitalCode)
                .HasMaxLength(100)
                .HasColumnName("HospitalCode");
            // 区域Id
            this.Property(t => t.AreaId)
                .HasMaxLength(100)
                .HasColumnName("AreaId");
            // 地址
            this.Property(t => t.Address)
                .HasMaxLength(500)
                .HasColumnName("Address");
            // 网址
            this.Property(t => t.NetAddress)
                .HasColumnName("NetAddress");
            // 电话
            this.Property(t => t.PhoneNum)
                .HasMaxLength(200)
                .HasColumnName("PhoneNum");
            // 邮编
            this.Property(t => t.ZipCode)
                .HasMaxLength(100)
                .HasColumnName("ZipCode");
            // 医院类型标识
            this.Property(t => t.HospitalTypeFlag)
                .HasColumnName("HospitalTypeFlag");
            // 邮件
            this.Property(t => t.Email)
                .HasMaxLength(500)
                .HasColumnName("Email");
            // 医院类型
            this.Property(t => t.HospitalType)
                .HasMaxLength(100)
                .HasColumnName("HospitalType");
            // 拼音全
            this.Property(t => t.PyFull)
                .HasMaxLength(500)
                .HasColumnName("PyFull");
            // 拼音简称
            this.Property(t => t.PyShort)
                .HasMaxLength(500)
                .HasColumnName("PyShort");
            // 来自
            this.Property(t => t.ComeFrom)
                .HasMaxLength(100)
                .HasColumnName("ComeFrom");
            //// 来自
            //this.Property(t => t.YsId)
            //    .HasMaxLength(100)
            //    .HasColumnName("YsId");
            // 是否已验证
            // 1.是
            // 2.否
            this.Property(t => t.IsVerify)
                .HasColumnName("IsVerify");
            //费森医院编码
            this.Property(t => t.hospital_code)
                .HasMaxLength(200)
                .HasColumnName("hospital_code");
            //费森医院名称
            this.Property(t => t.hospital_name_SFE)
                .HasMaxLength(500)
                .HasColumnName("hospital_name_SFE");
            //云势序号
            this.Property(t => t.serial_number)
                .HasMaxLength(100)
                .HasColumnName("serial_number");
            //云势状态
            this.Property(t => t.status)
                .HasMaxLength(50)
                .HasColumnName("status");
            //云势原因
            this.Property(t => t.reason)
                .HasMaxLength(200)
                .HasColumnName("reason");
            //云势唯一医院ID
            this.Property(t => t.unique_hospital_id)
                .HasMaxLength(100)
                .HasColumnName("unique_hospital_id");
            //云势医院ID
            this.Property(t => t.yunshi_hospital_id)
                .HasMaxLength(100)
                .HasColumnName("yunshi_hospital_id");
            //云势正确医院名称
            this.Property(t => t.hospital_name)
                .HasMaxLength(500)
                .HasColumnName("hospital_name");
            //云势正确医院别名（曾用名）
            this.Property(t => t.hospital_alias)
                .HasMaxLength(500)
                .HasColumnName("hospital_alias");
            //云势医院等级
            this.Property(t => t.hospital_level)
                .HasMaxLength(100)
                .HasColumnName("hospital_level");
            //云势正确医院类别
            this.Property(t => t.hospital_category)
                .HasMaxLength(50)
                .HasColumnName("hospital_category");
            //云势医院性质
            this.Property(t => t.hospital_nature)
                .HasMaxLength(20)
                .HasColumnName("hospital_nature");
            //云势医院组织机构代码
            this.Property(t => t.hospital_organization_code)
                .HasMaxLength(100)
                .HasColumnName("hospital_organization_code");
            //云势正确省份
            this.Property(t => t.province)
                .HasMaxLength(50)
                .HasColumnName("province");
            //云势正确城市
            this.Property(t => t.city)
                .HasMaxLength(20)
                .HasColumnName("city");
            //云势正确地区
            this.Property(t => t.area)
                .HasMaxLength(100)
                .HasColumnName("area");
            //云势正确地址
            this.Property(t => t.YsAddress)
                .HasMaxLength(500)
                .HasColumnName("YsAddress");
            //云势邮编
            this.Property(t => t.post_code)
                .HasMaxLength(10)
                .HasColumnName("post_code");
            //云势医院电话
            this.Property(t => t.hospital_phone)
                .HasMaxLength(50)
                .HasColumnName("hospital_phone");
            //云势官网
            this.Property(t => t.website)
                .HasMaxLength(100)
                .HasColumnName("website");
            //云势床位数
            this.Property(t => t.number_of_beds)
                .HasColumnName("number_of_beds");
            //云势门诊量
            this.Property(t => t.number_of_outpatient)
                .HasColumnName("number_of_outpatient");
            //云势住院量
            this.Property(t => t.hospitalization)
                .HasColumnName("hospitalization");
            //云势医院简介
            this.Property(t => t.hospital_intro)
                .HasColumnName("hospital_intro");
            //云势修改人
            this.Property(t => t.modifier)
                .HasMaxLength(50)
                .HasColumnName("modifier");
            //云势修改时间
            this.Property(t => t.change_time)
                .HasColumnName("change_time");
            //云势数据更新类型
            this.Property(t => t.data_update_type)
                .HasMaxLength(500)
                .HasColumnName("data_update_type");
        }
    }
}
