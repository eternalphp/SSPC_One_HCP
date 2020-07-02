using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.MeetModels
{
    /// <summary>
    /// 学习概览
    /// </summary>
    [DataContract]
    public class MeetStudyViewModel
    {
        public string Title { get; set; }
    }
}
