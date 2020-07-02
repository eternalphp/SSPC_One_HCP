using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class MenuInfoMap : BaseEntityTypeConfiguration<MenuInfo>
    {
        public MenuInfoMap()
        {
            // Primary Key
            // Properties
            // 菜单名称
            //    如果为多语言则为多语言表中的key
            this.Property(t => t.MenuName)
                .HasMaxLength(50)
                .HasColumnName("MenuName");
            // 菜单前的图标
            this.Property(t => t.MenuIcons)
                .HasMaxLength(50)
                .HasColumnName("MenuIcons");
            // 菜单地址
            this.Property(t => t.MenuPath)
                .HasMaxLength(500)
                .HasColumnName("MenuPath");
            // 链接地址
            this.Property(t => t.LinkPath)
                .HasMaxLength(500)
                .HasColumnName("LinkPath");
            // 父级菜单id，默认为guid-empty
            this.Property(t => t.ParentId)
                .HasMaxLength(36)
                .HasColumnName("ParentId");
            // 组件地址
            this.Property(t => t.Component)
                .HasMaxLength(500)
                .HasColumnName("Component");
            // 是否隐藏
            this.Property(t => t.Hidden)
                .HasColumnName("Hidden");
            // 是否为单级目录
            this.Property(t => t.Leaf)
                .HasColumnName("Leaf");
            // 排序
            this.Property(t => t.Sort)
                .HasColumnName("Sort");
            // 参数
            this.Property(t => t.Props)
                .HasColumnName("Props");
            //  都能访问的目录
            this.Property(t => t.IsNormal)
                .HasColumnName("IsNormal");
            // Table & Column Mappings
            this.ToTable("MenuInfo");


        }
    }
}
