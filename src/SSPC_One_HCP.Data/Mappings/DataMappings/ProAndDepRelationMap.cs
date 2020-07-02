using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class ProAndDepRelationMap:BaseEntityTypeConfiguration<ProAndDepRelation>
    {
        public ProAndDepRelationMap()
        {
            this.ToTable("ProAndDepRelation");

            this.Property(t => t.ProductId)
                .HasMaxLength(36)
                .HasColumnName("ProductId");

            this.Property(t => t.DepartmentId)
                .HasMaxLength(36)
                .HasColumnName("DepartmentId");
        }
    }
}
