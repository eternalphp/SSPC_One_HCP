using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class BannerInfoMap : BaseEntityTypeConfiguration<BannerInfo>
    {
        public BannerInfoMap()
        {
            this.ToTable("BannerInfo");
        }
    }
}
