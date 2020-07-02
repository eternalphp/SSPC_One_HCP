using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class UserInfoMap : BaseEntityTypeConfiguration<UserModel>
    {
        public UserInfoMap()
        {
            // Primary Key
            // Properties
            // SAP编号
            this.Property(t => t.Code)
                .HasMaxLength(50)
                .HasColumnName("Code");
            // 员工号
            this.Property(t => t.EmployeeNo)
                .HasMaxLength(50)
                .HasColumnName("EmployeeNo");
            // 域帐号
            this.Property(t => t.ADAccount)
                .HasMaxLength(200)
                .HasColumnName("ADAccount");
            // 中文名
            this.Property(t => t.ChineseName)
                .HasMaxLength(200)
                .HasColumnName("ChineseName");
            // 英文名称
            this.Property(t => t.EnglishName)
                .HasMaxLength(200)
                .HasColumnName("EnglishName");
            // 个人邮箱
            this.Property(t => t.PersonalEmail)
                .HasMaxLength(200)
                .HasColumnName("PersonalEmail");
            //公司邮箱
            this.Property(t => t.CompanyEmail)
                .HasMaxLength(200)
                .HasColumnName("CompanyEmail");
            // 手机号码
            this.Property(t => t.MobileNo)
                .HasMaxLength(200)
                .HasColumnName("MobileNo");
            // 身份证号
            this.Property(t => t.IdCardNumber)
                .HasMaxLength(200)
                .HasColumnName("IdCardNumber");
            //是否禁用
            this.Property(t => t.IsDisabled)
                .IsRequired()
                .HasColumnName("IsDisabled");
            //汇报人Id
            this.Property(t => t.ReporterId)
                .HasMaxLength(50)
                .HasColumnName("ReporterId");
            //组织架构Id
            this.Property(t => t.OrganizationId)
                .HasMaxLength(50)
                .HasColumnName("OrganizationId");
            //职位Id
            this.Property(t => t.PositionId)
                .HasMaxLength(50)
                .HasColumnName("PositionId");
            // 成本中心
            this.Property(t => t.CostCenter)
                .HasMaxLength(50)
                .HasColumnName("CostCenter");
            // 密码
            this.Property(t => t.Password)
                .HasMaxLength(500)
                .HasColumnName("Password");
            // 部门经理
            this.Property(t => t.ManagerId)
                .HasMaxLength(50)
                .HasColumnName("ManagerId");
            this.Ignore(t => t.UserName);
            // Table & Column Mappings
            this.ToTable("UserInfo");
        }
    }
}
