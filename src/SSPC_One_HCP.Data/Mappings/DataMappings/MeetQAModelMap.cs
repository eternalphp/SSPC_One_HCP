using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    /// <summary>
    /// 
    /// </summary>
    public class MeetQAModelMap:BaseEntityTypeConfiguration<MeetQAModel>
    {
        public MeetQAModelMap()
        {
            this.ToTable("MeetQAModel");

            this.Property(t => t.MeetId)
                .HasMaxLength(36)
                .HasColumnName("MeetId");

            this.Property(t => t.QAType)
                .HasColumnName("QAType");

            this.Property(t => t.QuestionId)
                .HasMaxLength(36)
                .HasColumnName("QuestionId");
        }
    }
}
