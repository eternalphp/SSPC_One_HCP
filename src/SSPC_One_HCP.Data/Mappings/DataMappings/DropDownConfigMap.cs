using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class DropDownConfigMap : BaseEntityTypeConfiguration<DropDownConfig>
    {
        public DropDownConfigMap()
        {
            // Primary Key
            // Properties
            // 下拉值
            this.Property(t => t.DropDownValue)
                .HasMaxLength(50)
                .HasColumnName("DropDownValue");
            // 下拉文本
            this.Property(t => t.DorpDownText)
                .HasMaxLength(50)
                .HasColumnName("DorpDownText");
            // 下拉类型
            this.Property(t => t.DropDownType)
                .HasMaxLength(20)
                .HasColumnName("DropDownType");
            // Table & Column Mappings
            this.ToTable("DropDownConfig");


        }
    }
}
