using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class CompetingProductMap : BaseEntityTypeConfiguration<CompetingProduct>
    {
        public CompetingProductMap()
        {
            this.ToTable("CompetingProduct");

            this.Property(t => t.CompeteProductId)
                .HasColumnName("CompeteProductId");

            this.Property(t => t.ProductName)
                .HasMaxLength(200)
                .HasColumnName("ProductName");

            this.Property(t => t.MedicineName)
                .HasMaxLength(200)
                .HasColumnName("MedicineName");

            this.Property(t => t.MedicineSource)
                .HasMaxLength(200)
                .HasColumnName("MedicineSource");
        }
    }
}
