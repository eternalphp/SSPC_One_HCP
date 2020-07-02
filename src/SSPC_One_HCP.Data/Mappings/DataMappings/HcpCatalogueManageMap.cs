using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    /// <summary>
    /// BU与目录表映射
    /// </summary>
    public class HcpCatalogueManageMap : BaseEntityTypeConfiguration<HcpCatalogueManage>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HcpCatalogueManageMap()
        {
            // 设置表名
            this.ToTable("HcpCatalogueManage");
        }
    }
}
