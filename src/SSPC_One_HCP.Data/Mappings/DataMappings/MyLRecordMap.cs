using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MyLRecordMap:BaseEntityTypeConfiguration<MyLRecord>
    {
        public MyLRecordMap()
        {
            this.ToTable("MyLRecord");

            this.Property(t => t.UnionId)
                .HasColumnName("UnionId");

            this.Property(t => t.LObjectId)
                .HasMaxLength(36)
                .HasColumnName("LObjectId");

            this.Property(t => t.LObjectType)
                .HasColumnName("LObjectType");

            this.Property(t => t.LDate)
                .HasColumnName("LDate");

            this.Property(t => t.LDateStart)
                .HasColumnName("LDateStart");

            this.Property(t => t.LDateEnd)
                .HasColumnName("LDateEnd");

            this.Property(t => t.IsRead)
                .HasColumnName("IsRead");

            this.Property(t => t.LObjectDate)
                .HasColumnName("LObjectDate");

            this.Property(t => t.WxUserId)
                .HasMaxLength(36)
                .HasColumnName("WxUserId");
        }
    }
}
