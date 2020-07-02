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
using Organization = SSPC_One_HCP.Core.Domain.Models.DataModels.Organization;

namespace SSPC_One_HCP.Services.MasterData
{
    /// <summary>
    /// 部门监听
    /// </summary>
    public class OrganizationPushListener : IPushListener<MDS.Api.Models.Organization>
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
            _rep.All<Organization>().Delete();
            _rep.SaveChanges();
            _logger.Warn("推送主数据开始【Organization】**********");
        }

        public void OnReceiveItems(ReceiveItemsOptions<MDS.Api.Models.Organization> options)
        {
            try
            {

                foreach (var item in options.Datas)
                {
                    var id = item.Id.ToString().ToUpper();

                    Organization thisOrganization = new Organization
                    {
                        Id = id,
                        Code = item.Code,
                        IsDisabled = item.IsDisabled,
                        IsDeleted = item.IsDisabled ? 1 : 2,
                        IsEnabled = 0,
                        CreateTime = DateTime.Now,
                        Level = item.Level ?? 0,
                        Name = item.Name,
                        ManagerId = item?.ManagerId?.ToString()?.ToUpper(),
                        ParentId = item?.ParentId?.ToString()?.ToUpper(),
                        Path = item.Path
                    };
                    MasterDataPush.Organizations.Add(thisOrganization);
                }
                _logger.Warn($"推送主数据ING【Organization】{MasterDataPush.Organizations.Count}**********");
                _logger.Warn($"推送主数据ING【Organization】{options.Total}**********");
                if (options.IsEnd)
                {
                    _rep.BulkCopyInsert(MasterDataPush.Organizations);

                }
            }
            catch (Exception e)
            {
                _logger.Warn("推送主数据错误 ---开始--- 【Organization】****ERROR******");
                _logger.Warn(e.Message);
                _logger.Warn(e.StackTrace);
                _logger.Warn(e.Source);
                _logger.Warn("推送主数据错误  ---结束--- 【Organization】****ERROR******");
            }

        }

        public void OnEnd()
        {
            /*
             *  BUBM 广阔市场事业部        43352C28-BFCB-4885-B4D5-7AF8DA5968EB
             *  BUN  肾科业务部            958BA0E4-9BE5-42AB-BB9F-C4357BCF98BC
             *  BUS  专业医疗部            620C91CC-724E-477D-B48E-3ECE1F3A9D16
             *  BUT  输血事业部            8B3DCA0E-57A9-471E-A75C-C5491C693826
             *  BUAS 麻醉/手术业务部       2D55347E-1B17-4D35-A7A5-DF1F35FF62C3
             *  BUI  输注事业部            145D8E7E-9B7B-4F39-AFC8-F74F51231289
             */

            _rep.Where<Organization>(s =>
                s.Path.Contains("43352C28-BFCB-4885-B4D5-7AF8DA5968EB") ||
                s.Id == "43352C28-BFCB-4885-B4D5-7AF8DA5968EB").Update(s => new Organization
                {
                    Remark = "BUBM"
                });
            _rep.Where<Organization>(s =>
                s.Path.Contains("958BA0E4-9BE5-42AB-BB9F-C4357BCF98BC") ||
                s.Id == "958BA0E4-9BE5-42AB-BB9F-C4357BCF98BC").Update(s => new Organization
                {
                    Remark = "BUN"
                });
            _rep.Where<Organization>(s =>
                s.Path.Contains("620C91CC-724E-477D-B48E-3ECE1F3A9D16") ||
                s.Id == "620C91CC-724E-477D-B48E-3ECE1F3A9D16").Update(s => new Organization
                {
                    Remark = "BUS"
                });
            _rep.Where<Organization>(s =>
                s.Path.Contains("8B3DCA0E-57A9-471E-A75C-C5491C693826") ||
                s.Id == "8B3DCA0E-57A9-471E-A75C-C5491C693826").Update(s => new Organization
                {
                    Remark = "BUT"
                });
            _rep.Where<Organization>(s =>
                s.Path.Contains("2D55347E-1B17-4D35-A7A5-DF1F35FF62C3") ||
                s.Id == "2D55347E-1B17-4D35-A7A5-DF1F35FF62C3").Update(s => new Organization
                {
                    Remark = "BUAS"
                });
            _rep.Where<Organization>(s =>
                s.Path.Contains("145D8E7E-9B7B-4F39-AFC8-F74F51231289") ||
                s.Id == "145D8E7E-9B7B-4F39-AFC8-F74F51231289").Update(s => new Organization
                {
                    Remark = "BUI"
                });
            _stopwatch.Stop();
            MasterDataPush.Organizations.Clear();
            _logger.Warn($"推送主数据结束【Organization】{_stopwatch.ElapsedMilliseconds}**********");
        }
    }
}
