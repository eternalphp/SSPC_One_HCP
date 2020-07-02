using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MeetInfoMap : BaseEntityTypeConfiguration<MeetInfo>
    {
        public MeetInfoMap()
        {
            this.ToTable("MeetInfo");

            this.Property(t => t.MeetTitle)
                .HasMaxLength(100)
                .HasColumnName("MeetTitle");

            this.Property(t => t.MeetSubject)
                .HasMaxLength(500)
                .HasColumnName("MeetSubject");

            this.Property(t => t.MeetType)
                .HasColumnName("MeetType");

            this.Property(t => t.MeetDep)
                .HasMaxLength(100)
                .HasColumnName("MeetDep");

            this.Property(t => t.MeetIntroduction)
                .HasMaxLength(500)
                .HasColumnName("MeetIntroduction");

            this.Property(t => t.MeetStartTime)
                .HasColumnName("MeetStartTime");

            this.Property(t => t.MeetEndTime)
                .HasColumnName("MeetEndTime");

            this.Property(t => t.MeetDate)
                .HasColumnName("MeetDate");

            this.Property(t => t.Speaker)
                .HasMaxLength(36)
                .HasColumnName("Speaker");

            this.Property(t => t.SpeakerDetail)
                .HasColumnName("SpeakerDetail");

            this.Property(t => t.ReplayAddress)
                .HasMaxLength(1000)
                .HasColumnName("ReplayAddress");

            this.Property(t => t.MeetAddress)
                .HasMaxLength(1000)
                .HasColumnName("MeetAddress");

            this.Property(t => t.MeetData)
                .HasMaxLength(100)
                .HasColumnName("MeetData");

            this.Property(t => t.MeetCodeUrl)
                .HasColumnName("MeetCodeUrl");

            this.Property(t => t.MeetCity)
                .HasMaxLength(500)
                .HasColumnName("MeetCity");

            this.Property(t => t.MeetingNumber)
                .HasColumnName("MeetingNumber");

            this.Property(t => t.MeetSite)
                .HasColumnName("MeetSite");

            this.Property(t => t.MeetCoverSmall)
                .HasColumnName("MeetCoverSmall");

            this.Property(t => t.MeetCoverBig)
                .HasColumnName("MeetCoverBig");

            this.Property(t => t.IsCompleted)
                .HasColumnName("IsCompleted");

            this.Property(t => t.IsChoiceness)
                .HasColumnName("IsChoiceness");

            this.Property(t => t.IsHot)
                .HasColumnName("IsHot");

            this.Property(t => t.OldId)
                .HasMaxLength(36)
                .HasColumnName("OldId");

            this.Property(t => t.ApprovalNote)
                .HasMaxLength(500)
                .HasColumnName("ApprovalNote");
            
            this.Property(t => t.Source)
                .HasMaxLength(50)
                .HasColumnName("Source");

            this.Property(t => t.SourceId)
                .HasMaxLength(50)
                .HasColumnName("SourceId");

            this.Property(t => t.SourceHospital)
                .HasMaxLength(500)
                .HasColumnName("SourceHospital");

            this.Property(t => t.SourceDepartment)
                .HasColumnName("SourceDepartment");

            this.Property(t => t.HasReminded)
                .HasColumnName("HasReminded");

            this.Property(t => t.InvitationDetail)
                .HasColumnName("InvitationDetail");
        }
    }
}
