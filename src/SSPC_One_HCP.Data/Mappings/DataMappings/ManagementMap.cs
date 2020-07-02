using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    /// <summary>
    /// 短信模板类
    /// </summary>
    public class ManagementMap : BaseEntityTypeConfiguration<Management>
    {
        public ManagementMap() {
            this.ToTable("Management");

            this.Property(t => t.ManagementId)
                .HasMaxLength(50)
                .HasColumnName("ManagementId");

            this.Property(t => t.ManagementWord)
                .HasColumnName("ManagementWord");

            this.Property(t => t.IsCompleted)
                .HasColumnName("IsCompleted");

            this.Property(t => t.OldManagementId)
                .HasColumnName("OldManagementId");
        }
    }
}
