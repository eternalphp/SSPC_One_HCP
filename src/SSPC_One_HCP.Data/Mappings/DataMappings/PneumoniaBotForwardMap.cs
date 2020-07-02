using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    /// <summary>
    /// 肺炎BOT转发记录
    /// </summary>
    public class PneumoniaBotForwardMap : BaseEntityTypeConfiguration<PneumoniaBotForward>
    {
        public PneumoniaBotForwardMap() {
            this.ToTable("PneumoniaBotForward");

        }
    }
}
