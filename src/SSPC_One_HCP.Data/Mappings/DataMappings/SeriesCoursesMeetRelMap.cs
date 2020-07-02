using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class SeriesCoursesMeetRelMap : BaseEntityTypeConfiguration<SeriesCoursesMeetRel>
    {
        public SeriesCoursesMeetRelMap()
        {
            this.ToTable("SeriesCoursesMeetRel");
            this.Property(t => t.SeriesCoursesId)
                .IsRequired()
                .HasMaxLength(36)
                .HasColumnName("SeriesCoursesId");

            this.Property(t => t.MeetInfoId)
                .IsRequired()
                .HasMaxLength(36)
                .HasColumnName("MeetInfoId");
        }
    }
}
