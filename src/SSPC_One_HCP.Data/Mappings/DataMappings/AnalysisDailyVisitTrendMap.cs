using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    /// <summary>
    /// 获取用户访问小程序数据日趋势
    /// </summary>
    public class AnalysisDailyVisitTrendMap : BaseEntityTypeConfiguration<AnalysisDailyVisitTrend>
    {
        public AnalysisDailyVisitTrendMap()
        {
            this.ToTable("AnalysisDailyVisitTrend");

            this.Property(t => t.ref_date)
                .HasMaxLength(100)
                .HasColumnName("ref_date");

            this.Property(t => t.session_cnt) 
                .HasColumnName("session_cnt");

            this.Property(t => t.visit_pv) 
                .HasColumnName("visit_pv");

            this.Property(t => t.visit_uv) 
                .HasColumnName("visit_uv");

            this.Property(t => t.visit_uv_new) 
                .HasColumnName("visit_uv_new");

            this.Property(t => t.stay_time_uv) 
                .HasColumnName("stay_time_uv");

            this.Property(t => t.stay_time_session) 
                .HasColumnName("stay_time_session");

            this.Property(t => t.visit_depth) 
                .HasColumnName("visit_depth");

            
        }
    }
}
