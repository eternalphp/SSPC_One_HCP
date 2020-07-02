using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Implementations.Dto
{
    public class MeetSearchInputDto
    {
        /// <summary>
        /// 科室ID
        /// </summary>
        public IEnumerable<string> DepartmentIds { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public IEnumerable<int> Years { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public IEnumerable<int> Months { get; set; }
        /// <summary>
        /// 会议状态
        /// 0、待开播
        /// 1、进行中
        /// 2、已完成
        /// </summary>
        public IEnumerable<int?> MeetStates { get; set; }
    }
}
