using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Newtonsoft.Json;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Cache;
using log4net;
using SSPC_One_HCP.Services.Utils;

namespace SSPC_One_HCP.Services.Implementations
{
    public class WxRegisterService : IWxRegisterService
    {
        private readonly IEfRepository _rep;
        private readonly ILog _logger = LogManager.GetLogger("WarnFileLogger");
        private readonly IWordBlackListService _wordBlackListService;
        public WxRegisterService(IEfRepository rep, IWordBlackListService wordBlackListService)
        {
            _rep = rep;
            _wordBlackListService = wordBlackListService;
        }
        /// <summary>
        /// 创建微信用户
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel AddDocUser(WxUserModel wxUserModel)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            WxUserModel wxUser = new WxUserModel
            {
                Id = Guid.NewGuid().ToString(),
                UnionId = wxUserModel.UnionId,
                IsDeleted = 0,
                IsCompleteRegister = 0,
                IsVerify = 0,
                IsEnabled = 0,
                CreateTime = DateTime.Now
            };
            _rep.Insert(wxUser);
            _rep.SaveChanges();

            LoggerHelper.WriteLogInfo($"---- AddDocUser Begin 创建微信用户 ---------------");
            LoggerHelper.WriteLogInfo($"[doctor.Id]:{wxUser?.Id}");
            LoggerHelper.WriteLogInfo($"[doctor.UnionId]:{ wxUser?.UnionId }");
            LoggerHelper.WriteLogInfo($"[doctor.IsVerify]:{ wxUser?.IsVerify }");
            LoggerHelper.WriteLogInfo($"---- AddDocUser End 创建微信用户  ----------------");

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                wxUser = wxUser
            };

