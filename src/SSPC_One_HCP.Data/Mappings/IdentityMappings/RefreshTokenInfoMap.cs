using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.IdentityMappings
{
    public class RefreshTokenInfoMap : BaseEntityTypeConfiguration<RefreshTokenInfo>
    {
        public RefreshTokenInfoMap() : base()
        {
            // Table & Column Mappings
            this.ToTable("RefreshTokenInfo");
            this.Property(t => t.AppId)
                .HasMaxLength(36)
                .HasColumnName("AppId");

            this.Property(t => t.Subject)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("Subject");

            this.Property(t => t.AppClientId)
                .IsRequired()
                .HasMaxLength(36)
                .HasColumnName("AppClientId");

            this.Property(t => t.IssuedUtc)
                .IsRequired()
                .HasColumnName("IssuedUtc");

            this.Property(t => t.ExpiresUtc)
               .IsRequired()
               .HasColumnName("ExpiresUtc");

            this.Property(t => t.ProtectedTicket)
               .IsRequired()
               .HasMaxLength(1000)
               .HasColumnName("ProtectedTicket");
        }
    }
}
