using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Services.Utils;
using System.Configuration;

namespace SSPC_One_HCP.Services.Implementations
{
    public class WechatActionHistoryService : IWechatActionHistoryService
    {
        private readonly IEfRepository _rep;
        private string interfaceKeywords = ConfigurationManager.AppSettings["InterfaceKeywords"];
        public WechatActionHistoryService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 新增用户行为数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddActionHistory(RowNumModel<WechatActionHistory> model, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (model == null)
            {
                rvm.Msg = "the model is null";
                rvm.Success = false;
            }
            else
            {
                try
                {
                    _rep.Insert(new WechatActionHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ActionType = model.SearchParams.ActionType,
                        Content=model.SearchParams.Content,
                        ContentId = model.SearchParams.ContentId,
                        UnionId = workUser?.WxUser?.UnionId??"",
                        WxuserId=workUser?.WxUser?.Id??"",
                        IsDeleted=0,
                        IsEnabled = 0,
                        StaySeconds = model.SearchParams.StaySeconds??0,
                        CreateTime = DateTime.Now
                    });
                    _rep.SaveChanges();
                    rvm.Msg = "";
                    rvm.Success = true;
                }
                catch (Exception e)
                {
                    rvm.Msg = e.Message;
                    rvm.Success = false;
                    LoggerHelper.WriteLogInfo("[AddActionHistory Error:]"+e.Message);
                }
            }

            return rvm;
        }

        /// <summary>
        /// 获取期刊列表 
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetMagaZineList()
        {
            //从Webconfig中获取热搜期刊
            ReturnValueModel rvm = new ReturnValueModel();
            List<string> MagaZineList = new List<string>();
            var SendMaga = _rep.SqlQuery<ThirdPartyKeyWord>("select * from ThirdPartyKeyWord where IsDeleted=0 and KeyWordType=2").OrderByDescending(s=>s.CreateTime).FirstOrDefault();
            string SendText = SendMaga.KeyWordContent;
            try
            {
                MagaZineList = SendText.Split(',').ToList();                
                rvm.Success = true;
                rvm.Msg = "";
            }
            catch (Exception e)
            {
                rvm.Success = false;
                rvm.Msg = e.Message;
            }
            
            rvm.Result = new
            {
                rows = MagaZineList
            };
            return rvm;
        }
       
    }
}
