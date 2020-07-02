using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class QuestionModelMap:BaseEntityTypeConfiguration<QuestionModel>
    {
        public QuestionModelMap()
        {
            this.ToTable("QuestionModel");

            this.Property(t => t.QuestionType)
                .HasColumnName("QuestionType");

            this.Property(t => t.QuestionContent)
                .HasMaxLength(500)
                .HasColumnName("QuestionContent");

            this.Property(t => t.QuestionOfA)
                .HasMaxLength(200)
                .HasColumnName("QuestionOfA");
            
            this.Property(t => t.MeetId)
                .HasMaxLength(36)
                .HasColumnName("MeetId");
        }
    }
}
