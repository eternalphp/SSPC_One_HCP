using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServices
{
    public class MeetingResult
    {
        /// <summary>
        /// 状态（1、成功 -1、失败）
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public string result { get; set; }
    }
    public class MeetingItem
    {
        /// <summary>
        /// 科室会议ID
        /// </summary>
        public string ActivityID { get; set; }

        /// <summary>
        /// 科室会编号
        /// </summary>
        public string MeetingNumber { get; set; }

        /// <summary>
        /// 科室会标题
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// 活动状态1:有效0：无效
        /// </summary>
        public string Stutas { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatTime { get; set; }

        /// <summary>
        /// 召开时间
        /// </summary>
        public DateTime HoldTime { get; set; }

        /// <summary>
        /// 医院
        /// </summary>
        public string Hospital { get; set; }

        /// <summary>
        /// 医院ID
        /// </summary>
        public string HospitalID { get; set; }

        /// <summary>
        /// 科室
        /// </summary>
        public string KeShi { get; set; }

        /// <summary>
        /// 科室ID
        /// </summary>
        public string KeShiID { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// 参与人数
        /// </summary>
        public int PartInNum { get; set; }
    }
}
