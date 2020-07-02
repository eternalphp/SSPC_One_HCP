using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.Models.IdentityEntities
{
    public class RefreshTokenInfo : BaseEntity
    {
        public virtual string AppId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TokenId { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// AppClientId
        /// </summary>
        public string AppClientId { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime IssuedUtc { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime ExpiresUtc { get; set; }

        /// <summary>
        /// 包含凭据
        /// </summary>
        public string ProtectedTicket { get; set; }
    }
}
