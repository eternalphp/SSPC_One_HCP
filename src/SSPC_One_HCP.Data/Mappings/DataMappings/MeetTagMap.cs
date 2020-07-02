using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MeetTagMap : BaseEntityTypeConfiguration<MeetTag>
    {
        public MeetTagMap()
        {
            this.ToTable("MeetTag");

            this.Property(t => t.MeetId)
                .HasMaxLength(36)
                .HasColumnName("MeetId");

            this.Property(t => t.TagId)
                .HasMaxLength(36)
                .HasColumnName("TagId");
        }
    }
}
