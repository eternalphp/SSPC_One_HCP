using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.WxMedicine;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;

namespace SSPC_One_HCP.Services.Implementations
{
    public class WxMedicineService : IWxMedicineService
    {
        private readonly IEfRepository _rep;
        private string interfaceKeywords = ConfigurationManager.AppSettings["InterfaceKeywords"];
        public WxMedicineService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 新增热搜和个人搜索
        /// </summary>
        /// <param name="model"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>MedicineHotSearch
        public ReturnValueModel AddHotSearch(RowNumModel<MedicineHotSearchViewModel> model, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (model == null)
            {
                rvm.Success = false;
                rvm.Msg = "the MedicineHotSearch is null ";
            }
            else
            {
                try
                {
                    //新增热搜记录 热搜前判断是否存在
                    _rep.Insert(new MedicineHotSearch()
                    {
                        KeyWord = model.SearchParams.KeyWord,
                        Type = model.SearchParams.Type,
                        Id = Guid.NewGuid().ToString(),
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        IsEnabled = 0,
                        IsDeleted = 0
                    });
                    //个人搜索记录新增 若存在更新时间
                    var Wxuserid = workUser?.WxUser?.Id ?? "";
                    var Mol = _rep.Where<MedicineSearchHistory>(s =>
                        s.KeyWord == model.SearchParams.KeyWord && s.Wxuserid == Wxuserid && s.Type == model.SearchParams.Type).FirstOrDefault();
                    if (Mol != null)
                    {
                        Mol.UpdateTime = DateTime.Now;
                        Mol.IsDeleted = 0;
                        _rep.Update(Mol);
                    }
                    else
                    {
                        _rep.Insert(new MedicineSearchHistory()
                        {
                            Id = Guid.NewGuid().ToString(),
                            KeyWord = model.SearchParams.KeyWord,
                            CreateTime = DateTime.Now,
                            IsEnabled = 0,
                            IsDeleted = 0,
                            Type = model.SearchParams.Type,
                            Wxuserid = workUser?.WxUser?.Id ?? ""
                        });
                    }
                    _rep.SaveChanges();
                    rvm.Success = true;
                    rvm.Msg = "";
                }
                catch (Exception e)
                {
                    rvm.Success = false;
                    rvm.Msg = e.Message;
                    LoggerHelper.WriteLogInfo("[AddHotSearch Error]" + e.Message);

                }

            }

            return rvm;
        }

        /// <summary>
        ///  删除个人搜索列表记录
        /// </summary>
        /// <param name="historyId"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>

        public ReturnValueModel DeleteSearchHistory(RowNumModel<WxMedicineDelViewModel> model)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            rvm.Msg = "";
            rvm.Success = true;
            if (model.SearchParams.historyId.Count > 0)
            {
                foreach (var hisid in model.SearchParams.historyId)
                {
                    var hismodel = _rep.Where<MedicineSearchHistory>(s => s.Id == hisid).FirstOrDefault();
                    if (hismodel != null)
                    {
                        hismodel.IsDeleted = 1;
                        hismodel.UpdateTime = DateTime.Now;
                        _rep.Update(hismodel);
                        _rep.SaveChanges();
                    }
                }
            }
            return rvm;
        }

        /// <summary>
        ///查询热搜列表和个人搜索列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetHotSearchList(RowNumModel<MedicineHotSearch> model, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            rvm.Msg = "";
            rvm.Success = true;
            //热搜列表   根据配置项中的数据获取 
            var SendMedic = _rep.SqlQuery<ThirdPartyKeyWord>("select * from ThirdPartyKeyWord where IsDeleted=0 and KeyWordType=1").OrderByDescending(s => s.CreateTime).FirstOrDefault();
            if (SendMedic == null)
            {
                rvm.Success = false;
                rvm.Msg = "SendMedic 为空";
                return rvm;
            }
            string SendText = SendMedic.KeyWordContent;
            string[] MedicineArr = SendText.Split(';');
            List<MedicineVal> MedList = new List<MedicineVal>();
            for (int i = 0; i < MedicineArr.Length; i++)
            {
                foreach (var medval in MedicineArr[i].Split(','))
                {
                    MedList.Add(new MedicineVal()
                    {
                        Type = (i + 1).ToString(),
                        TypeContent = medval
                    });
                }

            }

            //var hotlist = (from a in _rep.Table<MedicineHotSearch>().Where(s => s.IsDeleted != 1) select a);

            //if (model?.SearchParams != null)
            //{
            //    var keyword = model?.SearchParams?.KeyWord ?? "";
            //    var Type = model?.SearchParams.Type ?? "1";
            //    hotlist=hotlist.Where(s => s.KeyWord.Contains(keyword)&&s.Type==Type);
            //}

            //var hotsearchlist = (from a in hotlist group a by a.KeyWord into g select new
            //{
            //    HotName = g.Key,
            //    Count= hotlist.Where(s=>s.KeyWord==g.Key).Count(),
            //    Type = hotlist.Where(s => s.KeyWord == g.Key).FirstOrDefault().Type
            //}).OrderByDescending(s=>s.Count).ToList();

            //个人搜索历史列表
            var historylist = (from a in _rep.Table<MedicineSearchHistory>().Where(s => s.IsDeleted != 1) select a);
            if (workUser != null)
            {
                historylist = historylist.Where(s => s.Wxuserid == workUser.WxUser.Id);
            }
            historylist = historylist.OrderByDescending(s => s.UpdateTime).ThenByDescending(s => s.CreateTime);
            rvm.Result = new
            {
                rows = new
                {

                    medicinelist = historylist.Where(s => s.Type == "1").ToPaginationList(model.PageIndex, model.PageSize),
                    medicinehotlist = MedList.Where(s => s.Type == "1").ToList(),
                    indicationlist = historylist.Where(s => s.Type == "2").ToPaginationList(model.PageIndex, model.PageSize),
                    indicationhotlist = MedList.Where(s => s.Type == "2").ToList(),
                    interactionlist = historylist.Where(s => s.Type == "3").ToPaginationList(model.PageIndex, model.PageSize),
                    interactionhotlist = MedList.Where(s => s.Type == "3").ToList(),
                }
            };

            return rvm;
        }

        public class MedicineVal
        {
            public string Type { get; set; }

            public string TypeContent { get; set; }
        }
    }
}
