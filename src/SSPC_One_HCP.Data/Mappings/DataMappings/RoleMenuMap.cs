using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class RoleMenuMap : BaseEntityTypeConfiguration<RoleMenu>
    {
        public RoleMenuMap()
        {
            // Primary Key
            // Properties
            // 菜单主键
            this.Property(t => t.MenuId)
                .HasColumnName("MenuId");
            // 角色主键
            this.Property(t => t.RoleId)
                .HasColumnName("RoleId");
            // Table & Column Mappings
            this.ToTable("RoleMenu");


        }
    }
}
