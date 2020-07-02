using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Implementations
{
    public class SpreadQRCodeService : IBaseService<SpreadQRCode>
    {

        private readonly IEfRepository _rep;

        public SpreadQRCodeService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 获取列表 支持分页
        /// </summary>
        /// <param name="item"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetList( RowNumModel<SpreadQRCode> item, WorkUser workUser)
        {
            var list = _rep.Where<SpreadQRCode>(x => x.IsDeleted != 1 && x.SpreadQRType == item.SearchParams.SpreadQRType);
            if (!string.IsNullOrEmpty(item?.SearchParams?.SpreadAppId))
            {
                list = list.Where<SpreadQRCode>(x => x.SpreadAppId == item.SearchParams.SpreadAppId);
            }
            if (!string.IsNullOrEmpty(item?.SearchParams?.SpreadName))
            {
                list = list.Where<SpreadQRCode>(x => x.SpreadName.Contains(item.SearchParams.SpreadName));
            }

          
            var total = list.Count();
            var rows = list.OrderByDescending(x => x.CreateTime).ToPaginationList(item?.PageIndex, item?.PageSize).ToList();

            if (item.SearchParams.SpreadQRType == 1)
            {
                rows = rows.Select(x => new SpreadQRCode()
                {
                    Id = x.Id,
                    SpreadAppId = x.SpreadAppId,
                    SpreadName = x.SpreadName,
                    SpreadQRCodeUrl = x.SpreadQRCodeUrl,
                    RegisteredCount = _rep.Where<WxUserModel>(z => z.SourceAppId.Equals(x.SpreadAppId) && z.SourceType.Equals("4") && z.IsCompleteRegister == 1).Count(),
                    VisitorsCount = _rep.Where<QRcodeRecord>(z => z.AppId.Equals(x.SpreadAppId) && z.SourceType.Equals("4")).Count(),
                    CreateTime = x.CreateTime,
                }).ToList();
            }
            var result = new ReturnValueModel()
            {
                Msg = "Success",
                Success = true,
                Result = new
                {
                    total,
                    rows
                }
            };
            return result;
        }

        /// <summary>
        /// 详细信息
        /// </summary>
        /// <param name="item"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetItem(SpreadQRCode item, WorkUser workUser)
        {
            var list = _rep.FirstOrDefault<SpreadQRCode>(x => x.IsDeleted != 1 && x.SpreadQRType == item.SpreadQRType);
            var result = new ReturnValueModel()
            {
                Msg = "Success",
                Success = true,
                Result = new
                {
                    list
                }
            };
            return result;
            throw new NotImplementedException();
        }

        /// <summary>
        /// 增改
        /// </summary>
        /// <param name="data"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateItem(SpreadQRCode data, WorkUser workUser)
        {
            var item = _rep.FirstOrDefault<SpreadQRCode>(x => x.IsDeleted != 1 && x.Id == data.Id);
            if (item == null)
            {
                data.Id = Guid.NewGuid().ToString();
                data.CreateTime = DateTime.Now;
                data.CreateUser = workUser?.User?.Id ?? "";
                _rep.Insert(data);
            }
            else
            {
                item.SpreadAppId = data.SpreadAppId;
                item.SpreadName = data.SpreadName;
                item.SpreadQRCodeUrl = data.SpreadQRCodeUrl;
                item.UpdateTime = DateTime.Now;
                item.CreateUser = workUser?.User?.Id ?? "";
                _rep.Update(item);
            }
            var val = _rep.SaveChanges() > 0 ? true : false;
            var result = new ReturnValueModel() { Msg = "Success", Success = val, Result = data };
            return result;
        }

        /// <summary>
        /// 伪删除
        /// </summary>
        /// <param name="data"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel DeleteItem(SpreadQRCode data, WorkUser workUser)
        {
            var result = new ReturnValueModel() { };
            var item = _rep.FirstOrDefault<SpreadQRCode>(x => x.IsDeleted != 1  && x.Id == data.Id);
            if (item != null)
            {
                item.IsDeleted = 1;
                item.UpdateUser= workUser?.User?.Id ?? "";
                item.UpdateTime = DateTime.Now;
                _rep.Update(item);
                _rep.SaveChanges();
                result.Success = true;
                result.Msg = "Success";
                result.Result = new { item };

            }
            else
            {
                result.Success = false;
                result.Msg = "Invalid Id.";
            }
            return result;
           
        }

        /// <summary>
        /// 访问记录数+1
        /// </summary>
        /// <param name="data"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddItem(SpreadQRCode data, WorkUser workUser)
        {
            var result = new ReturnValueModel() { };
            var item = _rep.FirstOrDefault<SpreadQRCode>(x => x.IsDeleted != 1 && x.Id == data.Id);
            if (item != null)
            {
                item.VisitorsCount += 1;
                _rep.Update(item);
                _rep.SaveChanges();
                result.Success = true;
                result.Msg = "Success";
                result.Result = new { item };

            }
            else
            {
                result.Success = false;
                result.Msg = "Invalid Id.";
            }
            return result;

        }
    }
}
