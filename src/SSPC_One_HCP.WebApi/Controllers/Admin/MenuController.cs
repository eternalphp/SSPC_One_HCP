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
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'MenuController'
    public class MenuController : BaseApiController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'MenuController'
    {
        private readonly IMenuService _menuService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="menuService">菜单服务</param>
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }
        /// <summary>
        /// 根据用户权限显示菜单
        /// </summary>
        /// <response code="400">请求错误</response>
        /// <response code="401">无权访问</response>
        /// <response code="500">服务器错误</response>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetMenusByRole()
        {
            var ret = _menuService.GetMenus(WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 新增或更新菜单信息
        /// </summary>
        /// <param name="menuInfo">菜单信息</param>
        /// <response code="400">请求错误</response>
        /// <response code="401">无权访问</response>
        /// <response code="500">服务器错误</response>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateMenu(MenuInfo menuInfo)
        {
            var ret = _menuService.InsertOrUpdateMenu(menuInfo, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <param name="rowMenu">分页、搜索</param>
        /// <response code="400">请求错误</response>
        /// <response code="401">无权访问</response>
        /// <response code="500">服务器错误</response>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult MenuList(RowNumModel<MenuInfoViewModel> rowMenu)
        {
            var ret = _menuService.MenuList(rowMenu);
            return Ok(ret);
        }
        /// <summary>
        /// 父级菜单下拉
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ParentMenus()
        {
            var ret = _menuService.ParentMenus();
            return Ok(ret);
        }
        /// <summary>
        /// 逻辑删除菜单
        /// </summary>
        /// <param name="menuInfo">菜单信息，传入主键Id</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DelMenu(MenuInfo menuInfo)
        {
            var ret = _menuService.DeleteMenu(menuInfo, WorkUser);
            return Ok(ret);
        }
        /// <summary>
        /// 更新菜单是否都可访问
        /// </summary>
        /// <param name="menuIds">菜单主键的集合</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateNormal(List<string> menuIds)
        {
            var ret = _menuService.UpdateNormal(menuIds);
            return Ok(ret);
        }
    }
}
