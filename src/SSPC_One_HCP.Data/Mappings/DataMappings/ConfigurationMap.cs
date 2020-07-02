using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class ConfigurationMap : BaseEntityTypeConfiguration<Configuration>
    {
        public ConfigurationMap()
        {
            // Primary Key
            // Properties
            // 编号
            this.Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id");
            // 配置标识
            this.Property(t => t.ConfigureName)
                .HasMaxLength(20)
                .HasColumnName("ConfigureName");
            // 配置标识值
            this.Property(t => t.ConfigureValue)
                .HasMaxLength(200)
                .HasColumnName("ConfigureValue");
            // Table & Column Mappings
            this.ToTable("Configuration");


        }
    }
}
