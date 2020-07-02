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
    public interface IRoleService
    {
        /// <summary>
        /// 权限列表
        /// </summary>
        /// <param name="rowRole">分页搜索</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel GetRoleList(RowNumModel<RoleInfo> rowRole, WorkUser workUser);

        /// <summary>
        /// 新增或删除角色
        /// </summary>
        /// <param name="roleInfo">角色基本信息</param>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        ReturnValueModel InsertOrUpdateRole(RoleInfo roleInfo, WorkUser workUser);

        /// <summary>
        /// 绑定用户角色
        /// </summary>
        /// <param name="userRoleViewModel">用户角色信息</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel InsertOrUpdateUserRole(UserRoleViewModel userRoleViewModel, WorkUser workUser);

        /// <summary>
        /// 绑定用户角色
        /// </summary>
        /// <param name="roleInfo">权限信息，是需要传入角色Id</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        ReturnValueModel BindUserRole(RoleInfo roleInfo, WorkUser workUser);

        /// <summary>
        /// 角色绑定可查看菜单
        /// </summary>
        /// <param name="roleMenuViewModel">角色菜单信息</param>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        ReturnValueModel BindRoleMenu(RoleMenuViewModel roleMenuViewModel, WorkUser workUser);

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="roleInfo">当前角色,传入Id</param>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        ReturnValueModel GetRoleMenus(RoleInfo roleInfo, WorkUser workUser);

        /// <summary>
        /// 菜单父级下拉
        /// </summary>
        /// <returns></returns>
        ReturnValueModel GetMenuSelect();
    }
}
