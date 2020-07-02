using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MeetSpeakerMap:BaseEntityTypeConfiguration<MeetSpeaker>
    {
        public MeetSpeakerMap()
        {
            this.ToTable("MeetSpeaker");

            this.Property(t => t.MeetId)
                .HasMaxLength(36)
                .HasColumnName("MeetId");

            this.Property(t => t.SpeakerName)
                .HasMaxLength(200)
                .HasColumnName("SpeakerName");

            this.Property(t => t.SpeakerDetail) 
                .HasColumnName("SpeakerDetail");
        }
    }
}
