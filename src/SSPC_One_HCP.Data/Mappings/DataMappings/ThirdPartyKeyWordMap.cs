using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class ThirdPartyKeyWordMap : BaseEntityTypeConfiguration<ThirdPartyKeyWord>
    {
        public ThirdPartyKeyWordMap()
        {
            this.ToTable("ThirdPartyKeyWord");

            this.Property(t => t.KeyWordContent)
                .HasMaxLength(100)
                .HasColumnName("KeyWordContent");

            this.Property(t => t.KeyWordType)
                .HasColumnName("KeyWordType");

            this.Property(t => t.KeyWordSort)
                .HasColumnName("KeyWordSort");
        }
    }
}
