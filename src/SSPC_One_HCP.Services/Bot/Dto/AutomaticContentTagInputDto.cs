using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Bot.Dto
{
    public class AutomaticContentTagInputDto
    {
        public string ContentId { get; set; }
        public string Content { get; set; }
    }
    public class DeleteAutomaticContentTagInputDto
    {
        public string ContentId { get; set; }
        public string TagName { get; set; }
    }
}
