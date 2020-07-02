using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MyMeetOrderMap:BaseEntityTypeConfiguration<MyMeetOrder>
    {
        public MyMeetOrderMap()
        {
            this.ToTable("MyMeetOrder");

            this.Property(t => t.UnionId)
                .HasColumnName("UnionId");

            this.Property(t => t.MeetId)
                .HasMaxLength(36)
                .HasColumnName("MeetId");

            this.Property(t => t.IsRemind)
                .HasColumnName("IsRemind");

            this.Property(t => t.HasReminded)
                .HasColumnName("HasReminded");

            this.Property(t => t.RemindTime)
                .HasColumnName("RemindTime");

            this.Property(t => t.RemindOffsetMinutes)
                .HasColumnName("RemindOffsetMinutes");

            this.Property(t => t.JoinInMeetTime)
                .HasColumnName("JoinInMeetTime");

            this.Property(t => t.WxUserId)
                .HasMaxLength(36)
                .HasColumnName("WxUserId");
        }
    }
}
