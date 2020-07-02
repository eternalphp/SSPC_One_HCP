using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class BotSaleConfigureMap : BaseEntityTypeConfiguration<BotSaleConfigure>
    {
        public BotSaleConfigureMap()
        {
            this.ToTable("BotSaleConfigure");

            this.Property(t => t.KBSBotId)
                .HasMaxLength(36);

            this.Property(t => t.BotName)
                .HasMaxLength(500);
        }
    }
}
