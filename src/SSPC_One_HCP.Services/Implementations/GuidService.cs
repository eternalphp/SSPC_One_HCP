using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.Record;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.GuidModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Utils;

namespace SSPC_One_HCP.Services.Implementations
{
    public  class GuidService
    {
        private readonly IEfRepository _rep;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rep"></param>
        public GuidService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 添加临床指南数据
        /// </summary>
        /// <param name="guiditems"></param>
        public void AddGuidInfo(GuidItem[] guiditems)
        {
            try
            {
                foreach (GuidItem guidItem in guiditems)
                { 
                    _rep.Insert(new GuidVisit()
                    {
                        Id = Guid.NewGuid().ToString(),
                        userid = guidItem.uid,
                        ActionType = guidItem.actionType,
                        GuideId = guidItem.guideId,
                        GuideName = guidItem.guideName,
                        GuideType = guidItem.guideType,
                        CreateTime = DateTime.Now.AddDays(-1),
                        UpdateTime = DateTime.Now,
                        IsEnabled = 0,
                        IsDeleted = 0
                    });
                    _rep.SaveChanges();
                }

            }
            catch (Exception e)
            {
                LoggerHelper.WriteLogInfo("[AddGuidInfo]:"+e.Message);
            }
        }
    }
}
