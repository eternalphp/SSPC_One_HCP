using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class WordBlackListMap : BaseEntityTypeConfiguration<WordBlackList>
    {
        public WordBlackListMap()
        {
            ToTable("WordBlackList");

            this.Property(t => t.Words)
                .HasColumnName("Words");

            this.Property(t => t.Type)
                .HasMaxLength(50)
                .HasColumnName("Type");
        }
    }
}
