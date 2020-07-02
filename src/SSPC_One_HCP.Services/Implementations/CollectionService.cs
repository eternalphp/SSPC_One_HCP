using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Enums;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SSPC_One_HCP.Services.Implementations
{
    public class CollectionService : ICollectionService
    {
        private readonly IEfRepository _rep;

        public CollectionService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 收藏功能
        /// </summary>
        /// <param name="collectionInfo">收藏信息</param>
        /// <param name="workUser">当前操作用户</param>
        /// <returns></returns>
        public ReturnValueModel CollectionMeet(MyCollectionInfo collectionInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var myCollections = _rep.FirstOrDefault<MyCollectionInfo>(s => s.UnionId == workUser.WxUser.UnionId && s.CollectionDataId == collectionInfo.CollectionDataId && s.CollectionType == collectionInfo.CollectionType);
            if (myCollections == null)
            {
                collectionInfo.Id = Guid.NewGuid().ToString();
                collectionInfo.UnionId = workUser.WxUser.UnionId;
                collectionInfo.CreateTime = DateTime.Now;
                _rep.Insert(collectionInfo);
            }
            else
            {
                _rep.Delete<MyCollectionInfo>(myCollections);
            }
            _rep.SaveChanges();
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                collectionInfo
            };

            return rvm;
        }

        /// <summary>
        /// 收藏功能
        /// </summary>
        /// <param name="collectionInfo">收藏信息</param>
        /// <returns></returns>
        public ReturnValueModel CollectionMeet(MyCollectionInfo collectionInfo)
        {
            ReturnValueModel rvm = new ReturnValueModel();          
            var myCollections = _rep.FirstOrDefault<MyCollectionInfo>(s =>
                s.UnionId == collectionInfo.UnionId && s.CollectionDataId == collectionInfo.CollectionDataId &&
                s.CollectionType == collectionInfo.CollectionType);
            if (myCollections == null)
            {
                collectionInfo.Id = Guid.NewGuid().ToString();
                collectionInfo.CreateTime = DateTime.Now;
                _rep.Insert(collectionInfo);               
            }
            else
            {
                _rep.Delete<MyCollectionInfo>(myCollections);
            }
            _rep.SaveChanges();
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                collectionInfo
            };

            return rvm;
        }

        /// <summary>
        /// 收藏列表
        /// </summary>
        /// <param name="rowCollection">分页、搜索</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel CollectionList(RowNumModel<CollectionViewModel> rowCollection, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            IEnumerable<CollectionViewModel> list = null;

            var type = rowCollection.SearchParams?.MyCollectionInfo.CollectionType ?? -1;
            //收藏夹全局搜索
            if (type == -1)
            {
                IEnumerable<CollectionViewModel> list1 = null;
                IEnumerable<CollectionViewModel> list2 = null;
                list1 = from a in _rep.Table<MyCollectionInfo>()
                        join b in _rep.Where<MeetInfo>(s => s.IsCompleted == EnumComplete.Approved) on a.CollectionDataId equals b.Id
                        where a.IsDeleted != 1 && a.UnionId == workUser.WxUser.UnionId &&
                              b.IsDeleted != 1
                        select new CollectionViewModel
                        {
                            MyCollectionInfo = a,
                            MeetInfo = b
                        };

                if (!string.IsNullOrEmpty(rowCollection.SearchParams?.SearchTitle))
                {
                    list1 = list1.Where(s =>
                        s.MeetInfo.MeetTitle.Contains(rowCollection.SearchParams?.SearchTitle));
                }
                list2 = from a in _rep.Table<MyCollectionInfo>()
                        join b in _rep.Where<DataInfo>(s => (s.IsCompleted == EnumComplete.Approved || s.IsCompleted == EnumComplete.Locked || s.IsCompleted == EnumComplete.WillDelete)) on a.CollectionDataId equals b.Id
                        where a.IsDeleted != 1 && a.UnionId == workUser.WxUser.UnionId &&
                              b.IsDeleted != 1
                        select new CollectionViewModel
                        {
                            MyCollectionInfo = a,
                            DataInfo = b
                        };
                if (!string.IsNullOrEmpty(rowCollection.SearchParams?.SearchTitle))
                {
                    list2 = list2.Where(s =>
                        s.DataInfo.Title.Contains(rowCollection.SearchParams?.SearchTitle));
                }

                list = list1.Union(list2);
            }
            else
            {
                if (type == 5)
                {
                    list = from a in _rep.Table<MyCollectionInfo>()
                           join b in _rep.Where<MeetInfo>(s => s.IsCompleted == EnumComplete.Approved) on a.CollectionDataId equals b.Id
                           where a.CollectionType == 5 && a.IsDeleted != 1 && a.UnionId == workUser.WxUser.UnionId &&
                                 b.IsDeleted != 1
                           select new CollectionViewModel
                           {
                               MyCollectionInfo = a,
                               MeetInfo = b
                           };

                    if (!string.IsNullOrEmpty(rowCollection.SearchParams?.SearchTitle))
                    {
                        list = list.Where(s =>
                            s.MeetInfo.MeetTitle.Contains(rowCollection.SearchParams?.SearchTitle));
                    }
                }
                else
                {
                    list = from a in _rep.Table<MyCollectionInfo>()
                           join b in _rep.Where<DataInfo>(s => (s.IsCompleted == EnumComplete.Approved || s.IsCompleted == EnumComplete.Locked || s.IsCompleted == EnumComplete.WillDelete)) on a.CollectionDataId equals b.Id
                           where a.CollectionType == type && a.IsDeleted != 1 && a.UnionId == workUser.WxUser.UnionId &&
                                 b.IsDeleted != 1
                           select new CollectionViewModel
                           {
                               MyCollectionInfo = a,
                               DataInfo = b
                           };
                    if (!string.IsNullOrEmpty(rowCollection.SearchParams?.SearchTitle))
                    {
                        list = list.Where(s =>
                            s.DataInfo.Title.Contains(rowCollection.SearchParams?.SearchTitle));
                    }
                }
            }

            //switch (type)
            //{
            //    case 5:
            //        list = from a in _rep.Table<MyCollectionInfo>()
            //            join b in _rep.Table<MeetInfo>() on a.CollectionDataId equals b.Id
            //            where a.CollectionType == 5 && a.IsDeleted != 1 && a.UnionId == workUser.WxUser.UnionId &&
            //                  b.IsDeleted != 1
            //            select new CollectionViewModel
            //            {
            //                MyCollectionInfo = a,
            //                MeetInfo = b
            //            };

            //        if (!string.IsNullOrEmpty(rowCollection.SearchParams?.SearchTitle))
            //        {
            //            list = list.Where(s =>
            //                s.MeetInfo.MeetTitle.Contains(rowCollection.SearchParams?.SearchTitle));
            //        }

            //        break;
            //    case 2:
            //    case 3:
            //    case 4:
            //        list = from a in _rep.Table<MyCollectionInfo>()
            //            join b in _rep.Table<DataInfo>() on a.CollectionDataId equals b.Id
            //            where a.CollectionType == type && a.IsDeleted != 1 && a.UnionId == workUser.WxUser.UnionId &&
            //                  b.IsDeleted != 1
            //            select new CollectionViewModel
            //            {
            //                MyCollectionInfo = a,
            //                DataInfo = b
            //            };
            //        if (!string.IsNullOrEmpty(rowCollection.SearchParams?.SearchTitle))
            //        {
            //            list = list.Where(s =>
            //                s.DataInfo.Title.Contains(rowCollection.SearchParams?.SearchTitle));
            //        }
            //        break;

            //}

            var total = list.Count();
            var rows = list.OrderByDescending(s => s.MyCollectionInfo.CreateTime)
                .ToPaginationList(rowCollection.PageIndex, rowCollection.PageSize).ToList();
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                total,
                rows
            };

            return rvm;
        }
    }
}
