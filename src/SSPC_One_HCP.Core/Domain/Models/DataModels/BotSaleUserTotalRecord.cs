using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.DataModels
{

    /// <summary>
    /// 销售 用户访问BOT总数记录
    /// </summary>
    [DataContract]
    public class BotSaleUserTotalRecord : BaseEntity
    {
        
        /// <summary>
        /// 销售用户编号
        /// </summary>
        public string SaleUserId { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }
    }
  
}
