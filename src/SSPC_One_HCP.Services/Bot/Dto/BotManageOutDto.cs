


using System;


namespace SSPC_One_HCP.Services.Bot.Dto
{
    /// <summary>
    /// bot管理
    /// </summary>	
    public class BotManageOutDto
    {
        /// <summary>
        /// bot名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 男欢迎语编号
        /// </summary>
        public Guid WelcomingIdMan { get; set; }
        /// <summary>
        /// 女欢迎语编号
        /// </summary>
        public Guid WelcomingIdWom { get; set; }
        /// <summary>
        /// 引导语
        /// </summary>
        public string GuideLanguage { get; set; }
        /// <summary>
        /// 超时提醒
        /// </summary>
        public string OverTimeRemind { get; set; }
        /// <summary>
        /// 超时时间（分钟）
        /// </summary>
        public int? OverTime { get; set; }
        /// <summary>
        /// 应用程序ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 发布状态
        /// </summary>
        public int TrainType { get; set; }
        /// <summary>
        /// 推荐
        /// </summary>
        public string Recommend { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationTime { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        public string CreatorId { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }
        /// <summary>
        /// 最后修改用户
        /// </summary>
        public string LastModifierId { get; set; }

    }

}
