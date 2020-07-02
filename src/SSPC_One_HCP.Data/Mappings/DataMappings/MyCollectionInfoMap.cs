using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MyCollectionInfoMap:BaseEntityTypeConfiguration<MyCollectionInfo>
    {
        public MyCollectionInfoMap()
        {
            this.ToTable("MyCollectionInfo");

            this.Property(t => t.UnionId)
                .HasColumnName("UnionId");

            this.Property(t => t.CollectionDataId)
                .HasColumnName("CollectionDataId");

            this.Property(t => t.CollectionType)
                .HasColumnName("CollectionType");

            this.Property(t => t.WxUserId)
                .HasMaxLength(36)
                .HasColumnName("WxUserId");
        }
    }
}