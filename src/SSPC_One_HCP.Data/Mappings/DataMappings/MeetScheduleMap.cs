using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MeetScheduleMap:BaseEntityTypeConfiguration<MeetSchedule>
    {
        public MeetScheduleMap()
        {
            this.ToTable("MeetSchedule");

            this.Property(t => t.MeetId)
                .HasMaxLength(36)
                .HasColumnName("MeetId");

            this.Property(t => t.ScheduleStart)
                .HasColumnName("ScheduleStart");

            this.Property(t => t.ScheduleEnd)
                .HasColumnName("ScheduleEnd");

            this.Property(t => t.ScheduleContent)
                .HasMaxLength(500)
                .HasColumnName("ScheduleContent");

            this.Property(t => t.MeetSpeakerId)
                .HasMaxLength(36)
                .HasColumnName("MeetSpeakerId");

            this.Property(t => t.AMPM)
                 .HasColumnName("AMPM");
            this.Property(t => t.Sort)              
                .HasColumnName("Sort");
            this.Property(t => t.Topic)
                .HasMaxLength(100)
                .HasColumnName("Topic");
            this.Property(t => t.Speaker)
                .HasMaxLength(50)
                .HasColumnName("Speaker");

        }
    }
}
