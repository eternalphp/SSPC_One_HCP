using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using SSPC_One_HCP.Services.Utils;
using SSPC_One_HCP.Core.Domain.Enums;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using System.Collections.Generic;

namespace SSPC_One_HCP.Services.Implementations
{
    public class WxKnowledgeService : IWxKnowledgeService
    {
        private readonly IEfRepository _rep;

        public WxKnowledgeService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// 知识库页面数据
        /// </summary>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetWxKnowledgePage(WorkUser workUser)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();//监视代码运行时间
            ReturnValueModel rvm = new ReturnValueModel();

            var rels = _rep.Table<MediaDataRel>();
            var depts = _rep.Table<DepartmentInfo>();

            var list = (from b in _rep.Table<DataInfo>()
                        where b.IsDeleted != 1
                        select new
                        {
                            b,
                            a = (from c in rels
                                 join d in depts on c.DeptId equals d.Id
                                 where c.DataInfoId == b.Id && c.IsDeleted != 1 && d.IsDeleted != 1
                                 select d.DepartmentName)
                        }).Where(s => s.a.Contains(workUser.WxUser.DepartmentName));
            var list2 = (
                //修改人：ywk  修改日期：2019-03-21 产品资料根据部门过滤修改
                //from a in _rep.Table<MediaDataRel>() 
                from a in _rep.Table<BuProDeptRel>()
                join c in _rep.Table<ProductInfo>() on a.ProId equals c.Id
                join d in depts on a.DeptId equals d.Id
                //where a.IsDeleted != 1 && d.IsDeleted != 1 && d.DepartmentName == workUser.WxUser.DepartmentName
                where a.IsDeleted != 1 && d.IsDeleted != 1 && c.IsDeleted != 1
                select c
                ).Distinct();


            //部门过滤
            if (workUser != null)
            {
                if (!string.IsNullOrEmpty(workUser.WxUser.DepartmentName))
                {
                    list = list.Where(s => s.a.Contains(workUser.WxUser.DepartmentName));
                    //list2 = list2.Where(s => s.Department == workUser.WxUser.DepartmentName);
                }
            }


            LoggerHelper.WriteLogInfo("[GetWxKnowledgePage]产品sql:" + list2.ToString());

