using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;

namespace SSPC_One_HCP.Services.Implementations
{
    /// <summary>
    /// 小程序通用服务实现
    /// </summary>
    public class WxCommonService : IWxCommonService
    {
        /// <summary>
        /// 声明
        /// </summary>
        private readonly IEfRepository _rep;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rep">仓储</param>
        public WxCommonService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 获取医院
        /// </summary>
        /// <param name="rowNum">模糊搜索，HospitalName</param>
        /// <returns></returns>
        public Task<ReturnValueModel> GetHospital(RowNumModel<HospitalInfo> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            try
            {
                string hospitalName = rowNum?.SearchParams?.HospitalName;

                string sql = "select * from HospitalInfo where IsDeleted<>1 and IsVerify=1";

                if (!string.IsNullOrEmpty(hospitalName))
                {
                    hospitalName = string.Join("%", hospitalName.ToArray());
                    sql += $" and HospitalName like '%{hospitalName}%'";
                }

                var list = from a in _rep.SqlQuery<HospitalInfo>(sql)
                           select a.HospitalName;

                var total = list.Count();
                var rows = list.Distinct().OrderBy(s => s).ToPaginationList(rowNum?.PageIndex, rowNum?.PageSize).ToList();

                rvm.Success = true;
                rvm.Msg = "success";
                rvm.Result = new
                {
                    total,
                    hospitalNames = rows
                };
            }
            catch (Exception ex)
            {
                rvm.Success = false;
                rvm.Msg = ex.Message;
            }

            return Task.FromResult(rvm);
        }

        /// <summary>
        /// 获取科室
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValueModel> GetDept(DepartmentInfo departmentInfo)
        {
            ReturnValueModel rvm = new ReturnValueModel();

             
            var dept = await _rep.Where<DepartmentInfo>(s => s.IsDeleted != 1 && (string.IsNullOrEmpty(departmentInfo.DepartmentName) || s.DepartmentName.Contains(departmentInfo.DepartmentName))).Distinct().ToListAsync();

            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                deptNames = dept.OrderByDescending(s=>s.Remark).Select(s => s.DepartmentName)
            };

            return rvm;
        }
    }
}
