using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class PositionMap : BaseEntityTypeConfiguration<Position>
    {
        public PositionMap()
        {
            this.Property(t => t.Code)
                .HasMaxLength(50)
                .HasColumnName("Code");

            this.Property(t => t.Name)
                .HasMaxLength(200)
                .HasColumnName("Name");

            this.Property(t => t.IsDisabled)
                .HasColumnName("IsDisabled");

            this.Property(t => t.OrganizationId)
                .HasMaxLength(50)
                .HasColumnName("OrganizationId");

            this.Property(t => t.ReporterId)
                .HasMaxLength(50)
                .HasColumnName("ReporterId");

            this.Property(t => t.HolderId)
                .HasMaxLength(50)
                .HasColumnName("HolderId");

            this.ToTable("Position");

        }
    }
}
