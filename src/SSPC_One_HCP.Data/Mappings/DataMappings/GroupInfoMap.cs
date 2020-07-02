using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class GroupInfoMap : BaseEntityTypeConfiguration<GroupInfo>
    {
        public GroupInfoMap()
        {
            this.ToTable("GroupInfo");

            this.Property(t => t.GroupName)
                .HasMaxLength(500)
                .HasColumnName("GroupName");

            this.Property(t => t.GroupType)
                .HasMaxLength(500)
                .HasColumnName("GroupType");
        }
    }
}
