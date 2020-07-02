using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.IdentityMappings
{
    public class AccountLoginMap :BaseEntityTypeConfiguration<AccountLoginInfo>
    {
        public AccountLoginMap() : base()
        {
            // Table & Column Mappings
            this.ToTable("AccountLoginInfo");

            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(36)
                .HasColumnName("UserId");

            this.Property(t => t.ProviderKey)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("ProviderKey");

            this.Property(t => t.LoginProvider)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("LoginProvider");
        }
    }
}
