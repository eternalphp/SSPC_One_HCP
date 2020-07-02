using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    [DataContract]
    [NotMapped]
    public class MediaDataRelViewModel : MediaDataRel
    {
        [DataMember]
        public string Title { get; set; }
    }
}