            //产品
            var proList = (from a in list2
                           select a).OrderBy(s=>s.Sort);
            //学术
            var academicList = list.Where(s => s.b.MediaType == 2).Take(4).ToList();
            //播客
            var podcastList = (from a in list.Where(b => (b.b.IsCompleted == EnumComplete.Approved || b.b.IsCompleted == EnumComplete.Locked || b.b.IsCompleted == EnumComplete.WillDelete))
                               join c in _rep.Table<MyReadRecord>() on a.b.Id equals c.DataInfoId
                                   into ac
                               from cc in ac.DefaultIfEmpty()
                               where cc.UnionId == workUser.WxUser.UnionId || cc == null
                               select new
                               {
                                   a,
                                   IsRead = cc.IsRead
                               }).Where(s => s.a.b.MediaType == 3).Take(2).ToList();

            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                proList,
                academicList,
                podcastList
            };
            stopwatch.Stop();//结束
            rvm.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
            return rvm;
        }

        /// <summary>
        /// 播客列表
        /// </summary>
        /// <param name="rowData">分页</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel PodcastList(RowNumModel<DataInfo> rowData, WorkUser workUser)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();//监视代码运行时间
            ReturnValueModel rvm = new ReturnValueModel();
            var rels = _rep.Table<MediaDataRel>();
            //var depts = _rep.Table<DepartmentInfo>();
            var deptlist = from a in _rep.Table<DepartmentInfo>().Where(s=>s.IsDeleted==0) select a;

            var list = (from b in _rep.Table<DataInfo>()

                        where b.IsDeleted != 1 && (b.IsCompleted == EnumComplete.Approved || b.IsCompleted == EnumComplete.Locked || b.IsCompleted == EnumComplete.WillDelete)
                        select new
                        {
                            b,
                            a = (from c in rels
                                 join d in deptlist on c.DeptId equals d.Id
                                 where c.DataInfoId == b.Id
                                 select d)
                            // }).Where(s => s.a.Select(v => v.DepartmentName).Contains(workUser.WxUser.DepartmentName));
                        });
            //过滤  如果不是游客 要增加部门过滤
            if (workUser != null)
            {
                if (!string.IsNullOrEmpty(workUser.WxUser.HospitalName))
                {
                    list = list.Where(s => s.b.Dept.Contains(workUser.WxUser.DepartmentName));
                }
            }
            var mrr = _rep.Table<MyReadRecord>();
            var podcastList = (from a in list
                               select new
                               {
                                   a,
                                   IsRead = mrr.Any(s => s.DataInfoId == a.b.Id && s.UnionId == workUser.WxUser.UnionId) ? 1 : 2
                               }).Where(s => s.a.b.MediaType == 3);
            if (!string.IsNullOrEmpty(rowData.SearchParams.Title))
            {
                podcastList = podcastList.Where(s => s.a.b.Title.Contains(rowData.SearchParams.Title));
            }
            //是否推荐
            if ((rowData.SearchParams.IsSelected ?? 0) == 1)
            {
                 
                podcastList = podcastList.OrderByDescending(s => s.a.b.IsSelected).Take(2);
            }



            var total = podcastList.Count();
            //播客精选置顶，未播放的按时间排序，已经播放的在下面 
            var rows = podcastList.OrderByDescending(o => o.IsRead).ThenByDescending(s => s.a.b.Sort)
                .ToPaginationList(rowData.PageIndex, rowData.PageSize).ToList();
            LoggerHelper.WriteLogInfo("[PodcastList]:rows sql：" + podcastList.OrderByDescending(s => s.a.b.Sort).ThenByDescending(o => o.IsRead).ToString());
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                total,
                rows
            };
            stopwatch.Stop();//结束
            rvm.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
            return rvm;

        }

        /// <summary>
        /// 更改已读未读状态
        /// </summary>
        /// <param name="myReadRecord"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel IsReadStatus(MyReadRecord myReadRecord, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var mrr = _rep.FirstOrDefault<MyReadRecord>(s =>
                s.DataInfoId == myReadRecord.DataInfoId && s.UnionId == workUser.WxUser.UnionId);
            if (mrr == null)
            {
                mrr = myReadRecord;
                mrr.UnionId = workUser.WxUser.UnionId;
                mrr.Id = Guid.NewGuid().ToString();
                mrr.IsRead = 1;
                mrr.CreateTime = DateTime.Now;
                mrr.CreateUser = workUser.WxUser.Id;
                _rep.Insert(mrr);
                _rep.SaveChanges();
                rvm.Success = true;
                rvm.Msg = "";
                rvm.Result = new
                {
                    mrr
                };
            }
            else
            {
                rvm.Success = true;
                rvm.Msg = "";
                rvm.Result = new
                {
                    mrr
                };
            }

            return rvm;
        }

        /// <summary>
        /// 产品资料列表
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetProductMediaList(RowNumModel<MediaDataRelViewModel> dataInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var rels = _rep.Table<MediaDataRel>();
            //var depts = _rep.Table<DepartmentInfo>();
            var list = from a in _rep.Table<DataInfo>()
                       where a.IsDeleted != 1 && a.MediaType == 1 && (a.IsCompleted == EnumComplete.Approved)
                       select new
                       {
                           a,
                           b = (from c in rels
                                where c.DataInfoId == a.Id
                                select c.ProId),
                           IsHot = (a.ClickVolume > 66 ? 1 : 0)

                       };
            LoggerHelper.WriteLogInfo("[GetProductMediaList]开始：");
            LoggerHelper.WriteLogInfo(list.ToString());

            if (!string.IsNullOrEmpty(dataInfo.SearchParams.ProId))
            {
                list = list.Where(s => s.b.Contains(dataInfo.SearchParams.ProId));
            }

            if (!string.IsNullOrEmpty(dataInfo.SearchParams.Title))
            {
                list = list.Where(s => s.a.Title.Contains(dataInfo.SearchParams.Title));
            }
            //产品列表明细中  根据使用人所在科室 匹配过滤 ywk 2019-03-22
            if (workUser != null)
            {
                list = list.Where(s => s.a.Dept.Contains(workUser.WxUser.DepartmentName));
            }


            var total = list.Count();
            var rows = list.OrderByDescending(o => o.a.CreateTime)
                .ToPaginationList(dataInfo.PageIndex, dataInfo.PageSize);


            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                total = total,
                rows = rows
            };

            return rvm;
        }

        /// <summary>
        /// 临床指南列表
        /// </summary>
        /// <param name="dataInfo">分页、搜索</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetGuideList(RowNumModel<DataInfo> dataInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = from a in _rep.Where<DataInfo>(s => s.IsDeleted != 1)
                           //join b in _rep.Table<BuProDeptRel>() on a.Id equals b.DataInfoId 
                       where a.MediaType == 2 && a.IsDeleted != 1
                       select a;
            if (!string.IsNullOrEmpty(dataInfo.SearchParams.Title))
            {
                list = list.Where(s => s.Title.Contains(dataInfo.SearchParams.Title));
            }

            if (workUser != null)
            {
                var UserDept = workUser.WxUser.DepartmentName;
                list = list.Where(s => s.Dept == UserDept);
            }

            var total = list.Count();
            var rows = list.OrderByDescending(o => o.CreateTime)
                .ToPaginationList(dataInfo.PageIndex, dataInfo.PageSize);


            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                total = total,
                rows = rows
            };

            return rvm;
        }

        /// <summary>
        /// 播客详情
        /// </summary>
        /// <param name="dataInfo">传入Id</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AudioMediaDetail(DataInfo dataInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var detail = _rep.FirstOrDefault<DataInfo>(s => s.Id == dataInfo.Id);
            var isCollection = _rep.Where<MyCollectionInfo>(s =>
                    s.UnionId == workUser.WxUser.UnionId && s.CollectionDataId == detail.Id && s.CollectionType == 3)
                .Any()
                ? 1
                : 2;

            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                detail = detail,
                isCollection = isCollection
            };

            return rvm;
        }
        /// <summary>
        /// 知识库详情
        /// </summary>
        /// <param name="dataInfo">传入Id</param>
        /// <returns></returns>
        public ReturnValueModel WxKnowledgeDetail(DataInfo dataInfo)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var detail = _rep.FirstOrDefault<DataInfo>(s => s.Id == dataInfo.Id);
            var isCollection = _rep.Where<MyCollectionInfo>(s =>
                    //s.UnionId == workUser.WxUser.UnionId && s.CollectionDataId == detail.Id && (s.CollectionType == 3 || s.CollectionType == 4)) 
                    s.UnionId == dataInfo.CreateUser && s.CollectionDataId == detail.Id)
                        .Any()
                        ? 1
                        : 2;
            var isLike = _rep.FirstOrDefault<ProductInfoLike>(x => x.ProID.Equals(detail.Id) && x.CreateUser.Equals(dataInfo.CreateUser));

            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                detail,
                isCollection = isCollection,
                IsLike = isLike?.IsLike ?? 0,
            };
            //点击量自增1
            DataInfo datamodel = _rep.FirstOrDefault<DataInfo>(s => s.Id == dataInfo.Id);
            try
            {
                if (datamodel != null)
                {

                    long ClickTimes = (datamodel?.ClickVolume ?? 0) + 1;
                    datamodel.ClickVolume = ClickTimes;
                    _rep.Update(datamodel);
                    _rep.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLogInfo("[知识库详情-WxKnowledgeDetail Error]*****开始******");
                LoggerHelper.WriteLogInfo(ex.Message);
                LoggerHelper.WriteLogInfo("[知识库详情-WxKnowledgeDetail Error]*****结束******");
            }

            return rvm;
        }
        /// <summary>
        /// 根据用户获取用药参考
        /// </summary>
        /// <param name="wxuser">微信用户</param>
        /// <returns></returns>
        public ReturnValueModel ClinicalguidelinesByUser(WorkUser workUser)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();//监视代码运行时间
            //创建公共返回数据对象返回数据
            ReturnValueModel rvm = new ReturnValueModel();
            if (workUser != null)
            {
                //通过用户Id查询用户
                if (workUser.WxUser.Id == null || workUser.WxUser.Id == "")
                {
                    rvm.Success = false;
                    rvm.Msg = "The user did not find it.";
                }
                else
                {
                    var SendUser = _rep.SqlQuery<DoctorModel>("select * from DoctorModel where Id='" + workUser.WxUser.Id + "'");                  

                    if (SendUser != null)
                    {
                        var SendDeptName = SendUser.FirstOrDefault();
                        var SendList = _rep.Where<FsysArticle>(x => true);
                        //根据获取到的用户科室名称
                        if (string.IsNullOrEmpty(SendDeptName.DepartmentName))
                        {
                            SendList = SendList.OrderBy(x => Guid.NewGuid()).Take(2).OrderByDescending(x => x.CreateTime);
                        }
                        else
                        {
                            //根据查询到的科室名称查询临床指南
                            SendList = SendList.Where(x => x.IsDeleted == 0 && x.DepartmentId.Contains(SendDeptName.DepartmentName)).OrderByDescending(c => c.CreateTime).Take(2);
                        }
                        rvm.Success = true;
                        rvm.Msg = "success";
                        rvm.Result = new
                        {
                            SendList
                        };

                    }
                    else
                    {
                        rvm.Success = false;
                        rvm.Msg = "The user does not exist";
                    }
                }
            }
            stopwatch.Stop();//结束
            rvm.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
            return rvm;
        }

        public ReturnValueModel IsLike(ProductInfoLikeView date)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();//监视代码运行时间
            ReturnValueModel rvm = new ReturnValueModel();
            if (string.IsNullOrEmpty(date.UserID))
            {
                rvm.Success = false;
            }
            else
            {
                rvm.Success = true;
                var likeinfo = _rep.FirstOrDefault<ProductInfoLike>(x => x.CreateUser.Equals(date.UserID) && x.ProID.Equals(date.ProID));
                if (likeinfo == null)
                {
                    var likedate = new ProductInfoLike()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProID = date.ProID,
                        IsLike = date.IsLike,
                        CreateUser = date.UserID,
                        CreateTime = DateTime.Now,
                    };
                    _rep.Insert<ProductInfoLike>(likedate);
                    //rvm.Result = likedate;
                }
                else
                {
                    likeinfo.IsLike = date.IsLike;
                    likeinfo.UpdateUser = date.UserID;
                    likeinfo.UpdateTime = DateTime.Now;
                }
                _rep.SaveChanges();
            }
            stopwatch.Stop();//结束
            rvm.ResponseTime = stopwatch.Elapsed.TotalMilliseconds;
            return rvm;

        }
    }
}