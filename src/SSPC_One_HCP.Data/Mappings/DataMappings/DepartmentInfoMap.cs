using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class DepartmentInfoMap: BaseEntityTypeConfiguration<DepartmentInfo>
    {
        public DepartmentInfoMap()
        {
            this.ToTable("DepartmentInfo");

            this.Property(t => t.DepartmentName)
                .HasColumnName("DepartmentName");

            this.Property(t => t.DepartmentType)
                .HasColumnName("DepartmentType");
        }
    }
}
