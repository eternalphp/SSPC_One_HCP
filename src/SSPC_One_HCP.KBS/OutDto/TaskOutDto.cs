using System;
using System.Collections.Generic;
using System.Text;

namespace SSPC_One_HCP.KBS.OutDto
{
    public class TaskOutDto
    {
        public string Result { get; set; }

        /// <summary>
        /// 流程编号
        /// </summary>
        public string ProcessId { get; set; }
        /// <summary>
        /// 任务编号
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 活动任务编号
        /// </summary>
        public string ActivityId { get; set; }
        /// <summary>
        /// AppId
        /// </summary>
        public string LuisAppId { get; set; }
        /// <summary>
        /// Bot编号
        /// </summary>
        public string BotManageId { get; set; }
        /// <summary>
        /// 知识包编号
        /// </summary>
        public string FaqPackageId { get; set; }
        /// <summary>
        /// 节点类型
        /// </summary>
        public int NodeType { get; set; }
        /// <summary>
        /// 是否新建任务
        /// </summary>
        public bool IsNewTask { get; set; }
        public string FaqId { get; set; }

        public string TaskItemId { get; set; }
        /// <summary>
        /// 推荐FAQ
        /// </summary>
        public List<string> FAQRecommends { get; set; } = new List<string>();
    }
}
