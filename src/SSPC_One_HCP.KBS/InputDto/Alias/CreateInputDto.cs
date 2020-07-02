using System;
using System.Collections.Generic;
using System.Text;

namespace SSPC_One_HCP.KBS.InputDto.Alias
{
    public class CreateInputDto
    {
        /// <summary>
        /// 标准名
        /// </summary>    
        public string StandardName { get; set; }
        public List<AliasItemsInputDto> AliasItems { get; set; }
    }
    public class AliasItemsInputDto
    {
        public string AliasName { get; set; }
    }

}
