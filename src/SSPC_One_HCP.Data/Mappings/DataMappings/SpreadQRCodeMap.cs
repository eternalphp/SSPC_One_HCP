using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class SpreadQRCodeMap : BaseEntityTypeConfiguration<SpreadQRCode>
    {
        public SpreadQRCodeMap()
        {
            this.ToTable("SpreadQRCode");

            this.Property(t => t.SpreadAppId)
                .HasMaxLength(50)
                .HasColumnName("SpreadAppId");

            this.Property(t => t.SpreadName)
                .HasMaxLength(100)
                .HasColumnName("SpreadName");

            this.Property(t => t.SpreadQRType)
                .HasColumnName("SpreadQRType");

            this.Property(t => t.SpreadQRCodeUrl)
                .HasMaxLength(256)
                .HasColumnName("SpreadQRCodeUrl");

            this.Property(t => t.RegisteredCount)              
                .HasColumnName("RegisteredCount");

            this.Property(t => t.VisitorsCount)
                .HasColumnName("VisitorsCount");

        }
    }
}
