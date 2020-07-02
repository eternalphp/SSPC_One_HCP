using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MyReadRecordMap : BaseEntityTypeConfiguration<MyReadRecord>
    {

        public MyReadRecordMap()
        {
            this.ToTable("MyBrowseRecord");

            this.Property(t => t.UnionId)
                .HasMaxLength(50)
                .HasColumnName("UnionId");

            this.Property(t => t.DataInfoId)
                .HasMaxLength(36)
                .HasColumnName("DataInfoId");

            this.Property(t => t.IsRead)
                .HasColumnName("IsRead");

            this.Property(t => t.WxUserId)
                .HasMaxLength(36)
                .HasColumnName("WxUserId");
        }

    }
}
