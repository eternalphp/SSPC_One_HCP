using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class SeriesCoursesMap : BaseEntityTypeConfiguration<SeriesCourses>
    {
        public SeriesCoursesMap()
        {
            this.ToTable("SeriesCourses");
            this.Property(t => t.CourseTitle)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("CourseTitle");

            this.Property(t => t.Speaker)
             .HasMaxLength(500)
             .HasColumnName("Speaker");

            this.Property(t => t.Hospital)
          .HasMaxLength(500)
          .HasColumnName("Hospital");

        }
    }
}
