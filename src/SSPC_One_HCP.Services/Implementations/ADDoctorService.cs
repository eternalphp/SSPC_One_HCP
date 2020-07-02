using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.DoctorModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.OpenXmlFormats.Dml;
using System.Data.Entity.SqlServer;
using SSPC_One_HCP.Core.Utils;


namespace SSPC_One_HCP.Services.Implementations
{
    /// <summary>
    /// 医生
    /// </summary>
    public class ADDoctorService : IADDoctorService
    {
        private readonly IEfRepository _rep;
        private readonly IConfig _config;

        public ADDoctorService(IEfRepository rep, IConfig config)
        {
            _rep = rep;
            _config = config;
        }

        /// <summary>
        /// [废弃]导出医生信息
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ReturnValueModel ExportDoctorList(RowNumModel<WxUserModel> rowNum, List<string> ids = null)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = from a in _rep.Where<WxUserModel>(s => s.IsDeleted != 1 && s.IsCompleteRegister == 1)
                    .Where(rowNum.SearchParams)
                       select new DoctorViewModel
                       {
                           Id = a.Id,
                           PVMId = a.PVMId,
                           YSId = a.yunshi_doctor_id,
                           UserName = a.UserName,
                           Mobile = a.Mobile,
                           CreateTime = a.CreateTime,
                           HospitalName = a.HospitalName,
                           DepartmentName = a.DepartmentName,
                           Title = a.Title,
                           Province = a.Province,
                           City = a.City,
                           Area = a.Area,
                           School = a.School,
                           IsVerify = a.IsVerify == 1 ? "已认证" :
                               a.IsVerify == 2 ? "不确定" :
                               a.IsVerify == 3 ? "认证失败" :
                               a.IsVerify == 4 ? "申诉中" :
                               a.IsVerify == 5 ? "认证中" :
                               a.IsVerify == 6 ? "申诉拒绝" : "未认证"
                       };
            if ((ids?.Count() ?? 0) > 0)
            {
                list = list.Where(x => ids.Contains(x.Id));
            }
            var _path = AppDomain.CurrentDomain.BaseDirectory;
            LoggerHelper.WriteLogInfo("[ExportDoctorList] _path:" + _path);
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            var filename = "\\download\\DoctorInfo_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx";
            LoggerHelper.WriteLogInfo("[ExportDoctorList] filename:" + filename);
            var helper = new ExcelHelper(_path + filename);

