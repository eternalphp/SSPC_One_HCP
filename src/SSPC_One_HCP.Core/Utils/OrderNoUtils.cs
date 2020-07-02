using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Utils
{
    /// <summary>
    /// 编号辅助类
    /// </summary>
    public static class OrderNoUtils
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        /// <param name="text">编号中的文本</param>
        /// <param name="type">编号类型</param>
        /// <returns></returns>
        public static string GetOrderNumber(string text, int? type = null)
        {
            var orderNo = "";
            if (type == null)
            {
                orderNo = DateTime.Now.ToString("yyyyMMddhhmmsss")+ text;
            }

            return orderNo;
        }
    }
}
