using System.Threading.Tasks;

namespace SSPC_One_HCP.KBS
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class Result 
    {

        /// <summary>
        /// 
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TraceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object Data { get; set; }

    }

    /// <summary>
    /// AD验证登录
    /// </summary>
    public class AdResult {
        public int Error_code { get; set; }
       
        public object Data { get; set; }
    }

}
