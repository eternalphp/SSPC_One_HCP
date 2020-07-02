using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class WechatActionHistoryMap : BaseEntityTypeConfiguration<WechatActionHistory>
    {
        public WechatActionHistoryMap()
        {
            this.ToTable("WechatActionHistory"); 

            this.Property(t => t.ActionType)
                .HasColumnName("ActionType");

            this.Property(t => t.Content)
                .HasMaxLength(500) 
                .HasColumnName("Content");

            this.Property(t => t.ContentId)
                .HasMaxLength(100)
                .HasColumnName("ContentId");

            this.Property(t => t.UnionId)
                .HasMaxLength(100)
                .HasColumnName("UnionId");

            this.Property(t => t.WxuserId)
                .HasMaxLength(100)
                .HasColumnName("WxuserId");

            this.Property(t => t.StaySeconds) 
                .HasColumnName("StaySeconds");
        }
    }
}
