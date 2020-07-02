using MDS.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using log4net;
using MDS.Api;
using Newtonsoft.Json;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Data;
using Position = SSPC_One_HCP.Core.Domain.Models.DataModels.Position;

namespace SSPC_One_HCP.Services.MasterData
{
    /// <summary>
    /// 职位监听
    /// </summary>
    public class PositionPushListener : IPushListener<MDS.Api.Models.Position>
    {
        private readonly ILog _logger = LogManager.GetLogger("WarnFileLogger");
        private readonly IEfRepository _rep = ContainerManager.Resolve<IEfRepository>();
        public void Dispose()
        {
        }

        public void OnBegin()
        {
            _logger.Warn("推送主数据开始【Position】**********");
        }

        public void OnReceiveItems(ReceiveItemsOptions<MDS.Api.Models.Position> options)
        {
            try
            {
                var pIds = options.Datas.Select(s => s.Id.ToString().ToUpper()).ToList();
                var poses = _rep.Where<Position>(s => pIds.Contains(s.Id));
                if (poses.Any())
                {
                    poses.Delete();
                    _rep.SaveChanges();
                }
                foreach (var item in options.Datas)
                {
                    var id = item.Id.ToString().ToUpper();

                    Position thisOrganization = new Position
                    {
                        Id = id,
                        Code = item.Code,
                        IsDisabled = item.IsDisabled,
                        IsDeleted = item.IsDisabled ? 1 : 2,
                        IsEnabled = 0,
                        CreateTime = DateTime.Now,
                        Name = item.Name,
                        OrganizationId = item?.OrganizationId?.ToString()?.ToUpper(),
                        ReporterId = item?.ReporterId?.ToString()?.ToUpper(),
                        HolderId = item?.HolderId?.ToString()?.ToUpper(),

                    };
                    MasterDataPush.Positions.Add(thisOrganization);
                }

                if (options.IsEnd)
                {
                    _rep.BulkCopyInsert(MasterDataPush.Positions);
                    _rep.SaveChanges();
                }
                
            }
            catch (Exception e)
            {
                _logger.Warn("推送主数据错误 ---开始--- 【Position】****ERROR******");
                _logger.Warn(e.Message);
                _logger.Warn(e.StackTrace);
                _logger.Warn(e.Source);
                _logger.Warn("推送主数据错误  ---结束--- 【Position】****ERROR******");
            }
        }

        public void OnEnd()
        {
            MasterDataPush.Positions.Clear();
            _logger.Warn("推送主数据结束【Position】**********");
        }
    }
}
