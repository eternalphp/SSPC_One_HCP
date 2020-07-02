using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class WechatPublicAccountMap : BaseEntityTypeConfiguration<WechatPublicAccount>
    {
        public WechatPublicAccountMap()
        {
            this.ToTable("WechatPublicAccount");

            this.Property(t => t.AppId)
                .HasMaxLength(200)
                .HasColumnName("AppId");

            this.Property(t => t.Name)
                .HasMaxLength(500)
                .HasColumnName("Name");

            this.Property(t => t.Summary)
                .HasColumnName("Summary");

            this.Property(t => t.ClickVolume)
                .HasColumnName("ClickVolume");

        }
    }
}
