using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Services.HCP.Dto
{
    public class SeriesCoursesInputDto
    {
        /// <summary>
        /// 系列课程
        /// </summary>
        [DataMember]
        public SeriesCourses Series { get; set; }

        /// <summary>
        /// 系列课程以会议关系
        /// </summary>
        [DataMember]
        public List<SeriesCoursesMeetRel> SeriesCoursesMeetRels { get; set; }
    }
}
