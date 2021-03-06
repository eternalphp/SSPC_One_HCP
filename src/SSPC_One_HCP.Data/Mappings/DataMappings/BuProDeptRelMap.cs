﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class BuProDeptRelMap : BaseEntityTypeConfiguration<BuProDeptRel>
    {
        public BuProDeptRelMap()
        {
            this.ToTable("BuProDeptRel");

            this.Property(t => t.BuName)
                .HasMaxLength(50)
                .HasColumnName("BuName");

            this.Property(t => t.ProId)
                .HasMaxLength(36)
                .HasColumnName("ProId");

            this.Property(t => t.DeptId)
                .HasMaxLength(36)
                .HasColumnName("DeptId");
        }
    }
}
