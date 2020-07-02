using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MeetPicMap : BaseEntityTypeConfiguration<MeetPic>
    {
        public MeetPicMap()
        {
            this.ToTable("MeetPic");

            this.Property(t => t.MeetId)
                .HasMaxLength(36)
                .HasColumnName("MeetId");

            this.Property(t => t.MeetPicName)
                .HasMaxLength(200)
                .HasColumnName("MeetPicName");

            this.Property(t => t.MeetPicType)
                .HasMaxLength(200)
                .HasColumnName("MeetPicType");

            this.Property(t => t.MeetPicUrl)
                .HasColumnName("MeetPicUrl");
        }
    }
}
