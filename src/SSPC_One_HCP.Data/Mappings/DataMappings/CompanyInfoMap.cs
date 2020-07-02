using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class CompanyInfoMap : BaseEntityTypeConfiguration<CompanyInfo>
    {
        public CompanyInfoMap()
        {
            // Primary Key
            // Properties
            // 公司名称
            this.Property(t => t.CompanyName)
                .HasMaxLength(50)
                .HasColumnName("CompanyName");
            // 公司号
            this.Property(t => t.CompanyNum)
                .HasMaxLength(200)
                .HasColumnName("CompanyNum");
            // Table & Column Mappings
            this.ToTable("CompanyInfo");


        }
    }
}
