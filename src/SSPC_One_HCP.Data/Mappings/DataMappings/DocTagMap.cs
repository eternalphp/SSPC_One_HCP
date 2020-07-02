using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class DocTagMap : BaseEntityTypeConfiguration<DocTag>
    {
        public DocTagMap()
        {
            this.ToTable("DocTag");

            this.Property(t => t.DocId)
                .HasMaxLength(36)
                .HasColumnName("DocId");

            this.Property(t => t.TagId)
                .HasMaxLength(36)
                .HasColumnName("TagId");
        }
    }
}
