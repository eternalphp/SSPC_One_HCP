using SSPC_One_HCP.Core.Cache;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Services.WeChat.Dto;
using SSPC_One_HCP.Services.Services.WeChat.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace SSPC_One_HCP.Services.Services.WeChat.Implementations
{
    public class WcLiveBroadcastPVService : IWcLiveBroadcastPVService
    {
        private readonly IEfRepository _rep;
        private readonly ICacheManager _cache;

        static readonly object locker = new object();
        public WcLiveBroadcastPVService(IEfRepository rep, ICacheManager cache)
        {
            _rep = rep;
            _cache = cache;
        }
        /// <summary>
        /// 记录明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ReturnValueModel AddLiveBroadcastPV(LiveBroadcastPV dto)
        {

            ReturnValueModel rvm = new ReturnValueModel();

            var meetInfodata = _rep.FirstOrDefault<MeetInfo>(s => s.Id == dto.MeetInfoId && s.IsDeleted != 1);
            if (meetInfodata != null)
            {
                var model = new LiveBroadcastPV
                {
                    Id = Guid.NewGuid().ToString(),
                    MeetInfoId = meetInfodata.Id,
                    UnionId = dto.UnionId,
                    OpenId = dto.OpenId,
                    WxName = dto.WxName,
                    WxPicture = dto.WxPicture,
                    CreateTime = DateTime.UtcNow.AddHours(8),
                };
                string _host = ConfigurationManager.AppSettings["HostUrl"];
                var files = GetFiles();
                var tourist = GetRandomNumber(files);
                var wxName = string.IsNullOrEmpty(model.WxName) ? "游客" : model.WxName;
                var wxPicture = string.IsNullOrEmpty(model.WxPicture) ? $"{_host}/Content/images/Tourist/{tourist}" : model.WxPicture;

                model.WxName = wxName;
                model.WxPicture = wxPicture;
                _rep.Insert(model);

                meetInfodata.PVCount = meetInfodata.PVCount.GetValueOrDefault() + 1;
                meetInfodata.UpdateTime = DateTime.UtcNow.AddHours(8);
                _rep.Update(meetInfodata);

                _rep.SaveChanges();

                lock (locker)
                {
                    SetWx(meetInfodata.Id, meetInfodata.PVCount.GetValueOrDefault(), dto.OpenId, dto.WxName, dto.WxPicture);
                }

            }


            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = "";
            return rvm;
        }
        /// <summary>
        /// 查询累计点击数
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ReturnValueModel GetLiveBroadcastPV(string id)
        {

            ReturnValueModel rvm = new ReturnValueModel();
            //rvm.Msg = "";
            //rvm.Success = true;
            //rvm.Result = new
            //{
            //    Count = "1W+",
            //    Users = new List<LiveBroadcastPVDto>()
            //};
            //return rvm;
            lock (locker)
            {
                var cacheDto = _cache.Get<PvCacheDto>(id);
                if (cacheDto == null)
                {
                    var meetInfodata = _rep.FirstOrDefault<MeetInfo>(s => s.Id == id && s.IsDeleted != 1);
                    if (meetInfodata == null)
                    {
                        rvm.Msg = "获取会议失败";
                        rvm.Success = true;
                        return rvm;
                    }

                    var pvdata = _rep.Where<LiveBroadcastPV>(s => s.MeetInfoId == meetInfodata.Id && s.IsDeleted != 1)
                        .OrderByDescending(o => o.CreateTime).Skip(0).Take(50).ToList();

                    cacheDto = new PvCacheDto
                    {
                        Count = meetInfodata.PVCount.GetValueOrDefault().ToString(),
                    };
                    var d = pvdata.GroupBy(o => new
                    {
                        o.OpenId,
                        o.WxName,
                        o.WxPicture
                    }).Select(o => new
                    {
                        o.Key.OpenId,
                        o.Key.WxName,
                        o.Key.WxPicture
                    }).ToList();

                    string _host = ConfigurationManager.AppSettings["HostUrl"];
                    var files = GetFiles();
                    var tourist = GetRandomNumber(files);
                    for (int i = 0; i < 3; i++)
                    {
                        var wxName = string.IsNullOrEmpty(d[i].WxName) ? "游客" : d[i].WxName;
                        var wxPicture = string.IsNullOrEmpty(d[i].WxPicture) ? $"{_host}/Content/images/Tourist/{tourist}" : d[i].WxPicture;
                        cacheDto.CacheWxs.Add(new PvCacheWxDto
                        {
                            OpenId = d[i].OpenId,
                            WxName = wxName,
                            WxPicture = wxPicture,
                            CreateTime = DateTime.UtcNow.AddHours(8),
                        });
                    }

                    if (_cache.IsSet(id))
                    {
                        _cache.Remove(id);
                    }
                    _cache.Set<PvCacheDto>(id, cacheDto, 3);
                }
                rvm.Msg = "";
                rvm.Success = true;
                rvm.Result = new
                {
                    Count = cacheDto.Count,
                    Users = cacheDto.CacheWxs
                };
                return rvm;
            }
        }

        void SetWx(string meetInfoId, int count, string openId, string wxName, string wxPicture)
        {
            PvCacheDto cacheDto = new PvCacheDto
            {
                Count = count.ToString(),
                CacheWxs = new List<PvCacheWxDto>()
            };
            var dtos = _cache.Get<PvCacheDto>(meetInfoId);
            if (dtos?.CacheWxs == null || dtos?.CacheWxs.Count <= 0)
            {
                cacheDto.CacheWxs.Add(new PvCacheWxDto
                {
                    OpenId = openId,
                    WxName = wxName,
                    WxPicture = wxPicture,
                    CreateTime = DateTime.UtcNow.AddHours(8),
                });
            }
            else
            {
                cacheDto.CacheWxs = dtos.CacheWxs;
                var m = cacheDto.CacheWxs.FirstOrDefault(o => o.OpenId == openId);
                if (m != null)
                {
                    cacheDto.CacheWxs.Remove(m);
                }
                cacheDto.CacheWxs.Add(new PvCacheWxDto
                {
                    OpenId = openId,
                    WxName = wxName,
                    WxPicture = wxPicture,
                    CreateTime = DateTime.UtcNow.AddHours(8),
                });
            }
            cacheDto.CacheWxs = cacheDto.CacheWxs.OrderByDescending(o => o.CreateTime).Skip(0).Take(3).ToList();

            if (_cache.IsSet(meetInfoId))
            {
                _cache.Remove(meetInfoId);
            }

            _cache.Set<PvCacheDto>(meetInfoId, cacheDto, 3);
        }

        string GetRandomNumber(FileInfo[] files)
        {
            if (files.Length <= 0)
            {
                return "Tourist.jpg";
            }
            Random rnd = new Random();
            int index = rnd.Next(files.Length);
            return files[index].Name;
        }

        FileInfo[] GetFiles()
        {
            var path = HostingEnvironment.MapPath("/Content/images/Tourist");
            DirectoryInfo root = new DirectoryInfo(path);
            return root.GetFiles();

        }
    }
}
