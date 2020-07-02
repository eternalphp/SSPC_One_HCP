using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
   public class QRcodeRecordMap: BaseEntityTypeConfiguration<QRcodeRecord>
    {
        public QRcodeRecordMap() {
            this.ToTable("QRcodeRecord");

            this.Property(t => t.AppId)
                .HasMaxLength(200)
                .HasColumnName("AppId");

            this.Property(t => t.UnionId)
                .HasMaxLength(200)
                .HasColumnName("UnionId");

            this.Property(t => t.Isregistered)
                .HasColumnName("Isregistered");

            this.Property(t => t.SourceType)
                .HasColumnName("SourceType");

            this.Property(t => t.WxSceneId)
                .HasColumnName("WxSceneId");
        }
    }
}
