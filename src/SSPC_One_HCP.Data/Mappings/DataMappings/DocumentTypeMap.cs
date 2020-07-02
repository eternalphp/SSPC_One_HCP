using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class DocumentTypeMap:BaseEntityTypeConfiguration<DocumentType>
    {
        public DocumentTypeMap()
        {
            this.Property(t => t.ImgUrl)
                .IsMaxLength()
                .HasColumnName("ImgUrl");

            this.Property(t => t.TypeValue)
                .HasMaxLength(100)
                .HasColumnName("TypeValue");

            this.ToTable("DocumentType");
        }
    }
}
