using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Services.HCP.Interfaces;
using System;
using System.Linq;

namespace SSPC_One_HCP.Services.Services.HCP.Implementations
{
    public class HcpCatalogueManageService : IHcpCatalogueManageService
    {
        private readonly IEfRepository _rep;

        public HcpCatalogueManageService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 新增修改目录
        /// </summary>
        /// <param name="view"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateCatalogue(HcpCatalogueManage view, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var exisModel = _rep.FirstOrDefault<HcpCatalogueManage>(s => s.CatalogueName == view.CatalogueName && s.BuName == view.BuName && s.IsDeleted == 0);
            if (exisModel != null)
            {
                rvm.Msg = "fail";
                rvm.Success = false;
                rvm.Result = $"部门【{exisModel.BuName}】中已经存在【{exisModel.CatalogueName}】";
                return rvm;
            }

            HcpCatalogueManage hcpCatalogueInfo = _rep.FirstOrDefault<HcpCatalogueManage>(s => s.Id == view.Id && s.IsDeleted == 0);
            if (hcpCatalogueInfo == null)
            {
                hcpCatalogueInfo = new HcpCatalogueManage();
                hcpCatalogueInfo.Id = Guid.NewGuid().ToString();
                hcpCatalogueInfo.BuName = view.BuName;
                hcpCatalogueInfo.CatalogueName = view.CatalogueName;
                hcpCatalogueInfo.CreateTime = DateTime.Now;
                hcpCatalogueInfo.CreateUser = workUser.User.Id;
                _rep.Insert(hcpCatalogueInfo);
            }
            else
            {
                hcpCatalogueInfo.CatalogueName = view.CatalogueName;
                _rep.Update(hcpCatalogueInfo);
            }
            _rep.SaveChanges();
            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = hcpCatalogueInfo;
            return rvm;
        }
        /// <summary>
        /// 查询目录列表
        /// </summary>
        /// <param name="row"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetHcpCatalogueList(string buName, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var data = _rep.Where<HcpCatalogueManage>(s => s.BuName == buName && s.IsDeleted == 0).ToList();

            //如果目录列表为空 生成默认目录
            if (data == null || data.Count <= 0)
            {
                var defaultCatalogue = new HcpCatalogueManage();
                defaultCatalogue.Id = Guid.NewGuid().ToString();
                defaultCatalogue.BuName = buName;
                defaultCatalogue.CatalogueName = "默认目录";
                defaultCatalogue.CreateTime = DateTime.Now;
                defaultCatalogue.CreateUser = workUser.User.Id;
                _rep.Insert(defaultCatalogue);
                data.Add(defaultCatalogue);
                _rep.SaveChanges();
            }
            var res = data.OrderByDescending(o => o.CreateTime).Select(o => new
            {
                o.Id,
                o.BuName,
                o.CatalogueName,
                o.CreateTime,
                IsDefault = o.CatalogueName == "默认目录" ? true : false
            }).ToList();
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = res;
            return rvm;
        }
        /// <summary>
        /// 分页查询目录列表
        /// </summary>
        /// <param name="row"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetHcpCataloguePageList(RowNumModel<HcpCatalogueManage> row, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var data = _rep.Where<HcpCatalogueManage>(s => s.BuName == row.SearchParams.BuName && s.IsDeleted == 0);

            var total = data.Count();
            var rows = data.OrderBy(o => o.CatalogueName).ToPaginationList(row.PageIndex, row.PageSize);
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
        /// 删除资料
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel DeleteHcpCatalogue(HcpCatalogueManage model, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(model?.Id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }
            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {
                    var data = _rep.FirstOrDefault<HcpCatalogueManage>(s => s.Id == model.Id && s.IsDeleted == 0);
                    if (data == null)
                    {
                        rvm.Success = false;
                        rvm.Msg = "Invalid Id";
                        return rvm;
                    }
                    //删除目录时候，原目录下所有资料移交到默认目录中（默认目录使用BU名称）
                    //data.CatalogueName = data.BuName;
                    data.IsDeleted = 1;
                    data.UpdateTime = DateTime.Now;
                    data.UpdateUser = workUser.User.Id;
                    _rep.Update(data);


                    //查询默认目录 BuName=CatalogueName 为默认目录
                    var defaultCatalogue = _rep.FirstOrDefault<HcpCatalogueManage>(s => s.CatalogueName == "默认目录" && s.BuName == model.BuName && s.IsDeleted == 0);
                    if (defaultCatalogue == null)
                    {
                        defaultCatalogue = new HcpCatalogueManage();
                        defaultCatalogue.Id = Guid.NewGuid().ToString();
                        defaultCatalogue.BuName = model.BuName;
                        defaultCatalogue.CatalogueName = "默认目录";
                        defaultCatalogue.CreateTime = DateTime.Now;
                        defaultCatalogue.CreateUser = workUser.User.Id;
                        _rep.Insert(defaultCatalogue);
                    }

                    var dataRelList = _rep.Where<HcpDataCatalogueRel>(s => s.HcpCatalogueManageId == data.Id && s.IsDeleted == 0);
                    foreach (var dataRel in dataRelList)
                    {
                        dataRel.HcpCatalogueManageId = defaultCatalogue.Id;
                        _rep.Update(dataRel);
                    }

                    _rep.SaveChanges();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    return rvm;
                }
            }

            rvm.Success = true;
            rvm.Msg = "success";
            return rvm;
        }
    }
}
