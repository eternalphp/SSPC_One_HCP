using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class GroupTagRelMap : BaseEntityTypeConfiguration<GroupTagRel>
    {
        public GroupTagRelMap()
        {
            this.ToTable("GroupTagRel");

            this.Property(t => t.TagGroupId)
                .HasMaxLength(500)
                .HasColumnName("TagGroupId");

            this.Property(t => t.TagId)
                .HasMaxLength(500)
                .HasColumnName("TagId ");
        }
    }
}