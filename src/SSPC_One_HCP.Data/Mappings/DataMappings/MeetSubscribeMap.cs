using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MeetSubscribeMap : BaseEntityTypeConfiguration<MeetSubscribe>
    {
        public MeetSubscribeMap()
        {
            this.ToTable("MeetSubscribe");

            this.Property(t => t.UserId)
                .HasMaxLength(36)
                .HasColumnName("UserId");

        }
    }
}
