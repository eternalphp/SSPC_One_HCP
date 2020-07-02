using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class ProductInfoMap : BaseEntityTypeConfiguration<ProductInfo>
    {
        public ProductInfoMap()
        {
            this.ToTable("ProductInfo");

            this.Property(t => t.ProductName)
                .HasMaxLength(500)
                .HasColumnName("ProductName");

            this.Property(t => t.ProductDesc)
                .HasMaxLength(5000)
                .HasColumnName("ProductDesc");

            this.Property(t => t.ProductUrl)
                .HasColumnName("ProductUrl");

            this.Property(t => t.ProductPicName)
                .HasColumnName("ProductPicName");

            this.Property(t => t.Sort)
                .HasColumnName("Sort");

            this.Property(t => t.IsCompleted)
                .HasColumnName("IsCompleted");

            this.Property(t => t.OldId)
                .HasMaxLength(36)
                .HasColumnName("OldId");

            this.Property(t => t.ApprovalNote)
                .HasMaxLength(500)
                .HasColumnName("ApprovalNote");

        }
    }
}
