using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class RegisterModelMap:BaseEntityTypeConfiguration<RegisterModel>
    {
        public RegisterModelMap()
        {
            this.ToTable("RegisterModel");

            this.Property(t => t.UnionId)
                .HasMaxLength(50)
                .HasColumnName("UnionId");

            this.Property(t => t.SignUpName)
                .HasColumnName("SignUpName");

            this.Property(t => t.WxUserId)
                .HasMaxLength(36)
                .HasColumnName("WxUserId");
        }
    }
}
