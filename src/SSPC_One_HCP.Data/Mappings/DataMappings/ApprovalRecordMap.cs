using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class ApprovalRecordMap : BaseEntityTypeConfiguration<ApprovalRecord>
    {
        public ApprovalRecordMap()
        {
            // Primary Key
            // Properties
            // 单据主键
            this.Property(t => t.AssetsMainId)
                .HasMaxLength(200)
                .HasColumnName("AssetsMainId");
            // 操作人
            this.Property(t => t.OperationUser)
                .HasMaxLength(50)
                .HasColumnName("OperationUser");
            // 动作：提交、同意、驳回
            this.Property(t => t.OperationAction)
                .HasColumnName("OperationAction");
            // 操作时间
            this.Property(t => t.OperationDate)
                .HasColumnName("OperationDate");
            // Table & Column Mappings
            this.ToTable("ApprovalRecord");


        }
    }
}
