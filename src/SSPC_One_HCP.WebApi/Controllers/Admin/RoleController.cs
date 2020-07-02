using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SSPC_One_HCP.WebApi.Controllers.Admin
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'RoleController'
    public class RoleController : BaseApiController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'RoleController'
    {
        private readonly IRoleService _roleService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleService">权限服务</param>
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="rowRole">分页、搜索</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetRoleList(RowNumModel<RoleInfo> rowRole)
        {
            var ret = _roleService.GetRoleList(rowRole, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 新增或删除角色
        /// </summary>
        /// <param name="roleInfo">角色基本信息</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateRole(RoleInfo roleInfo)
        {
            var ret = _roleService.InsertOrUpdateRole(roleInfo, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 角色绑定用户
        /// </summary>
        /// <param name="userRoleViewModel">角色用户绑定信息</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateUserRole(UserRoleViewModel userRoleViewModel)
        {
            var ret = _roleService.InsertOrUpdateUserRole(userRoleViewModel, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 绑定用户角色
        /// </summary>
        /// <param name="roleInfo">权限信息，是需要传入角色Id</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult BindUserRole(RoleInfo roleInfo)
        {
            var ret = _roleService.BindUserRole(roleInfo, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 角色绑定可查看菜单
        /// </summary>
        /// <param name="roleMenuViewModel">角色菜单信息</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult BindRoleMenu(RoleMenuViewModel roleMenuViewModel)
        {
            var ret = _roleService.BindRoleMenu(roleMenuViewModel, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="roleInfo">当前角色,传入Id</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetRoleMenus(RoleInfo roleInfo)
        {
            var ret = _roleService.GetRoleMenus(roleInfo, WorkUser);
            return Ok(ret);
        }
    }
}
