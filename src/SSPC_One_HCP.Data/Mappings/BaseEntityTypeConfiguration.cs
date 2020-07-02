using SSPC_One_HCP.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings
{
    public class BaseEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : BaseEntity, new()
    {
        public BaseEntityTypeConfiguration()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(36)
                .HasColumnName("Id");
                //.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.IsDeleted)
                .HasColumnName("IsDeleted")
                .HasColumnAnnotation("Default", -1);

            this.Property(t => t.CompanyCode)
                //.IsRequired()
                .HasMaxLength(50)
                .HasColumnName("CompanyCode");

            this.Property(t => t.CreateTime)
                .HasColumnName("CreateTime");

            this.Property(t => t.CreateUser)
                .HasMaxLength(36)
                .HasColumnName("CreateUser");

            this.Property(t => t.UpdateTime)
                .HasColumnName("UpdateTime");

            this.Property(t => t.UpdateUser)
                .HasMaxLength(36)
                .HasColumnName("UpdateUser");

            this.Property(t => t.IsEnabled)
                .HasColumnName("IsEnabled");

            this.Property(t => t.Remark)
                .HasColumnName("Remark");
        }
    }
}
