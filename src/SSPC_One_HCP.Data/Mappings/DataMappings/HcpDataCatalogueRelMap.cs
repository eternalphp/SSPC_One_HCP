using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    /// <summary>
    /// 资料与目录表映射
    /// </summary>
    public class HcpDataCatalogueRelMap : BaseEntityTypeConfiguration<HcpDataCatalogueRel>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HcpDataCatalogueRelMap()
        {
            // 设置表名
            this.ToTable("HcpDataCatalogueRel");
        }
    }
}
