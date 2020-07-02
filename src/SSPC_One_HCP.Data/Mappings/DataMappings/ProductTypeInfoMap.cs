using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class ProductTypeInfoMap:BaseEntityTypeConfiguration<ProductTypeInfo>
    {
        public ProductTypeInfoMap()
        {
            this.ToTable("ProductTypeInfo");

            this.Property(t => t.TypeId)
                .HasColumnName("TypeId");

            this.Property(t => t.SubTitle)
                .HasMaxLength(20)
                .HasColumnName("SubTitle");

            this.Property(t => t.ContentDepType)
                .HasMaxLength(100)
                .HasColumnName("ContentDepType");

            this.Property(t => t.SubTypeUrl)
                .HasColumnName("SubTypeUrl");

            this.Property(t => t.IsCompleted)
                .HasColumnName("IsCompleted");
        }
    }
}
