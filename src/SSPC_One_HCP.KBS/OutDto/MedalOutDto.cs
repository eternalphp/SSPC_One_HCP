using System;
using System.Collections.Generic;
using System.Text;

namespace SSPC_One_HCP.KBS.OutDto
{
    public class MedalOutDto
    {
        public List<MedalInfo> Activateds { get; set; }
        public List<MedalInfo> NotActivateds { get; set; }
    }

    public class MedalInfo
    {
        public string MedalSrc { get; set; }
        public string MedalName { get; set; }
    }
}
