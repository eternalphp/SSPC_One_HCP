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

    public class BotSaleUserMedalInfoMap : BaseEntityTypeConfiguration<BotSaleUserMedalInfo>
    {

        public BotSaleUserMedalInfoMap()
        {
            this.ToTable("BotSaleUserMedalInfo");
            this.Property(t => t.BotMedalRuleId)
              .HasMaxLength(36);
            this.Property(t => t.SaleUserId)
            .HasMaxLength(36);
            this.Property(t => t.MedalSrc)
           .HasMaxLength(2000);
        }
    }
}
