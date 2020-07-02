using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.LinqExtented;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SSPC_One_HCP.Services.Bot
{
    public partial class BotManageService : IBotManageService
    {
        //private readonly IEfRepository _rep;
        //public BotManageService(IEfRepository rep)
        //{
        //    _rep = rep;
        //}
        /// <summary>
        /// 勋章业务规则配置-新增或修改
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateMedalBusinessConfigure(BotMedalBusinessConfigure dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                var data = _rep.FirstOrDefault<BotMedalBusinessConfigure>(o => o.Id == dto.Id && o.IsDeleted != 1);

                if (data == null)
                {
                    var configure = new BotMedalBusinessConfigure()
                    {
                        Id = Guid.NewGuid().ToString(),
                        KBSBotId = dto.KBSBotId,
                        KBSBotName = dto.KBSBotName,
                        FaqPackageId = dto.FaqPackageId,
                        FaqPackageName = dto.FaqPackageName,
                        MedalName = dto.MedalName,
                        MedalYSrc = dto.MedalYSrc,
                        MedalNSrc = dto.MedalNSrc,
                        CreateTime = DateTime.Now,
                        CreateUser = workUser.User.Id,
                        Remark = dto.Remark,
                    };
                    _rep.Insert(configure);
                    _rep.SaveChanges();
                    rvm.Msg = "success";
                    rvm.Success = true;
                    rvm.Result = data;
                }
                else
                {
                    data.KBSBotId = dto.KBSBotId;
                    data.KBSBotName = dto.KBSBotName;
                    data.FaqPackageId = dto.FaqPackageId;
                    data.FaqPackageName = dto.FaqPackageName;
                    data.MedalName = dto.MedalName;
                    data.MedalYSrc = dto.MedalYSrc;
                    data.MedalNSrc = dto.MedalNSrc;
                    data.UpdateTime = DateTime.Now;
                    data.UpdateUser = workUser.User.Id;
                    data.Remark = dto.Remark;
                    _rep.Update(data);
                    _rep.SaveChanges();
                    rvm.Msg = "success";
                    rvm.Success = true;
                    rvm.Result = data;
                }
            }
            catch (Exception e)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = e.Message;
            }

            return rvm;
        }
        /// <summary>
        /// 勋章业务规则配置- 根据ID获取配置信息
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetMedalBusinessConfigure(string id)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }
            var model = _rep.FirstOrDefault<BotMedalBusinessConfigure>(o => o.Id == id && o.IsDeleted != 1);
            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = model;
            return rvm;
        }
        /// <summary>
        /// 勋章业务规则配置- 分页查询
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetMedalBusinessConfigureList(RowNumModel<BotMedalBusinessConfigure> row, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var cnfigures = _rep.Where<BotMedalBusinessConfigure>(s => s.IsDeleted != 1);

            if (row.SearchParams.KBSBotId != null)
            {
                cnfigures = cnfigures.Where(o => o.KBSBotId == row.SearchParams.KBSBotId);
            }
            if (row.SearchParams.KBSBotName != null)
            {
                cnfigures = cnfigures.Where(o => o.KBSBotName.Contains(row.SearchParams.KBSBotName));
            }
            if (row.SearchParams.FaqPackageId != null)
            {
                cnfigures = cnfigures.Where(o => o.FaqPackageId == row.SearchParams.FaqPackageId);
            }
            if (row.SearchParams.FaqPackageName != null)
            {
                cnfigures = cnfigures.Where(o => o.FaqPackageName.Contains(row.SearchParams.FaqPackageName));
            }
            if (row.SearchParams.MedalName != null)
            {
                cnfigures = cnfigures.Where(o => o.MedalName.Contains(row.SearchParams.MedalName));
            }

            var total = cnfigures.Count();
            var rows = cnfigures.OrderBy(o => o.KBSBotName).ToPaginationList(row.PageIndex, row.PageSize);

            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                total = total,
                rows = rows,
            };

            return rvm;
        }
        /// <summary>
        /// 勋章业务规则配置- 删除
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel DeleteMedalBusinessConfigure(BotMedalBusinessConfigure dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(dto?.Id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }
            BotMedalBusinessConfigure model = _rep.FirstOrDefault<BotMedalBusinessConfigure>(s => s.IsDeleted != 1 && s.Id == dto.Id);
            if (model == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid Id.";
                return rvm;
            }
            model.IsDeleted = 1;
            model.UpdateTime = DateTime.Now;
            model.UpdateUser = workUser.User.Id;
            _rep.Update(model);
            _rep.SaveChanges();

            rvm.Success = true;
            rvm.Msg = "success";
            return rvm;
        }
    }
}
