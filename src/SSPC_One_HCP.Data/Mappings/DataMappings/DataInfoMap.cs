using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class DataInfoMap:BaseEntityTypeConfiguration<DataInfo>
    {
        public DataInfoMap()
        {
            this.ToTable("DataInfo");

            this.Property(t => t.ProductTypeInfoId)
                .IsRequired()
                .HasColumnName("ProductTypeInfoId");

            this.Property(t => t.Title)
                .HasMaxLength(100)
                .HasColumnName("Title");

            this.Property(t => t.DataContent)
                .HasMaxLength(500)
                .HasColumnName("DataContent");

            this.Property(t => t.DataType)
                .HasMaxLength(36)
                .HasColumnName("DataType");

            this.Property(t => t.DataOrigin)
                .HasColumnName("DataOrigin");

            this.Property(t => t.DataUrl)
                .HasColumnName("DataUrl");

            this.Property(t => t.DataLink)
                .HasColumnName("DataLink");

            this.Property(t => t.IsRead)
                .HasColumnName("IsRead");

            this.Property(t => t.IsSelected)
                .HasColumnName("IsSelected");

            this.Property(t => t.IsCopyRight)
                .HasColumnName("IsCopyRight");

            this.Property(t => t.MediaTime)
                .HasMaxLength(500)
                .HasColumnName("MediaTime");

            this.Property(t => t.MediaType)
                .HasColumnName("MediaType");

            this.Property(t => t.BuName)
                .HasMaxLength(50)
                .HasColumnName("BuName");

            this.Property(t => t.Dept)
                .HasColumnName("Dept");

            this.Property(t => t.Product)
                .HasMaxLength(500)
                .HasColumnName("Product");

            this.Property(t => t.Sort)
                .HasColumnName("Sort");

            this.Property(t => t.IsCompleted)
                .HasColumnName("IsCompleted");

            this.Property(t => t.OldId)
                .HasColumnName("OldId");

            this.Property(t => t.ApprovalNote)
                .HasColumnName("ApprovalNote");

            this.Property(t => t.IsPublic)
                .HasColumnName("IsPublic");

            this.Property(t => t.IsChoiceness)
                .HasColumnName("IsChoiceness");

            this.Property(t => t.ClickVolume)
                .HasColumnName("ClickVolume");

            this.Property(t => t.KnowImageUrl)
                .HasMaxLength(500)
                .HasColumnName("KnowImageUrl");

            this.Property(t => t.KnowImageName)
                .HasMaxLength(500)
                .HasColumnName("KnowImageName");


                
    }
    }
}
