using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class AdQRCodeMap : BaseEntityTypeConfiguration<AdQRCode>
    {
        public AdQRCodeMap()
        {
            this.ToTable("AdQRCode");

            this.Property(t => t.BuName)
                .HasMaxLength(50)
                .HasColumnName("BuName");

            this.Property(t => t.AppName)
                .HasMaxLength(100)
                .HasColumnName("AppName");

            this.Property(t => t.AppUrl)
                .HasMaxLength(500)
                .HasColumnName("AppUrl");

            this.Property(t => t.QRCodePicUrl)
                .HasMaxLength(500)
                .HasColumnName("QRCodePicUrl");

            this.Property(t => t.VisitAmount)
                .HasColumnName("VisitAmount");
           
        }
    }
}
