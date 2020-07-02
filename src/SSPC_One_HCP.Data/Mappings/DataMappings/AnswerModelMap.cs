using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class AnswerModelMap:BaseEntityTypeConfiguration<AnswerModel>
    {
        public AnswerModelMap()
        {
            this.ToTable("AnswerModel");

            this.Property(t => t.QuestionId)
                .HasMaxLength(36)
                .HasColumnName("QuestionId");

            this.Property(t => t.AnswerContent)
                .HasMaxLength(500)
                .HasColumnName("AnswerContent");

            this.Property(t => t.Sort)
                .HasMaxLength(11)
                .HasColumnName("Sort");

            this.Property(t => t.IsRight)
                .HasColumnName("IsRight");
        }
    }
}
