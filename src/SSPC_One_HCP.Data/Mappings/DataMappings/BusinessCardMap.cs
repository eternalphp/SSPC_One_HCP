using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class BusinessCardMap : BaseEntityTypeConfiguration<BusinessCard>
    {
        public BusinessCardMap()
        {
            this.Property(t => t.WxUserId)
                .HasMaxLength(50)
                .HasColumnName("WxUserId");

            this.Property(t => t.OwnerWxUserId)
                .HasMaxLength(50)
                .HasColumnName("OwnerWxUserId");

            this.ToTable("BusinessCard");

        }
    }
}
