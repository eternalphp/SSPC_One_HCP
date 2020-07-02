using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
   public class QRcodeExtensionMap: BaseEntityTypeConfiguration<QRcodeExtension>
    {
        public QRcodeExtensionMap() {
            this.ToTable("QRcodeExtension");

            this.Property(t => t.AppId)
                .HasColumnName("AppId")
                .HasMaxLength(150);

            this.Property(t => t.AppName)
                .HasColumnName("AppName")
                .HasMaxLength(150);

            this.Property(t => t.AppType)
                .HasColumnName("AppType")
                .HasMaxLength(150);

            this.Property(t => t.AppUrl)
                .HasColumnName("AppUrl")
                .HasMaxLength(300);

            this.Property(t => t.AppImangeUrl)
                .HasColumnName("AppimangeUrl")
                .HasMaxLength(300);

            this.Property(t => t.AppImangeName)
                .HasColumnName("AppimangeName")
                .HasMaxLength(300);
        }
    }
}
