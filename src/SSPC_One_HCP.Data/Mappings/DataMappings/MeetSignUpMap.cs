using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MeetSignUpMap:BaseEntityTypeConfiguration<MeetSignUp>
    {
        public MeetSignUpMap()
        {
            this.Property(t => t.MeetId)
                .HasMaxLength(36)
                .HasColumnName("MeetId");

            this.Property(t => t.SignUpUserId)
                .HasMaxLength(36)
                .HasColumnName("SignUpUserId");

            this.Property(t => t.IsSignIn)
                .HasColumnName("IsSignIn");

            this.Property(t => t.SignInTime)
                .HasColumnName("SignInTime");

            this.Property(t => t.IsKnewDetail)
                .HasColumnName("IsKnewDetail");

            this.ToTable("MeetSignUp");
        }
    }
}
