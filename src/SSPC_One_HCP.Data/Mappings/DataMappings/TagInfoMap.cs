using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class TagInfoMap : BaseEntityTypeConfiguration<TagInfo>
    {
        public TagInfoMap()
        {
            this.ToTable("TagInfo");

            this.Property(t => t.TagName)
                .HasMaxLength(500)
                .HasColumnName("TagName");

            this.Property(t => t.TagType)
                .HasMaxLength(500)
                .HasColumnName("TagType");

            this.Property(t => t.TagRule)
                .HasColumnName("TagRule");

            this.Property(t => t.TextKey)
                .HasMaxLength(500)
                .HasColumnName("TextKey");
        }
    }
}
