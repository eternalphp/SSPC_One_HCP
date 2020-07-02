using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class LiveOnlineMap : BaseEntityTypeConfiguration<LiveOnline>
    {
        public LiveOnlineMap()
        {
            this.ToTable("LiveOnline");

        }
    }
}
