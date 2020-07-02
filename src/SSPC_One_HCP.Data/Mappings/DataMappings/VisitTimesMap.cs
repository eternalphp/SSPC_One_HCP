using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class VisitTimesMap : BaseEntityTypeConfiguration<VisitTimes>
    {
        public VisitTimesMap()
        {
            this.ToTable("VisitTimes");

            this.Property(t => t.UnionId)
                .HasMaxLength(100)
                .HasColumnName("UnionId");

            this.Property(t => t.WxuserId)
                .HasMaxLength(100)
                .HasColumnName("WxuserId");

            this.Property(t => t.VisitStart) 
                .HasColumnName("VisitStart");

            this.Property(t => t.VisitEnd)
                .HasColumnName("VisitEnd");

            this.Property(t => t.Isvisitor)
                .HasColumnName("Isvisitor");


            this.Property(t => t.StaySeconds)
                .HasColumnName("StaySeconds");
        }
    }
}
