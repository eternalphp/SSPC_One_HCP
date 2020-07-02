using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.MenuModels;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Core.LinqExtented;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;

namespace SSPC_One_HCP.Services.Implementations
{
    /// <summary>
    /// 菜单实现
    /// </summary>
    public class MenuService : IMenuService
    {
        private readonly IEfRepository _rep;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rep">仓储注入</param>
        public MenuService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 根据权限获取菜单
        /// </summary>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        public ReturnValueModel GetMenus(WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var canRoleIds = workUser.Roles.Select(s => s.Id).ToList();
            var norMenus = _rep.Where<MenuInfo>(s => s.IsDeleted != 1 && s.IsNormal == true).Select(s => s.Id).ToList();
            var roleMenus = _rep.Where<RoleMenu>(s => canRoleIds.Contains(s.RoleId)).Select(s => s.MenuId).Union(norMenus).Distinct().ToList();
            var allMenus = _rep.Where<MenuInfo>(s => s.IsDeleted != 1).OrderBy(s => s.Sort).ToList();
            string rootId = Guid.Empty.ToString();
            var roots = allMenus.Where(s => s.ParentId == rootId && roleMenus.Contains(s.Id));
            List<MenuViewModel> menus = new List<MenuViewModel>();
            foreach (var root in roots)
            {
                MenuViewModel menu = new MenuViewModel
                {
                    Name = root.MenuName ?? "",
                    Component = root.Component ?? "",
                    IconCls = root.MenuIcons ?? "",
                    Path = root.MenuPath ?? "",
                    Leaf = root.Leaf ?? false,
                    Hidden = root.Hidden ?? false,
                    LinkPath = root.LinkPath ?? "",
                    Children = new List<MenuViewModel>()
                };
                GetChildrenMenus(allMenus, root, menu, roleMenus);
                menus.Add(menu);
            }

            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = menus;
            return rvm;
        }

