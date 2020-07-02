using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using SSPC_One_HCP.Core.Domain.Models.DataModels;


namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
   
   public class MyFixedDateMap: BaseEntityTypeConfiguration<MyFixedDate>
    {
        public MyFixedDateMap() {
            this.ToTable("MyFixedDate");

            this.Property(t => t.Sort)
                .HasColumnName("Sort");

            this.Property(t => t.Type)
                .HasColumnName("Type");

            this.Property(t => t.Text)
                .HasColumnName("Text");

            
        }
       
    }
}