            return rvm;
        }

        /// <summary>
        /// 验证手机号
        /// </summary>
        /// <param name="moblie"></param>
        /// <returns></returns>
        public ReturnValueModel CheckMoblie(string mobile)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var isReginUser = _rep.Where<WxUserModel>(x => x.IsDeleted != 1 && x.IsCompleteRegister == 1 && x.Mobile == mobile).Count();
            rvm.Success = isReginUser > 0 ? false : true;
            rvm.Result = isReginUser;
            return rvm;
        }

        /// <summary>
        /// 验证验证码是否正确
        /// </summary>
        /// <param name="workUser"></param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public ReturnValueModel VerifyCode(WorkUser workUser, string code)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            string unionId = workUser?.WxUser?.UnionId;

            var wxUser = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.UnionId == unionId && s.Code == code /*&& s.CodeTime > DateTime.Now*/);

            if (wxUser != null)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    wxUser = wxUser
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
        /// 发送验证码
        /// </summary>
        /// <param name="workUser"></param>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        public ReturnValueModel SendVerifyCode(WorkUser workUser, string mobile)
        {
            LoggerHelper.WriteLogInfo("[SendVerifyCode]:******发送验证码开始******");
            ReturnValueModel rvm = new ReturnValueModel();
            string unionId = workUser?.WxUser?.UnionId;

            var wxUser = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.UnionId == unionId);
            LoggerHelper.WriteLogInfo("[SendVerifyCode]:wxUser------is null: " + (wxUser == null));
            LoggerHelper.WriteLogInfo("[SendVerifyCode]:wxUser------UnionId:" + unionId);
            LoggerHelper.WriteLogInfo("[SendVerifyCode]:wxUser------手机号:" + mobile);

            if (wxUser != null)
            {
                //手机号唯一 才允许注册
                var isReginUser = (_rep.Where<WxUserModel>(x => x.IsDeleted != 1 && x.IsCompleteRegister == 1 && x.Mobile.Equals(mobile))?.Count() ?? 0) > 0;
                if (isReginUser)
                {
                    rvm.Msg = "fail";
                    rvm.Success = false;
                    LoggerHelper.WriteLogInfo(mobile + "拒绝注册" + unionId);
                    LoggerHelper.WriteLogInfo("[SendVerifyCode]:******短信验证码结束******");
                    return rvm;
                }
                var code = RandomUtil.GenerateRandomCode(6);
                wxUser.Code = code;
                LoggerHelper.WriteLogInfo("[SendVerifyCode]验证码:" + code);

                //wxUser.Mobile = mobile;
                wxUser.CodeTime = DateTime.Now.AddMinutes(1);

                //发送验证码
                //SendVerifyCode(Mobile, code);
                SendSmsModel ssm = new SendSmsModel
                {
                    PhoneNumbers = mobile,
                    CompanyCode = "4033",
                    TemplateId = "FKSMS0047",
                    SystemId = "3",
                    SignName = "费卡中国",
                    ParamName = JsonConvert.SerializeObject(new { code = code }).Base64Encoding()
                };

                _rep.Update(wxUser);
                _rep.SaveChanges();
                LoggerHelper.WriteLogInfo("[SendVerifyCode]短信发送开始:");
                var sr = SmsUtil.SendMessage(ssm);
                LoggerHelper.WriteLogInfo("[SendVerifyCode]短信发送结束:");
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    sr
                };
            }
            else
            {
                rvm.Msg = "fail";
                rvm.Success = false;
            }
            LoggerHelper.WriteLogInfo("[SendVerifyCode]:发送结果：" + rvm.Msg);
            LoggerHelper.WriteLogInfo("[SendVerifyCode]:******短信验证码结束******");
            return rvm;

        }

        ///// <summary>
        ///// 更新微信用户的医院、科室信息
        ///// </summary>
        ///// <param name="wxUserModel"></param>
        ///// <returns></returns>
        //public ReturnValueModel UpdateWxUser(WxUserModel wxUserModel)
        //{
        //    ReturnValueModel rvm = new ReturnValueModel();

        //    var wxUser = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.UnionId == wxUserModel.UnionId);
        //    if (wxUser != null)
        //    {
        //        wxUser.HospitalName = wxUserModel.HospitalName;
        //        wxUser.DepartmentName = wxUserModel.DepartmentName;
        //        wxUser.UpdateTime = DateTime.Now;
        //        wxUser.creation_time = DateTime.Now;
        //        wxUser.IsCompleteRegister = 1;
        //        if (wxUser.IsVerify == 0) wxUser.IsVerify = 5; //完成注册后变成认证中

        //        //创建手写签名
        //        RegisterModel model = new RegisterModel();
        //        model.UnionId = wxUser.UnionId;
        //        //图片存储地址
        //        model.SignUpName = wxUser.Remark;

        //        _rep.Insert(model);
        //        _rep.Update(wxUser);
        //        _rep.SaveChanges();

        //        rvm.Msg = "success";
        //        rvm.Success = true;
        //        rvm.Result = new
        //        {
        //            wxUser = wxUser
        //        };
        //    }
        //    else
        //    {
        //        rvm.Msg = "fail";
        //        rvm.Success = false;
        //    }

        //    return rvm;
        //}

        /// <summary>
        /// 更新医生医院及科室
        /// </summary>
        /// <param name="wxUserModel">
        /// 微信用户信息：
        /// HospitalName，
        /// DepartmentName
        /// </param>
        /// <param name="isRegistering">是否想注册用户</param>
        /// <returns></returns>
        public ReturnValueModel UpdateDocHospitalOrDept(WorkUser workUser, WxUserModel wxUserModel, bool isRegistering)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var id = workUser?.WxUser?.Id;
            var doc = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.Id == id);
            if (doc == null)
            {
                rvm.Success = false;
                rvm.Msg = "此用户未通过微信授权";
                return rvm;
            }

            if (doc.IsVerify == 1)
            {
                rvm.Success = false;
                rvm.Msg = "已认证用户无法修改个人信息";
                return rvm;
            }

            if (isRegistering && doc.IsCompleteRegister == 1)
            {
                LoggerHelper.WriteLogInfo($"---- UpdateDocHospitalOrDept Begin 重复注册了！！ ----------");
                LoggerHelper.WriteLogInfo($"[doctor.Id]:{doc?.Id}\n");
                LoggerHelper.WriteLogInfo($"[doctor.UnionId]:{ doc?.UnionId }\n");
                LoggerHelper.WriteLogInfo($"[doctor.WxName]:{ doc?.WxName }\n");
                LoggerHelper.WriteLogInfo($"[doctor.IsVerify]:{ doc?.IsVerify }\n");
                LoggerHelper.WriteLogInfo($"---- UpdateDocHospitalOrDept End   -------------------------------------------");
            }

            //认证失败的修改信息后，变成申诉中
            if (!isRegistering && doc.IsVerify == 3 || doc.IsVerify == 2 || doc.IsVerify == 6)
            {
                LoggerHelper.WriteLogInfo($"---- UpdateDocHospitalOrDept Begin 认证失败的修改信息后，变成申诉中 ----------");
                LoggerHelper.WriteLogInfo($"[doctor.Id]:{doc?.Id}\n");
                LoggerHelper.WriteLogInfo($"[doctor.UnionId]:{ doc?.UnionId }\n");
                LoggerHelper.WriteLogInfo($"[doctor.WxName]:{ doc?.WxName }\n");
                LoggerHelper.WriteLogInfo($"[doctor.IsVerify]:{ doc?.IsVerify }\n");
                LoggerHelper.WriteLogInfo($"---- UpdateDocHospitalOrDept End   -------------------------------------------");

                doc.IsVerify = 4;
            }

            //用户名
            if (!string.IsNullOrEmpty(wxUserModel.UserName))
            {
                string userName = wxUserModel.UserName;
                var isQua = IsQualified(userName);
                if (!isQua.Success)
                {
                    return isQua;
                }
                doc.UserName = userName;
                doc.Mobile = wxUserModel.Mobile;//手机号
            }

            //医院名称
            if (!string.IsNullOrEmpty(wxUserModel.HospitalName))
            {
                var hospital = _rep.FirstOrDefault<HospitalInfo>(s => s.IsDeleted != 1 && s.HospitalName == wxUserModel.HospitalName);
                //医院名不存在就插入
                if (hospital == null)
                {
                    hospital = new HospitalInfo
                    {
                        Id = Guid.NewGuid().ToString().ToUpper(),
                        CreateTime = DateTime.Now,
                        CreateUser = workUser.WxUser.UnionId,
                        HospitalName = wxUserModel.HospitalName,
                        ComeFrom = "self",
                        IsVerify = 0
                    };
                    _rep.Insert(hospital);
                    _rep.SaveChanges();
                }
                doc.HospitalName = wxUserModel.HospitalName;
            }
            //科室
            if (!string.IsNullOrEmpty(wxUserModel.DepartmentName))
            {
                doc.DepartmentName = wxUserModel.DepartmentName;
            }
            //职称
            if (!string.IsNullOrEmpty(wxUserModel.Title))
            {
                doc.Title = wxUserModel.Title;
            }
            //职务
            if (!string.IsNullOrEmpty(wxUserModel.DoctorPosition))
            {
                doc.DoctorPosition = wxUserModel.DoctorPosition;
            }
            //医生办公室电话
            if (!string.IsNullOrEmpty(wxUserModel.doctor_office_tel))
            {
                doc.doctor_office_tel = wxUserModel.doctor_office_tel;
            }
            //微信信息-城市
            if (!string.IsNullOrEmpty(wxUserModel.WxCity))
            {
                doc.WxCity = wxUserModel.WxCity;
            }
            //微信信息-微信名
            if (!string.IsNullOrEmpty(wxUserModel.WxName))
            {
                doc.WxName = wxUserModel.WxName;
            }
            //微信信息-国家
            if (!string.IsNullOrEmpty(wxUserModel.WxCountry))
            {
                doc.WxCountry = wxUserModel.WxCountry;
            }
            //微信信息-头像
            if (!string.IsNullOrEmpty(wxUserModel.WxPicture))
            {
                doc.WxPicture = wxUserModel.WxPicture;
            }
            //微信信息-性别
            if (!string.IsNullOrEmpty(wxUserModel.WxGender))
            {
                doc.WxGender = wxUserModel.WxGender;
            }
            //微信信息-省
            if (!string.IsNullOrEmpty(wxUserModel.WxProvince))
            {
                doc.WxProvince = wxUserModel.WxProvince;
            }

            if (!string.IsNullOrEmpty(wxUserModel.Province))
            {
                doc.Province = wxUserModel.Province;
            }
            if (!string.IsNullOrEmpty(wxUserModel.City))
            {
                doc.City = wxUserModel.City;
            }
            if (!string.IsNullOrEmpty(wxUserModel.Area))
            {
                doc.Area = wxUserModel.Area;
            }

            doc.UpdateTime = DateTime.Now;

            _rep.Update(doc);
            _rep.SaveChanges();

            //CacheWxUser(doc);

            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                doc
            };


            return rvm;
        }

        /// <summary>
        /// 上传签名
        /// </summary>
        /// <param name="httpRequest">请求</param>
        /// <returns></returns>
        public ReturnValueModel UploadSign(WorkUser workUser, HttpRequest httpRequest)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var root = HostingEnvironment.MapPath("~/");
            var unionId = workUser?.WxUser?.UnionId; //httpRequest["UnionId"];
            var user = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.UnionId == unionId);
            #region 保存文件
            HttpFileCollection files = httpRequest.Files;
            var path = root + @"Sign";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var ext = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."));
            var fileName = Guid.NewGuid().ToString() + ext;
            files[0].SaveAs(path + @"/" + fileName);
            #endregion

            #region 插入数据
            var rModel = _rep.FirstOrDefault<RegisterModel>(s => s.UnionId == unionId);

            if (rModel == null)
            {
                rModel = new RegisterModel
                {
                    Id = Guid.NewGuid().ToString(),
                    UnionId = unionId,
                    CreateTime = DateTime.Now,
                    SignUpName = $"/Sign/{fileName}"
                };
                _rep.Insert(rModel);
            }
            else
            {
                rModel.SignUpName = $"/Sign/{fileName}";
                rModel.UpdateTime = DateTime.Now;
                _rep.Update(rModel);
            }
            #endregion

            #region 完成注册
            user.IsCompleteRegister = 1;
            if (user.IsVerify == 0) user.IsVerify = 5; //完成注册后变成认证中
            user.creation_time = DateTime.Now;
            _rep.Update(user);
            #endregion

            _rep.SaveChanges();

            //CacheWxUser(user);  //用户状态被改变，所以要重新缓存。

            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                rModel,
                user.IsVerify,
                user.IsCompleteRegister
            };
            return rvm;
        }

        /// <summary>
        /// 上传医生个人信息证明图片
        /// </summary>
        /// <param name="httpRequest">请求</param>
        /// <returns></returns>
        public ReturnValueModel UploadPicture(WorkUser workUser, HttpRequest httpRequest)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var root = HostingEnvironment.MapPath("~/");
            var unionId = workUser?.WxUser?.UnionId; //httpRequest["UnionId"];
            var user = _rep.FirstOrDefault<WxUserModel>(s => s.IsDeleted != 1 && s.UnionId == unionId);
            if (user == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid union Id";
                return rvm;
            }
            try
            {
                #region 保存文件
                HttpFileCollection files = httpRequest.Files;
                if (files == null || files.Count == 0)
                {
                    rvm.Success = false;
                    rvm.Msg = "Files not found.";
                    return rvm;
                }
                var dir = @"DoctorPictures";
                var path = root + dir;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                foreach (var f in files)
                {
                    var ext = Path.GetExtension(files[0].FileName);
                    var fileName = Guid.NewGuid().ToString() + ext;
                    var downloadPath = "/" + dir + "/" + fileName;
                    files[0].SaveAs(path + "\\" + fileName);
                    user.Pictures = (user.Pictures ?? "") + downloadPath + "|";
                }
                #endregion

                user.UpdateTime = DateTime.Now;
                _rep.Update(user);

                _rep.SaveChanges();

                rvm.Success = true;
                rvm.Msg = "success";
                rvm.Result = new
                {
                    path = user.Pictures
                };
            }
            catch (Exception ex)
            {
                rvm.Success = false;
                rvm.Msg = ex.Message;
                LoggerHelper.WriteLogInfo("[UploadPicture]Exception ex:" + ex.Message);
            }

            return rvm;
        }

        /// <summary>
        /// 缓存微信用户信息
        /// </summary>
        /// <returns></returns>
        public void CacheWxUser(WxUserModel wxUser)
        {
            if (wxUser == null) return;

            var cache = ContainerManager.Resolve<ICacheManager>();
            var workUser = cache.Get<WorkUser>(wxUser.Id);
            if (workUser == null)
            {
                LoggerHelper.WriteLogInfo("WxRegisterService.CacheWxUser: workUser is null");
                workUser = new WorkUser();
            }
            workUser.WxUser = wxUser;
            cache.Set(wxUser.Id, workUser, 12);
            LoggerHelper.WriteLogInfo("WxRegisterService.CacheWxUser: wxUser.UserName:" + wxUser.UserName);
            LoggerHelper.WriteLogInfo("WxRegisterService.CacheWxUser: wxUser.IsVerify:" + wxUser.IsVerify);
        }

        /// <summary>
        /// 获取缓存中的微信用户信息
        /// </summary>
        /// <returns></returns>
        public WorkUser GetWorkUser(string userid)
        {
            var cache = ContainerManager.Resolve<ICacheManager>();
            var workUser = cache.Get<WorkUser>(userid);
            if (workUser == null)
            {
                LoggerHelper.WriteLogInfo("WxRegisterService.GetWorkUser: workUser is null");
            }
            else if (workUser.WxUser == null)
            {
                LoggerHelper.WriteLogInfo("WxRegisterService.GetWorkUser: workUser.WxUser is null");
            }
            return workUser;
        }

        /// <summary>
        /// 注册名字验证
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public ReturnValueModel IsQualified(string UserName)
        {

            ReturnValueModel rvm = new ReturnValueModel() { Success = true, Msg = "true" };
            string[] wordBlackList = _wordBlackListService.GetWordBlackListByType("doctor_name");
            //过滤姓名关键字
            foreach (string word in wordBlackList)
            {
                if (UserName.Contains(word))
                {
                    rvm.Success = false;
                    rvm.Msg = "invalid_doctor_name";
                    rvm.Result = new
                    {
                        InvalidWords = string.Join(",", wordBlackList)
                    };
                    return rvm;
                }
            }
            //屏蔽名字
            wordBlackList = _wordBlackListService.GetWordBlackListByType("doctor_name2");
            var d2 = (wordBlackList.Where(x => x.Equals(UserName))?.Count() ?? 0) > 0;
            if (d2)
            {
                rvm.Success = false;
                rvm.Msg = "invalid_doctor_name";
                rvm.Result = new
                {
                    InvalidWords = string.Join(",", wordBlackList)
                };
                return rvm;
            }

            return rvm;
        }


        /// <summary>
        /// 缓存销售微信用户信息
        /// </summary>
        /// <returns></returns>
        public void CacheWxSaleUser(WxSaleUserModel wxSaleUser)
        {
            if (wxSaleUser == null) return;

            var cache = ContainerManager.Resolve<ICacheManager>();
            var workUser = cache.Get<WorkUser>(wxSaleUser.Id);
            if (workUser == null)
            {
                LoggerHelper.WriteLogInfo("WxRegisterService.CacheWxUser: WxSaleUserModel is null");
                workUser = new WorkUser();
            }
            workUser.WxSaleUser = wxSaleUser;
            cache.Set(wxSaleUser.Id, workUser, 12);
            LoggerHelper.WriteLogInfo("WxRegisterService.CacheWxUser: WxSaleUserModel.UserName:" + wxSaleUser.UserName);
        }

        /// <summary>
        /// 获取缓存中的销售微信用户信息
        /// </summary>
        /// <returns></returns>
        public WorkUser GetWxSaleUser(string id)
        {
            var cache = ContainerManager.Resolve<ICacheManager>();
            var workUser = cache.Get<WorkUser>(id);
            if (workUser == null)
            {
                LoggerHelper.WriteLogInfo("WxRegisterService.GetWorkUser: WorkUser is null");
            }
            else if (workUser.WxSaleUser == null)
            {
                LoggerHelper.WriteLogInfo("WxRegisterService.GetWorkUser: workUser.WorkUser is null");
            }
            return workUser;
        }
        public void RemoveWxSaleUser(WxSaleUserModel wxSaleUser)
        {
            if (wxSaleUser == null) return;

            var cache = ContainerManager.Resolve<ICacheManager>();
            var workUser = cache.Get<WorkUser>(wxSaleUser.Id);
            if (workUser == null)
            {
                LoggerHelper.WriteLogInfo("WxRegisterService.CacheWxUser: WxSaleUserModel is null");
                workUser = new WorkUser();
            }
            workUser.WxSaleUser = wxSaleUser;
            cache.Remove(wxSaleUser.Id);
            LoggerHelper.WriteLogInfo("WxRegisterService.CacheWxUser: WxSaleUserModel.UserName:" + wxSaleUser.UserName);
        }
    }
}
