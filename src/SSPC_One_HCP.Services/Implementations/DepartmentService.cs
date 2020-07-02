using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels.DepartmentModels;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IEfRepository _rep;

        public DepartmentService(IEfRepository rep)
        {
            _rep = rep;
        }

        public ReturnValueModel AddOrUpdateDepartmentInfo(DepartmentInfo department,WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var item = _rep.FirstOrDefault<DepartmentInfo>(s => s.Id == department.Id);
            if (item!=null)
            {
                department.Id = item.Id;
                item = department;
                item.UpdateTime = DateTime.Now;
                item.UpdateUser = workUser.User.Id;
                _rep.Update(item);
                _rep.SaveChanges();
            }
            else
            {
                department.Id = Guid.NewGuid().ToString();
                department.CreateTime = DateTime.Now;
                department.CreateUser = workUser.User.Id;
                _rep.Insert(department);
                _rep.SaveChanges();
            }
            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                department = department
            };

            return rvm;
        }

        /// <summary>
        /// 获取科室列表
        /// </summary>
        /// <param name="type">科室类型(1:普通科室, 2:其它科室, null:所有科室)</param>
        /// <returns></returns>
        public ReturnValueModel GetDepartmentList(DepartmentInfo department)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = _rep.Where<DepartmentInfo>(s => s.IsDeleted != 1);

            if (department != null)
            {
                if (department.DepartmentType.HasValue)
                {
                    list = list.Where(s => s.DepartmentType == department.DepartmentType.Value);
                }
            }

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                list = from a in list
                       select new DepartmentViewModel
                       {
                           Id = a.Id,
                           DepartmentName = a.DepartmentName,
                           DepartmentType = a.DepartmentType
                       }
            };

            return rvm;
        }

        /// <summary>
        /// 获取BU列表
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetBUList(WorkUser workUser = null)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = _rep.Where<BuInfo>(s => s.IsDeleted != 1);

            if (workUser != null && workUser.Organization != null)
            {
                list = list.Where(s => s.BuName == workUser.Organization.BuName);
            }

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                list = list
            };

            return rvm;
        }


    }
}
