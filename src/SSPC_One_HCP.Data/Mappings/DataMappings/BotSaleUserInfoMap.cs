using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{

    public class BotSaleUserInfoMap : BaseEntityTypeConfiguration<BotSaleUserInfo>
    {

        public BotSaleUserInfoMap()
        {
            this.ToTable("BotSaleUserInfo");

        
        }
    }
}
