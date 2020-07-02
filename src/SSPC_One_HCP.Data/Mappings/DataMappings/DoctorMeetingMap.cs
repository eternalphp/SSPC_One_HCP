using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class DoctorMeetingMap : BaseEntityTypeConfiguration<DoctorMeeting>
    {
        public DoctorMeetingMap()
        {
            this.ToTable("DoctorMeeting");
            this.Property(t => t.MeetingID).HasMaxLength(36);
            this.Property(t => t.DoctorID).HasMaxLength(36);
            this.Property(t => t.TagGroupID).HasMaxLength(36);
        }
    }
}
