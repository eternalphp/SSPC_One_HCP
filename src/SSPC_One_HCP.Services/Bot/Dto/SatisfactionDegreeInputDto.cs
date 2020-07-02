using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Bot.Dto
{
    public class SatisfactionDegreeInputDto
    {
        /// <summary>
        /// 活动任务编号
        /// </summary>
        [DataMember]
        public string TaskItemId { get; set; }
        /// <summary>
        /// 状态
        /// Y：满意
        /// N：不满意
        /// </summary>
        [DataMember]
        public string State { get; set; }
    }
}
