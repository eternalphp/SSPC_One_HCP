using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.GuidModels
{
    /// <summary>
    /// 指南接口返回 结果
    /// </summary>
    public class GuidResult
    {
        public GuidData data { get; set; }
        public bool success { get; set; }
        public string msg { get; set; }
    }
    /// <summary>
    /// 指南数据
    /// </summary>
    public class GuidData
    {
        public int count { get; set; }
        public int rowsPerPage { get; set; }
        public int nowPage { get; set; }
        public GuidItem[] items { get; set; }
    }
    /// <summary>
    /// 指南集合
    /// </summary>
    public class GuidItem
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
