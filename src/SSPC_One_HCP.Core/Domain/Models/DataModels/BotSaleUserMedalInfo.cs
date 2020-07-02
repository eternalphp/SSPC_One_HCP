using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{

    /// <summary>
    /// 销售 用户勋章
    /// </summary>
    [DataContract]
    public class BotSaleUserMedalInfo : BaseEntity
    {
        /// <summary>
        /// 勋章规则配置主键
        /// </summary>
        public string BotMedalRuleId { get; set; }
        /// <summary>
        /// 销售用户编号
        /// </summary>
        public string SaleUserId { get; set; }
        /// <summary>
        /// 勋章图片地址
        /// </summary>
        public string MedalSrc { get; set; }
        /// <summary>
        /// 勋章类型
        /// </summary>
        public int MedalType { get; set; }
        /// <summary>
        /// 勋章名称
        /// </summary>
        public string MedalName { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum SaleUserMedalType
    {
        /// <summary>
        /// 次数
        /// </summary>
        Number = 0,
        /// <summary>
        /// 知识包
        /// </summary>
        Pack = 1,
    }
}
