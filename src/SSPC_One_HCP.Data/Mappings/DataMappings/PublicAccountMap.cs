using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class PublicAccountMap : BaseEntityTypeConfiguration<PublicAccount>
    {
        public PublicAccountMap(){
            this.ToTable("PublicAccount");

            this.Property(t => t.AppId)
                .HasMaxLength(70)
                .HasColumnName("AppId");

            this.Property(t => t.PublicAccountName)
                .HasMaxLength(100)
                .HasColumnName("PublicAccountName");

            this.Property(t => t.AppUrl)
                .HasColumnName("AppUrl");

            this.Property(t => t.Iseffective)
                .HasColumnName("Iseffective");

            this.Property(t => t.Dept)
                .HasColumnName("Dept");

            //this.Property(t => t.IsSort)
            //    .HasMaxLength(100)
            //    .HasColumnName("IsSort");

            this.Property(t => t.ImageUrl)
                .HasMaxLength(200)
                .HasColumnName("ImageUrl");

            this.Property(t => t.ImageName)
                .HasMaxLength(100)
                .HasColumnName("ImageName");
        }
    }
}
