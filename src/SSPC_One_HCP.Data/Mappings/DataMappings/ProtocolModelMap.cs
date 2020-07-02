using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class ProtocolModelMap:BaseEntityTypeConfiguration<ProtocolModel>
    {
        public ProtocolModelMap()
        {
            this.ToTable("ProtocolModel");

            this.Property(t => t.ProctocolName)
                .HasMaxLength(50)
                .HasColumnName("ProctocolName");

            this.Property(t => t.ProctocolType)
                .HasColumnName("ProctocolType");

            this.Property(t => t.ProctocolUrl)
                .HasColumnName("ProctocolUrl");
        }
    }
}