        /// <summary>
        /// 递归调用
        /// </summary>
        /// <param name="menuInfos">原始数据集</param>
        /// <param name="root">一级菜单</param>
        /// <param name="menu">最终菜单</param>
        /// <param name="roleMenus">可访问菜单</param>
        private void GetChildrenMenus(List<MenuInfo> menuInfos, MenuInfo root, MenuViewModel menu, List<string> roleMenus)
        {
            var children = menuInfos.Where(s => s.ParentId == root.Id && roleMenus.Contains(s.Id)).ToList();
            foreach (var child in children)
            {
                MenuViewModel childMenu = new MenuViewModel
                {
                    Name = child.MenuName ?? "",
                    Component = child.Component ?? "",
                    IconCls = child.MenuIcons ?? "",
                    Path = child.MenuPath ?? "",
                    Leaf = child.Leaf ?? false,
                    Hidden = child.Hidden ?? false,
                    LinkPath = child.LinkPath ?? "",
                    Children = new List<MenuViewModel>()
                };
                GetChildrenMenus(menuInfos, child, childMenu, roleMenus);
                menu.Children.Add(childMenu);
            }
        }
        /// <summary>
        /// 新增或更新菜单信息
        /// </summary>
        /// <param name="menuInfo">菜单信息</param>
        /// <param name="workUser">当前操作用户</param>
        /// <returns></returns>
        public ReturnValueModel InsertOrUpdateMenu(MenuInfo menuInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var menus = _rep.Where<MenuInfo>(s => s.IsDeleted != 1).ToList();
            var menu = _rep.FirstOrDefault<MenuInfo>(s => s.Id == menuInfo.Id);

            if (menu == null)
            {
                //if (menus.Any(s => (s.MenuPath == menuInfo.MenuPath || s.LinkPath == menuInfo.LinkPath) && s.MenuPath != "/"))
                //{
                //    rvm.Success = false;
                //    rvm.Msg = "pathName";
                //    rvm.Result = new
                //    {

                //    };
                //    return rvm;
                //}
                menu = menuInfo;
                menu.Id = Guid.NewGuid().ToString().ToUpper();
                menu.CreateTime = DateTime.Now;
                menu.CreateUser = workUser.User.Id;
                _rep.Insert(menu);
            }
            else
            {
                menu.MenuName = menuInfo.MenuName;
                menu.MenuIcons = menuInfo.MenuIcons;
                menu.MenuPath = menuInfo.MenuPath;
                menu.LinkPath = menuInfo.LinkPath;
                menu.ParentId = menuInfo.ParentId;
                menu.Component = menuInfo.Component;
                menu.Hidden = menuInfo.Hidden;
                menu.Leaf = menuInfo.Leaf;
                menu.Sort = menuInfo.Sort;
                menu.Props = menuInfo.Props;
                menu.UpdateTime = DateTime.Now;
                menu.UpdateUser = workUser.User.Id;
                _rep.Update(menu);
            }

            _rep.SaveChanges();
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                menu = menu
            };
            return rvm;
        }
        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <param name="rowMenu">分页、搜索</param>
        /// <returns></returns>
        public ReturnValueModel MenuList(RowNumModel<MenuInfoViewModel> rowMenu)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var list = (from a in _rep.Table<MenuInfo>()
                        join b in _rep.Table<MenuInfo>() on a.ParentId equals b.Id
                            into ab
                        from bb in ab.DefaultIfEmpty()
                        where a.IsDeleted != 1 && bb.IsDeleted != 1
                        select new MenuInfoViewModel
                        {
                            Id = a.Id,
                            Component = a.Component,
                            Hidden = a.Hidden,
                            Leaf = a.Leaf,
                            LinkPath = a.LinkPath,
                            MenuIcons = a.MenuIcons,
                            MenuName = a.MenuName,
                            ParentId = a.ParentId,
                            MenuPath = a.MenuPath,
                            ParentMenuName = bb == null ? "RootMenu" : bb.MenuName,
                            Sort = a.Sort,
                            CompanyCode = a.CompanyCode,
                            Props = a.Props,
                            Remark = a.Remark,
                            IsNormal = a.IsNormal
                        }).Where(rowMenu.SearchParams);
            var total = list.Count();
            var rows = list.OrderBy(s => s.MenuPath).ToPaginationList(rowMenu.PageIndex, rowMenu.PageSize);
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
        /// 获取父级菜单下拉
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel ParentMenus()
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var eId = Guid.Empty.ToString();
            //根目录
            List<object> menus = new List<object>
            {
                new
                {
                    value = eId,
                    label = "RootMenu",
                }
            };
            var list = _rep.Where<MenuInfo>(s => s.ParentId == eId && s.IsDeleted != 1 && s.Hidden != true).OrderBy(o => o.Sort).Select(s => new
            {
                value = s.Id,
                label = s.MenuName
            }).ToList();
            menus.AddRange(list);
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                list = menus
            };
            return rvm;
        }
        /// <summary>
        /// 逻辑删除菜单
        /// </summary>
        /// <param name="menuInfo">菜单信息</param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel DeleteMenu(MenuInfo menuInfo, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var menu = _rep.FirstOrDefault<MenuInfo>(s => s.Id == menuInfo.Id);
            if (menu != null)
            {
                menu.IsDeleted = 1;
                menu.UpdateTime = DateTime.Now;
                menu.UpdateUser = workUser.User.Id;
                _rep.Update(menu);
                _rep.SaveChanges();
                rvm.Success = true;
                rvm.Msg = "";
                rvm.Result = new
                {
                    menu
                };
            }

            return rvm;
        }
        /// <summary>
        /// 更新菜单可访问状态
        /// </summary>
        /// <param name="menuIds">菜单主键的集合</param>
        /// <returns></returns>
        public ReturnValueModel UpdateNormal(List<string> menuIds)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var menus = _rep.Table<MenuInfo>();
            menus.Update(s => new MenuInfo
            {
                IsNormal = false
            });
            _rep.SaveChanges();
            menus.Where(s => menuIds.Contains(s.Id)).Update(s => new MenuInfo
            {
                IsNormal = true
            });
            _rep.SaveChanges();
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                menus
            };
            return rvm;
        }
    }
}
