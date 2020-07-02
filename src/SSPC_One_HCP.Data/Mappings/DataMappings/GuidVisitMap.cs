using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class GuidVisitMap : BaseEntityTypeConfiguration<GuidVisit>
    {
        public GuidVisitMap()
        {
            this.ToTable("GuidVisit");

            this.Property(t => t.userid)
                .HasMaxLength(100)
                .HasColumnName("userid");

            this.Property(t => t.ActionType)
                .HasMaxLength(100)
                .HasColumnName("ActionType");

            this.Property(t => t.GuideId) 
                .HasColumnName("GuideId");

            this.Property(t => t.GuideType) 
                .HasColumnName("GuideType");

            this.Property(t => t.GuideName)
                .HasMaxLength(500)
                .HasColumnName("GuideName");

            this.Property(t => t.Email)
                .HasMaxLength(100)
                .HasColumnName("Email");

            this.Property(t => t.Keyword)
                .HasMaxLength(100)
                .HasColumnName("Keyword");

            this.Property(t => t.VisitStart)
                .HasColumnName("VisitStart");

            this.Property(t => t.VisitEnd)
                .HasColumnName("VisitEnd");

            this.Property(t => t.StaySeconds)
                .HasColumnName("StaySeconds");
        }
    }
}
