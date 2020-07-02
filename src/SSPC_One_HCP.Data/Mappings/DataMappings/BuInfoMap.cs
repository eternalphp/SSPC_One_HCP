using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class BuInfoMap : BaseEntityTypeConfiguration<BuInfo>
    {
        public BuInfoMap()
        {
            this.ToTable("BuInfo");

            this.Property(t => t.BuName)
                .HasMaxLength(500)
                .HasColumnName("BuName");
        }
    }
}
