using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Enums;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.KBS;
using SSPC_One_HCP.Services.Services.WeChat.Dto;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;
using System;
using System.Linq;

namespace SSPC_One_HCP.Services.Services.WeChat.Implementations
{
    public class WcPneumoniaBotService : IWcPneumoniaBotService
    {
        private readonly IEfRepository _rep;
        public WcPneumoniaBotService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        ///  肺炎Bot转发记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ReturnValueModel AddPneumoniaBotForward(PneumoniaBotForward dto)
        {
            ReturnValueModel rvm = new ReturnValueModel();



            var model = new PneumoniaBotForward
            {
                Id = Guid.NewGuid().ToString(),
                UserId = dto.UserId,
                UnionId = dto.UnionId,
                OpenId = dto.OpenId,
                PageName = dto.PageName,
                CreateTime = DateTime.UtcNow.AddHours(8),
            };
            _rep.Insert(model);
            _rep.SaveChanges();

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = "";
            return rvm;
        }


        /// <summary>
        /// 新增 肺炎Bot操作记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ReturnValueModel AddPneumoniaBotOperationRecord(PneumoniaBotOperationRecordInputDto dto)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            PneumoniaBotOperationRecord model = null;
            bool isInsert = true;
            if (dto.ClickTime > 0 && dto.LeaveTime > 0)
            {
                model = _rep.FirstOrDefault<PneumoniaBotOperationRecord>(o =>
                    o.UnionId == dto.UnionId && o.Remark == dto.ClickTime.ToString() && o.ControlId == dto.ControlId);
                isInsert = model == null ? true : false;
            }
            if (model == null)
            {
                model = new PneumoniaBotOperationRecord
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = dto.UserId,
                    UnionId = dto.UnionId,
                    OpenId = dto.OpenId,
                    WxName = dto.WxName,
                    WxPicture = dto.WxPicture,
                    WxGender = dto.WxGender,
                    WxCountry = dto.WxCountry,
                    WxCity = dto.WxCity,
                    WxProvince = dto.WxProvince,
                    ModulesClicked = dto.ModulesClicked,
                    ControlId = dto.ControlId,
                    ControlName = dto.ControlName,
                    ClickTime = DateTime.UtcNow.AddHours(8),
                    CreateTime = DateTime.UtcNow.AddHours(8),
                };
            }

            if (dto.ClickTime > 0 && dto.LeaveTime > 0)
            {
                model.Remark = dto.ClickTime.ToString();
                model.ClickTime = Tool.GetDateTimeMilliseconds(dto.ClickTime.GetValueOrDefault());
                model.LeaveTime = Tool.GetDateTimeMilliseconds(dto.LeaveTime.GetValueOrDefault());

                var time1 = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd hh:mm:ss}", model.ClickTime));
                var time2 = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd hh:mm:ss}", model.LeaveTime));

                TimeSpan clickTime = new TimeSpan(time1.Ticks);
                TimeSpan leaveTime = new TimeSpan(time2.Ticks);
                TimeSpan ts = clickTime.Subtract(leaveTime).Duration();

                string dateDiff;
                if (ts.Days > 0)
                    dateDiff = $"{ts.Days.ToString()}天{ts.Hours.ToString()}小时{ts.Minutes.ToString()}分钟{ts.Seconds.ToString()}秒";
                else if (ts.Hours > 0)
                    dateDiff = $"{ts.Hours.ToString()}小时{ts.Minutes.ToString()}分钟{ts.Seconds.ToString()}秒";
                else if (ts.Minutes > 0)
                    dateDiff = $"{ts.Minutes.ToString()}分钟{ts.Seconds.ToString()}秒";
                else
                    dateDiff = $"{ts.Seconds.ToString()}秒";

                model.ResidenceTime = dateDiff;
                //  model.PlayTime = dateDiff;
            }

            if (string.IsNullOrEmpty(dto.UserId) && string.IsNullOrEmpty(dto.UnionId))
                model.Type = 0;
            else if (!string.IsNullOrEmpty(dto.UserId) && string.IsNullOrEmpty(dto.UnionId))
                model.Type = 1;
            else
                model.Type = 2;

            if (isInsert)
            {
                _rep.Insert(model);
            }
            else
            {
                _rep.Update(model);

            }
            _rep.SaveChanges();

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = "";
            return rvm;
        }

        /// <summary>
        /// 分页查询AI主播知识播报
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetAiBroadcastPageList(RowNumModel<AiBroadcastInputDto> dto)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var list = _rep.Where<DataInfo>(s => s.IsDeleted != 1 && s.MediaType == 3 && s.IsCompleted == EnumComplete.Approved);
            var total = list.Count();
            //先按照Sort字段升序排列，再按照创建时间倒序排列
            var rows = list.OrderBy(s => s.Sort).ThenByDescending(s => s.CreateTime).ToPaginationList(dto.PageIndex, dto.PageSize);
            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total,
                rows
            };

            return rvm;
        }
        /// <summary>
        /// 获取音频媒体详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetAiBroadcast(DataInfo dataInfo)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var detail = _rep.FirstOrDefault<DataInfo>(o => o.Id == dataInfo.Id);
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                detail = detail,
                isCollection = 2
            };

            return rvm;
        }

    }
}