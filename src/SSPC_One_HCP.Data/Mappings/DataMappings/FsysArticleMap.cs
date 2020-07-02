using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class FsysArticleMap : BaseEntityTypeConfiguration<FsysArticle>
    {
        public FsysArticleMap()
        {
            this.ToTable("FsysArticle");

            this.Property(t => t.DepartmentId)
                .HasColumnName("DepartmentId");

            this.Property(t => t.ArticleTitle)
                .HasMaxLength(200)
                .HasColumnName("ArticleTitle");

            this.Property(t => t.ArticleUrl)
                .HasMaxLength(256)
                .HasColumnName("ArticleUrl");

            this.Property(t => t.ArticleSort)
                .HasColumnName("ArticleSort");

            this.Property(t => t.ArticleIsHot)
                .HasColumnName("ArticleIsHot");

            this.Property(t => t.ArticleSource)
                .HasMaxLength(200)
                .HasColumnName("ArticleSource");

            this.Property(t => t.PublishedDate)
                .HasColumnName("PublishedDate");
        }
    }
}
