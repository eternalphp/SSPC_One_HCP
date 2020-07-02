using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MeetAndProAndDepRelationMap:BaseEntityTypeConfiguration<MeetAndProAndDepRelation>
    {
        public MeetAndProAndDepRelationMap()
        {
            this.ToTable("MeetAndProAndDepRelation");

            this.Property(t => t.MeetId)
                .HasMaxLength(36)
                .HasColumnName("MeetId");

            this.Property(t => t.ProductId)
                .HasMaxLength(36)
                .HasColumnName("ProductId");

            this.Property(t => t.DepartmentId)
                .HasColumnName("DepartmentId");

            this.Property(t => t.BuName)
                .HasMaxLength(50)
                .HasColumnName("BuName");
        }
    }
}
