using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class EdaCheckInRecordMap : BaseEntityTypeConfiguration<EdaCheckInRecord>
    {
        public EdaCheckInRecordMap()
        {
            this.ToTable("EdaCheckInRecord");

            this.Property(t => t.AppId)
                .HasMaxLength(200)
                .HasColumnName("AppId");

            //UnionId
            this.Property(t => t.UnionId)
               .HasMaxLength(200)
               .HasColumnName("UnionId");

            //OpenId
            this.Property(t => t.OpenId)
                .HasMaxLength(200)
                .HasColumnName("OpenId");

            //用户名称
            this.Property(t => t.UserName)
                .HasMaxLength(100)
                .HasColumnName("UserName");

            //微信昵称
            this.Property(t => t.WxName)
              .HasMaxLength(200)
              .HasColumnName("WxName");

            //科内会编号
            this.Property(t => t.ActivityID)
                .HasMaxLength(50)
                .HasColumnName("ActivityID");

            //访问时间
            this.Property(t => t.VisitTime)
                .HasColumnName("VisitTime");


        }
    }
}
