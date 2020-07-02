using System;
using System.Linq;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Interfaces;

namespace SSPC_One_HCP.Services.Implementations
{
    public class WxLearnService : IWxLearnService
    {
        private readonly IEfRepository _rep;

        public WxLearnService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// 记录学习时间
        /// </summary>
        /// <param name="myLRecord">学习记录信息</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddLearn(MyLRecord myLRecord,WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            int RemindOffsetMinutes = 0;
            try
            {
                /*
                 * 播客：传入学习时长、开始时间
                 * 其他：开始和结束时间
                 */
                if (myLRecord != null)
                {
                    myLRecord.Id = Guid.NewGuid().ToString();
                    myLRecord.LDate = DateTime.Now;
                    myLRecord.UnionId = workUser.WxUser.UnionId;
                    myLRecord.CreateTime = DateTime.Now;
                    myLRecord.LObjectDate = myLRecord.LObjectDate ?? 0;
                    myLRecord.WxUserId = workUser.WxUser.Id;
                    switch (myLRecord.LObjectType)
                    {
                        case 1://文章
                        case 2://文档
                            myLRecord.LObjectDate =(int)(myLRecord.LDateEnd - myLRecord.LDateStart)?.TotalSeconds;
                            _rep.Insert(myLRecord);
                            _rep.SaveChanges();
                            break;
                        case 3://播客
                            _rep.Insert(myLRecord);
                            _rep.SaveChanges();
                            break;
                        case 4://视频
                            myLRecord.LObjectDate = (int)(myLRecord.LDateEnd - myLRecord.LDateStart)?.TotalSeconds;
                            _rep.Insert(myLRecord);
                            _rep.SaveChanges();
                            break;
                        case 5://会议
                            var meet = _rep.FirstOrDefault<MeetInfo>(s => s.Id == myLRecord.LObjectId);
                            //if (meet.MeetEndTime > myLRecord.LDate)
                            //{
                            myLRecord.LObjectDate = (int)(myLRecord.LDateEnd - myLRecord.LDateStart)?.TotalSeconds;

                            _rep.Insert(myLRecord);
                            _rep.SaveChanges();
                            //}
                            break;
                        case 9:
                           
                            _rep.Insert(myLRecord);
                            _rep.SaveChanges();
                            break;
                        default:
                            break;
                    }

                }

                /*
                 * 知识库打开过后，点击量就增加 
                 */
                if (myLRecord != null)
                {
                    string lObjectId = myLRecord.LObjectId;

                    DataInfo datainfo = _rep.Table<DataInfo>().Where(a => a.Id == lObjectId).FirstOrDefault();
                    if (datainfo != null)
                    {
                        datainfo.ClickVolume = datainfo.ClickVolume+1;
                        _rep.Update(datainfo);
                        _rep.SaveChanges();
                    }
                   
                }

                rvm.Msg = "success";
                rvm.Success = true; 
                return rvm;
            }
            catch (Exception ex)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                return rvm;
            }
        }

        /// <summary>
        /// 批量插入数据测试
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="NowDate">开始日期</param>
        /// <returns></returns>
        public ReturnValueModel AddLearnForTest(string userid,DateTime NowDate)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            DateTime currenTime=DateTime.Now;
            while (NowDate<= currenTime)
            {
                _rep.Insert(new MyLRecord()
                {
                    Id = Guid.NewGuid().ToString(),
                    UnionId = "oXfEK6MYG6s0WFD6NzAdP7I472TE",
                    LObjectId = "ccf58b81-4f64-4c7f-baf8-e4edb52a8e62",
                    LDate = NowDate,
                    CreateTime = NowDate,
                    IsDeleted = 0,
                    IsEnabled = 0,
                    LDateStart = NowDate,
                    LDateEnd = NowDate,
                    WxUserId = userid
                });
                _rep.SaveChanges();
                NowDate = NowDate.AddDays(1);
            }
         
            rvm.Success = false;
            return rvm;
        }
    }
}