using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MeetQAResultMap : BaseEntityTypeConfiguration<MeetQAResult>
    {
        public MeetQAResultMap()
        {
            this.Property(t => t.MeetId)
                .HasMaxLength(36)
                .HasColumnName("MeetId");

            this.Property(t => t.MeetQAId)
                .HasMaxLength(36)
                .HasColumnName("MeetQAId");

            this.Property(t => t.SignUpUserId)
                .HasMaxLength(36)
                .HasColumnName("SignUpUserId");

            this.Property(t => t.UserAnswerId)
                .HasMaxLength(36)
                .HasColumnName("UserAnswerId");

            this.Property(t => t.UserAnswer)
                .HasMaxLength(500)
                .HasColumnName("UserAnswer");

            this.ToTable("MeetQAResult");
        }
    }
}
