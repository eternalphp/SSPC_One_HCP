using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class BotMedalStandardConfigureMap : BaseEntityTypeConfiguration<BotMedalStandardConfigure>
    {
        public BotMedalStandardConfigureMap()
        {
            this.ToTable("BotMedalStandardConfigure");

            this.Property(t => t.KBSBotId)
                .HasMaxLength(36);

            this.Property(t => t.MedalName)
                .HasMaxLength(500);

            this.Property(t => t.MedalYSrc)
                .HasMaxLength(2000);

            this.Property(t => t.MedalNSrc)
                .HasMaxLength(2000);

        }
    }
}
