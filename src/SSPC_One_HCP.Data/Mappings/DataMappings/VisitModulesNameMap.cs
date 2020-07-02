using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class VisitModulesNameMap : BaseEntityTypeConfiguration<VisitModulesName>
    {
        public VisitModulesNameMap()
        {
            this.ToTable("VisitModulesName");

            this.Property(t => t.ModulesName)
                .HasMaxLength(100)
                .HasColumnName("ModulesName");

            this.Property(t => t.ModulesNo)
                .HasMaxLength(100)
                .HasColumnName("ModulesNo");

            this.Property(t => t.ModulesUrl)
                .HasMaxLength(100)
                .HasColumnName("ModulesUrl"); 

        }
    }
}
