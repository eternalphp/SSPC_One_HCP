using SSPC_One_HCP.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using SSPC_One_HCP.Core.Domain.Models;
using SSPC_One_HCP.Core.Utils;
using System.Configuration;
using System.Data;

namespace SSPC_One_HCP.Data.Data
{
    public partial class EfRepository : IEfRepository
    {
        #region Fields

        private IDbContext _context;
        private dynamic _entities;
        //private bool _disposed;
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public EfRepository(IDbContext context)
        {
            this._context = context;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get full error
        /// </summary>
        /// <param name="exc">Exception</param>
        /// <returns>Error</returns>
        protected string GetFullErrorText(DbEntityValidationException exc)
        {
            var msg = string.Empty;
            foreach (var validationErrors in exc.EntityValidationErrors)
                foreach (var error in validationErrors.ValidationErrors)
                    msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
            return msg;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual T GetById<T>(params object[] id) where T : class, new()
        {
            //see some suggested performance optimization (not tested)

            return _context.Set<T>().Find(id);
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Insert<T>(T entity) where T : class, new()
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                _context.Set<T>().Add(entity);

                //this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void InsertList<T>(IEnumerable<T> entities) where T : class, new()
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    _context.Set<T>().Add(entity);

                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update<T>(T entity) where T : class, new()
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                //_context.Set<T>().Update(entity);
                //this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void UpdateList<T>(IEnumerable<T> entities) where T : class, new()
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }
        /// <summary>
        /// 根据条件更新
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="includeProperties">条件树</param>
        /// <param name="UpdateProperties">更新树</param>
        /// <returns></returns>
        public virtual int UpdateExtend<T>(Expression<Func<T, bool>> includeProperties, Expression<Func<T, T>> UpdateProperties) where T : class, new()
        {
            return _context.Set<T>().Where(includeProperties).Update(UpdateProperties);
        }

        /// <summary>
        /// 记录更新操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldT"></param>
        /// <param name="newT"></param>
        public virtual void UpdateWithCompare<T>(T oldT, T newT) where T : class, new()
        {
            //var props = typeof(T).GetProperties();
            //var dataId = typeof(T).GetProperties().FirstOrDefault(s => s.Name == "Id")?.GetValue(oldT) ?? 0;
            //var workUser = ContainerManager.Resolve<ICacheManager>().Get<WorkUser>("CurrentUser");
            //foreach (var prop in props)
            //{
            //    if (prop.Name == "Id" || prop.Name == "CreateTime" || prop.Name == "CreatorId" || prop.Name == "UpdateTime" || prop.Name == "UpdatorId")
            //        continue;
            //    var oldValue = (prop.GetValue(oldT) ?? "").ToString();
            //    var newValue = (prop.GetValue(newT) ?? "").ToString();
            //    if (oldValue != newValue)
            //    {
            //        T_LogUpdateField field = new T_LogUpdateField
            //        {
            //            CompanyCode = workUser.CompanyCode,
            //            TableName = typeof(T).Name,
            //            DataId = (int)(dataId ?? 0),
            //            NewValue = newValue,
            //            OldValue = oldValue,
            //            FieldName = prop.Name,
            //            CreateTime = DateTime.Now,
            //            CreatorId = workUser.User.Id
            //        };
            //        Insert(field);
            //    }
            //}
            ////除去对模型的附属
            //RemoveHoldingEntityInContext(oldT);
            ////_context.Detach(newT);
            ////模型状态改为修改
            //_context.Entry<T>(newT).State = EntityState.Modified;
            //SaveChanges();
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Delete<T>(T entity) where T : class, new()
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                var entry = _context.Entry(entity);
                RemoveHoldingEntityInContext(entity);
                if (entry.State == EntityState.Detached)
                    _context.Set<T>().Attach(entity);
                _context.Set<T>().Remove(entity);
                //_context.Entry(entity).State = EntityState.Deleted;
                //this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void DeleteList<T>(IEnumerable<T> entities) where T : class, new()
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                {
                    var entry = _context.Entry(entity);
                    RemoveHoldingEntityInContext(entity);
                    if (entry.State == EntityState.Detached)
                        _context.Set<T>().Attach(entity);
                    _context.Set<T>().Remove(entity);
                }
                //_context.Set<T>().Remove(entity);

                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// 获取第一个或默认的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public virtual T FirstOrDefault<T>(Expression<Func<T, bool>> includeProperties) where T : class, new()
        {
            return _context.Set<T>().FirstOrDefault(includeProperties);
        }

        //public IQueryable<T> Find<T>(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includeProperties) where T : class, new()
        //{
        //    return All(includeProperties).Where(criteria);
        //}
        /// <summary>
        /// 大批量插入
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="queryable">数据集</param>
        /// <returns></returns>
        public virtual void BulkCopyInsert<T>(IEnumerable<T> queryable, string tableName = null) where T : BaseEntity
        {

            try
            {


                if (_context.Database.Connection.State != ConnectionState.Open)
                {
                    _context.Database.Connection.Open();
                }
                SqlBulkCopy sqlBulkCopy = new SqlBulkCopy((SqlConnection)_context.Database.Connection)
                {

                    BatchSize = 50000,
                    DestinationTableName = !string.IsNullOrEmpty(tableName) ? tableName : typeof(T).Name,
                    BulkCopyTimeout = 3600
                };
                var dt = queryable.ToDataTable<T>(sqlBulkCopy);
                sqlBulkCopy.WriteToServer(dt);
                sqlBulkCopy.Close();
                if (_context.Database.Connection.State != ConnectionState.Closed)
                {
                    _context.Database.Connection.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual DbSet<T> Table<T>() where T : class, new()
        {
            return _context.Set<T>();
        }
        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> All<T>() where T : class, new()
        {
            return _context.Set<T>();
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking<T>() where T : class, new()
        {
            return _context.Set<T>().AsNoTracking();
        }
        /// <summary>
        /// where方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public virtual IQueryable<T> Where<T>(Expression<Func<T, bool>> includeProperties) where T : class, new()
        {
            return _context.Set<T>().Where(includeProperties);
        }

        /// <summary>
        /// Entities
        /// </summary>
        public virtual IDbSet<T> GetEntities<T>() where T : class, new()
        {
            return _context.Set<T>();
        }

        #endregion

        #region 异步加载
        /// <summary>
        /// 异步获取平均值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual Task<double> AverageAsync<T>(Expression<Func<T, int>> expression) where T : class, new()
        {
            return _context.Set<T>().AverageAsync(expression);
        }
        /// <summary>
        /// 异步获取数据条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual Task<int> CountAsync<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return _context.Set<T>().CountAsync(expression);
        }
        /// <summary>
        /// 根据条件异步获取单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return _context.Set<T>().FirstOrDefaultAsync(expression);
        }
        /// <summary>
        /// 根据条件异步获取首个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual Task<T> FirstAsync<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return _context.Set<T>().FirstAsync(expression);
        }
        #endregion

        #region 执行SQL
        /// <summary>
        /// 执行SQL获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters) where T : class, new()
        {
            return _context.SqlQuery<T>(sql, parameters);
        }
        /// <summary>
        /// 执行SQL获取单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual T SqlQueryObject<T>(string sql, params object[] parameters)
        {
            var list = _context.SqlQuery<T>(sql, parameters);
            if (list.Any())
            {
                return list.FirstOrDefault();
            }
            return default(T);
        }
        /// <summary>
        /// 执行存储过程并加载结束时的实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual IList<T> ExecuteStoredProcedureList<T>(string commandText, params object[] parameters)
            where T : class, new()
        {
            return _context.ExecuteStoredProcedureList<T>(commandText, parameters);
        }
        /// <summary>
        /// 执行SQL文
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="doNotEnsureTransaction"></param>
        /// <param name="timeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            return _context.ExecuteSqlCommand(sql, doNotEnsureTransaction, timeout, parameters);
        }
        #endregion

        /// <summary>
        /// 保存修改
        /// </summary>
        /// <returns></returns>
        public virtual int SaveChanges()
        {
            return _context.SaveChanges();
        }
        /// <summary>
        /// 提交并刷新
        /// </summary>
        public virtual void CommitAndRefreshChanges()
        {
            _context.CommitAndRefreshChanges();
        }
        /// <summary>
        /// 回滚
        /// </summary>
        public virtual void RollbackChanges()
        {
            _context.RollbackChanges();
        }
        /// <summary>
        /// 异步保存
        /// </summary>
        /// <returns></returns>
        public virtual Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }


        protected virtual IDbSet<T> Entities<T>() where T : class, new()
        {
            if (_entities == null || !(_entities is IDbSet<T>))
                _entities = _context.Set<T>();
            return _entities as IDbSet<T>;
        }

        /// <summary>
        /// 回收释放
        /// </summary>
        private bool m_disposed;

        public virtual void Dispose()
        {
            if (!m_disposed)
            {
                // Release unmanaged resources
                _context = null;
                m_disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        //用于监测Context中的Entity是否存在，如果存在，将其Detach，防止出现问题。
        private bool RemoveHoldingEntityInContext<T>(T entity) where T : class
        {
            var objContext = ((IObjectContextAdapter)_context).ObjectContext;
            var objSet = objContext.CreateObjectSet<T>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);

            Object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);

            if (exists)
            {
                objContext.Detach(foundEntity);
            }

            return (exists);
        }

        public Database Database
        {
            get { return _context.Database; }
        }
    }
}
