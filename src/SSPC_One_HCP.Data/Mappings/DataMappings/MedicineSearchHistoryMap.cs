using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MedicineSearchHistoryMap : BaseEntityTypeConfiguration<MedicineSearchHistory>
    {
        public MedicineSearchHistoryMap()
        {
            this.ToTable("MedicineSearchHistory");

            this.Property(t => t.KeyWord)
                .HasMaxLength(200)
                .HasColumnName("KeyWord");

            this.Property(t => t.Type)
                .HasMaxLength(50)
                .HasColumnName("Type");

            this.Property(t => t.Wxuserid)
                .HasMaxLength(100)
                .HasColumnName("Wxuserid");
        }
    }
}