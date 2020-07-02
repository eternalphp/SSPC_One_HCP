using EntityFramework.Extensions;
using SSPC_One_HCP.Core.Cache;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.MenuModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Implementations
{
    /// <summary>
    /// 权限服务
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly IEfRepository _rep;
        private readonly ICommonService _commonService;
        private readonly ICacheManager _cacheManager;

        public RoleService(IEfRepository rep, ICommonService commonService, ICacheManager cacheManager)
        {
            _rep = rep;
            _commonService = commonService;
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 权限列表
        /// </summary>
        /// <param name="rowRole">分页搜索</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel GetRoleList(RowNumModel<RoleInfo> rowRole, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = (from a in _rep.Table<RoleInfo>()
                        where a.IsDeleted != 1
                        select a).Where(rowRole.SearchParams);
            var total = list.Count();
            var rows = list.OrderBy(o => o.RoleName).ToPaginationList(rowRole.PageIndex, rowRole.PageSize);
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                total = total,
                rows = rows
            };
            return rvm;
        }
        /// <summary>
        /// 新增或删除角色
        /// </summary>
        /// <param name="roleInfo">角色基本信息</param>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        public ReturnValueModel InsertOrUpdateRole(RoleInfo roleInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var role = _rep.FirstOrDefault<RoleInfo>(s => s.Id == roleInfo.Id);
            if (role == null)
            {
                roleInfo.Id = Guid.NewGuid().ToString();
                roleInfo.CreateTime = DateTime.Now;
                roleInfo.CreateUser = workUser.User.Id;
                _rep.Insert(roleInfo);
            }
            else
            {
                role.RoleName = roleInfo.RoleName;
                role.RoleDesc = roleInfo.RoleName;
                role.CompanyCode = roleInfo.CompanyCode;
                role.UpdateTime = DateTime.Now;
                role.UpdateUser = workUser.User.Id;
                _rep.Update(role);
            }

            _rep.SaveChanges();
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                role = role
            };
            return rvm;
        }
        /// <summary>
        /// 绑定用户角色
        /// </summary>
        /// <param name="userRoleViewModel">用户角色信息</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel InsertOrUpdateUserRole(UserRoleViewModel userRoleViewModel, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {
                    var roleInCom = _rep.FirstOrDefault<RoleInfo>(s => s.Id == userRoleViewModel.RoleId);
                    if (roleInCom != null)
                    {
                        var urList = _rep.Where<UserRole>(s => s.RoleId == userRoleViewModel.RoleId /*&& s.CompanyCode == roleInCom.CompanyCode*/);
                        var sapCodes = urList.Select(s => s.SapCode).ToList().Except(userRoleViewModel.SapCodeList).ToList();
                        if (sapCodes.Any())
                        {
                            foreach (var item in sapCodes)
                            {
                                _cacheManager.Remove(item);
                            }
                        }
                        urList.Delete();
                        _rep.SaveChanges();

                        foreach (var sapCode in userRoleViewModel.SapCodeList)
                        {
                            var ur = new UserRole
                            {
                                Id = Guid.NewGuid().ToString(),
                                SapCode = sapCode,
                                RoleId = userRoleViewModel.RoleId,
                                UserId = userRoleViewModel.UserId,
                                CompanyCode = roleInCom.CompanyCode,
                                CreateTime = DateTime.Now,
                                CreateUser = workUser.User.Id
                            };
                            _rep.Insert(ur);
                            _cacheManager.Remove(sapCode);
                            _rep.SaveChanges();
                        }

                        rvm.Success = true;
                        rvm.Msg = "";
                        rvm.Result = new
                        {

                        };
                        tran.Commit();
                    }
                    else
                    {
                        rvm.Success = false;
                        rvm.Msg = "there was no role in table";
                        rvm.Result = new
                        {
                            roleId = userRoleViewModel.RoleId
                        };
                    }
                }
                catch (Exception e)
                {
                    rvm.Success = false;
                    rvm.Msg = e.Message;
                    rvm.Result = new
                    {
                        e
                    };
                }
            }



            return rvm;
        }
        /// <summary>
        /// 绑定用户角色
        /// </summary>
        /// <param name="roleInfo">权限信息，是需要传入角色Id</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel BindUserRole(RoleInfo roleInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var isAdmin = _commonService.IsAdmin(workUser);

            //获取角色ID对应的角色列表
            var roleInCom = _rep.FirstOrDefault<RoleInfo>(s => s.Id == roleInfo.Id);
            var userHasRole = (from a in _rep.All<UserModel>()
                               join b in _rep.All<UserRole>() on a.Code equals b.SapCode
                               where (a.CompanyCode == b.CompanyCode
                                     && b.CompanyCode == roleInCom.CompanyCode || isAdmin)
                                     && a.IsDeleted != 1 && b.IsDeleted != 1
                                     && b.RoleId == roleInfo.Id
                               select a).ToList().Select(s => new {key= s.Code, label= s.ChineseName + '-' + s.Code } );
            var userNoRole = _rep.SqlQuery<UserRoleView>($@"select a.Code as 'key',a.ChineseName+'-'+a.Code as 'label'   from UserInfo a 
            left join UserRole b on a.EmployeeNo = b.SapCode
            left join RoleInfo c on b.RoleId = c.id
            where ISNULL(b.RoleId, '') != '{roleInfo.Id}' and ISNULL(c.RoleName, '') != '系统管理员' and a.IsDeleted!=1 ").ToList();

            //只能有一个权限
            var hasRole = _rep.Where<UserRole>(x => true).Select(x => x.SapCode).ToList();
            userNoRole = userNoRole.Where(x => !hasRole.Contains(x.key)).ToList();

            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                userHasRole = userHasRole,
                userNoRole = userNoRole
            };
            return rvm;
        }
        /// <summary>
        /// 角色绑定可查看菜单
        /// </summary>
        /// <param name="roleMenuViewModel">角色菜单信息</param>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        public ReturnValueModel BindRoleMenu(RoleMenuViewModel roleMenuViewModel, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {
                    var roleInCom = _rep.FirstOrDefault<RoleInfo>(s => s.Id == roleMenuViewModel.RoleId);
                    if (roleInCom != null)
                    {
                        var urList = _rep.Where<RoleMenu>(s => s.RoleId == roleMenuViewModel.RoleId).ToList();
                        foreach (var item in urList)
                        {
                            _rep.Delete(item);
                            _rep.SaveChanges();
                        }
                        var rms = _rep.Where<MenuInfo>(s => s.IsDeleted != 1).ToList();
                        List<string> pMenusIds = new List<string>();
                        //var gEmpty = Guid.Empty.ToString();
                        GetParentMenuIds(pMenusIds, roleMenuViewModel.MenuIds, rms);
                        //List<string> rootMenus = new List<string>();
                        roleMenuViewModel.MenuIds.AddRange(pMenusIds);
                        roleMenuViewModel.MenuIds = roleMenuViewModel.MenuIds.Distinct().ToList();
                        foreach (var menuId in roleMenuViewModel.MenuIds)
                        {
                            //var rm = rms.FirstOrDefault<MenuInfo>(s => s.Id == menuId);
                            //rootMenus.Add(rm.ParentId);
                            var ur = new RoleMenu
                            {
                                Id = Guid.NewGuid().ToString(),
                                MenuId = menuId,
                                RoleId = roleMenuViewModel.RoleId,
                                CompanyCode = roleInCom.CompanyCode,
                                CreateTime = DateTime.Now,
                                CreateUser = workUser.User.Id
                            };
                            _rep.Insert(ur);
                        }
                        _rep.SaveChanges();
                        rvm.Success = true;
                        rvm.Msg = "";
                        rvm.Result = new
                        {
                            roleMenuViewModel.MenuIds
                        };
                        tran.Commit();
                    }
                    else
                    {
                        rvm.Success = false;
                        rvm.Msg = "there was no role in table";
                        rvm.Result = new
                        {
                            roleId = roleMenuViewModel.RoleId
                        };
                    }
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    rvm.Success = false;
                    rvm.Msg = e.Message;
                    rvm.Result = new
                    {
                        e
                    };
                }
            }
            return rvm;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="roleInfo">当前角色,传入Id</param>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        public ReturnValueModel GetRoleMenus(RoleInfo roleInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            //var canRoleIds = workUser.Roles.Select(s => s.Id).ToList();
            var eEmpty = Guid.Empty.ToString();
            var hasRole = _rep.Where<RoleMenu>(s => s.RoleId == roleInfo.Id).Select(s => s.MenuId).Distinct().ToList();
            var hasRoleMenus =
                _rep.Where<MenuInfo>(s => hasRole.Contains(s.Id) && (s.ParentId != eEmpty || s.Leaf == true)).Select(s => s.Id);
            var allMenus = _rep.Where<MenuInfo>(s => s.IsDeleted != 1).OrderBy(s => s.Sort).ToList();
            var rootId = Guid.Empty.ToString();
            var roots = allMenus.Where(s => s.ParentId == rootId);
            List<MenuViewModel> menus = new List<MenuViewModel>();
            foreach (var root in roots)
            {
                MenuViewModel menu = new MenuViewModel
                {
                    Id = root.Id,
                    Children = new List<MenuViewModel>(),
                    Label = root.MenuName
                };
                GetChildrenMenus(allMenus, root, menu);
                menus.Add(menu);
            }

            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                menus = menus,
                hasRoleMenus = hasRoleMenus
            };
            return rvm;
        }
        /// <summary>
        /// 菜单父级下拉
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetMenuSelect()
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var rootId = Guid.Empty.ToString();
            var menus = new List<MenuInfo>
            {
                new MenuInfo
                {
                    MenuName = "message.MenuRoot"
                }
            };
            var list = _rep.Where<MenuInfo>(s => s.IsDeleted != 1).OrderBy(s => s.Sort);
            menus.AddRange(list);
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                menus = menus
            };

            return rvm;
        }

        /// <summary>
        /// 递归调用
        /// </summary>
        /// <param name="menuInfos">原始数据集</param>
        /// <param name="root">一级菜单</param>
        /// <param name="menu">最终菜单</param>
        /// <param name="roleMenus">可访问菜单</param>
        private void GetChildrenMenus(List<MenuInfo> menuInfos, MenuInfo root, MenuViewModel menu)
        {
            var children = menuInfos.Where(s => s.ParentId == root.Id).ToList();
            foreach (var child in children)
            {
                MenuViewModel childMenu = new MenuViewModel
                {
                    Id = child.Id,
                    Children = new List<MenuViewModel>(),
                    Label = child.MenuName
                };
                GetChildrenMenus(menuInfos, child, childMenu);
                menu.Children.Add(childMenu);
            }
        }

        /// <summary>
        /// 获取父级菜单Id
        /// </summary>
        /// <param name="pMenuIds"></param>
        /// <param name="selectedMenuIds"></param>
        /// <param name="menus"></param>
        private void GetParentMenuIds(List<string> pMenuIds, List<string> selectedMenuIds, List<MenuInfo> menus)
        {

            List<string> sMenuIds = new List<string>();
            if (selectedMenuIds.Any())
            {
                foreach (var sm in selectedMenuIds)
                {
                    var rm = menus.FirstOrDefault<MenuInfo>(s => s.Id == sm);
                    if (rm != null)
                    {
                        pMenuIds.Add(rm.ParentId);
                        sMenuIds.Add(rm.ParentId);
                    }
                }

                GetParentMenuIds(pMenuIds, sMenuIds, menus);
            }

        }
    }
}