            var _listHeader = new List<string>()
            {
                "Id", "PVMId", "YSId", "UserName", "Mobile","CreateTime", "HospitalName", "DepartmentName", "Title", "Province",
                "City", "Area", "School", "IsVerify"
            };
            var isSuccess = helper.Export<DoctorViewModel>(list.ToList(), _listHeader);
            if (isSuccess)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    fileurl = filename
                };
            }
            else
            {
                rvm.Msg = "fail";
                rvm.Success = false;
            }

            return rvm;
        }

        /// <summary>
        /// 医生列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        private List<DoctorViewModel> DoctorList(RowNumModel<DoctorViewModel> rowNum)
        {
            var tagslist = from c in _rep.Table<TagInfo>().Where(s => s.IsDeleted != 1 && s.TagType == "D2")
                           join d in _rep.Table<DocTag>()
                               on c.Id equals d.TagId
                           select new
                           {
                               DocId = d.DocId,
                               TagName = c.TagName
                           };

            string fkLibAppId = _config.GetFkLibAppId();//费卡文库AppId

            var wechatlist = from a in _rep.Table<WechatPublicAccount>()
                             where a.IsDeleted != 1
                             select a;

            var list = from a in _rep.Where<WxUserModel>(s => s.IsDeleted != 1 && s.IsCompleteRegister == 1 && s.IsSalesPerson != 1)
                       join b in _rep.Where<DepartmentInfo>(s => s.IsDeleted != 1) on a.DepartmentName equals b.DepartmentName
                       select new DoctorViewModel
                       {
                           Id = a.Id,
                           PVMId = a.PVMId,
                           YSId = a.yunshi_doctor_id,
                           UserName = a.UserName, //姓名
                           UnionId = a.UnionId,
                           Mobile = a.Mobile,
                           HospitalName = a.HospitalName,
                           HPAddress = a.HPAddress,
                           DepartmentName = a.DepartmentName,
                           Title = a.Title, //职称
                           Province = a.Province,
                           City = a.City,
                           Area = a.Area,
                           School = a.School,
                           CreateTime = a.CreateTime,
                           IsVerify = a.IsVerify.ToString(),
                           gender = a.RegistrationGender,
                           DocTags = tagslist.Where(s => s.DocId == a.Id).Select(s => s.TagName).ToList(),
                           DataSource = (a.SourceType == "1" || a.SourceType == "2" || a.SourceType == "4") ? "2" //平台生成的二维码
                                : (a.SourceType == "3" && a.SourceAppId == fkLibAppId) ? "1" //费卡文库
                                : (a.SourceType == "3" && wechatlist.Count(s => s.AppId == a.SourceAppId) > 0) ? "0" //绑定小程序的公众号
                                : "3",  //其它小程序场景
                           DepartmentID = b.Id,
                           WxScene = a.WxSceneId,
                           WxName = a.WxName,
                           BasicLevelName = a.RegistrationIsBasicLevel == null ? "" : a.RegistrationIsBasicLevel == 1 ? "是" : "否",
                           RegistrationIsBasicLevel = a.RegistrationIsBasicLevel,
                           RegistrationAgea = a.RegistrationAge,
                           RegistrationGender = a.RegistrationGender,
                       };


            if (rowNum.SearchParams != null)
            {
                if (!string.IsNullOrEmpty(rowNum.SearchParams.HospitalName))
                {
                    list = list.Where(s => s.HospitalName.Contains(rowNum.SearchParams.HospitalName));
                }

                if (!string.IsNullOrEmpty(rowNum.SearchParams.DepartmentName))
                {
                    list = list.Where(s => s.DepartmentID == rowNum.SearchParams.DepartmentName);
                }

                if (!string.IsNullOrEmpty(rowNum.SearchParams.UserName))
                {
                    list = list.Where(s => s.UserName.Contains(rowNum.SearchParams.UserName));
                }

                if (!string.IsNullOrEmpty(rowNum.SearchParams.School))
                {
                    list = list.Where(s => s.School.Contains(rowNum.SearchParams.School));
                }

                if (!string.IsNullOrEmpty(rowNum.SearchParams.IsVerify))
                {
                    list = list.Where(s => s.IsVerify == rowNum.SearchParams.IsVerify);
                }

                if (!string.IsNullOrEmpty(rowNum.SearchParams.Province))
                {
                    list = list.Where(s => s.Province.Contains(rowNum.SearchParams.Province));
                }

                if (!string.IsNullOrEmpty(rowNum.SearchParams.City))
                {
                    list = list.Where(s => s.City.Contains(rowNum.SearchParams.City));
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.DataSource))
                {
                    list = list.Where(s => s.DataSource.Equals(rowNum.SearchParams.DataSource));
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.WxName))
                {
                    list = list.Where(s => s.WxName.Contains(rowNum.SearchParams.WxName));
                }
                if (rowNum.SearchParams.IsH5 == 1)
                {
                    list = list.Where(o => o.RegistrationIsBasicLevel != null);
                }



                if (rowNum.SearchParams.DocTags != null)
                {
                    IList<string> docTagsList = rowNum.SearchParams.DocTags;
                    if (docTagsList.Count() > 0)
                    {
                        foreach (var docTag in docTagsList)
                        {
                            list = list.Where(s => s.DocTags.Contains(docTag));
                        }
                    }
                }


            }

            var res = list.ToList();
            res.ForEach(o =>
            {
                o.CreateTime = o.CreateTime.Value.AddHours(8);
            });
            return res;
        }

        /// <summary>
        ///  1、已认证
        /// 2、不确定
        /// 3、认证失败
        /// 4、申诉中
        /// 5、认证中
        /// 6、申诉拒绝
        /// </summary>
        /// <param name="isVerify"></param>
        /// <returns></returns>
        private string ToVerify(string isVerify)
        {
            var result = string.Empty;
            switch (isVerify)
            {
                case "1":
                    result = "已认证";
                    break;
                case "2":
                    result = "不确定";
                    break;
                case "3":
                    result = "认证失败";
                    break;
                case "4":
                    result = "申诉中";
                    break;
                case "5":
                    result = "认证中";
                    break;
                case "6":
                    result = "申诉拒绝";
                    break;
                default:
                    result = isVerify;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 导出医生信息
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ReturnValueModel ExportDoctorList(RowNumModel<DoctorViewModel> rowNum, List<string> ids = null)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var list = DoctorList(rowNum);
            var result = list.Select(a => new DoctorViewModel
            {
                Id = a.Id,
                PVMId = a.PVMId,
                YSId = a.YSId,
                UserName = a.UserName,
                Mobile = a.Mobile,
                HospitalName = a.HospitalName,
                DepartmentName = a.DepartmentName,
                Title = a.Title,
                Province = a.Province,
                City = a.City,
                Area = a.Area,
                School = a.School,
                IsVerify = ToVerify(a.IsVerify),
                WxScene = Core.WxScene.WxSceneList.Keys.Contains(a.WxScene) ? Core.WxScene.WxSceneList[a.WxScene] : a.WxScene,
                CreateTime = a.CreateTime,
                BasicLevelName = a.RegistrationIsBasicLevel == null ? "" : a.RegistrationIsBasicLevel == 1 ? "是" : "否",
                RegistrationAgea = a.RegistrationAgea,
                UnionId = a.UnionId,
            });

            if ((ids?.Count() ?? 0) > 0)
            {
                result = result.Where(x => ids.Contains(x.Id));
            }
            var _path = AppDomain.CurrentDomain.BaseDirectory;
            LoggerHelper.WriteLogInfo("[ExportDoctorList] _path:" + _path);
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            var filename = "\\download\\DoctorInfo_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx";
            LoggerHelper.WriteLogInfo("[ExportDoctorList] filename:" + filename);
            var helper = new ExcelHelper(_path + filename);

            var _listHeader = new List<string>()
            {
                "Id","UnionId", "PVMId", "YSId", "UserName", "Mobile", "HospitalName", "DepartmentName", "Title", "Province",
                "City", "School", "IsVerify","WxScene","CreateTime","BasicLevelName","RegistrationAgea"
            };
            var isSuccess = helper.Export<DoctorViewModel>(result.ToList(), _listHeader);
            if (isSuccess)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    fileurl = filename
                };
            }
            else
            {
                rvm.Msg = "fail";
                rvm.Success = false;
            }

            return rvm;
        }

        /// <summary>
        /// 获取医生列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel GetDoctorList(RowNumModel<DoctorViewModel> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var list = DoctorList(rowNum);
            var total = list.Count();
            var rows = list.OrderByDescending(s => s.CreateTime)
                .ToPaginationList(rowNum.PageIndex, rowNum.PageSize);

            rows = rows.Select(a =>
                new DoctorViewModel
                {
                    Id = a.Id,
                    PVMId = a.PVMId,
                    YSId = a.YSId,
                    UserName = a.UserName, //姓名
                    Mobile = a.Mobile,
                    HospitalName = a.HospitalName,
                    HPAddress = a.HPAddress,
                    DepartmentName = a.DepartmentName,
                    Title = a.Title, //职称
                    Province = a.Province,
                    City = a.City,
                    Area = a.Area,
                    School = a.School,
                    CreateTime = a.CreateTime,
                    IsVerify = a.IsVerify,
                    gender = a.gender,
                    DocTags = a.DocTags,
                    DataSource = a.DataSource,//其它小程序场景
                    DepartmentID = a.DepartmentID,
                    WxScene = Core.WxScene.WxSceneList.Keys.Contains(a.WxScene) ? Core.WxScene.WxSceneList[a.WxScene] : a.WxScene,
                    BasicLevelName = a.BasicLevelName,
                    RegistrationAgea = a.RegistrationAgea
                }).ToList();
            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total = total,
                rows = rows
            };

            return rvm;
        }



        private string GetDocTagById(string docId)
        {
            var list = from a in _rep.Table<TagInfo>()
                       join b in _rep.Table<DocTag>().Where(s => s.DocId == docId)
                           on a.Id equals b.TagId
                       select new
                       {
                           TagName = a.TagName
                       };
            string docTagNames = string.Empty;
            foreach (var model in list)
            {
                docTagNames += model.TagName + ",";
            }

            return docTagNames.TrimEnd(',');
        }

        /// <summary>
        /// 获取医生详情
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel GetDoctorDetail(DoctorViewModel viewModel)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (string.IsNullOrEmpty(viewModel?.Id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }

            var wxUser = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.Id == viewModel.Id);

            if (wxUser == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid Id.";
                return rvm;
            }
            wxUser.WxGender = string.IsNullOrEmpty(wxUser.RegistrationGender) ? wxUser.WxGender : wxUser.RegistrationGender;

            //遍历更新图片路径
            if (!string.IsNullOrEmpty(wxUser.Pictures))
            {
                string picUrls = wxUser.Pictures;
                string picurlstr = string.Empty;
                if (picUrls.IndexOf('|') > -1)
                {
                    foreach (var val in picUrls.Split('|'))
                    {
                        picurlstr += val.TrimStart('/') + "|";
                    }
                }

                wxUser.Pictures = picurlstr.TrimEnd('|');
            }

            //标签组
            var taggroups = (from a in _rep.Table<TagGroup>().Where(s => s.IsDeleted != 1)
                             join b in _rep.Table<GroupTagRel>().Where(s => s.IsDeleted != 1)
                                 on a.Id equals b.TagGroupId
                             join c in _rep.Table<DocTag>().Where(s => s.IsDeleted != 1 && s.DocId == viewModel.Id)
                                 on b.TagId equals c.TagId
                             select a.TagGroupName).Distinct();

            //医生的手动标签
            var tags = from a in _rep.Where<DocTag>(s => s.IsDeleted != 1)
                       join b in _rep.Where<TagInfo>(s => s.IsDeleted != 1 && s.TagType == "D2") on a.TagId equals b.Id
                       where a.DocId == viewModel.Id
                       select b.TagName;


            //医生自动标签  文章，文档，播客，视频 | 1，2，3，4

            string ObjectTypeNameStr = "文章,文档,播客,视频";
            string ObjectTypeStr = "1,2,3,4";
            List<string> autotaglist = new List<string>();
            for (int i = 0; i < ObjectTypeNameStr.Split(',').Length; i++)
            {
                string tagname = GetStudyTag(
                    Convert.ToInt32(ObjectTypeStr.Split(',')[i]), ObjectTypeNameStr.Split(',')[i], viewModel.Id
                );
                if (!string.IsNullOrEmpty(tagname))
                {
                    autotaglist.Add(tagname);
                }
            }

            string fkLibAppId = _config.GetFkLibAppId();//费卡文库AppId

            var wechatlist = from a in _rep.Table<WechatPublicAccount>()
                             where a.IsDeleted != 1
                             select a;

            rvm.Msg = "success";
            rvm.Success = true;

            rvm.Result = new
            {
                doctor = wxUser,
                DataSource = (wxUser.SourceType == "1" || wxUser.SourceType == "2" || wxUser.SourceType == "4") ? "2" //平台生成的二维码 
                    : (wxUser.SourceType == "3" && wxUser.SourceAppId == fkLibAppId) ? "1" //费卡文库
                    : (wxUser.SourceType == "3" && wechatlist.Where(s => s.AppId == wxUser.SourceAppId).Count() > 0) ? "0" //绑定小程序的公众号
                    : "3", //其它小程序场景
                tags,
                WxScene = Core.WxScene.WxSceneList.Keys.Contains(wxUser.WxSceneId) ? Core.WxScene.WxSceneList[wxUser.WxSceneId] : wxUser.WxSceneId,
                autotags = autotaglist,
                taggroups = taggroups
            };

            return rvm;
        }

        /// <summary>
        /// 获取学习的标签 
        /// </summary>
        /// <param name="LObjectType"></param>
        /// <param name="ObjectType"></param>
        /// <param name="wxuserid"></param>
        /// <returns></returns>

        protected string GetStudyTag(int LObjectType, string ObjectTypeName, string wxuserid)
        {
            string ObjectTypeStr = "文章,文档,播客,视频";
            var autotagslist = from a in _rep.Table<WxUserModel>().Where(s => s.IsDeleted != 1 && s.Id == wxuserid)
                               join b in _rep.Table<MyLRecord>().Where(s => s.IsDeleted != 1 && s.LObjectType == LObjectType)
                                   on a.Id equals b.WxUserId
                               select b;
            string StudyTag = string.Empty;

            int tenTag = 10;
            int thirtyTag = 30;
            int fiftyTag = 50;
            if (autotagslist.Count() > 0)
            {
                //转化成小时
                var studymin = autotagslist.Sum(s => s.LObjectDate) / 3600;


                if (studymin >= tenTag)
                {
                    StudyTag = ObjectTypeName + "_" + tenTag.ToString() + "h";
                }
                if (studymin >= thirtyTag)
                {
                    StudyTag = ObjectTypeName + "_" + thirtyTag.ToString() + "h";
                }
                if (studymin >= fiftyTag)
                {
                    StudyTag = ObjectTypeName + "_" + fiftyTag.ToString() + "h";
                }
            }

            return StudyTag;
        }

        /// <summary>
        /// 给医生加手动标签
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public ReturnValueModel UpdateTagsOfDoctor(DoctorTagView viewModel, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (string.IsNullOrEmpty(viewModel.DocId))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'DocId' is required.";
                return rvm;
            }

            var existUser = _rep.Where<WxUserModel>(s => s.IsDeleted != 1 && s.Id == viewModel.DocId).Any();
            if (!existUser)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid DocId.";
                return rvm;
            }

            var tags = _rep.Where<DocTag>(s => s.DocId == viewModel.DocId);
            _rep.DeleteList(tags);
            _rep.SaveChanges();

            if (viewModel.Tags != null)
            {
                foreach (string tagName in viewModel.Tags)
                {
                    TagInfo tagModel = _rep.FirstOrDefault<TagInfo>(s =>
                        s.IsDeleted != 1 && s.TagType == "D2" && s.TagName == tagName);
                    DocTag docTagModel = null;

                    if (tagModel == null)
                    {
                        tagModel = new TagInfo
                        {
                            Id = Guid.NewGuid().ToString(),
                            TagName = tagName,
                            TagType = "D2", //医生手动标签
                            CreateTime = DateTime.Now,
                            CreateUser = workUser.User.Id
                        };
                        _rep.Insert(tagModel);
                        _rep.SaveChanges();
                    }
                    else
                    {
                        docTagModel = _rep.FirstOrDefault<DocTag>(s =>
                            s.IsDeleted != 1 && s.DocId == viewModel.DocId && s.TagId == tagModel.Id);
                    }

                    if (docTagModel == null)
                    {
                        docTagModel = new DocTag
                        {
                            Id = Guid.NewGuid().ToString(),
                            DocId = viewModel.DocId,
                            TagId = tagModel.Id,
                            CreateTime = DateTime.Now,
                            CreateUser = workUser.User.Id
                        };
                        _rep.Insert(docTagModel);
                        _rep.SaveChanges();
                    }
                }
            }


            //医生的手动标签
            var tags1 = from a in _rep.Where<DocTag>(s => s.IsDeleted != 1)
                        join b in _rep.Where<TagInfo>(s => s.IsDeleted != 1 && s.TagType == "D2") on a.TagId equals b.Id
                        where a.DocId == viewModel.DocId
                        select b.TagName;

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                viewModel.DocId,
                Tags = tags1
            };

            return rvm;
        }

        /// <summary>
        /// 新增或编辑标签
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateTagInfo(TagInfo viewModel, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (string.IsNullOrEmpty(viewModel.TagName))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'TagName' is required.";
                return rvm;
            }

            var dataTageName = _rep.FirstOrDefault<TagInfo>(s =>
                    s.IsDeleted != 1 && s.TagType == "D2" && s.TagName == viewModel.TagName);

            if (dataTageName == null)
            {
                if (string.IsNullOrEmpty(viewModel.Id))
                {
                    viewModel.Id = Guid.NewGuid().ToString();
                    viewModel.TagType = "D2"; //医生手动标签
                    viewModel.CreateTime = DateTime.Now;
                    viewModel.CreateUser = workUser.User.Id;
                    _rep.Insert(viewModel);
                }
                else
                {
                    var data = _rep.FirstOrDefault<TagInfo>(s =>
                     s.IsDeleted != 1 && s.TagType == "D2" && s.Id == viewModel.Id);
                    data.TagName = viewModel.TagName;
                    data.UpdateTime = DateTime.Now;
                    data.UpdateUser = workUser.User.Id;
                    _rep.Update(data);
                }
            }
            else
            {
                rvm.Success = false;
                rvm.Msg = "the groupname is exist";
                return rvm;
            }
            var result = _rep.SaveChanges();
            rvm.Msg = "success";
            rvm.Success = true;
            return rvm;
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public ReturnValueModel DeleteTagInfo(TagInfo viewModel, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (string.IsNullOrEmpty(viewModel.Id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }

            TagInfo tagModel =
                _rep.FirstOrDefault<TagInfo>(s => s.IsDeleted != 1 && s.TagType == "D2" && s.Id == viewModel.Id);
            if (tagModel != null)
            {
                tagModel.IsDeleted = 1;
                _rep.Update(tagModel);
                _rep.SaveChanges();
            }

            rvm.Msg = "success";
            rvm.Success = true;

            return rvm;
        }

        /// <summary>
        /// 获取医生手动标签列表
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetTagList()
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var tags = from a in _rep.Where<TagInfo>(s => s.IsDeleted != 1 && s.TagType == "D2")
                       select new
                       {
                           a.Id,
                           a.TagName
                       };

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                tags
            };
            return rvm;
        }

        /// <summary>
        /// 获取医生学习时间
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel GetDoctorLearn(RowNumModel<DoctorLearnViewModel> rowNum, bool IsExported = false)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var msus = _rep.Where<MeetSignUp>(s => s.IsSignIn == 1 && s.IsDeleted != 1);
            var meetcount = _rep.Where<MyMeetOrder>(s => s.IsDeleted != 1);

            var tagslist = from c in _rep.Table<TagInfo>().Where(s => s.IsDeleted != 1 && s.TagType == "D2")
                           join d in _rep.Table<DocTag>()
                               on c.Id equals d.TagId
                           select new
                           {
                               DocId = d.DocId,
                               TagName = c.TagName
                           };
            //临床指南
            var guidlist = _rep.Where<GuidVisit>(s => s.IsDeleted != 1);
            //用药参考
            var medicinelist = _rep.Where<WechatActionHistory>(s => s.IsDeleted != 1 && s.ActionType == 1);
            //期刊查询
            var booklist = _rep.Where<WechatActionHistory>(s => s.IsDeleted != 1 && s.ActionType == 2);
            //文章
            var record1 = _rep.Where<MyLRecord>(s => s.IsDeleted != 1 && s.LObjectType == 1);
            //文档
            var record2 = _rep.Where<MyLRecord>(s => s.IsDeleted != 1 && s.LObjectType == 2);
            //播客
            var record3 = _rep.Where<MyLRecord>(s => s.IsDeleted != 1 && s.LObjectType == 3);
            //视频
            var record4 = _rep.Where<MyLRecord>(s => s.IsDeleted != 1 && s.LObjectType == 4);
            //会议
            var record5 = _rep.Where<MyLRecord>(s => s.IsDeleted != 1 && s.LObjectType == 5);
            //录播
            var record9 = _rep.Where<MyLRecord>(s => s.IsDeleted != 1 && s.LObjectType == 9);

            if (rowNum.SearchParams.Learn_Start != null || rowNum.SearchParams.Learn_End != null)
            {
                var StartTime = rowNum.SearchParams.Learn_Start.Value.AddSeconds(1);
                var EndTime = rowNum.SearchParams.Learn_End.Value.AddDays(1).AddSeconds(-1);
                guidlist = guidlist.Where(s => s.CreateTime >= StartTime && s.CreateTime <= EndTime);
                medicinelist = medicinelist.Where(s => s.CreateTime >= StartTime && s.CreateTime <= EndTime);
                booklist = booklist.Where(s => s.CreateTime >= StartTime && s.CreateTime <= EndTime);
                //文章
                record1 = record1.Where(s => s.CreateTime >= StartTime && s.CreateTime <= EndTime);
                //文档
                record2 = record2.Where(s => s.CreateTime >= StartTime && s.CreateTime <= EndTime);
                //播客
                record3 = record3.Where(s => s.CreateTime >= StartTime && s.CreateTime <= EndTime);
                //视频
                record4 = record4.Where(s => s.CreateTime >= StartTime && s.CreateTime <= EndTime);
                //会议
                record5 = record5.Where(s => s.CreateTime >= StartTime && s.CreateTime <= EndTime);
                //录播
                record9 = record9.Where(s => s.CreateTime >= StartTime && s.CreateTime <= EndTime);
            }
            var wechatlist = _rep.Where<WechatPublicAccount>(s => s.IsDeleted != 1);

            string fkLibAppId = _config.GetFkLibAppId();//费卡文库AppId

            var list = (from a in _rep.Where<WxUserModel>(s => s.IsDeleted != 1 && s.IsCompleteRegister == 1 && (s.IsSalesPerson ?? 0) != 1)
                        select new DoctorLearnViewModel
                        {
                            Id = a.Id,
                            DoctorName = a.UserName,
                            HospitalName = a.HospitalName,
                            DepartmentName = a.DepartmentName,
                            Province = a.Province,
                            School = a.School,
                            Title = a.Title,
                            DataSource = (a.SourceType == "1" || a.SourceType == "2" || a.SourceType == "4") ? "2" //平台生成的二维码
                      : (a.SourceType == "3" && a.SourceAppId == fkLibAppId) ? "1" //费卡文库
                      : (a.SourceType == "3" && wechatlist.Count(s => s.AppId == a.SourceAppId) > 0) ? "0" //绑定小程序的公众号
                      : "3", //其它小程序场景
                            IsVerify = a.IsVerify.ToString(),
                            //手动标签
                            DocTags = tagslist.Where(s => s.DocId == a.Id).Select(s => s.TagName).ToList(),
                            //Lable = tagslist.Where(s => s.DocId == a.Id).Select(s => s.TagName).ToList(),
                            //产品资料
                            DocLearnTime = record2.Where(o => o.WxUserId == a.Id).Sum(o => o.LObjectDate) ?? 0,
                            VideoLearnTime = record4.Where(o => o.WxUserId == a.Id).Sum(o => o.LObjectDate) ?? 0,
                            ArticleLearnTime = record1.Where(o => o.WxUserId == a.Id).Sum(o => o.LObjectDate) ?? 0,
                            //播客
                            PodcastLearnTime = record3.Where(o => o.WxUserId == a.Id).Sum(o => o.LObjectDate) ?? 0,
                            //会议次数
                            MeetCount = msus.Count(s => s.SignUpUserId == a.Id)
                                      + meetcount.Count(s => s.CreateUser == a.Id),
                            GuidVistTime = guidlist.Where(s => s.userid == a.Id).Sum(s => s.StaySeconds) ?? 0,
                            MedicineVistTime = medicinelist.Where(s => s.WxuserId == a.Id).Sum(s => s.StaySeconds) ?? 0,
                            BookVisitTime = booklist.Where(s => s.WxuserId == a.Id).Sum(s => s.StaySeconds) ?? 0,
                            BroadcastTime = record9.Where(o => o.WxUserId == a.Id).Sum(o => o.LObjectDate) ?? 0,
                        }).Where(s => s.DoctorName != null && s.DoctorName != "");
            if (rowNum.SearchParams != null)
            {
                if (!string.IsNullOrEmpty(rowNum.SearchParams.HospitalName))
                {
                    list = list.Where(s => s.HospitalName.Contains(rowNum.SearchParams.HospitalName));
                }

                if (!string.IsNullOrEmpty(rowNum.SearchParams.DepartmentName))
                {
                    list = list.Where(s => s.DepartmentName == rowNum.SearchParams.DepartmentName);
                }

                if (!string.IsNullOrEmpty(rowNum.SearchParams.DoctorName))
                {
                    list = list.Where(s => s.DoctorName.Contains(rowNum.SearchParams.DoctorName));
                }

                if (!string.IsNullOrEmpty(rowNum.SearchParams.School))
                {
                    list = list.Where(s => s.School.Contains(rowNum.SearchParams.School));
                }

                if (!string.IsNullOrEmpty(rowNum.SearchParams.IsVerify))
                {
                    list = list.Where(s => s.IsVerify == rowNum.SearchParams.IsVerify);
                }

                if (!string.IsNullOrEmpty(rowNum.SearchParams.Province))
                {
                    list = list.Where(s => s.Province.Contains(rowNum.SearchParams.Province));
                }

                if (!string.IsNullOrEmpty(rowNum.SearchParams.DataSource))
                {
                    list = list.Where(s => s.DataSource == rowNum.SearchParams.DataSource);
                }

                if (rowNum.SearchParams.DocTags != null)
                {
                    IList<string> docTagsList = rowNum.SearchParams.DocTags;
                    if (docTagsList.Count() > 0)
                    {
                        foreach (var docTag in docTagsList)
                        {
                            list = list.Where(s => s.DocTags.Contains(docTag));
                        }
                    }
                }
            }


            if (IsExported)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = list.OrderBy(s => s.DoctorName).ToList();
                return rvm;
            }
            else
            {
                var total = list.Count();
                var rows = list.OrderBy(s => s.DoctorName).ToPaginationList(rowNum.PageIndex, rowNum.PageSize);
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

        /// <summary>
        ///  获取医生学习记录列表
        /// </summary>
        /// <param name="wxUserID"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel GetStudyList(RowNumModel<DoctorDetailViewModel> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                //个人学习记录
                var myLRecordlist =
                    from a in _rep.Table<MyLRecord>().OrderByDescending(s => s.LDateStart)
                        .Where(s => s.WxUserId == rowNum.SearchParams.wxuserid && s.IsDeleted != 1)
                    select new
                    {
                        dataday = SqlFunctions.DateAdd("Day", 0, SqlFunctions.DatePart("year", a.CreateTime) + "-" +
                                                                (SqlFunctions.DatePart("month", a.CreateTime) < 10 ?
                                                                    ("0" + SqlFunctions.DatePart("month", a.CreateTime)) : SqlFunctions.StringConvert((decimal)SqlFunctions.DatePart("month", a.CreateTime))) + "-" +
                                                                (SqlFunctions.DatePart("day", a.CreateTime) < 10 ?
                                                                    ("0" + SqlFunctions.DatePart("day", a.CreateTime)) : SqlFunctions.StringConvert((decimal)SqlFunctions.DatePart("day", a.CreateTime))
                                                                )),
                        LDateStart = a.LDateStart,
                        LDateEnd = a.LDateEnd,
                        LObjectId = a.LObjectId,
                        LObjectDate = a.LObjectDate,
                        LObjectType = a.LObjectType,
                        Createtime = a.CreateTime,
                        ID = a.Id,
                    };

                //用药和期刊查询
                var wechatlist = from a in _rep.Table<WechatActionHistory>().Where(s => s.IsDeleted != 1 && s.WxuserId == rowNum.SearchParams.wxuserid)
                                 select new
                                 {

                                     dataday = SqlFunctions.DateAdd("Day", 0, SqlFunctions.DatePart("year", a.CreateTime) + "-" +
                                                                              (SqlFunctions.DatePart("month", a.CreateTime) < 10 ?
                                                                                  ("0" + SqlFunctions.DatePart("month", a.CreateTime)) : SqlFunctions.StringConvert((decimal)SqlFunctions.DatePart("month", a.CreateTime))) + "-" +
                                                                              (SqlFunctions.DatePart("day", a.CreateTime) < 10 ?
                                                                                  ("0" + SqlFunctions.DatePart("day", a.CreateTime)) : SqlFunctions.StringConvert((decimal)SqlFunctions.DatePart("day", a.CreateTime))
                                                                              )),
                                     Studytime = a.StaySeconds,
                                     Title = (a.ActionType == 1 ? "用药参考：" : "期刊查询：") + a.Content,
                                     CreateTime = a.CreateTime,
                                     ActionType = a.ActionType,
                                     ID = a.Id
                                 };

                var medicinelist = wechatlist.Where(s => s.ActionType == 1);
                var booklist = wechatlist.Where(s => s.ActionType == 2);

                //临床指南
                var guidlist = from a in _rep.Table<GuidVisit>()
                        .Where(s => s.IsDeleted != 1 && s.userid == rowNum.SearchParams.wxuserid)
                               select new
                               {
                                   dataday = SqlFunctions.DateAdd("Day", 0, SqlFunctions.DatePart("year", a.CreateTime) + "-" +
                                                                            (SqlFunctions.DatePart("month", a.CreateTime) < 10 ?
                                                                                ("0" + SqlFunctions.DatePart("month", a.CreateTime)) : SqlFunctions.StringConvert((decimal)SqlFunctions.DatePart("month", a.CreateTime))) + "-" +
                                                                            (SqlFunctions.DatePart("day", a.CreateTime) < 10 ?
                                                                                ("0" + SqlFunctions.DatePart("day", a.CreateTime)) : SqlFunctions.StringConvert((decimal)SqlFunctions.DatePart("day", a.CreateTime))
                                                                            )),
                                   Studytime = a.StaySeconds,
                                   Title = "临床指南：" + a.ActionType + "-" + a.GuideName,
                                   CreateTime = a.VisitStart,
                                   ID = a.Id,
                               };

                if (rowNum.SearchParams != null)
                {
                    DateTime? begindate =
                        Convert.ToDateTime(rowNum.SearchParams.begin_date.ToString("yyyy-MM-dd") + " 00:00:00");
                    DateTime? enddate =
                        Convert.ToDateTime(rowNum.SearchParams.end_date.ToString("yyyy-MM-dd") + " 23:59:59");
                    myLRecordlist = myLRecordlist.Where(s =>
                        s.Createtime >= begindate && s.Createtime <= enddate);
                    if (!string.IsNullOrEmpty(rowNum.SearchParams.MediaType))
                    {
                        int MediaType = Convert.ToInt32(rowNum.SearchParams.MediaType);
                        myLRecordlist = myLRecordlist.Where(s => s.LObjectType == MediaType);
                    }
                    else
                    {
                        myLRecordlist = myLRecordlist.Where(s => s.LObjectType != 9);
                    }
                    //临床指南
                    guidlist = guidlist.Where(s => s.CreateTime >= begindate && s.CreateTime <= enddate);
                    //用药参考
                    medicinelist = medicinelist.Where(s => s.CreateTime >= begindate && s.CreateTime <= enddate);
                    //期刊查询
                    booklist = booklist.Where(s => s.CreateTime >= begindate && s.CreateTime <= enddate);
                }


                //个人学习记录 1.关联会议信息  2.学习资料 3.WechatActionHistory  4.临床指南 
                var allList = (from a in myLRecordlist
                               join b in _rep.Table<MeetInfo>().Where(s => s.IsDeleted != 1)
                                   on a.LObjectId equals b.Id
                               select new
                               {
                                   DateBegin = (SqlFunctions.DatePart("HOUR", a.LDateStart) < 10 ? "0" + SqlFunctions.DateName("HOUR", a.LDateStart) : SqlFunctions.DateName("HOUR", a.LDateStart)) + ":" + (SqlFunctions.DatePart("MINUTE", a.LDateStart) < 10 ? "0" + SqlFunctions.DateName("MINUTE", a.LDateStart) : SqlFunctions.DateName("MINUTE", a.LDateStart)),
                                   Studytime = a.LObjectDate,
                                   dataday = a.dataday,
                                   Title = b.MeetTitle,
                                   ID = a.ID

                               }).Union(
                    from a in myLRecordlist
                    join b in _rep.Table<DataInfo>().Where(s => s.IsDeleted != 1)
                        on a.LObjectId equals b.Id
                    select new
                    {
                        DateBegin = (SqlFunctions.DatePart("HOUR", a.LDateStart) < 10 ? "0" + SqlFunctions.DateName("HOUR", a.LDateStart) : SqlFunctions.DateName("HOUR", a.LDateStart)) + ":" + (SqlFunctions.DatePart("MINUTE", a.LDateStart) < 10 ? "0" + SqlFunctions.DateName("MINUTE", a.LDateStart) : SqlFunctions.DateName("MINUTE", a.LDateStart)),
                        Studytime = a.LObjectDate,
                        dataday = a.dataday,
                        Title = b.Title,
                        ID = a.ID
                    }
                );


                //根据类型判断是否要用药，期刊或者临床指南
                if (rowNum.SearchParams != null)
                {

                    switch (rowNum.SearchParams.MediaType)
                    {

                        case "":
                            allList = allList.Union(
                                from a in guidlist
                                select new
                                {
                                    DateBegin =
                                        (SqlFunctions.DatePart("HOUR", a.CreateTime) < 10
                                            ? "0" + SqlFunctions.DateName("HOUR", a.CreateTime)
                                            : SqlFunctions.DateName("HOUR", a.CreateTime)) + ":" +
                                        (SqlFunctions.DatePart("MINUTE", a.CreateTime) < 10
                                            ? "0" + SqlFunctions.DateName("MINUTE", a.CreateTime)
                                            : SqlFunctions.DateName("MINUTE", a.CreateTime)),
                                    Studytime = a.Studytime,
                                    dataday = a.dataday,
                                    Title = a.Title,
                                    ID = a.ID
                                }
                            ).Union(
                                from a in medicinelist
                                select new
                                {
                                    DateBegin =
                                        (SqlFunctions.DatePart("HOUR", a.CreateTime) < 10
                                            ? "0" + SqlFunctions.DateName("HOUR", a.CreateTime)
                                            : SqlFunctions.DateName("HOUR", a.CreateTime)) + ":" +
                                        (SqlFunctions.DatePart("MINUTE", a.CreateTime) < 10
                                            ? "0" + SqlFunctions.DateName("MINUTE", a.CreateTime)
                                            : SqlFunctions.DateName("MINUTE", a.CreateTime)),
                                    Studytime = a.Studytime,
                                    dataday = a.dataday,
                                    Title = a.Title,
                                    ID = a.ID
                                }
                            ).Union(
                                from a in booklist
                                select new
                                {
                                    DateBegin =
                                        (SqlFunctions.DatePart("HOUR", a.CreateTime) < 10
                                            ? "0" + SqlFunctions.DateName("HOUR", a.CreateTime)
                                            : SqlFunctions.DateName("HOUR", a.CreateTime)) + ":" +
                                        (SqlFunctions.DatePart("MINUTE", a.CreateTime) < 10
                                            ? "0" + SqlFunctions.DateName("MINUTE", a.CreateTime)
                                            : SqlFunctions.DateName("MINUTE", a.CreateTime)),
                                    Studytime = a.Studytime,
                                    dataday = a.dataday,
                                    Title = a.Title,
                                    ID = a.ID
                                }
                            );
                            break;
                        case "6":
                            allList = allList.Union(
                                from a in guidlist
                                select new
                                {
                                    DateBegin =
                                        (SqlFunctions.DatePart("HOUR", a.CreateTime) < 10
                                            ? "0" + SqlFunctions.DateName("HOUR", a.CreateTime)
                                            : SqlFunctions.DateName("HOUR", a.CreateTime)) + ":" +
                                        (SqlFunctions.DatePart("MINUTE", a.CreateTime) < 10
                                            ? "0" + SqlFunctions.DateName("MINUTE", a.CreateTime)
                                            : SqlFunctions.DateName("MINUTE", a.CreateTime)),
                                    Studytime = a.Studytime,
                                    dataday = a.dataday,
                                    Title = a.Title,
                                    ID = a.ID
                                }
                                    );
                            break;
                        case "7":
                            allList = allList.Union(
                                from a in medicinelist
                                select new
                                {
                                    DateBegin =
                                        (SqlFunctions.DatePart("HOUR", a.CreateTime) < 10
                                            ? "0" + SqlFunctions.DateName("HOUR", a.CreateTime)
                                            : SqlFunctions.DateName("HOUR", a.CreateTime)) + ":" +
                                        (SqlFunctions.DatePart("MINUTE", a.CreateTime) < 10
                                            ? "0" + SqlFunctions.DateName("MINUTE", a.CreateTime)
                                            : SqlFunctions.DateName("MINUTE", a.CreateTime)),
                                    Studytime = a.Studytime,
                                    dataday = a.dataday,
                                    Title = a.Title,
                                    ID = a.ID
                                }
                            );
                            break;
                        case "8":
                            allList = allList.Union(
                                from a in booklist
                                select new
                                {
                                    DateBegin =
                                        (SqlFunctions.DatePart("HOUR", a.CreateTime) < 10
                                            ? "0" + SqlFunctions.DateName("HOUR", a.CreateTime)
                                            : SqlFunctions.DateName("HOUR", a.CreateTime)) + ":" +
                                        (SqlFunctions.DatePart("MINUTE", a.CreateTime) < 10
                                            ? "0" + SqlFunctions.DateName("MINUTE", a.CreateTime)
                                            : SqlFunctions.DateName("MINUTE", a.CreateTime)),
                                    Studytime = a.Studytime,
                                    dataday = a.dataday,
                                    Title = a.Title,
                                    ID = a.ID
                                }
                            );
                            break;
                        default: break;
                    }

                }


                //学习时间倒序显示
                allList = allList.OrderByDescending(s => s.dataday).ThenByDescending(s => s.DateBegin);

                var aaa = from b in allList
                          group b by b.dataday
                    into g
                          select new
                          {
                              Date = g.Key,
                              children = allList.Where(s => s.dataday == g.Key)
                          };
                rvm.Success = true;
                rvm.Result = new
                {
                    rows = aaa.OrderByDescending(s => s.Date)
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
        ///  获取用户的模块浏览记录
        /// </summary>
        /// <param name="wxUserID"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel GetVisitModulesList(RowNumModel<DoctorDetailViewModel> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {

                var myvisitList =
                   (from a in _rep.Table<VisitModules>().OrderByDescending(s => s.VisitStart)
                       .Where(s => s.WxUserid == rowNum.SearchParams.wxuserid && s.IsDeleted != 1)
                       .OrderBy(s => s.CreateTime)
                    join b in _rep.Table<VisitModulesName>().Where(s => s.IsDeleted != 1) on a.ModulePageNo equals b
                        .ModulesNo
                    select new
                    {
                        dataday = SqlFunctions.DateAdd("Day", 0, SqlFunctions.DatePart("year", a.CreateTime) + "-" +
                                                                 (SqlFunctions.DatePart("month", a.CreateTime) < 10 ?
                                                                     ("0" + SqlFunctions.DatePart("month", a.CreateTime)) : SqlFunctions.StringConvert((decimal)SqlFunctions.DatePart("month", a.CreateTime))) + "-" +
                                                                 (SqlFunctions.DatePart("day", a.CreateTime) < 10 ?
                                                                     ("0" + SqlFunctions.DatePart("day", a.CreateTime)) : SqlFunctions.StringConvert((decimal)SqlFunctions.DatePart("day", a.CreateTime))
                                                                 )),
                        VisitStart = a.VisitStart,
                        VisitEnd = a.VisitEnd,
                        StaySeconds = a.StaySeconds,
                        Createtime = a.CreateTime,
                        ModuleName = b.ModulesName
                    }).Union(
                       from a in _rep.Table<GuidVisit>().OrderBy(s => s.CreateTime).Where(s => s.userid == rowNum.SearchParams.wxuserid && s.IsDeleted != 1)
                       select new
                       {
                           dataday = SqlFunctions.DateAdd("Day", 0, SqlFunctions.DatePart("year", a.CreateTime) + "-" +
                                                                    (SqlFunctions.DatePart("month", a.CreateTime) < 10 ?
                                                                        ("0" + SqlFunctions.DatePart("month", a.CreateTime)) : SqlFunctions.StringConvert((decimal)SqlFunctions.DatePart("month", a.CreateTime))) + "-" +
                                                                    (SqlFunctions.DatePart("day", a.CreateTime) < 10 ?
                                                                        ("0" + SqlFunctions.DatePart("day", a.CreateTime)) : SqlFunctions.StringConvert((decimal)SqlFunctions.DatePart("day", a.CreateTime))
                                                                    )),
                           VisitStart = a.VisitStart,
                           VisitEnd = a.VisitEnd,
                           StaySeconds = a.StaySeconds.Value,
                           Createtime = a.CreateTime,
                           ModuleName = "临床指南"
                       }
                       ).Distinct();
                if (rowNum.SearchParams != null)
                {
                    DateTime begindate =
                        Convert.ToDateTime(rowNum.SearchParams.begin_date.ToString("yyyy-MM-dd") + " 00:00:00");
                    DateTime enddate =
                        Convert.ToDateTime(rowNum.SearchParams.end_date.ToString("yyyy-MM-dd") + " 23:59:59");
                    myvisitList = myvisitList.Where(s =>
                        s.Createtime >= begindate && s.Createtime <= enddate);
                }

                //年月日信息
                var datelist = from a in myvisitList
                               group a by new { a.dataday }
                    into g
                               select new
                               {
                                   showdate = g.Key.dataday
                               };


                var listtemp = myvisitList.GroupBy(s => s.dataday);

                var rows = from b in myvisitList
                           group b by b.dataday
                    into g
                           select new
                           {
                               Date = g.Key,
                               children = myvisitList.Where(s => s.dataday == g.Key).OrderByDescending(s => s.VisitStart)
                           };
                rvm.Success = true;
                rvm.Result = new
                {
                    rows = rows.OrderByDescending(s => s.Date)
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
        /// 删除用户信息 IsDeleted=1
        /// </summary>
        /// <param name="wxuserids"></param>
        /// <returns></returns>
        public ReturnValueModel DelWxUserModel(List<string> wxuserids)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            rvm.Msg = "success";
            rvm.Success = true;
            foreach (var wxuserid in wxuserids)
            {
                using (var tran = _rep.Database.BeginTransaction())
                {
                    try
                    {
                        WxUserModel model = _rep.FirstOrDefault<WxUserModel>(s => s.Id == wxuserid);
                        if (model == null)
                        {
                            rvm.Success = true;
                            rvm.Msg = "User not found.";
                            break;
                        }
                        else
                        {
                            model.IsDeleted = 1;
                            _rep.Update(model);

                            var MyMeetOrderList = _rep.Where<MyMeetOrder>(s => s.UnionId == model.UnionId);
                            foreach (var item in MyMeetOrderList)
                            {
                                item.UnionId = null; //去掉UnionId的绑定关系，此用户重新注册后不会看到这些老数据
                                item.WxUserId = wxuserid; //记录用户ID，以后统计时能找到和用户的关系
                            }

                            _rep.UpdateList(MyMeetOrderList);

                            var MyLRecordList = _rep.Where<MyLRecord>(s => s.UnionId == model.UnionId);
                            foreach (var item in MyLRecordList)
                            {
                                item.UnionId = null; //去掉UnionId的绑定关系，此用户重新注册后不会看到这些老数据
                                item.WxUserId = wxuserid; //记录用户ID，以后统计时能找到和用户的关系
                            }

                            _rep.UpdateList(MyLRecordList);

                            var MyCollectionInfoList = _rep.Where<MyCollectionInfo>(s => s.UnionId == model.UnionId);
                            foreach (var item in MyCollectionInfoList)
                            {
                                item.UnionId = null; //去掉UnionId的绑定关系，此用户重新注册后不会看到这些老数据
                                item.WxUserId = wxuserid; //记录用户ID，以后统计时能找到和用户的关系
                            }

                            _rep.UpdateList(MyCollectionInfoList);

                            var MyReadRecordList = _rep.Where<MyReadRecord>(s => s.UnionId == model.UnionId);
                            foreach (var item in MyReadRecordList)
                            {
                                item.UnionId = null; //去掉UnionId的绑定关系，此用户重新注册后不会看到这些老数据
                                item.WxUserId = wxuserid; //记录用户ID，以后统计时能找到和用户的关系
                            }

                            _rep.UpdateList(MyReadRecordList);

                            var RegisterModelList = _rep.Where<RegisterModel>(s => s.UnionId == model.UnionId);
                            foreach (var item in RegisterModelList)
                            {
                                item.UnionId = null; //去掉UnionId的绑定关系，此用户重新注册后不会看到这些老数据
                                item.WxUserId = wxuserid; //记录用户ID，以后统计时能找到和用户的关系
                            }

                            _rep.UpdateList(RegisterModelList);

                            _rep.SaveChanges();
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        rvm.Success = false;
                        rvm.Msg = ex.Message;
                        LoggerHelper.WriteLogInfo("[DelWxUserModel]*****Error开始******");
                        LoggerHelper.WriteLogInfo("[DelWxUserModel]错误信息：" + ex.Message);
                        LoggerHelper.WriteLogInfo("[DelWxUserModel]*****Error结束******");
                    }




                }
            }

            return rvm;

        }

        /// <summary>
        /// 后台用户认证审核
        /// </summary>
        /// <param name="rownum"></param>
        /// <returns></returns>
        public ReturnValueModel DoctorVerify(RowNumModel<DoctorVerifyViewModel> rownum)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            WxUserModel model = _rep.FirstOrDefault<WxUserModel>(s => s.Id == rownum.SearchParams.wxuserid);
            if (model != null)
            {
                using (var tran = _rep.Database.BeginTransaction())
                {
                    try
                    {
                        model.IsVerify = (rownum.SearchParams?.verifyStatus ?? "1") == "1" ? 1 : 6;
                        model.Remark = rownum.SearchParams?.remarks;
                        _rep.Update(model);
                        _rep.SaveChanges();
                        tran.Commit();
                        rvm.Msg = "success";
                        rvm.Success = true;
                    }
                    catch (Exception e)
                    {
                        LoggerHelper.WriteLogInfo("[后台用户认证审核-DoctorVerify ]Error:" + e.Message);
                        rvm.Msg = e.Message;
                        rvm.Success = false;
                    }

                }
            }

            return rvm;
        }



        /// <summary>
        /// 申诉用户详情
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public ReturnValueModel DoctorVerifyDetail(DoctorViewModel viewModel)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (string.IsNullOrEmpty(viewModel?.Id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }

            var wxUser = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.Id == viewModel.Id);

            if (wxUser == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid Id.";
                return rvm;
            }


            rvm.Msg = "success";
            rvm.Success = true;

            rvm.Result = new
            {
                doctor = wxUser,

            };

            return rvm;
        }

        public ReturnValueModel GetDoctorByTagDep(RowNumModel<DoctorMeeting> rownum)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var sql = string.Empty;
            var whereSql = string.Empty;
            if ((rownum.SearchParams?.DepartmentList?.Count() ?? 0) > 0)
            {
                whereSql += $" and e.DepartmentName in('{string.Join("','", rownum.SearchParams.DepartmentList)}') ";
                sql = $@"select * from DoctorModel e where  e.IsDeleted!=1 {whereSql}";
            }

            if ((rownum.SearchParams?.TagGroupList?.Count() ?? 0) > 0)
            {
                whereSql += $" and a.TagGroupId in('{string.Join("','", rownum.SearchParams.TagGroupList)}')";
                sql = $@"
select distinct e.Id ,e.UserName,e.DepartmentName from GroupTagRel a
join TagGroup b on b.Id=a.TagGroupId
join TagInfo c on c.Id=a.[TagId ]
join  DocTag d on d.TagId=a.[TagId ]
join DoctorModel e on d.DocId=e.id
where a.IsDeleted!=1 and b.IsDeleted!=1 and c.IsDeleted!= 1 and d.IsDeleted!=1 and e.IsDeleted!=1 { whereSql}
";
            }

            var list = _rep.SqlQuery<DoctorModel>(sql).ToList().Distinct();

            var total = list.Count();
            var rows = list.ToPaginationList(rownum.PageIndex, rownum.PageSize);
            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total = total,
                rows = rows
            };
            return rvm;
        }



        /// <summary>
        /// 获取内部员工
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel GetInternalStaffList(RowNumModel<DoctorViewModel> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var list = InternalStaffList(rowNum);
            var total = list.Count();
            var rows = list.OrderByDescending(s => s.CreateTime).ToPaginationList(rowNum.PageIndex, rowNum.PageSize).ToList();

            rows.ForEach(o =>
            {
                o.CreateTime = o.CreateTime.Value.AddHours(8);
            });
            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                total = total,
                rows = rows
            };

            return rvm;
        }

        /// <summary>
        /// 导出内部员工
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ReturnValueModel ExportInternalStaffList(RowNumModel<DoctorViewModel> rowNum, List<string> ids = null)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var list = InternalStaffList(rowNum);
            var result = list.ToList().Select(a => new DoctorViewModel
            {
                Id = a.Id,
                UserName = a.UserName,
                Province = a.Province,
                City = a.City,
                Area = a.Area,
                IsVerify = ToVerify(a.IsVerify),
                WxScene = Core.WxScene.WxSceneList.Keys.Contains(a.WxScene) ? Core.WxScene.WxSceneList[a.WxScene] : a.WxScene,
                CreateTime = a.CreateTime,
            });

            if ((ids?.Count() ?? 0) > 0)
            {
                result = result.Where(x => ids.Contains(x.Id));
            }
            var _path = AppDomain.CurrentDomain.BaseDirectory;
            LoggerHelper.WriteLogInfo("[ExportDoctorList] _path:" + _path);
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            var filename = "\\download\\InternalStaff_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx";
            LoggerHelper.WriteLogInfo("[ExportInternalStaffList] filename:" + filename);
            var helper = new ExcelHelper(_path + filename);

            var _listHeader = new List<string>()
            {
                "Id","UserName",  "Province","City", "Area", "IsVerify","WxScene","CreateTime"
            };
            var isSuccess = helper.Export<DoctorViewModel>(result.ToList(), _listHeader);
            if (isSuccess)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    fileurl = filename
                };
            }
            else
            {
                rvm.Msg = "fail";
                rvm.Success = false;
            }

            return rvm;
        }

        /// <summary>
        /// 内部员工列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        private IQueryable<DoctorViewModel> InternalStaffList(RowNumModel<DoctorViewModel> rowNum)
        {
            var tagslist = from c in _rep.Table<TagInfo>().Where(s => s.IsDeleted != 1 && s.TagType == "D2")
                           join d in _rep.Table<DocTag>()
                               on c.Id equals d.TagId
                           select new
                           {
                               DocId = d.DocId,
                               TagName = c.TagName
                           };

            string fkLibAppId = _config.GetFkLibAppId();//费卡文库AppId

            var wechatlist = from a in _rep.Table<WechatPublicAccount>()
                             where a.IsDeleted != 1
                             select a;

            var list = from a in _rep.Where<WxUserModel>(s => s.IsDeleted != 1 && s.IsCompleteRegister == 1 && s.IsSalesPerson == 2)
                       select new DoctorViewModel
                       {
                           Id = a.Id,
                           UserName = a.UserName, //姓名
                           Mobile = a.Mobile,
                           Province = a.Province,
                           City = a.City,
                           Area = a.Area,
                           CreateTime = a.CreateTime,
                           IsVerify = a.IsVerify.ToString(),
                           gender = a.WxGender,
                           DocTags = tagslist.Where(s => s.DocId == a.Id).Select(s => s.TagName).ToList(),
                           DataSource = (a.SourceType == "1" || a.SourceType == "2" || a.SourceType == "4") ? "2" //平台生成的二维码
                                : (a.SourceType == "3" && a.SourceAppId == fkLibAppId) ? "1" //费卡文库
                                : (a.SourceType == "3" && wechatlist.Count(s => s.AppId == a.SourceAppId) > 0) ? "0" //绑定小程序的公众号
                                : "3",  //其它小程序场景
                           WxScene = a.WxSceneId,
                           WxName = a.WxName,
                       };

            if (rowNum.SearchParams != null)
            {
                if (!string.IsNullOrEmpty(rowNum.SearchParams.UserName))
                {
                    list = list.Where(s => s.UserName.Contains(rowNum.SearchParams.UserName));
                }

                if (!string.IsNullOrEmpty(rowNum.SearchParams.School))
                {
                    list = list.Where(s => s.School.Contains(rowNum.SearchParams.School));
                }

                if (!string.IsNullOrEmpty(rowNum.SearchParams.IsVerify))
                {
                    list = list.Where(s => s.IsVerify == rowNum.SearchParams.IsVerify);
                }

                if (!string.IsNullOrEmpty(rowNum.SearchParams.Province))
                {
                    list = list.Where(s => s.Province.Contains(rowNum.SearchParams.Province));
                }

                if (!string.IsNullOrEmpty(rowNum.SearchParams.City))
                {
                    list = list.Where(s => s.City.Contains(rowNum.SearchParams.City));
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.DataSource))
                {
                    list = list.Where(s => s.DataSource.Equals(rowNum.SearchParams.DataSource));
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.WxName))
                {
                    list = list.Where(s => s.WxName.Contains(rowNum.SearchParams.WxName));
                }


                if (rowNum.SearchParams.DocTags != null)
                {
                    IList<string> docTagsList = rowNum.SearchParams.DocTags;
                    if (docTagsList.Count() > 0)
                    {
                        foreach (var docTag in docTagsList)
                        {
                            list = list.Where(s => s.DocTags.Contains(docTag));
                        }
                    }
                }
            }

            return list;
        }

    }
}
