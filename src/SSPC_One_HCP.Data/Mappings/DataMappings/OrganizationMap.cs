using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class OrganizationMap : BaseEntityTypeConfiguration<Organization>
    {
        public OrganizationMap()
        {
            this.Property(t => t.Code)
                .HasMaxLength(50)
                .HasColumnName("Code");

            this.Property(t => t.Name)
                .HasMaxLength(500)
                .HasColumnName("Name");

            this.Property(t => t.IsDisabled)
                .HasColumnName("IsDisabled");

            this.Property(t => t.Level)
                .HasColumnName("Level");

            this.Property(t => t.ParentId)
                .HasMaxLength(50)
                .HasColumnName("ParentId");

            this.Property(t => t.ManagerId)
                .HasMaxLength(50)
                .HasColumnName("ManagerId");

            this.Property(t => t.Path)
                .HasMaxLength(500)
                .HasColumnName("Path");

            this.ToTable("Organization");

        }
    }
}
