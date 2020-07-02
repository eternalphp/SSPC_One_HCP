using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class UserRoleMap:BaseEntityTypeConfiguration<UserRole>
    {
        public UserRoleMap()
        {
            // Primary Key
            // Properties
            // 用户sap编码
            this.Property(t => t.SapCode)
                .HasMaxLength(50)
                .HasColumnName("SapCode");
            // 用户主键
            this.Property(t => t.UserId)
                .HasMaxLength(36)
                .HasColumnName("UserId");
            // 角色主键
            this.Property(t => t.RoleId)
                .HasMaxLength(36)
                .HasColumnName("RoleId");
            // Table & Column Mappings
            this.ToTable("UserRole");


        }
    }
}
