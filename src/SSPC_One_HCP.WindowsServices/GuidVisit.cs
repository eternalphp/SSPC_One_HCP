using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServices
{

    /// <summary>
    /// 指南接口返回 结果
    /// </summary>
    public class GuidVisitResult
    {
        public GuidVisitData data { get; set; }
        public bool success { get; set; }
        public string msg { get; set; }
    }
    /// <summary>
    /// 指南数据
    /// </summary>
    public class GuidVisitData
    {
        public int count { get; set; }
        public int rowsPerPage { get; set; }
        public int nowPage { get; set; }
        public GuidVisitItem[] items { get; set; }
    }
    /// <summary>
    /// 指南集合
    /// </summary>
    public class GuidVisitItem
    {
        public string uid { get; set; }
        public string actionType { get; set; }
        public int guideId { get; set; }
        public string guideType { get; set; }
        public string guideName { get; set; }
        public string createTime { get; set; }
        public string email { get; set; }
        public string keyword { get; set; }
        /// <summary>
        /// 停留时间
        /// </summary>
        public int stayTime { get; set; }
    }
}
