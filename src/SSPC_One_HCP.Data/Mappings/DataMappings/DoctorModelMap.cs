using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class DoctorModelMap : BaseEntityTypeConfiguration<WxUserModel>
    {
        public DoctorModelMap()
        {
            this.ToTable("DoctorModel");

            this.Property(t => t.UnionId)
                .HasColumnName("UnionId");

            this.Property(t => t.IsSalesPerson)
                 .HasColumnName("IsSalesPerson");

            this.Property(t => t.OpenId)
                .HasMaxLength(128)
                .HasColumnName("OpenId");

            this.Property(t => t.UserName)
                .HasMaxLength(50)
                .HasColumnName("UserName");

            this.Property(t => t.Mobile)
                .HasMaxLength(20)
                .HasColumnName("Mobile");

            this.Property(t => t.HospitalName)
                .HasMaxLength(100)
                .HasColumnName("HospitalName");

            this.Property(t => t.DepartmentName)
                .HasMaxLength(100)
                .HasColumnName("DepartmentName");

            this.Property(t => t.Province)
                .HasMaxLength(20)
                .HasColumnName("Province");

            this.Property(t => t.City)
                .HasMaxLength(20)
                .HasColumnName("City");

            this.Property(t => t.Area)
                .HasMaxLength(20)
                .HasColumnName("Area");

            this.Property(t => t.HPAddress)
                .HasMaxLength(50)
                .HasColumnName("HPAddress");

            this.Property(t => t.School)
                .HasMaxLength(50)
                .HasColumnName("School");

            this.Property(t => t.Title)
                .HasMaxLength(200)
                .HasColumnName("Title");

            this.Property(t => t.DoctorPosition)
                .HasMaxLength(200)
                .HasColumnName("DoctorPosition");

            this.Property(t => t.IsVerify)
                .HasColumnName("IsVerify");

            this.Property(t => t.IsCompleteRegister)
                .HasColumnName("IsCompleteRegister");

            this.Property(t => t.WxName)
                .HasMaxLength(128)
                .HasColumnName("WxName");

            this.Property(t => t.WxPicture)
                 .HasMaxLength(256)
                .HasColumnName("WxPicture");

            this.Property(t => t.WxGender)
                .HasMaxLength(10)
                .HasColumnName("WxGender");

            this.Property(t => t.WxCountry)
                .HasMaxLength(256)
                .HasColumnName("WxCountry");

            this.Property(t => t.WxCity)
                .HasMaxLength(128)
                .HasColumnName("WxCity");

            this.Property(t => t.WxProvince)
                .HasMaxLength(128)
                .HasColumnName("WxProvince");

            this.Property(t => t.PVMId)
                 .HasMaxLength(128)
                .HasColumnName("PVMId");

            //this.Property(t => t.YSId)
            //    .HasColumnName("YSId");

            this.Property(t => t.FCKId)
                .HasMaxLength(128)
                .HasColumnName("FCKId");

            this.Property(t => t.Code)
                .HasMaxLength(11)
                .HasColumnName("Code");

            this.Property(t => t.CodeTime)
                .HasColumnName("CodeTime");

            this.Property(t => t.OneHcpId)
                .HasMaxLength(36)
                .HasColumnName("OneHcpId");
            //费森医院编码
            this.Property(t => t.hospital_code)
                .HasMaxLength(100)
                .HasColumnName("hospital_code");
            //费森医院名称
            this.Property(t => t.hospital_name)
                .HasMaxLength(500)
                .HasColumnName("hospital_name");
            //费森医生编码
            this.Property(t => t.doctor_code)
                .HasMaxLength(100)
                .HasColumnName("doctor_code");
            //费森医生姓名
            this.Property(t => t.doctor_name)
                .HasMaxLength(200)
                .HasColumnName("doctor_name");
            //费森科室
            this.Property(t => t.department)
                .HasMaxLength(500)
                .HasColumnName("department");
            //费森职称
            this.Property(t => t.job_title_SFE)
                .HasMaxLength(200)
                .HasColumnName("job_title_SFE");
            //费森职务
            this.Property(t => t.position_SFE)
                .HasMaxLength(200)
                .HasColumnName("position_SFE");
            //费森是否信息客户
            this.Property(t => t.is_infor_customer)
                .HasColumnName("is_infor_customer");
            //云势线下HCP主键
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
            //云势唯一医生ID
            this.Property(t => t.unique_doctor_id)
                .HasMaxLength(100)
                .HasColumnName("unique_doctor_id");
            //云势医院ID
            this.Property(t => t.yunshi_hospital_id)
                .HasMaxLength(100)
                .HasColumnName("yunshi_hospital_id");
            //云势医院名称
            this.Property(t => t.yunshi_hospital_name)
                .HasMaxLength(500)
                .HasColumnName("yunshi_hospital_name");
            //云势医生ID
            this.Property(t => t.yunshi_doctor_id)
                .HasMaxLength(100)
                .HasColumnName("yunshi_doctor_id");
            //云势医生姓名
            this.Property(t => t.name)
                .HasMaxLength(200)
                .HasColumnName("name");
            //云势省份
            this.Property(t => t.yunshi_province)
                .HasMaxLength(200)
                .HasColumnName("yunshi_province");
            //云势城市
            this.Property(t => t.yunshi_city)
                .HasMaxLength(200)
                .HasColumnName("yunshi_city");
            //云势标准科室
            this.Property(t => t.standard_department)
                .HasMaxLength(500)
                .HasColumnName("standard_department");
            //云势专业
            this.Property(t => t.profession)
                .HasMaxLength(200)
                .HasColumnName("profession");
            //云势性别
            this.Property(t => t.gender)
                .HasMaxLength(5)
                .HasColumnName("gender");
            //云势职称
            this.Property(t => t.job_title)
                .HasMaxLength(200)
                .HasColumnName("job_title");
            //云势职称
            this.Property(t => t.position)
                .HasMaxLength(200)
                .HasColumnName("position");
            //云势学术头衔
            this.Property(t => t.academic_title)
                .HasMaxLength(200)
                .HasColumnName("academic_title");
            //云势类型
            this.Property(t => t.type)
                .HasMaxLength(200)
                .HasColumnName("type");
            //云势证书类型
            this.Property(t => t.certificate_type)
                .HasMaxLength(50)
                .HasColumnName("certificate_type");
            //云势执业证书编码
            this.Property(t => t.certificate_code)
                .HasMaxLength(100)
                .HasColumnName("certificate_code");
            //云势学历
            this.Property(t => t.education)
                .HasMaxLength(200)
                .HasColumnName("education");
            //云势毕业院校
            this.Property(t => t.graduated_school)
                .HasMaxLength(500)
                .HasColumnName("graduated_school");
            //云势毕业时间
            this.Property(t => t.graduation_time)
                .HasMaxLength(20)
                .HasColumnName("graduation_time");
            //云势擅长
            this.Property(t => t.specialty)
                .HasMaxLength(1000)
                .HasColumnName("specialty");
            //云势医院简介
            this.Property(t => t.intro)
                .HasColumnName("intro");
            //云势科室电话
            this.Property(t => t.department_phone)
                .HasMaxLength(100)
                .HasColumnName("department_phone");
            //云势修改人
            this.Property(t => t.modifier)
                .HasMaxLength(500)
                .HasColumnName("modifier");
            //云势修改时间
            this.Property(t => t.change_time)
                .HasColumnName("change_time");
            //云势数据更新类型
            this.Property(t => t.data_update_type)
                .HasMaxLength(500)
                .HasColumnName("data_update_type");

            // 图片证明材料(内容可以是医院照片墙、排班表、工牌或胸卡、名片、挂号单)
            this.Property(t => t.Pictures)
               // .HasMaxLength(500)
                .HasColumnName("Pictures");
            
            ////医院照片墙
            //this.Property(t => t.photowall)
            //    .HasMaxLength(500)
            //    .HasColumnName("photowall");

            ////医院医生排班表
            //this.Property(t => t.doctor_working_schedule)
            //    .HasMaxLength(500)
            //    .HasColumnName("doctor_working_schedule");

            ////医生工牌或胸卡
            //this.Property(t => t.doctor_chest_card)
            //    .HasMaxLength(500)
            //    .HasColumnName("doctor_chest_card");

            ////医生名片
            //this.Property(t => t.doctor_business_card)
            //    .HasMaxLength(500)
            //    .HasColumnName("doctor_business_card");

            ////医院医生挂号单
            //this.Property(t => t.doctor_registration)
            //    .HasMaxLength(500)
            //    .HasColumnName("doctor_registration");

            //科室座机号码
            this.Property(t => t.doctor_office_tel)
                .HasMaxLength(500)
                .HasColumnName("doctor_office_tel");
            
            this.Property(t => t.SourceAppId)
                .HasMaxLength(100)
                .HasColumnName("SourceAppId");

            this.Property(t => t.SourceType)
                .HasMaxLength(20)
                .HasColumnName("SourceType"); 

            this.Property(t => t.WxSceneId)
                .HasMaxLength(20)
                .HasColumnName("WxSceneId");

        
        }
    }
}
