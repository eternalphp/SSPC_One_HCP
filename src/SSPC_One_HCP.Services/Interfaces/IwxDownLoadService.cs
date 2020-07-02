using SSPC_One_HCP.Core.Domain.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Interfaces
{
    /// <summary>
    /// 资料下载加密
    /// </summary>
    public interface IwxDownLoadService
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetDownLoadEncryptUrl(string urlContent);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="urlContent"></param>
        /// <returns></returns>
        ReturnValueModel GetDownLoadDecryptUrl(string urlContent);


        ReturnValueModel GetDecode(string urlContent);

        ReturnValueModel GuidTest();
    }
}
