using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class VisitModulesMap : BaseEntityTypeConfiguration<VisitModules>
    {
        public VisitModulesMap()
        {
            this.ToTable("VisitModules");

            this.Property(t => t.UnionId)
                .HasMaxLength(100)
                .HasColumnName("UnionId");

            this.Property(t => t.ModuleNo)
                .HasMaxLength(100)
                .HasColumnName("ModuleNo");

            this.Property(t => t.ModulePageNo)
                .HasMaxLength(100)
                .HasColumnName("ModulePageNo");

            this.Property(t => t.ModulePageUrl)
                .HasMaxLength(100)
                .HasColumnName("ModulePageUrl");


            this.Property(t => t.StaySeconds)
                .HasColumnName("StaySeconds");

            this.Property(t => t.VisitStart)
                .HasColumnName("VisitStart");

            this.Property(t => t.VisitEnd)
                .HasColumnName("VisitEnd");

        }
    }
}
