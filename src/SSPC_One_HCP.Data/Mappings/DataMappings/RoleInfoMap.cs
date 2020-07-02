using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class RoleInfoMap : BaseEntityTypeConfiguration<RoleInfo>
    {
        public RoleInfoMap()
        {
            // Primary Key
            // Properties
            // 角色名称
            this.Property(t => t.RoleName)
                .HasMaxLength(50)
                .HasColumnName("RoleName");
            // 角色描述
            this.Property(t => t.RoleDesc)
                .HasMaxLength(200)
                .HasColumnName("RoleDesc");
            // Table & Column Mappings
            this.ToTable("RoleInfo");


        }
    }
}
