using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Admin
{
    /// <summary>
    /// 科室
    /// </summary>
    public class DepartmentController : BaseApiController
    {
        private readonly IDepartmentService _departmentService;

        /// <summary>
        /// 科室
        /// </summary>
        /// <param name="departmentService"></param>
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// 获取科室列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetDepartmentList(DepartmentInfo department)
        {
            var ret = _departmentService.GetDepartmentList(department);
            return Ok(ret);
        }

        /// <summary>
        /// 新增或修改科室信息
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateDepartment(DepartmentInfo department)
        {
            var ret = _departmentService.AddOrUpdateDepartmentInfo(department, WorkUser);
            return Ok(ret);
        }

        /// <summary>
        /// 获取全部BU列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetAllBUList()
        {
            var ret = _departmentService.GetBUList();
            return Ok(ret);
        }

        /// <summary>
        /// 获取当前登录用户所属的BU列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetBUList()
        {
            var ret = _departmentService.GetBUList(WorkUser);
            return Ok(ret);
        }
    }
}
