using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class LanguageConfigMap : BaseEntityTypeConfiguration<LanguageConfig>
    {
        public LanguageConfigMap()
        {
            // Primary Key
            // Properties
            // 语言key
            this.Property(t => t.LanKey)
                .HasMaxLength(200)
                .HasColumnName("LanKey");
            // 语言展示
            this.Property(t => t.LanValue)
                .HasMaxLength(200)
                .HasColumnName("LanValue");
            // 语言类型
            this.Property(t => t.LanType)
                .HasMaxLength(200)
                .HasColumnName("LanType");
            // Table & Column Mappings
            this.ToTable("LanguageConfig");


        }
    }
}
