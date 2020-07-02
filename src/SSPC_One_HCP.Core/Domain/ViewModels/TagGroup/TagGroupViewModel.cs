using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Domain.ViewModels.TagGroup
{
    /// <summary>
    /// 标签组视图模型
    /// </summary>
    public class TagGroupViewModel
    {
        /// <summary>
        /// 标签组Id
        /// </summary>
        public string TaggroupId { set; get; }

        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string SearchKeywords { get; set; }

        /// <summary>
        /// 标签组名称
        /// </summary>
        public string TagGroupName { get; set; }
    }
}
