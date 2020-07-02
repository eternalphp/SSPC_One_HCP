using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.IdentityMappings
{
    public class AppClientInfoMap : BaseEntityTypeConfiguration<AppClientInfo>
    {
        public AppClientInfoMap() : base()
        {
            // Table & Column Mappings
            this.ToTable("AppClientInfo");
            //this.HasKey(s => s.AppName);

            this.Property(t => t.AppName)
                .IsRequired()
                .HasMaxLength(36)
                .HasColumnName("AppName");

            this.Property(t => t.Secret)
                .IsRequired()
                .HasMaxLength(1000)
                .HasColumnName("Secret");

            this.Property(t => t.ApplicationType)
                .IsRequired()
                .HasColumnName("ApplicationType");

            this.Property(t => t.AppClientName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("AppClientName");

            this.Property(t => t.Active)
               .IsRequired()
               .HasColumnName("Active");

            this.Property(t => t.RefreshTokenLifeTime)
               .IsRequired()
               .HasColumnName("RefreshTokenLifeTime");

            this.Property(t => t.AllowedOrigin)
              .HasMaxLength(200)
              .HasColumnName("AllowedOrigin");
        }

    }
}
