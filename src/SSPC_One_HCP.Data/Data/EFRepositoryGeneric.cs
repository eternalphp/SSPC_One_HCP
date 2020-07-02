using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.Models.DataModels;

namespace SSPC_One_HCP.Data.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EFRepositoryGeneric<TEntity> : EfRepository, IEfRepositoryGeneric<TEntity> where TEntity:class,new()
    {
        /// <summary>
        /// 上下文对象
        /// </summary>
        public IDbContext DbCxt { get; set; }

        private IDbContext _context = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public EFRepositoryGeneric(IDbContext context):base(context)
        {
            _context = context;
            DbCxt = context;
        }

        /// <summary>
        /// 查找所有匹配的
        /// </summary>
        /// <param name="express"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> FindList(Expression<Func<TEntity, bool>> express)
        {
            Func<TEntity, bool> lambda = express.Compile();
            return this._context.Set<TEntity>().Where(lambda).AsQueryable();
        }
        /// <summary>
        /// 局部更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int UpdatePartModel(TEntity entity)
        {
            this._context.Set<TEntity>().Attach(entity);
            this._context.Entry(entity).State = EntityState.Modified;
            var affectedrows = this._context.SaveChanges();
            return affectedrows;
        }
    }
}
