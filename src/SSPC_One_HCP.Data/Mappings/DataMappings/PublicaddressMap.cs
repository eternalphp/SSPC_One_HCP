using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class PublicaddressMap : BaseEntityTypeConfiguration<PublicAccount>
    {
        public PublicaddressMap(){
            this.ToTable("PublicAccount");

            this.Property(t => t.AppId)
                .IsRequired()
                .HasMaxLength(70)
                .HasColumnName("ProductTypeInfoId");

            this.Property(t => t.PublicAccountName)
                .HasMaxLength(100)
                .HasColumnName("PublicAccountName");

            this.Property(t => t.AppUrl)
                .HasColumnName("AppUrl");

            this.Property(t => t.Iseffective)
                .HasColumnName("Iseffective");

            this.Property(t => t.Dept)
                .HasColumnName("Dept");
        }
    }
}
