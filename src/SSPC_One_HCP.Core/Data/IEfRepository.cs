using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SSPC_One_HCP.Core.Domain.Models;

namespace SSPC_One_HCP.Core.Data
{
    public partial interface IEfRepository : IDisposable
    {
        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        T GetById<T>(params object[] id) where T : class, new();

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert<T>(T entity) where T : class, new();

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void InsertList<T>(IEnumerable<T> entities) where T : class, new();

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update<T>(T entity) where T : class, new();

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void UpdateList<T>(IEnumerable<T> entities) where T : class, new();
        /// <summary>
        /// 根据条件更新
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="includeProperties">条件树</param>
        /// <param name="UpdateProperties">更新树</param>
        /// <returns></returns>
        int UpdateExtend<T>(Expression<Func<T, bool>> includeProperties, Expression<Func<T, T>> UpdateProperties) where T : class, new();
        /// <summary>
        /// 记录更新操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldT"></param>
        /// <param name="newT"></param>
        void UpdateWithCompare<T>(T oldT, T newT) where T : class, new();
        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete<T>(T entity) where T : class, new();

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void DeleteList<T>(IEnumerable<T> entities) where T : class, new();

        /// <summary>
        /// 获取第一或默认
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        T FirstOrDefault<T>(Expression<Func<T, bool>> includeProperties) where T : class, new();

        /// <summary>
        /// 大批量插入
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="queryable">数据集</param>
        /// <returns></returns>
        void BulkCopyInsert<T>(IEnumerable<T> queryable, string tableName = null) where T : BaseEntity;
        IDbSet<T> GetEntities<T>() where T : class, new();

        /// <summary>
        /// Gets a table
        /// </summary>
        DbSet<T> Table<T>() where T : class, new();
        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> All<T>() where T : class, new();
        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking<T>() where T : class, new();
        /// <summary>
        /// where方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> Where<T>(Expression<Func<T, bool>> includeProperties) where T : class, new();
        /// <summary>
        /// 异步获取平均值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<double> AverageAsync<T>(Expression<Func<T, int>> expression) where T : class, new();

        /// <summary>
        /// 异步获取数据条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<int> CountAsync<T>(Expression<Func<T, bool>> expression) where T : class, new();

        /// <summary>
        /// 根据条件异步获取单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> expression) where T : class, new();

        /// <summary>
        /// 根据条件异步获取首个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> FirstAsync<T>(Expression<Func<T, bool>> expression) where T : class, new();

        /// <summary>
        /// 执行SQL获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters) where T : class, new();

        /// <summary>
        /// 执行SQL获取单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        T SqlQueryObject<T>(string sql, params object[] parameters);

        /// <summary>
        /// 执行存储过程并加载结束时的实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IList<T> ExecuteStoredProcedureList<T>(string commandText, params object[] parameters) where T : class, new();

        /// <summary>
        /// 执行SQL文
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="doNotEnsureTransaction"></param>
        /// <param name="timeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters);

        /// <summary>
        /// 保存修改
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
        /// <summary>
        /// 提交并且刷新改变
        /// </summary>
        void CommitAndRefreshChanges();
        /// <summary>
        /// 回滚提交的更改
        /// </summary>
        void RollbackChanges();
        /// <summary>
        /// 异步保存
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        Database Database { get; }
    }
}
