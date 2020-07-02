using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.Record;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;

namespace SSPC_One_HCP.Services.Implementations
{
    /// <summary>
    /// 用户访问模块
    /// </summary>
    public class VisitModulesService : IVisitModulesService
    {
        private readonly IEfRepository _rep;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rep"></param>
        public VisitModulesService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddVisitModules(RowNumModel<VisitModulesViewModel> model, WorkUser workUser)
        {
            LoggerHelper.WarnInTimeTest("***********************");
            LoggerHelper.WarnInTimeTest("Inner-[AddVisitModules] Start:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
            ReturnValueModel rvm = new ReturnValueModel();
            int isvisitor = 0;
            if (workUser == null || (string.IsNullOrEmpty(workUser.WxUser.Mobile)))
            {
                isvisitor = 1;
            }

            if (model != null)
            {

                TimeSpan ts = Convert.ToDateTime(model.SearchParams.VisitEnd) -
                                  Convert.ToDateTime(model.SearchParams.VisitStart);

                var seconds = Convert.ToInt32(ts.TotalSeconds);
                //插入数据
                _rep.Insert(new VisitModules()
                {
                    Id = Guid.NewGuid().ToString(),
                    UnionId = workUser?.WxUser.UnionId,
                    IsDeleted = 0,
                    IsEnabled = 0,
                    CreateTime = DateTime.Now,
                    ModuleNo = model.SearchParams.ModuleNo,
                    ModulePageNo = model.SearchParams.ModulePageNo,
                    ModulePageUrl = model.SearchParams.ModulePageUrl,
                    VisitStart = model.SearchParams.VisitStart,
                    VisitEnd = model.SearchParams.VisitEnd,
                    StaySeconds = seconds,  //停留秒数
                    Isvisitor = isvisitor,
                    WxUserid = workUser?.WxUser.Id

                });
                _rep.SaveChanges();

                rvm.Msg = "success";
                rvm.Success = true;
            }
            else
            {
                rvm.Msg = "参数为空或用户对象为空";
                rvm.Success = false;
            }
            LoggerHelper.WarnInTimeTest("Inner-[AddVisitModules] End" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
            return rvm;
        }

    }
}
