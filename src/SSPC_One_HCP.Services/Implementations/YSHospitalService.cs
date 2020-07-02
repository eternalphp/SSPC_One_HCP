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
    public class YSHospitalService : IYSHospitalService
    {
        private readonly IEfRepository _rep;

        public YSHospitalService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// 获取清洗医院列表
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel GetHospitalInfo(RowNumModel<HospitalViewModel> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = from a in _rep.Where<HospitalInfo>(s => s.IsDeleted != 1 && s.IsVerify == 0)
                       select new YXHospitalViewModel
                       {
                           Id = a.Id,
                           HospitalId = a.HospitalId,
                           HospitalName = a.HospitalName,
                           HospitalCode = a.HospitalCode,
                           //CustomerType = a.CustomerType == 1 ? "公有" : "私有",
                           CustomerType = a.CustomerType,
                           AreaId = a.AreaId,
                           Address = a.Address,
                           NetAddress = a.NetAddress,
                           PhoneNum = a.PhoneNum,
                           ZipCode = a.ZipCode,
                           HospitalTypeFlag = a.HospitalTypeFlag.ToString(),
                           Email = a.Email,
                           HospitalType = a.HospitalType,
                           ComeFrom = a.ComeFrom,
                           PyFull = a.PyFull,
                           PyShort = a.PyShort,
                           YsId = a.yunshi_hospital_id,
                           CreateTime = a.CreateTime,
                           //IsVerify = a.IsVerify == 0 ? "否" : "是",          
                           IsVerify = a.IsVerify,
                           hospital_code = a.hospital_code,
                           hospital_name_SFE = a.hospital_name_SFE,
                           serial_number = a.serial_number,
                           status = a.status,
                           reason = a.reason,
                           unique_hospital_id = a.unique_hospital_id,
                           yunshi_hospital_id = a.yunshi_hospital_id,
                           hospital_name = a.hospital_name,
                           hospital_phone = a.hospital_alias,
                           hospital_level = a.hospital_level,
                           hospital_category = a.hospital_category,
                           hospital_nature = a.hospital_nature,
                           hospital_organization_code = a.hospital_organization_code,
                           province = a.province,
                           city = a.city,
                           area = a.area,
                           YsAddress = a.YsAddress,
                           post_code = a.post_code,
                           website = a.website,
                           //number_of_beds = a.number_of_beds.ToString(),
                           number_of_beds = a.number_of_beds,
                           //number_of_outpatient = a.number_of_outpatient.ToString(),
                           number_of_outpatient = a.number_of_outpatient,
                           //hospitalization = a.hospitalization.ToString(),
                           hospitalization = a.hospitalization,
                           hospital_intro = a.hospital_intro,
                           //number_of_employees = a.number_of_employees.ToString(),
                           number_of_employees = a.number_of_employees,
                           modifier = a.modifier,
                           change_time = a.change_time,
                           data_update_type = a.data_update_type
                       };

            if (rowNum?.SearchParams != null)
            {
                if (!string.IsNullOrEmpty(rowNum.SearchParams.Id))
                {
                    list = list.Where(s => s.Id == rowNum.SearchParams.Id);
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.HospitalId))
                {
                    list = list.Where(s => s.HospitalId == rowNum.SearchParams.HospitalId);
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.HospitalName))
                {
                    list = list.Where(s => (s.HospitalName ?? "").Contains(rowNum.SearchParams.HospitalName));
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.PhoneNum))
                {
                    list = list.Where(s => (s.PhoneNum ?? "").Contains(rowNum.SearchParams.PhoneNum));
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.HospitalCode))
                {
                    list = list.Where(s => (s.HospitalCode ?? "").Contains(rowNum.SearchParams.HospitalCode));
                }
                if (rowNum.SearchParams.IsVerify.HasValue)
                {
                    list = list.Where(s => s.IsVerify == rowNum.SearchParams.IsVerify);
                }
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
        /// 批量清洗医院
        /// </summary>
        /// <param name="HospitalInfo"></param>
        /// <returns></returns>
        public ReturnValueModel AddUpdateHospitalInfo(List<YXHospitalViewModel> HospitalInfo)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (HospitalInfo == null || HospitalInfo.Count == 0)
            {
                rvm.Msg = "没有医院数据需要更新。";
                rvm.Success = false;
                return rvm;
            }

            List<HospitalInfo> list = new List<HospitalInfo>();

            foreach (var item in HospitalInfo)
            {
                var hospital = _rep.FirstOrDefault<HospitalInfo>(s => s.Id == item.Id);
                if (hospital != null)
                {
                    //item.Id = items.Id;
                    switch ((item.status ?? "").Trim().ToLower())
                    {
                        case "active":
                            hospital.IsVerify = 1;
                            break;
                        case "inactive":
                            hospital.IsVerify = 3;
                            break;
                        case "undetermined":
                            hospital.IsVerify = 2;
                            break;
                        default:
                            hospital.IsVerify = 0;
                            break;
                    }
                    //hospital.YsId = item.YsId;
                    hospital.hospital_code = item.hospital_code;
                    hospital.hospital_name_SFE = item.hospital_name_SFE;
                    hospital.serial_number = item.serial_number;
                    hospital.status = item.status;
                    hospital.reason = item.reason;
                    hospital.unique_hospital_id = item.unique_hospital_id;
                    hospital.yunshi_hospital_id = item.yunshi_hospital_id;
                    hospital.hospital_name = item.hospital_name;
                    hospital.hospital_alias = item.hospital_alias;
                    hospital.hospital_level = item.hospital_level;
                    hospital.hospital_category = item.hospital_category;
                    hospital.hospital_nature = item.hospital_nature;
                    hospital.hospital_organization_code = item.hospital_organization_code;
                    hospital.province = item.province;
                    hospital.city = item.city;
                    hospital.area = item.area;
                    hospital.YsAddress = item.YsAddress;
                    hospital.post_code = item.post_code;
                    hospital.hospital_phone = item.hospital_phone;
                    hospital.website = item.website;
                    //hospital.number_of_beds = Convert.ToInt32(item.number_of_beds);
                    hospital.number_of_beds = item.number_of_beds;
                    //hospital.number_of_outpatient = Convert.ToInt32(item.number_of_outpatient);
                    hospital.number_of_outpatient = item.number_of_outpatient;
                    //hospital.hospitalization = Convert.ToInt32(item.hospitalization);
                    hospital.hospitalization = item.hospitalization;
                    hospital.hospital_intro = item.hospital_intro;
                    //hospital.number_of_employees = Convert.ToInt32(item.number_of_employees);
                    hospital.number_of_employees = item.number_of_employees;
                    hospital.modifier = item.modifier;
                    if (item.change_time != null)
                    {
                        hospital.change_time = item.change_time;
                    }
                    else
                    {
                        hospital.change_time = DateTime.Now;
                    }

                    hospital.data_update_type = item.data_update_type;
                    _rep.Update(hospital);
                    _rep.SaveChanges();

                    LoggerHelper.WriteLogInfo($"---- AddUpdateHospitalInfo Update Begin 批量清洗医院 ---------------");
                    LoggerHelper.WriteLogInfo($"[doctor.Id]:{hospital?.Id}\n");
                    LoggerHelper.WriteLogInfo($"[doctor.HospitalName]:{ hospital?.HospitalName }\n");
                    LoggerHelper.WriteLogInfo($"[doctor.IsVerify]:{ hospital?.IsVerify }\n");
                    LoggerHelper.WriteLogInfo($"---- AddUpdateHospitalInfo Update End   ----------------------------");
                }
                else
                {
                    hospital = new HospitalInfo()
                    {
                        Id = Guid.NewGuid().ToString(),
                        IsDeleted = 0,
                        IsEnabled = 0,
                        //YsId = item.YsId,
                        hospital_code = item.hospital_code,
                        hospital_name_SFE = item.hospital_name_SFE,
                        serial_number = item.serial_number,
                        status = item.status,
                        reason = item.reason,
                        unique_hospital_id = item.unique_hospital_id,
                        yunshi_hospital_id = item.yunshi_hospital_id,
                        hospital_name = item.hospital_name,
                        hospital_alias = item.hospital_alias,
                        hospital_level = item.hospital_level,
                        hospital_category = item.hospital_category,
                        hospital_nature = item.hospital_nature,
                        hospital_organization_code = item.hospital_organization_code,
                        province = item.province,
                        city = item.city,
                        area = item.area,
                        YsAddress = item.YsAddress,
                        post_code = item.post_code,
                        hospital_phone = item.hospital_phone,
                        website = item.website,
                        //number_of_beds = Convert.ToInt32(item.number_of_beds),
                        number_of_beds = item.number_of_beds,
                        //number_of_outpatient = Convert.ToInt32(item.number_of_outpatient),
                        number_of_outpatient = item.number_of_outpatient,
                        //hospitalization = Convert.ToInt32(item.hospitalization),
                        hospitalization = item.hospitalization,
                        hospital_intro = item.hospital_intro,
                        //number_of_employees = Convert.ToInt32(item.number_of_employees),
                        number_of_employees = item.number_of_employees,
                        modifier = item.modifier,
                        change_time = item.change_time,
                        data_update_type = item.data_update_type,
                        CreateTime = DateTime.Now
                    };
                    switch ((item.status ?? "").Trim().ToLower())
                    {
                        case "active":
                            hospital.IsVerify = 1;
                            break;
                        case "inactive":
                            hospital.IsVerify = 3;
                            break;
                        case "undetermined":
                            hospital.IsVerify = 2;
                            break;
                        default:
                            hospital.IsVerify = 0;
                            break;
                    }
                    list.Add(hospital);

                    LoggerHelper.WriteLogInfo($"---- AddUpdateHospitalInfo Add Begin 批量清洗医院 ---------------");
                    LoggerHelper.WriteLogInfo($"[doctor.Id]:{hospital?.Id}\n");
                    LoggerHelper.WriteLogInfo($"[doctor.HospitalName]:{ hospital?.HospitalName }\n");
                    LoggerHelper.WriteLogInfo($"[doctor.IsVerify]:{ hospital?.IsVerify }\n");
                    LoggerHelper.WriteLogInfo($"---- AddUpdateHospitalInfo Add End   ----------------------------");
                }
            }

            if (list.Count() > 0)
                _rep.BulkCopyInsert(list);

            _rep.SaveChanges();


            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                //DoctorInfo = DoctorInfo
                msg = "批量更新医院成功！"
            };
            return rvm;
        }



        /// <summary>
        /// 獲取已清洗醫生医院
        /// </summary>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public ReturnValueModel UpdateHospitalInfo(RowNumModel<YXHospitalViewModel> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = from a in _rep.Where<HospitalInfo>(s => s.IsDeleted != 1 && !string.IsNullOrEmpty(s.unique_hospital_id))
                       select new YXHospitalViewModel
                       {
                           Id = a.Id,
                           HospitalId = a.HospitalId,
                           HospitalName = a.HospitalName,
                           HospitalCode = a.HospitalCode,
                           //CustomerType = a.CustomerType == 1 ? "公有" : "私有",
                           CustomerType = a.CustomerType,
                           AreaId = a.AreaId,
                           Address = a.Address,
                           NetAddress = a.NetAddress,
                           PhoneNum = a.PhoneNum,
                           ZipCode = a.ZipCode,
                           HospitalTypeFlag = a.HospitalTypeFlag.ToString(),
                           Email = a.Email,
                           HospitalType = a.HospitalType,
                           ComeFrom = a.ComeFrom,
                           PyFull = a.PyFull,
                           PyShort = a.PyShort,
                           YsId = a.yunshi_hospital_id,
                           CreateTime = a.CreateTime,
                           //IsVerify = a.IsVerify == 0 ? "否" : "是",
                           IsVerify = a.IsVerify,
                           hospital_code = a.hospital_code,
                           hospital_name_SFE = a.hospital_name_SFE,
                           serial_number = a.serial_number,
                           status = a.status,
                           reason = a.reason,
                           unique_hospital_id = a.unique_hospital_id,
                           yunshi_hospital_id = a.yunshi_hospital_id,
                           hospital_name = a.hospital_name,
                           hospital_phone = a.hospital_alias,
                           hospital_level = a.hospital_level,
                           hospital_category = a.hospital_category,
                           hospital_nature = a.hospital_nature,
                           hospital_organization_code = a.hospital_organization_code,
                           province = a.province,
                           city = a.city,
                           area = a.area,
                           //address = a.address,
                           post_code = a.post_code,
                           website = a.website,
                           //number_of_beds = a.number_of_beds.ToString(),
                           number_of_beds = a.number_of_beds,
                           //number_of_outpatient = a.number_of_outpatient.ToString(),
                           number_of_outpatient = a.number_of_outpatient,
                           //hospitalization = a.hospitalization.ToString(),
                           hospitalization = a.hospitalization,
                           hospital_intro = a.hospital_intro,
                           //number_of_employees = a.number_of_employees.ToString(),
                           number_of_employees = a.number_of_employees,
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
    }
}
