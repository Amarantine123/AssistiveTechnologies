using NFine.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NFine.Data
{
    /// <summary>
    /// Repository implementation
    /// </summary>
    public class RepositoryBase : IRepositoryBase, IDisposable
    {
        /// <summary>
        /// db context
        /// </summary>
        private NFineDbContext dbcontext = new NFineDbContext();
        /// <summary>
        /// db transtraction
        /// </summary>
        private DbTransaction dbTransaction { get; set; }
        /// <summary>
        /// implementation interface 
        /// </summary>
        /// <returns></returns>
        public IRepositoryBase BeginTrans()
        {
            DbConnection dbConnection = ((IObjectContextAdapter)dbcontext).ObjectContext.Connection;
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }
            dbTransaction = dbConnection.BeginTransaction();
            return this;
        }
        /// <summary>
        /// commit to db
        /// </summary>
        /// <returns></returns>
        public int Commit()
        {
            try
            {
                var returnValue = dbcontext.SaveChanges();
                if (dbTransaction != null)
                {
                    dbTransaction.Commit();
                }
                return returnValue;
            }
            catch (Exception)
            {
                if (dbTransaction != null)
                {
                    this.dbTransaction.Rollback();
                }
                throw;
            }
            finally
            {
                this.Dispose();
            }
        }
        /// <summary>
        /// dispose
        /// </summary>
        public void Dispose()
        {
            if (dbTransaction != null)
            {
                this.dbTransaction.Dispose();
            }
            this.dbcontext.Dispose();
        }
        /// <summary>
        /// insert one 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public int Insert<TEntity>(TEntity entity) where TEntity : class
        {
            dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            return dbTransaction == null ? this.Commit() : 0;
        }
        /// <summary>
        /// insert list
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitys">entity list</param>
        /// <returns></returns>
        public int Insert<TEntity>(List<TEntity> entitys) where TEntity : class
        {
            foreach (var entity in entitys)
            {
                dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            }
            return dbTransaction == null ? this.Commit() : 0;
        }
        /// <summary>
        /// update
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public int Update<TEntity>(TEntity entity) where TEntity : class
        {
            dbcontext.Set<TEntity>().Attach(entity);
            PropertyInfo[] props = entity.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                        dbcontext.Entry(entity).Property(prop.Name).CurrentValue = null;
                    dbcontext.Entry(entity).Property(prop.Name).IsModified = true;
                }
            }
            return dbTransaction == null ? this.Commit() : 0;
        }
        /// <summary>
        /// delete one
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete<TEntity>(TEntity entity) where TEntity : class
        {
            dbcontext.Set<TEntity>().Attach(entity);
            dbcontext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return dbTransaction == null ? this.Commit() : 0;
        }
        /// <summary>
        /// delete list
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var entitys = dbcontext.Set<TEntity>().Where(predicate).ToList();
            entitys.ForEach(m => dbcontext.Entry<TEntity>(m).State = EntityState.Deleted);
            return dbTransaction == null ? this.Commit() : 0;
        }
        /// <summary>
        /// get one by key
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public TEntity FindEntity<TEntity>(object keyValue) where TEntity : class
        {
            return dbcontext.Set<TEntity>().Find(keyValue);
        }
        /// <summary>
        /// get one by lambda
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return dbcontext.Set<TEntity>().FirstOrDefault(predicate);
        }
        /// <summary>
        /// get IQueryable
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> IQueryable<TEntity>() where TEntity : class
        {
            return dbcontext.Set<TEntity>();
        }
        /// <summary>
        /// get IQueryable by lambda
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<TEntity> IQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return dbcontext.Set<TEntity>().Where(predicate);
        }
        /// <summary>
        /// get list by sql
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public List<TEntity> FindList<TEntity>(string strSql) where TEntity : class
        {
            return dbcontext.Database.SqlQuery<TEntity>(strSql).ToList<TEntity>();
        }
        /// <summary>
        /// get list by sql and parameter and pagination
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="dbParameter"></param>
        /// <returns></returns>
        public List<TEntity> FindList<TEntity>(string strSql, DbParameter[] dbParameter) where TEntity : class
        {
            return dbcontext.Database.SqlQuery<TEntity>(strSql, dbParameter).ToList<TEntity>();
        }
        public List<TEntity> FindList<TEntity>(Pagination pagination) where TEntity : class,new()
        {
            bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
            string[] _order = pagination.sidx.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = dbcontext.Set<TEntity>().AsQueryable();
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(TEntity), "t");
                var property = typeof(TEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();
            return tempData.ToList();
        }
        /// <summary>
        /// get list by lamdba and pagination
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public List<TEntity> FindList<TEntity>(Expression<Func<TEntity, bool>> predicate, Pagination pagination) where TEntity : class,new()
        {
            bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
            string[] _order = pagination.sidx.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = dbcontext.Set<TEntity>().Where(predicate);
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(TEntity), "t");
                var property = typeof(TEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();
            return tempData.ToList();
        }
    }
}
