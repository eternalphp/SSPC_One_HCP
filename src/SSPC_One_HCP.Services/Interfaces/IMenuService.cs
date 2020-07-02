using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Interfaces
{
    /// <summary>
    /// 菜单接口
    /// </summary>
    public interface IMenuService
    {
        /// <summary>
        /// 根据权限获取菜单
        /// </summary>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        ReturnValueModel GetMenus(WorkUser workUser);

        /// <summary>
        /// 新增或更新菜单信息
        /// </summary>
        /// <param name="menuInfo">菜单信息</param>
        /// <param name="workUser">当前操作用户</param>
        /// <returns></returns>
        ReturnValueModel InsertOrUpdateMenu(MenuInfo menuInfo, WorkUser workUser);

        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <param name="rowMenu">分页、搜索</param>
        /// <returns></returns>
        ReturnValueModel MenuList(RowNumModel<MenuInfoViewModel> rowMenu);

        /// <summary>
        /// 获取父级菜单下拉
        /// </summary>
        /// <returns></returns>
        ReturnValueModel ParentMenus();

        /// <summary>
        /// 逻辑删除菜单
        /// </summary>
        /// <param name="menuInfo">菜单信息</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel DeleteMenu(MenuInfo menuInfo, WorkUser workUser);

        /// <summary>
        /// 更新菜单可访问状态
        /// </summary>
        /// <param name="menuIds">菜单主键的集合</param>
        /// <returns></returns>
        ReturnValueModel UpdateNormal(List<string> menuIds);
    }
}
