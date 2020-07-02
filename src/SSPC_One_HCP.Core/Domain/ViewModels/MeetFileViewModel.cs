using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels
{
    [NotMapped]
    [DataContract]
    public class MeetFileViewModel
    {
        [DataMember]
        public MeetFile MeetFileInfo { get; set; }

        [DataMember]
        public IEnumerable<FileInfoModel> FileInfo { get; set; }
    }
}
