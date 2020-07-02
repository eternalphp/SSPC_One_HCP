using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using log4net;
using MDS.Api;
using MDS.Api.Models;
using Newtonsoft.Json;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Services.MasterData
{
    public class UserPushListener : IPushListener<User>
    {
        private readonly ILog _logger = LogManager.GetLogger("WarnFileLogger");
        private readonly IEfRepository _rep = ContainerManager.Resolve<IEfRepository>();
        private Stopwatch _stopwatch = new Stopwatch();
        public void Dispose()
        {
        }

        public void OnBegin()
        {
            _stopwatch.Reset();
            _stopwatch.Start();
            _rep.Where<UserModel>(s => s.Code != "00000000-0000-0000-0000-000000000000").Delete();
            _rep.SaveChanges();
            _logger.Warn("推送主数据开始【User】**********");
        }

        public void OnReceiveItems(ReceiveItemsOptions<User> options)
        {
            try
            {
                //var codes = options.Datas.Select(s => s.Code).ToList();
                //var hasUser = _rep.Where<UserModel>(s => codes.Contains(s.Code));
                //if (hasUser.Any())
                //{
                //    hasUser.Delete();
                //    _rep.SaveChanges();
                //}
                
                foreach (var item in options.Datas)
                {
                    var userId = item.Id.ToString().ToUpper();
                    UserInfo thisUser = new UserInfo
                    {
                        Id = userId,
                        ADAccount = item?.ADAccount,
                        ChineseName = item?.ChineseName,
                        Code = item?.Code,
                        CompanyEmail = item?.CompanyEmail,
                        EmployeeNo = item?.EmployeeNo,
                        EnglishName = item?.EnglishName,
                        IdCardNumber = item?.IdCardNumber,
                        IsDisabled = item.IsDisabled,
                        MobileNo = item?.MobileNo,
                        OrganizationId = item?.OrganizationId?.ToString()?.ToUpper(),
                        PersonalEmail = item?.PersonalEmail,
                        PositionId = item?.PositionId?.ToString()?.ToUpper(),
                        ReporterId = item?.ReporterId?.ToString()?.ToUpper(),
                        IsDeleted = item.IsDisabled ? 1 : 2,
                        IsEnabled = 0,
                        CreateTime = DateTime.Now,
                        //Password = "123456"
                    };
                    //_rep.Insert(thisUser);
                    MasterDataPush.UserInfos.Add(thisUser);
                }

                if (options.IsEnd)
                {
                    _rep.BulkCopyInsert(MasterDataPush.UserInfos);
                    _rep.SaveChanges();
                }
                
            }
            catch (Exception e)
            {
                _logger.Warn("推送主数据错误 ---开始--- 【User】****ERROR******");
                _logger.Warn(e.Message);
                _logger.Warn(e.StackTrace);
                _logger.Warn(e.Source);
                _logger.Warn("推送主数据错误  ---结束--- 【User】****ERROR******");
            }
        }

        public void OnEnd()
        {
            MasterDataPush.UserInfos.Clear();
            _stopwatch.Stop();
            _logger.Warn($"推送主数据结束【User】{_stopwatch.ElapsedMilliseconds}**********");
        }


    }
}
