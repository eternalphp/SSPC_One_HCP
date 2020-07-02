using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MeetFileMap : BaseEntityTypeConfiguration<MeetFile>
    {
        public MeetFileMap()
        {
            this.ToTable("MeetFile");

            this.Property(t => t.MeetId)
                .HasMaxLength(36)
                .HasColumnName("MeetId");

            this.Property(t => t.Title)
                .HasMaxLength(100)
                .HasColumnName("Title");

            this.Property(t => t.FileName)
                .HasMaxLength(500)
                .HasColumnName("FileName");

            this.Property(t => t.FilePath)
                .HasColumnName("FilePath");

            this.Property(t => t.FileSize)
                .HasColumnName("FileSize");

            this.Property(t => t.FileType)
                .HasMaxLength(500)
                .HasColumnName("FileType");

            this.Property(t => t.IsCopyRight)
                .HasColumnName("IsCopyRight");
        }
    }
}
