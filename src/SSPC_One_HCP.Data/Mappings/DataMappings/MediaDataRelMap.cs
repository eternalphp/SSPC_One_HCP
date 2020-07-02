using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MediaDataRelMap : BaseEntityTypeConfiguration<MediaDataRel>
    {
        public MediaDataRelMap()
        {
            this.ToTable("MediaDataRel");

            this.Property(t => t.BuName)
                .HasMaxLength(50)
                .HasColumnName("BuName");

            this.Property(t => t.DeptId)
                .HasMaxLength(50)
                .HasColumnName("DeptId");

            this.Property(t => t.ProId)
                .HasMaxLength(50)
                .HasColumnName("ProId");

            this.Property(t => t.DataInfoId)
                .HasMaxLength(36)
                .HasColumnName("DataInfoId");
        }
    }
}
