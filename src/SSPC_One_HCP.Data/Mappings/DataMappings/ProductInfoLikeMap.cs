using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class ProductInfoLikeMap : BaseEntityTypeConfiguration<ProductInfoLike>
    {
        public ProductInfoLikeMap()
        {
            this.ToTable("ProductInfoLike");

            this.Property(t => t.ProID)
                .HasMaxLength(64)
                .HasColumnName("ProID");

            this.Property(t => t.IsLike)
                .HasColumnName("IsLike");
        }
    }
}
