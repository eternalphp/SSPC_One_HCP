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

namespace SSPC_One_HCP.Services.Implementations
{
    public class VisitTimesService : IVisitTimesService
    {
        private readonly IEfRepository _rep;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rep"></param>
        public VisitTimesService(IEfRepository rep)
        {
            _rep = rep;
        }
        public ReturnValueModel AddVisitTimes(RowNumModel<VisitTimesViewModel> model, WorkUser workUser)
        {
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
                _rep.Insert(new VisitTimes()
                {
                    Id = Guid.NewGuid().ToString(),
                    UnionId = workUser?.WxUser.UnionId,
                    WxuserId = workUser?.WxUser.Id,
                    VisitStart = model.SearchParams.VisitStart,
                    VisitEnd = model.SearchParams.VisitEnd,
                    Isvisitor = isvisitor,
                    StaySeconds = ts.Seconds,
                    IsEnabled = 0,
                    IsDeleted = 0

                });
                _rep.SaveChanges();
            }

            rvm.Msg = "success";
            rvm.Success = true;
            return rvm;


        }
    }
}
