using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class SendRateMap : BaseEntityTypeConfiguration<SendRate>
    {
        public SendRateMap()
        {
            this.ToTable("SendRate");

            this.Property(t => t.DoctorId)
                .HasMaxLength(36)
                .HasColumnName("DoctorId");

            this.Property(t => t.SendCycleType)                
                .HasColumnName("SendCycleType");

            this.Property(t => t.SendNumber)                
                .HasColumnName("SendNumber");

            this.Property(t => t.IsDefault)             
                .HasColumnName("IsDefault");
        }
    }
}
