using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SSPC_One_HCP.KBS.InputDto
{
    public class TaskInputDto
    {
        /// <summary>
        /// BOT编号
        /// </summary>
        [DataMember]
        public string BotManageId { get; set; }
        /// <summary>
        /// 用户输入
        /// </summary>
        [DataMember]
        public string UserInput { get; set; }
        /// <summary>
        /// 流程编号
        /// </summary>
        [DataMember]
        public string ProcessId { get; set; }
        /// <summary>
        /// 任务编号
        /// </summary>
        [DataMember]
        public string TaskId { get; set; }
        /// <summary>
        /// 活动任务编号
        /// </summary>
        [DataMember]
        public string ActivityId { get; set; }
        /// <summary>
        /// 知识包ID
        /// </summary>
        [DataMember]
        public string FaqPackageId { get; set; }
        /// <summary>
        /// LuisAppId
        /// </summary>
        [DataMember]
        public string LuisAppId { get; set; }
        [DataMember]
        public string FaqId { get; set; }

        public string Sign { get; set; }
        public string UserId { get; set; }
    }
}
