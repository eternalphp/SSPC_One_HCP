using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.IdentityEntities
{
    public class AppClientInfo : BaseEntity
    {

        public virtual string AppName { get; set; }
        /// <summary>
        /// 凭据加密信息
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 应用类型
        /// </summary>
        public int ApplicationType { get; set; }
        /// <summary>
        /// 授权Client名称
        /// </summary>
        public string AppClientName { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// 刷新token周期（单位秒）
        /// </summary>
        public int RefreshTokenLifeTime { get; set; }

        /// <summary>
        /// 跨越允许的Origin
        /// </summary>
        public string AllowedOrigin { get; set; }
    }
}
