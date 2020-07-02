using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class BannerInfoItemMap : BaseEntityTypeConfiguration<BannerInfoItem>
    {
        public BannerInfoItemMap()
        {
            this.ToTable("BannerInfoItem");

            
        }
    }
}
