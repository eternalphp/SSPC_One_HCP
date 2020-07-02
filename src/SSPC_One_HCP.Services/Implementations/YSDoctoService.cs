using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Implementations
{
    public class YSDoctoService : IYSDoctoService
    {
        private readonly IEfRepository _rep;

        public YSDoctoService(IEfRepository rep)
        {
            _rep = rep;
        }


        /// <summary>
        /// 获取清洗医生列表(认证中 用户 IsVerify=5 )
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel GetDoctorInfo(RowNumModel<DoctorViewModel> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var list = from a in _rep.Where<WxUserModel>(s => s.IsDeleted != 1 && s.IsCompleteRegister == 1 && s.IsVerify == 5 && (s.IsSalesPerson ?? 0) != 1)
                       select new YXDoctorViewModel
                       {
                           Id = a.Id,
                           PVMId = a.PVMId,
                           YSId = a.yunshi_doctor_id,
                           UserName = a.UserName,
                           Mobile = a.Mobile,
                           HospitalName = a.HospitalName,
                           DepartmentName = a.DepartmentName,
                           Title = a.Title,
                           DoctorPosition = a.DoctorPosition,
                           Province = a.Province,
                           City = a.City,
                           Area = a.Area,
                           School = a.School,
                           CreateTime = a.CreateTime,
                           IsVerify = a.IsVerify,
                           hospital_code = a.hospital_code,
                           hospital_name = a.hospital_name,
                           doctor_code = a.doctor_code,
                           doctor_name = a.doctor_name,
                           department = a.department,
                           job_title_SFE = a.job_title_SFE,
                           position_SFE = a.position_SFE,
                           //is_infor_customer = a.is_infor_customer == false ? "否" : "是",
                           is_infor_customer = a.is_infor_customer ? "Y" : "N",
                           serial_number = a.serial_number,
                           status = a.status,
                           reason = a.reason,
                           unique_doctor_id = a.unique_doctor_id,
                           yunshi_hospital_id = a.yunshi_hospital_id,
                           yunshi_hospital_name = a.yunshi_hospital_name,
                           yunshi_doctor_id = a.yunshi_doctor_id,
                           name = a.name,
                           standard_department = a.standard_department,
                           profession = a.profession,
                           gender = a.gender,
                           job_title = a.job_title,
                           position = a.position,
                           academic_title = a.academic_title,
                           type = a.type,
                           certificate_type = a.certificate_type,
                           certificate_code = a.certificate_code,
                           education = a.education,
                           graduated_school = a.graduated_school,
                           graduation_time = a.graduation_time,
                           specialty = a.specialty,
                           intro = a.intro,
                           department_phone = a.department_phone,
                           modifier = a.modifier,
                           change_time = a.change_time,
                           data_update_type = a.data_update_type,
                           Pictures = a.Pictures,
                           doctor_office_tel = a.doctor_office_tel                           
                       };

            if (rowNum?.SearchParams != null)
            {
                if (!string.IsNullOrEmpty(rowNum.SearchParams.Id))
                {
                    list = list.Where(s => s.Id == rowNum.SearchParams.Id);
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.YSId))
                {
                    list = list.Where(s => s.yunshi_doctor_id == rowNum.SearchParams.YSId);
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.UserName))
                {
                    list = list.Where(s => (s.UserName ?? "").Contains(rowNum.SearchParams.UserName));
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.Mobile))
                {
                    list = list.Where(s => (s.Mobile ?? "").Contains(rowNum.SearchParams.Mobile));
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.HospitalName))
                {
                    list = list.Where(s => (s.HospitalName ?? "").Contains(rowNum.SearchParams.HospitalName));
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.IsVerify))
                {
                    list = list.Where(s => s.IsVerify.ToString() == rowNum.SearchParams.IsVerify);
                }
            }

            var total = list.Count();
            var rows = list.OrderByDescending(s => s.CreateTime)
                .ToPaginationList(rowNum?.PageIndex, rowNum?.PageSize);

            LoggerHelper.WriteLogInfo($"---- GetDoctorInfo Begin 获取清洗医生列表 ---------------");
            LoggerHelper.WriteLogInfo($"[DateTime]:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\n");
            LoggerHelper.WriteLogInfo($"[doctorCount]:{total}\n");
            LoggerHelper.WriteLogInfo($"[doctorData]:{ Newtonsoft.Json.JsonConvert.SerializeObject(list) }\n");
            LoggerHelper.WriteLogInfo($"---- GetDoctorInfo End   ----------------------------");

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
        /// 性别转换 男:1 女:2 null:0
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        private string YsToGender(string gender)
        {
            var result = "0";
            switch (gender)
            {
                case "男":
                    result = "1";
                    break;
                case "女":
                    result = "2";
                    break;
                default:
                    result = "0";
                    break;
            }
            return result;
        }
        /// <summary>
        /// 批量清洗医生
        /// </summary>
        /// <param name="DoctorInfo"></param>
        /// <returns></returns>
        public ReturnValueModel AddUpdateDoctorInfo(List<YXDoctorViewModel> DoctorInfo)
        {
            LoggerHelper.WriteLogInfo($"---- AddUpdateDoctorInfo Begin 批量清洗医生 ---------------");

            LoggerHelper.WriteLogInfo("List<YXDoctorViewModel> DoctorInfo: " + Newtonsoft.Json.JsonConvert.SerializeObject(DoctorInfo));

            ReturnValueModel rvm = new ReturnValueModel();
            List<WxUserModel> list = new List<WxUserModel>();

            if (DoctorInfo == null || DoctorInfo.Count == 0)
            {
                rvm.Msg = "没有医生数据需要更新。";
                rvm.Success = false;
                return rvm;
            }

            foreach (var item in DoctorInfo)
            {
                var doctor = _rep.FirstOrDefault<WxUserModel>(s => s.Id == item.Id);
                if (doctor != null)
                {
                    //item.Id = items.Id;
                    string status = (item.status ?? "").Trim().ToLower();

                    switch (status)
                    {
                        case "active":
                            doctor.IsVerify = 1;   //已认证
                            break;
                        case "inactive":
                            if (doctor.IsVerify == 4) //原来是申诉中
                            {
                                doctor.IsVerify = 6;  //申诉拒绝
                            }
                            else
                            {
                                doctor.IsVerify = 3;  //认证失败
                            }
                            break;
                        case "undetermined":
                            doctor.IsVerify = 2;    //不确定
                            break;
                        default:
                            break;
                    }

                    //通过云势已认证用户
                    if (status == "active")
                    {
                        doctor.UserName = item.name;                     
                        doctor.HospitalName = item.yunshi_hospital_name;
                        //doctor.DepartmentName = item.standard_department;
                        doctor.Title = item.job_title;
                        doctor.WxGender = YsToGender(item.gender);
                        doctor.School = item.graduated_school;
                        doctor.Province = item.yunshi_province;
                        doctor.City = item.yunshi_city;
                    }
                   
                    doctor.hospital_code = item.hospital_code;
                    doctor.hospital_name = item.hospital_name;
                    doctor.doctor_code = item.doctor_code;
                    doctor.doctor_name = item.doctor_name;
                    doctor.department = item.department;
                    doctor.job_title_SFE = item.job_title_SFE;
                    doctor.position_SFE = item.position_SFE;
                    //doctor.is_infor_customer = item.is_infor_customer == "是" ? true : false;
                    doctor.is_infor_customer = string.Equals(item.is_infor_customer, "Y", StringComparison.OrdinalIgnoreCase) ? true : false;
                    doctor.serial_number = item.serial_number;
                    doctor.status = item.status;
                    doctor.reason = item.reason;
                    doctor.unique_doctor_id = item.unique_doctor_id;
                    doctor.yunshi_hospital_id = item.yunshi_hospital_id;
                    doctor.yunshi_hospital_name = item.yunshi_hospital_name;
                    doctor.yunshi_doctor_id = item.yunshi_doctor_id;
                    doctor.name = item.name;
                    doctor.standard_department = item.standard_department;
                    doctor.profession = item.profession;
                    doctor.gender = item.gender;
                    doctor.job_title = item.job_title;
                    doctor.position = item.position;
                    doctor.academic_title = item.academic_title;
                    doctor.type = item.type;
                    doctor.certificate_type = item.certificate_type;
                    doctor.certificate_code = item.certificate_code;
                    doctor.education = item.education;
                    doctor.graduated_school = item.graduated_school;
                    doctor.graduation_time = item.graduation_time;
                    doctor.specialty = item.specialty;
                    doctor.intro = item.intro;
                    doctor.department_phone = item.department_phone;
                    doctor.modifier = item.modifier;
                    doctor.change_time = item.change_time;
                    doctor.data_update_type = item.data_update_type;
                    doctor.yunshi_province = item.yunshi_province;
                    doctor.yunshi_city = item.yunshi_city;

                    if (item.change_time != null)
                    {
                        doctor.change_time = item.change_time;
                    }
                    else
                    {
                        doctor.change_time = DateTime.Now;
                    }

                    doctor.data_update_type = item.data_update_type;
                    _rep.Update(doctor);
                    _rep.SaveChanges();

                    LoggerHelper.WriteLogInfo($"---- AddUpdateDoctorInfo Update Begin 批量清洗医生 ---------------");
                    LoggerHelper.WriteLogInfo($"[doctor.Id]:{doctor?.Id}\n");
                    LoggerHelper.WriteLogInfo($"[doctor.UnionId]:{ doctor?.UnionId }\n");
                    LoggerHelper.WriteLogInfo($"[doctor.IsVerify]:{ doctor?.IsVerify }\n");
                    LoggerHelper.WriteLogInfo($"---- AddUpdateDoctorInfo Update End   ----------------------------");
                }
                else
                {
                    doctor = new WxUserModel()
                    {
                        Id = Guid.NewGuid().ToString(),

                        IsVerify = 0,
                        IsCompleteRegister = 0,
                        IsDeleted = 0,
                        IsEnabled = 0,
                        hospital_code = item.hospital_code,
                        hospital_name = item.hospital_name,
                        doctor_code = item.doctor_code,
                        doctor_name = item.doctor_name,
                        department = item.department,
                        job_title_SFE = item.job_title_SFE,
                        position_SFE = item.position_SFE,
                        //is_infor_customer = item.is_infor_customer == "是" ? true : false,
                        is_infor_customer = string.Equals(item.is_infor_customer, "Y", StringComparison.OrdinalIgnoreCase) ? true : false,
                        serial_number = item.serial_number,
                        status = item.status,
                        reason = item.reason,
                        unique_doctor_id = item.unique_doctor_id,
                        yunshi_hospital_id = item.yunshi_hospital_id,
                        yunshi_hospital_name = item.yunshi_hospital_name,
                        yunshi_doctor_id = item.yunshi_doctor_id,
                        name = item.name,
                        standard_department = item.standard_department,
                        profession = item.profession,
                        gender = item.gender,
                        job_title = item.job_title,
                        position = item.position,
                        academic_title = item.academic_title,
                        type = item.type,
                        certificate_type = item.certificate_type,
                        certificate_code = item.certificate_code,
                        education = item.education,
                        graduated_school = item.graduated_school,
                        graduation_time = item.graduation_time,
                        specialty = item.specialty,
                        intro = item.intro,
                        department_phone = item.department_phone,
                        modifier = item.modifier,
                        change_time = item.change_time,
                        data_update_type = item.data_update_type,
                        CreateTime = DateTime.Now
                    };
                    string status = (item.status ?? "").Trim().ToLower();
                    switch (status)
                    {
                        case "active":
                            doctor.IsVerify = 1;   //已认证
                            break;
                        case "inactive":
                            if (doctor.IsVerify == 4) //原来是申诉中
                            {
                                doctor.IsVerify = 6;  //申诉拒绝
                            }
                            else
                            {
                                doctor.IsVerify = 3;  //认证失败
                            }
                            break;
                        case "undetermined":
                            doctor.IsVerify = 2;   //不确定
                            break;
                        default:
                            break;
                    }
                    if (status == "active")
                    {
                        doctor.UserName = item.name;
                        doctor.HospitalName = item.yunshi_hospital_name;
                        //doctor.DepartmentName = item.standard_department;
                        doctor.Title = item.job_title;
                        doctor.WxGender = YsToGender(item.gender);
                        doctor.School = item.graduated_school;
                    }
                    list.Add(doctor);

                    LoggerHelper.WriteLogInfo($"---- AddUpdateDoctorInfo Add Begin 批量清洗医生 ---------------");
                    LoggerHelper.WriteLogInfo($"[doctor.Id]:{doctor?.Id}\n");
                    LoggerHelper.WriteLogInfo($"[doctor.UnionId]:{ doctor?.UnionId }\n");
                    LoggerHelper.WriteLogInfo($"[doctor.IsVerify]:{ doctor?.IsVerify }\n");
                    LoggerHelper.WriteLogInfo($"---- AddUpdateDoctorInfo Add End   ----------------------------");
                }
            }

            if (list.Count() > 0)
                _rep.BulkCopyInsert(list, "DoctorModel");

            _rep.SaveChanges();


            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                //DoctorInfo = DoctorInfo
                msg = "批量更新医生成功！"
            };
            LoggerHelper.WriteLogInfo($"---- AddUpdateDoctorInfo End 批量清洗医生 ---------------");
            return rvm;
        }



        /// <summary>
        /// 獲取已清洗醫生列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel UpdateDoctorInfo(RowNumModel<YXDoctorViewModel> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = from a in _rep.Where<WxUserModel>(s => s.IsDeleted != 1 && s.IsCompleteRegister == 1 && s.name != null)
                       select new YXDoctorViewModel
                       {
                           Id = a.Id,
                           PVMId = a.PVMId,
                           YSId = a.yunshi_doctor_id,
                           UserName = a.UserName,
                           Mobile = a.Mobile,
                           HospitalName = a.HospitalName,
                           DepartmentName = a.DepartmentName,
                           Title = a.Title,
                           Province = a.Province,
                           City = a.City,
                           Area = a.Area,
                           School = a.School,
                           CreateTime = a.CreateTime,
                           //IsVerify = a.IsVerify == 1 ? "已认证" :
                           //    a.IsVerify == 2 ? "不确定" :
                           //    a.IsVerify == 3 ? "认证失败" :
                           //    a.IsVerify == 4 ? "申诉中" : "未认证",
                           IsVerify = a.IsVerify,
                           hospital_code = a.hospital_code,
                           hospital_name = a.hospital_name,
                           doctor_code = a.doctor_code,
                           doctor_name = a.doctor_name,
                           department = a.department,
                           job_title_SFE = a.job_title_SFE,
                           position_SFE = a.position_SFE,
                           //is_infor_customer = a.is_infor_customer == false ? "否" : "是",
                           is_infor_customer = a.is_infor_customer ? "Y" : "N",
                           serial_number = a.serial_number,
                           status = a.status,
                           reason = a.reason,
                           unique_doctor_id = a.unique_doctor_id,
                           yunshi_hospital_id = a.yunshi_hospital_id,
                           yunshi_hospital_name = a.yunshi_hospital_name,
                           yunshi_doctor_id = a.yunshi_doctor_id,
                           name = a.name,
                           standard_department = a.standard_department,
                           profession = a.profession,
                           gender = a.gender,
                           job_title = a.job_title,
                           position = a.position,
                           academic_title = a.academic_title,
                           type = a.type,
                           certificate_type = a.certificate_type,
                           certificate_code = a.certificate_code,
                           education = a.education,
                           graduated_school = a.graduated_school,
                           graduation_time = a.graduation_time,
                           specialty = a.specialty,
                           intro = a.intro,
                           department_phone = a.department_phone,
                           modifier = a.modifier,
                           change_time = a.change_time,
                           data_update_type = a.data_update_type
                       };

            if (rowNum?.SearchParams != null)
            {
                list = list.Where(rowNum.SearchParams);
            }

            var total = list.Count();
            var rows = list.OrderByDescending(s => s.CreateTime)
                .ToPaginationList(rowNum?.PageIndex, rowNum?.PageSize);

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
        /// 获取批量申诉医生列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel GetAppealInfo()
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = from a in _rep.Where<WxUserModel>(s => s.IsDeleted != 1 && s.IsCompleteRegister == 1 && s.IsVerify == 4 && (s.IsSalesPerson ?? 0) != 1)
                       select new YXDoctorViewModel
                       {
                           Id = a.Id,
                           PVMId = a.PVMId,
                           YSId = a.yunshi_hospital_id,
                           UserName = a.UserName,
                           Mobile = a.Mobile,
                           HospitalName = a.HospitalName,
                           DepartmentName = a.DepartmentName,
                           Title = a.Title,
                           DoctorPosition = a.DoctorPosition,
                           Province = a.Province,
                           City = a.City,
                           Area = a.Area,
                           School = a.School,
                           CreateTime = a.CreateTime,
                           //IsVerify = a.IsVerify == 1 ? "已认证" :
                           //    a.IsVerify == 2 ? "不确定" :
                           //    a.IsVerify == 3 ? "认证失败" :
                           //    a.IsVerify == 4 ? "申诉中" : "未认证",
                           IsVerify = a.IsVerify,
                           hospital_code = a.hospital_code,
                           hospital_name = a.hospital_name,
                           doctor_code = a.doctor_code,
                           doctor_name = a.doctor_name,
                           department = a.department,
                           job_title_SFE = a.job_title_SFE,
                           position_SFE = a.position_SFE,
                           //is_infor_customer = a.is_infor_customer == false ? "否" : "是",
                           is_infor_customer = a.is_infor_customer?"Y":"N",
                           serial_number = a.serial_number,
                           status = a.status,
                           reason = a.reason,
                           unique_doctor_id = a.unique_doctor_id,
                           yunshi_hospital_id = a.yunshi_hospital_id,
                           yunshi_hospital_name = a.yunshi_hospital_name,
                           yunshi_doctor_id = a.yunshi_doctor_id,
                           name = a.name,
                           standard_department = a.standard_department,
                           profession = a.profession,
                           gender = a.gender,
                           job_title = a.job_title,
                           position = a.position,
                           academic_title = a.academic_title,
                           type = a.type,
                           certificate_type = a.certificate_type,
                           certificate_code = a.certificate_code,
                           education = a.education,
                           graduated_school = a.graduated_school,
                           graduation_time = a.graduation_time,
                           specialty = a.specialty,
                           intro = a.intro,
                           department_phone = a.department_phone,
                           modifier = a.modifier,
                           change_time = a.change_time,
                           data_update_type = a.data_update_type,

                           Pictures = a.Pictures,  // 图片证明材料，内容可以是医院照片墙、排班表、工牌或胸卡、名片、挂号单，用|分隔
                           //photowall = a.photowall,//照片墙
                           //doctor_working_schedule = a.doctor_working_schedule,// 医院医生排班表
                           //doctor_chest_card = a.doctor_chest_card,// 医生工牌或胸卡
                           //doctor_business_card = a.doctor_business_card,// 医生名片
                           //doctor_registration = a.doctor_registration,// 医院医生挂号单
                           doctor_office_tel = a.doctor_office_tel// 科室座机号码
                       };

            var total = list.Count();
            var rows = list.OrderByDescending(s => s.CreateTime)
                .ToList();

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
        /// 批量保存申诉医生列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel UpdateApperalInfo(List<YXDoctorViewModel> DoctorInfo)
        {
            return AddUpdateDoctorInfo(DoctorInfo);
        }
    }
}
