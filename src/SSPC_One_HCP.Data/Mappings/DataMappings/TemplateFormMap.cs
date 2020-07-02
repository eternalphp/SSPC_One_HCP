using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class TemplateFormMap : BaseEntityTypeConfiguration<TemplateForm>
    {
        public TemplateFormMap()
        {
            this.ToTable("TemplateForm");

            this.Property(t => t.FormID)
                .HasMaxLength(36)
                .HasColumnName("FormID");

            this.Property(t => t.OpenID)
                .HasMaxLength(36)
                .HasColumnName("OpenID");

            this.Property(t => t.Page)
                .HasMaxLength(128)
                .HasColumnName("Page");

            this.Property(t => t.SendTime)
                .HasColumnName("SendTime");
            this.Property(t => t.MsgID)
                .HasColumnName("MsgID");
        }
    }
}
