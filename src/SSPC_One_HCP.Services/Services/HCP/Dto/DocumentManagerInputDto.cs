using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels.ProductModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SSPC_One_HCP.Services.Services.HCP.Dto
{
    public class DocumentManagerInputDto
    {
        /// <summary>
        /// 知识库信息
        /// </summary>
        [DataMember]
        public DocumentManager dataInfo { get; set; }

        /// <summary>
        /// 知识库对应的产品及科室
        /// </summary>

        [DataMember]
        public DocumentManagerBuDeptSelectionViewModel ProductAndDeps { get; set; }
    }

    /// <summary>
    /// BU-产品-科室下拉框视图模型
    /// </summary>
    [DataContract]
    [NotMapped]
    public class DocumentManagerBuDeptSelectionViewModel
    {
        /// <summary>
        /// BU名称列表
        /// </summary>
        [DataMember]
        public IEnumerable<string> BuNameList { get; set; }

        /// <summary>
        /// 目录名称列表
        /// </summary>
        [DataMember]
        public IEnumerable<string> CatalogueNameList { get; set; }

        /// <summary>
        /// 产品列表(去重复)
        /// </summary>
        [DataMember]
        public IEnumerable<ProductSelectionViewModel> Products { get; set; }

        /// <summary>
        /// 科室列表(去重复)
        /// </summary>
        [DataMember]
        public IEnumerable<DepartmentSelectionViewModel> Departments { get; set; }

    }
}
