using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.TagGroup;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.LinqExtented;

namespace SSPC_One_HCP.Services.Implementations
{
    public class TagGroupService : ITagGroupService
    {
        private readonly IEfRepository _rep;
        public TagGroupService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 新增标签组
        /// </summary>
        /// <param name="TagGroupName"></param>
        /// <returns></returns>
        public ReturnValueModel AddTagGroup(string TagGroupName)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var Taggroup = _rep.FirstOrDefault<TagGroup>(a => a.TagGroupName == TagGroupName);
            if (Taggroup == null)
            {
                TagGroup tagGroup = new TagGroup()
                {
                    Id = Guid.NewGuid().ToString(),
                    TagGroupName = TagGroupName,
                    CreateTime = DateTime.Now,
                    IsEnabled = 0,
                    IsDeleted = 0
                };
                _rep.Insert<TagGroup>(tagGroup);
                _rep.SaveChanges();
            }

            if (Taggroup != null)
            {
                rvm.Success = false;
                rvm.Msg = "This name already exist";
                return rvm;
            }

            rvm.Msg = "success";
            rvm.Success = true;
            return rvm;
        }


        /// <summary>
        /// 获取标签组列表
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetTagGroupList(TagGroup model)
        {
            ReturnValueModel rvm = new ReturnValueModel();
           
            var rows = from a in _rep.Table<TagGroup>().Where(s => s.IsDeleted != 1) select a;
            if (model != null)
            {
                rows = rows.Where(s => s.TagGroupName.Contains(model.TagGroupName));
            }

            rvm.Success = true;
            rvm.Result = rows.OrderByDescending(s=>s.CreateTime);
            return rvm;
        }

