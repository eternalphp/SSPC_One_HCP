using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.ProductModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;

namespace SSPC_One_HCP.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IEfRepository _rep;

        public ProductService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="rowPro">分页、搜索</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetProductList(RowNumModel<SearchProductViewModel> rowPro, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (rowPro == null)
            {
                rvm.Success = false;
                rvm.Msg = "";
                rvm.Result = new
                {
                    total = 0,
                    rows = new List<object>()
                };
                return rvm;
            }

            var products = _rep.Where<ProductInfo>(s => s.IsDeleted != 1);
            if (rowPro.SearchParams != null && rowPro.SearchParams.Product != null)
            {
                products = products.Where(rowPro.SearchParams.Product);
            }

            var query = from a in products
                       join b in _rep.Where<BuProDeptRel>(s => s.IsDeleted != 1) on a.Id equals b.ProId into ab
                       from b1 in ab.DefaultIfEmpty()
                       join c in _rep.Where<BuInfo>(s => s.IsDeleted != 1) on b1.BuName equals c.BuName into bc
                       from c1 in bc.DefaultIfEmpty()
                       join d in _rep.Where<DepartmentInfo>(s => s.IsDeleted != 1) on b1.DeptId equals d.Id into bd
                       from d1 in bd.DefaultIfEmpty()
                       group new { b1, c1, d1 } by a into g1
                       select new
                       {
                           Product = g1.Key,
                           BuList = from v1 in g1
                                     where v1.c1 != null
                                     group v1 by v1.c1 into g2
                                     select g2.Key,
                           DeptList = from v1 in g1
                                      where v1.d1 != null
                                      group v1 by v1.d1 into g2
                                      select new DepartmentSelectionViewModel
                                      {
                                          DeptId = g2.Key.Id,
                                          DeptName = g2.Key.DepartmentName,
                                          DeptType = g2.Key.DepartmentType
                                      }
                       };

            if (rowPro.SearchParams != null)
            {
                if (!string.IsNullOrEmpty(rowPro.SearchParams.DepartmentName))
                {
                    query = query.Where(s => s.DeptList.Any(d => d.DeptName.Contains(rowPro.SearchParams.DepartmentName)));
                }
                if (!string.IsNullOrEmpty(rowPro.SearchParams.BuName))
                {
                    query = query.Where(s => s.BuList.Any(d => d.BuName == rowPro.SearchParams.BuName));
                }
            }

            var total = query.Count();
            query = query.OrderBy(o => o.Product.ProductName).ToPaginationList(rowPro.PageIndex, rowPro.PageSize);

            var list = new List<ProductListItemViewModel>();
            foreach (var q in query)
            {
                var item = new ProductListItemViewModel();
                item.Product = q.Product;
                item.DeptList = q.DeptList.Where(d => d.DeptType == 1);
                item.OtherDeptList = q.DeptList.Where(d => d.DeptType == 2);
                item.BuNameList = q.BuList.Select(d => d.BuName);
                item.DeptNames = item.DeptList.Any() ? item.DeptList.Select(d => d.DeptName).Aggregate((v, a) => v + "," + a) : "";
                item.OtherDeptNames = item.OtherDeptList.Any() ? item.OtherDeptList.Select(d => d.DeptName).Aggregate((v, a) => v + "," + a) : "";
                //item.BuNames = q.BuList.Any() ? q.BuList.Select(d => d.BuName).Aggregate((v, a) => v + "," + a) : "";
                list.Add(item);
            }

            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                total = total,
                rows = list,
            };

            return rvm;
        }
        /// <summary>
        /// 新增或更新产品信息
        /// </summary>
        /// <param name="productInfo">
        /// 产品信息，Id存在则更新，否则新增
        /// </param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateProduct(ProductDetailViewModel productInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {
                    var product = productInfo.Product;
                    var pro = _rep.FirstOrDefault<ProductInfo>(s => s.Id == product.Id);
                    if (pro == null)
                    {
                        pro = product;
                        pro.Id = Guid.NewGuid().ToString();
                        pro.CreateUser = workUser.User.Id;
                        pro.CreateTime = DateTime.Now;
                        _rep.Insert(pro);
                        rvm.Msg = "新增";
                    }
                    else
                    {
                        pro.ProductName = product.ProductName;
                        pro.ProductUrl = product.ProductUrl;
                        pro.ProductPicName = product.ProductPicName;
                        pro.ProductDesc = product.ProductDesc;
                        pro.Sort = product.Sort;
                        pro.Remark = product.Remark;
                        pro.UpdateTime = DateTime.Now;
                        pro.UpdateUser = workUser.User.Id;
                        _rep.Update(pro);
                        rvm.Msg = "更新";

                        var rels = _rep.Where<BuProDeptRel>(s => s.ProId == product.Id);
                        if (rels.Any())
                        {
                            rels.Delete();
                            _rep.SaveChanges();
                        }
                    }

                    if (productInfo.DeptIdList != null && productInfo.BuNameList != null)
                    {
                        foreach (var deptId in productInfo.DeptIdList)
                        {
                            foreach (var buName in productInfo.BuNameList)
                            {
                                BuProDeptRel bpdr = new BuProDeptRel
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProId = product.Id,
                                    BuName = buName,
                                    DeptId = deptId,
                                    CreateTime = DateTime.Now,
                                    CreateUser = workUser.User.Id
                                };
                                _rep.Insert(bpdr);
                            }
                        }
                    }
                    _rep.SaveChanges();

                    tran.Commit();

                    rvm.Success = true;
                    rvm.Result = new
                    {
                        pro
                    };
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    rvm.Msg = "fail";
                    rvm.Success = false;
                }
            }
            return rvm;
        }
        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="proList">传入模型数组，只需要Id</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel DelProduct(List<ProductInfo> proList, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            if (proList == null)
            {
                rvm.Success = false;
                return rvm;
            }

            var ids = proList.Where(s => s != null).Select(s => s.Id).ToList();

            var list = _rep.Where<ProductInfo>(s => ids.Contains(s.Id)).ToList();
            foreach(var item in list)
            {
                item.IsDeleted = 1;
            }
            _rep.UpdateList(list);
            _rep.SaveChanges();

            rvm.Success = true;
            rvm.Msg = "";

            return rvm;
        }

        /// <summary>
        /// 产品详情
        /// </summary>
        /// <param name="productInfo">传入Id</param>
        /// <returns></returns>
        public ReturnValueModel ProductDetail(ProductInfo productInfo)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (productInfo == null || string.IsNullOrEmpty(productInfo.Id))
            {
                rvm.Success = false;
                rvm.Msg = "";
                rvm.Result = new
                {
                    total = 0,
                    rows = new List<object>()
                };
                return rvm;
            }

            var query = from a in _rep.Where<ProductInfo>(s => s.Id == productInfo.Id)
                        join b in _rep.Where<BuProDeptRel>(s => s.IsDeleted != 1) on a.Id equals b.ProId into ab
                        from b1 in ab.DefaultIfEmpty()
                        join c in _rep.Where<BuInfo>(s => s.IsDeleted != 1) on b1.BuName equals c.BuName into bc
                        from c1 in bc.DefaultIfEmpty()
                        join d in _rep.Where<DepartmentInfo>(s => s.IsDeleted != 1) on b1.DeptId equals d.Id into bd
                        from d1 in bd.DefaultIfEmpty()
                        group new { b1, c1, d1 } by a into g1
                        select new
                        {
                            Product = g1.Key,
                            BuNameList = from v1 in g1
                                         where v1.c1 != null
                                         group v1 by v1.c1 into g2
                                         select g2.Key.BuName,
                            DeptList = from v1 in g1
                                       where v1.d1 != null && v1.d1.DepartmentType == 1
                                       group v1 by v1.d1 into g2
                                       select new DepartmentSelectionViewModel
                                       {
                                           DeptId = g2.Key.Id,
                                           DeptName = g2.Key.DepartmentName,
                                           DeptType = g2.Key.DepartmentType
                                       },
                            OtherDeptList = from v1 in g1
                                            where v1.d1 != null && v1.d1.DepartmentType == 2
                                            group v1 by v1.d1 into g2
                                            select new DepartmentSelectionViewModel
                                            {
                                                DeptId = g2.Key.Id,
                                                DeptName = g2.Key.DepartmentName,
                                                DeptType = g2.Key.DepartmentType
                                            }
                        };

            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = query.First();

            return rvm;
        }

        ///// <summary>
        ///// 产品绑定bu列表
        ///// </summary>
        ///// <param name="rowBuPro">分页、搜索</param>
        ///// <param name="workUser"></param>
        ///// <returns></returns>
        //public ReturnValueModel BuProList(RowNumModel<BuProDeptRelViewModel> rowBuPro, WorkUser workUser)
        //{
        //    ReturnValueModel rvm = new ReturnValueModel();

        //    var list = from a in _rep.Table<BuProDeptRel>()
        //               join b in _rep.Table<ProductInfo>() on a.ProId equals b.Id
        //               join c in _rep.Table<DepartmentInfo>() on a.DeptId equals c.Id
        //               group new { a, c } by new { b } into g
        //               select new BuProDeptRelViewModel
        //               {
        //                   BuNameList = g.Select(s => s.a.BuName).Distinct(),
        //                   DeptNameList = g.Select(s => s.c.DepartmentName).Distinct(),
        //                   ProductName = g.Key.b.ProductName,
        //                   ProId = g.Key.b.Id
        //               };

        //    if (!string.IsNullOrEmpty(rowBuPro.SearchParams.SearchDept))
        //    {
        //        list = list.Where(s => s.DeptNameList.Contains(rowBuPro.SearchParams.SearchDept));
        //    }
        //    if (!string.IsNullOrEmpty(rowBuPro.SearchParams.SearchBU))
        //    {
        //        list = list.Where(s => s.BuNameList.Contains(rowBuPro.SearchParams.SearchBU));
        //    }
        //    if (!string.IsNullOrEmpty(rowBuPro.SearchParams.ProductName))
        //    {
        //        list = list.Where(s => s.ProductName.Contains(rowBuPro.SearchParams.ProductName));
        //    }

        //    var total = list.Count();
        //    var rows = list.OrderByDescending(o => o.ProId).ToPaginationList(rowBuPro.PageIndex, rowBuPro.PageSize)
        //        .ToList().Select(s => new BuProDeptRelViewModel
        //        {
        //            ProductName = s.ProductName,
        //            ProId = s.ProId,
        //            DeptNames = s.DeptNameList.Any() ? s.DeptNameList.Aggregate((v, a) => v + "," + a) : "",
        //            BuNames = s.BuNameList.Any() ? s.BuNameList.Aggregate((v, a) => v + "," + a) : ""
        //        });
        //    rvm.Success = true;
        //    rvm.Msg = "";
        //    rvm.Result = new
        //    {
        //        total = total,
        //        rows = rows
        //    };
        //    return rvm;
        //}
        ///// <summary>
        ///// 添加关系
        ///// </summary>
        ///// <param name="buProDeptModel">关系信息</param>
        ///// <param name="workUser"></param>
        ///// <returns></returns>
        //public ReturnValueModel AddOrUpdateBuProDeptRel(BuProDeptModel buProDeptModel, WorkUser workUser)
        //{
        //    ReturnValueModel rvm = new ReturnValueModel();

        //    var rels = _rep.Where<BuProDeptRel>(s =>
        //        s.BuName == buProDeptModel.BuNameOld && s.ProId == buProDeptModel.ProIdOld);
        //    if (buProDeptModel.DeptIds.Any())
        //    {
        //        if (rels.Any())
        //        {
        //            rels.Delete();
        //            _rep.SaveChanges();
        //        }

        //        foreach (var item in buProDeptModel.DeptIds)
        //        {
        //            BuProDeptRel bpdr = new BuProDeptRel
        //            {
        //                Id = Guid.NewGuid().ToString(),
        //                ProId = buProDeptModel.ProId,
        //                BuName = buProDeptModel.BuName,
        //                DeptId = item,
        //                CreateTime = DateTime.Now,
        //                CreateUser = workUser.User.Id
        //            };
        //            _rep.Insert(bpdr);
        //        }

        //        _rep.SaveChanges();
        //        rvm.Success = true;
        //        rvm.Msg = "";
        //        rvm.Result = new
        //        {

        //        };
        //    }
        //    return rvm;
        //}
        ///// <summary>
        ///// 关系详情
        ///// </summary>
        ///// <param name="buProDeptModel">传入BuName和ProId</param>
        ///// <param name="workUser"></param>
        ///// <returns></returns>
        //public ReturnValueModel BuProDeptDetail(BuProDeptModel buProDeptModel, WorkUser workUser)
        //{
        //    ReturnValueModel rvm = new ReturnValueModel();

        //    var detail = (from a in _rep.Table<BuProDeptRel>()
        //                  where a.BuName == buProDeptModel.BuName && a.ProId == buProDeptModel.ProId
        //                  group a.DeptId by new { a.BuName, a.ProId }
        //        into g
        //                  select new BuProDeptModel
        //                  {
        //                      ProId = g.Key.ProId,
        //                      BuName = g.Key.BuName,
        //                      DeptIds = g.ToList()
        //                  }).FirstOrDefault();

        //    rvm.Success = true;
        //    rvm.Msg = "";
        //    rvm.Result = new
        //    {
        //        detail = detail
        //    };

        //    return rvm;
        //}

        /// <summary>
        /// 获取产品相关的BU和科室列表
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel BuProDeptRelMap(ProductBuDeptSelectionViewModel buProDeptModel, WorkUser workUser)
        {
            List<string> buNameList = null;
            List<string> proIdList = null;
            List<string> deptIdList = null;

            if (buProDeptModel != null)
            {
                if (buProDeptModel.BuNameList != null)
                {
                    buNameList = (from a in buProDeptModel.BuNameList
                                  where !string.IsNullOrEmpty(a)
                                  select a).ToList();
                }
                if (buProDeptModel.Products != null)
                {
                    proIdList = (from a in buProDeptModel.Products
                                 where a != null && !string.IsNullOrEmpty(a.ProId)
                                 select a.ProId).ToList();
                }
                if (buProDeptModel.Departments != null)
                {
                    deptIdList = (from a in buProDeptModel.Departments
                                  where a != null && !string.IsNullOrEmpty(a.DeptId)
                                  select a.DeptId).ToList();
                }
            }

            var result = new ProductBuDeptOtherDeptSelectionViewModel();
            
            var list = from a in _rep.Where<ProductInfo>(s => s.IsDeleted != 1)
                       join b in _rep.Where<BuProDeptRel>(s => s.IsDeleted != 1) on a.Id equals b.ProId
                       join c in _rep.Where<BuInfo>(s => s.IsDeleted != 1) on b.BuName equals c.BuName
                       join d in _rep.Where<DepartmentInfo>(s => s.IsDeleted != 1) on b.DeptId equals d.Id
                       select new
                       {
                           Product = a,
                           BU = c,
                           Department = d
                       };

            if (!string.IsNullOrEmpty(workUser?.Organization?.BuName))
            {
                //BU的人只能操作自己的BU
                string buName = workUser.Organization.BuName;
                buNameList = new List<string>() { buName };
                list = list.Where(s => s.BU != null && s.BU.BuName == buName);
            }
            else
            {
                if (buNameList != null && buNameList.Count > 0)
                {
                    list = list.Where(s => s.BU != null && buNameList.Contains(s.BU.BuName));
                }
            }

            if (proIdList != null && proIdList.Count > 0)
            {
                list = list.Where(s => s.Product != null && proIdList.Contains(s.Product.Id));
            }

            if (deptIdList != null && deptIdList.Count > 0)
            {
                list = list.Where(s => s.Department != null && deptIdList.Contains(s.Department.Id));
            }

            result.BuNameList = from a in list
                                where a.BU != null
                                group a by a.BU into g1
                                select g1.Key.BuName;

            if (buNameList != null && buNameList.Count > 0)
            {
                result.Products = from a in list
                                  where a.Product != null
                                  group a by a.Product into g1
                                  select new ProductSelectionViewModel
                                  {
                                      ProId = g1.Key.Id,
                                      ProName = g1.Key.ProductName
                                  };

                if (proIdList != null && proIdList.Count > 0)
                {
                    result.Departments = from a in list
                                         where a.Department != null && a.Department.DepartmentType == 1  //只要普通科室，去掉其它科室
                                         group a by a.Department into g1
                                         select new DepartmentSelectionViewModel
                                         {
                                             DeptId = g1.Key.Id,
                                             DeptName = g1.Key.DepartmentName,
                                             DeptType = g1.Key.DepartmentType
                                         };

                    result.OtherDepartments = from a in _rep.Where<DepartmentInfo>(s => s.IsDeleted != 1)
                                              where !result.Departments.Any(s => s.DeptId == a.Id)
                                              select new DepartmentSelectionViewModel
                                              {
                                                  DeptId = a.Id,
                                                  DeptName = a.DepartmentName,
                                                  DeptType = a.DepartmentType
                                              };
                }
            }

            ReturnValueModel rvm = new ReturnValueModel();
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = result;
            return rvm;
        }

        /// <summary>
        /// buprodept关系
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel BuProDeptRelMap(WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var pros = _rep.Table<ProductInfo>();
            var depts = _rep.Table<DepartmentInfo>();
            var list = from a in _rep.Table<BuProDeptRel>()
                       group a by a.BuName
                into g1
                       select new
                       {
                           BuName = g1.Key,
                           DeptPro = from b in g1
                                     join c in pros on b.ProId equals c.Id
                                     where b.BuName == g1.Key
                                     group b by c into g2
                                     select new
                                     {
                                         ProId = g2.Key.Id,
                                         ProName = g2.Key.ProductName,
                                         ProUrl = g2.Key.ProductUrl,
                                         Depts = from d in g2
                                                 join e in depts on d.DeptId equals e.Id
                                                 select e
                                     }

                       };
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                list
            };
            return rvm;
        }
        /// <summary>
        /// 产品列表（下拉框）
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetProList()
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = _rep.Table<ProductInfo>().Select(s => new
            {
                value = s.Id,
                label = s.ProductName
            });
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                list = list
            };
            return rvm;
        }




    }
}
