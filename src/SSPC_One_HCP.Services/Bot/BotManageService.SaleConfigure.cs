using SSPC_One_HCP.Core.Data;
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
        private readonly IEfRepository _rep;
        public BotManageService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// BOT配置- 新增或修改
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateSaleConfigure(BotSaleConfigure dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                var data = _rep.FirstOrDefault<BotSaleConfigure>(o => o.Id == dto.Id && o.IsDeleted == 0);
                if (data == null)
                {
                    var configure = new BotSaleConfigure()
                    {
                        Id = Guid.NewGuid().ToString(),
                        KBSBotId = dto.KBSBotId,
                        BotName = dto.BotName,
                        AppId = dto.AppId,
                        AppSecret = dto.AppSecret,
                        CreateTime = DateTime.Now,
                        CreateUser = workUser.User.Id,
                    };
                    _rep.Insert<BotSaleConfigure>(configure);
                    rvm.Msg = "success";
                    rvm.Success = true;
                    rvm.Result = data;

                    _rep.SaveChanges();
                }
                else
                {
                    data.KBSBotId = dto.KBSBotId;
                    data.BotName = dto.BotName;
                    data.AppId = dto.AppId;
                    data.AppSecret = dto.AppSecret;
                    data.UpdateTime = DateTime.Now;
                    data.UpdateUser = workUser.User.Id;
                    _rep.Update(data);
                    rvm.Msg = "success";
                    rvm.Success = true;
                    rvm.Result = data;
                    _rep.SaveChanges();
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
        /// BOT配置- 获取配置信息
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetMenusSaleConfigure()
        {
            var model = _rep.FirstOrDefault<BotSaleConfigure>(o => o.IsDeleted == 0);
            ReturnValueModel rvm = new ReturnValueModel
            {
                Msg = "fail",
                Success = false,
                Result = model
            };

            return rvm;
        }
        /// <summary>
        /// BOT配置- 获取配置列表信息
        /// </summary>
        /// <param name="row"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetMenusSaleConfigureList(RowNumModel<BotSaleConfigure> row, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var cnfigures = _rep.Where<BotSaleConfigure>(s => s.IsDeleted != 1);
            if (row.SearchParams != null && row.SearchParams.BotName != null)
            {
                cnfigures = cnfigures.Where(o => o.BotName.Contains(row.SearchParams.BotName));
            }

            var total = cnfigures.Count();
            var rows = cnfigures.OrderBy(o => o.BotName).ToPaginationList(row.PageIndex, row.PageSize);

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
        /// BOT配置- 删除
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel DeleteMenusSaleConfigure(BotSaleConfigure dto, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(dto?.Id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }
            BotSaleConfigure model = _rep.FirstOrDefault<BotSaleConfigure>(s => s.IsDeleted != 1 && s.Id == dto.Id);
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
