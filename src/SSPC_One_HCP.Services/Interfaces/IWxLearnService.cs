using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;

namespace SSPC_One_HCP.Services.Interfaces
{
    public interface IWxLearnService
    {
        /// <summary>
        /// 记录学习时间
        /// </summary>
        /// <param name="myLRecord">学习记录信息</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel AddLearn(MyLRecord myLRecord, WorkUser workUser);

        ReturnValueModel AddLearnForTest(string userid, DateTime NowDate);
    }
}