        /// <summary>
        /// 新增或编辑 标签组和标签关联
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateTagGroupRelation(RowNumModel<TagGroupRelViewModel> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            string taggroupid = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(rowNum.SearchParams?.GroupName ?? ""))
                {
                    throw new Exception("the groupname is null");
                }
                //如果TagGroupId为空 新增
                //TagGroupId不为空 编辑更新
                if (string.IsNullOrEmpty(rowNum.SearchParams?.TagGroupId ?? ""))
                {
                    #region  新增标签组
                    var Taggroup = _rep.FirstOrDefault<TagGroup>(a => a.TagGroupName == rowNum.SearchParams.GroupName);
                    taggroupid = Guid.NewGuid().ToString();
                    if (Taggroup == null)
                    {
                        TagGroup tagGroup = new TagGroup()
                        {
                            Id = taggroupid,
                            TagGroupName = rowNum.SearchParams?.GroupName,
                            CreateTime = DateTime.Now,
                            IsEnabled = 0,
                            IsDeleted = 0
                        };
                        _rep.Insert<TagGroup>(tagGroup);
                        _rep.SaveChanges();
                    }

                    if (Taggroup != null)
                    {
                        throw new Exception("the groupname is exist");
                    }
                    #endregion

                    #region  新增标签组和标签的对应关系 
                    foreach (var tagInfoid in rowNum.SearchParams.TabInfoIds)
                    {
                        _rep.Insert(new GroupTagRel()
                        {
                            Id = Guid.NewGuid().ToString(),
                            TagGroupId = taggroupid,
                            TagId = tagInfoid,
                            CreateTime = DateTime.Now,
                            IsEnabled = 0,
                            IsDeleted = 0
                        });
                        _rep.SaveChanges();
                    }
                    #endregion
                }
                else
                {
                    var Taggroup = _rep.FirstOrDefault<TagGroup>(a => a.Id == rowNum.SearchParams.TagGroupId);
                    taggroupid = rowNum.SearchParams?.TagGroupId;
                    if (Taggroup == null)
                    {
                        throw new Exception("the groupname is not exist");
                    }

                    //更新标签组名称
                    Taggroup.TagGroupName = rowNum.SearchParams?.GroupName;
                    _rep.Update(Taggroup);
                    _rep.SaveChanges();
                    var removelist = _rep.Table<GroupTagRel>().Where(p => p.TagGroupId == rowNum.SearchParams.TagGroupId && p.IsDeleted != 1).ToList();
                    removelist.ForEach(p => { p.IsDeleted = 1; _rep.Update(p); });
                    _rep.SaveChanges();

                    #region  新增标签组和标签的对应关系 
                    foreach (var tagInfoid in rowNum.SearchParams.TabInfoIds)
                    {
                        _rep.Insert(new GroupTagRel()
                        {
                            Id = Guid.NewGuid().ToString(),
                            TagGroupId = rowNum.SearchParams.TagGroupId,
                            TagId = tagInfoid,
                            CreateTime = DateTime.Now,
                            IsEnabled = 0,
                            IsDeleted = 0
                        });
                        _rep.SaveChanges();
                    }
                    #endregion

                }


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
                rows = new
                {
                    taggroupid = taggroupid
                }
            };
            return rvm;

        }


        /// <summary>
        ///  获取标签组对应的标签列表
        /// </summary>
        /// <param name="tagGroup"></param>
        /// <returns></returns>
        public ReturnValueModel GetTagGroupRelationList(TagGroup tagGroup)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                if (tagGroup == null)
                    throw new Exception("tagGroup is null");

                var tagInfolist = from a in _rep.Table<TagInfo>().Where(s => s.IsDeleted != 1)
                                  join b in _rep.Table<GroupTagRel>().Where(s => s.IsDeleted != 1 && s.TagGroupId == tagGroup.Id)
                                      on a.Id equals b.TagId
                                  select a;
                rvm.Success = true;
                rvm.Msg = "";
                rvm.Result = new
                {
                    rows = tagInfolist
                };
            }
            catch (Exception e)
            {
                rvm.Success = false;
                rvm.Msg = e.Message;
            }

            return rvm;
        }

        /// <summary>
        /// 标签组增加标签
        /// </summary>
        /// <param name="Tags"></param>
        /// <param name="tagGroup"></param>
        /// <returns></returns>
        public ReturnValueModel GroupAddTags(TagGroupTagViewModel tagGroupTagView)
        {
            List<GroupTagRel> groupTagRels = new List<GroupTagRel>();
            ReturnValueModel rvm = new ReturnValueModel();
            var taggroup = _rep.FirstOrDefault<TagGroup>(p => p.Id == tagGroupTagView.TagGroupId);
            if (taggroup == null)
            {
                rvm.Msg = "This tagGroup doesn't exist";
                rvm.Success = false;
                return rvm;
            }
            foreach (var itm in tagGroupTagView.TagsId)
            {
                groupTagRels.Add(new GroupTagRel
                {
                    Id = Guid.NewGuid().ToString(),
                    TagGroupId = tagGroupTagView.TagGroupId,
                    TagId = itm,
                    CreateTime = DateTime.Now
                });
            }
            _rep.InsertList<GroupTagRel>(groupTagRels);
            _rep.SaveChanges();
            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = groupTagRels;
            return rvm;
        }
        /// <summary>
        /// 标签组编辑标签
        /// </summary>
        /// <param name="Tags"></param>
        /// <param name="tagGroup"></param>
        /// <returns></returns>
        public ReturnValueModel GroupUpdateTags(TagGroupTagViewModel tagGroupTagView)
        {
            List<GroupTagRel> groupTagRels = new List<GroupTagRel>();
            ReturnValueModel rvm = new ReturnValueModel();
            var taggroup = _rep.FirstOrDefault<TagGroup>(p => p.Id == tagGroupTagView.TagGroupId);
            if (taggroup == null)
            {
                rvm.Msg = "This tagGroup doesn't exist";
                rvm.Success = false;
                return rvm;
            }
            var removelist = _rep.Table<GroupTagRel>().Where(p => p.TagGroupId == taggroup.Id).ToList();
            _rep.DeleteList(removelist);
            foreach (var itm in tagGroupTagView.TagsId)
            {
                groupTagRels.Add(new GroupTagRel
                {
                    Id = Guid.NewGuid().ToString(),
                    TagGroupId = tagGroupTagView.TagGroupId,
                    TagId = itm
                });
            }
            _rep.InsertList<GroupTagRel>(groupTagRels);
            _rep.SaveChanges();
            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = groupTagRels;
            return rvm;
        }
        /// <summary>
        ///  编辑时获取标签组和标签明细
        /// </summary>
        /// <param name="tagGroup"></param>
        /// <returns></returns>
        public ReturnValueModel GetGroupUpdateTagsList(TagGroup tagGroup)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var GroupName = _rep.Where<TagGroup>(s => s.Id == tagGroup.Id).FirstOrDefault();
            if (GroupName == null)
            {
                rvm.Success = false;
                rvm.Msg = "the tagGroup is null";
                return rvm;
            }

            string tagchild = string.Empty;
            var Tagchildlist =
                from a in _rep.Table<GroupTagRel>().Where(s => s.IsDeleted != 1 && s.TagGroupId == tagGroup.Id)
                join b in _rep.Table<TagInfo>().Where(s => s.IsDeleted != 1)
                    on a.TagId equals b.Id
                select b;
            foreach (var model in Tagchildlist)
            {
                tagchild += model.TagName + ",";
            }
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                rows = new
                {
                    groupname = GroupName.TagGroupName,
                    tagchild = tagchild.TrimEnd(',')
                }
            };
                 
            return rvm;

        }

        /// <summary>
        /// 删除标签组以及标签
        /// </summary>
        /// <param name="Tags"></param>
        /// <param name="tagGroup"></param>
        /// <returns></returns>
        public ReturnValueModel DeleteTagGroup(TagGroup tagGroup)
        {
            List<GroupTagRel> groupTagRels = new List<GroupTagRel>();
            ReturnValueModel rvm = new ReturnValueModel();
            var taggroup = _rep.FirstOrDefault<TagGroup>(p => p.Id == tagGroup.Id);
            if (taggroup == null)
            {
                rvm.Msg = "This tagGroup doesn't exist";
                rvm.Success = false;
                return rvm;
            }
            taggroup.IsDeleted = 1;
            _rep.Update(taggroup);
            var removelist = _rep.Table<GroupTagRel>().Where(p => p.TagGroupId == tagGroup.Id && p.IsDeleted != 1).ToList();
            removelist.ForEach(p => { p.IsDeleted = 1; _rep.Update(p); });
            _rep.SaveChanges();
            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = groupTagRels;
            return rvm;
        }

        /// <summary>
        /// 根据标签组 获得用户列表和标签明细
        /// </summary>
        /// <param name="tagGroup"></param>
        /// <returns></returns>
        public ReturnValueModel GetTagSearchList(RowNumModel<TagGroupViewModel> tagGroup)
        {
            List<GroupTagRel> groupTagRels = new List<GroupTagRel>();
            ReturnValueModel rvm = new ReturnValueModel();

            //标签组列表
            var groupTaglist= from a in _rep.Table<TagGroup>().Where(s => s.IsDeleted != 1) select a;
            //标签组名称过滤
            if (!string.IsNullOrEmpty(tagGroup.SearchParams.TagGroupName))
            {
                groupTaglist = groupTaglist.Where(s => s.TagGroupName.Contains(tagGroup.SearchParams.TagGroupName));
            }

            var taggrouplist = from a in _rep.Table<TagGroup>().Where(s => s.IsDeleted != 1) select a;

            //标签明细列表
            var tagInfolist = from a in _rep.Table<TagInfo>().Where(s => s.IsDeleted != 1)
                join b in _rep.Table<GroupTagRel>().Where(s => s.IsDeleted != 1 )
                    on a.Id equals b.TagId
                select new
                {
                    TagName=a.TagName,
                    Id=a.Id,
                    TagType=a.TagType,
                    TagGroupId = b.TagGroupId
                };
            if (!string.IsNullOrEmpty(tagGroup.SearchParams.TaggroupId))
            {
                var model = taggrouplist.Where(p => p.Id == tagGroup.SearchParams.TaggroupId).FirstOrDefault();
                tagInfolist = tagInfolist.Where(s => s.TagGroupId == tagGroup.SearchParams.TaggroupId);
                if (model == null)
                {
                    rvm.Msg = "This tagGroup doesn't exist";
                    rvm.Success = false;
                    return rvm;
                }
            }
             
          
           //标签组
            var groupTags = _rep.Where<GroupTagRel>(p => p.TagGroupId == tagGroup.SearchParams.TaggroupId && p.IsDeleted==0).ToList();
            var tagIds = groupTags.Select(p => p.TagId);

            
            var tagslist = from c in _rep.Table<TagInfo>().Where(s => s.IsDeleted != 1 && s.TagType == "D2" && tagIds.Contains(s.Id))
                           join d in _rep.Table<DocTag>().Where(s => s.IsDeleted != 1)
                               on c.Id equals d.TagId
                           select new
                           {
                               DocId = d.DocId,
                               TagName = c.TagName
                           };
            //用户标签列表
            var list = (from a in _rep.Where<WxUserModel>(s => s.IsDeleted != 1 && s.IsCompleteRegister == 1&& tagslist.Select(p=>p.DocId).Contains(s.Id))
                       join b in _rep.Table<DepartmentInfo>() on a.DepartmentName equals b.DepartmentName
                       //join c  in tagslist on  a.Id equals  c.DocId
                       select new DoctorViewModel
                       {
                           Id = a.Id,

                           UserName = a.UserName,   //姓名
                           Mobile = a.Mobile,
                           HospitalName = a.HospitalName, //所属医院
                           DepartmentName = a.DepartmentName,//科室
                           Title = a.Title,
                           Province = a.Province,
                           City = a.City,
                           Area = a.Area,
                           School = a.School,
                           CreateTime = a.CreateTime,
                           IsVerify = a.IsVerify.ToString(),
                           gender = a.WxGender,
                           DocTags = tagslist.Where(s => s.DocId == a.Id).Select(s => s.TagName).ToList(), //标签 
                       });
            //医生信息过滤
            if (!string.IsNullOrEmpty(tagGroup.SearchParams.SearchKeywords))
            {
                var keyword = tagGroup.SearchParams.SearchKeywords;
                list = list.Where(s => s.UserName.Contains(keyword) || s.HospitalName.Contains(keyword) || s.DepartmentName.Contains(keyword));
            }
            
            var total = list.Count();
            var rows = list.OrderByDescending(s => s.CreateTime)
                .ToPaginationList(tagGroup.PageIndex, tagGroup.PageSize);

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total = total,
                rows = new
                {
                    GroupTaglist= groupTaglist,
                    DocList = rows,
                    TagInfolist = tagInfolist
                }
            };

            return rvm;
        }

        /// <summary>
        /// 获取医生手动标签列表
        /// </summary>
        /// <param name="tagGroup"></param>
        /// <returns></returns>
        public ReturnValueModel GetManualTagList(RowNumModel<TagGroupViewModel> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = from a in _rep.Where<TagInfo>(s => s.IsDeleted != 1 && s.TagType == "D2")
                       select new
                       {
                           a.Id,
                           a.TagName,
                           a.CreateTime
                       };

            var total = list.Count();
            var rows = list.OrderByDescending(s => s.CreateTime)
                .ToPaginationList(rowNum.PageIndex, rowNum.PageSize);

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total = total,
                rows = rows
            };

            return rvm;
        }
    }
}
