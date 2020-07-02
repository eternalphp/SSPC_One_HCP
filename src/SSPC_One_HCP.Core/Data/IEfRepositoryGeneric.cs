using System;
using System.Linq;
using System.Linq.Expressions;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;

namespace SSPC_One_HCP.Core.Data
{
    /// <summary>
    /// 泛型仓储器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEfRepositoryGeneric<TEntity> : IEfRepository  
    {
        #region 公共方法
        /// <summary>
        /// 上下文
        /// </summary>
        IDbContext DbCxt { get; set; }

        /// <summary>
        /// 根据lamada表达式查询集合
        /// </summary>
        /// <param name="express">lamada表达式</param>
        /// <returns></returns>
        IQueryable<TEntity> FindList(Expression<Func<TEntity, bool>> express);
        /// <summary>
        /// 局部更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int UpdatePartModel(TEntity entity);
        #endregion
    }
}
