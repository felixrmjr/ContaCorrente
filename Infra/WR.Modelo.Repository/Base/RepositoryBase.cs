using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WR.Modelo.Domain.Entities.Base;
using WR.Modelo.Domain.Interfaces.Repositories;
using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Domain.Extensions;

namespace WR.Modelo.Repository.Base
{
    public class RepositoryBase<TContext, TEntity, TIdentity> : IRepositoryBase<TContext, TEntity, TIdentity> where TEntity
                                                              : EntityBase<TIdentity> where TContext
                                                              : IUnitOfWork<TContext>
    {
        public IUnitOfWork<TContext> UnitOfWork { get; }

        public RepositoryBase(IUnitOfWork<TContext> unitOfWork) => this.UnitOfWork = unitOfWork;

        protected DbSet<TEntity> DbSet => ((DbContext)UnitOfWork).Set<TEntity>();

        #region [ CRUD ]

        public virtual TEntity Save(TEntity entity)
        {
            DbSet.Add(entity);
            return entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            var result = DbSet.Attach(entity);
            result.State = EntityState.Modified;
            //result.Property(x => x.UsuarioCriacaoId).IsModified = false;
            //result.Property(x => x.DataCriacao).IsModified = false;

            return entity;
        }

        public virtual void Delete(TIdentity id)
        {
            TEntity entity = Get(id);
            DbSet.Remove(entity);
        }

        public virtual TEntity Get(TIdentity id) => DbSet.Find(id);

        public virtual IQueryable<TEntity> GetAll(QueryFilter filter = null)
        {
            var result = DbSet.AsQueryable();

            if (filter != null)
                result = AddQueryFilter(filter, result);

            return result;
        }

        public virtual IPagedList<TEntity> GetPaginated(QueryFilter filter, int start = 0, int limit = 10, bool orderByDescending = true, params Expression<Func<TEntity, object>>[] includes)
        {
            var result = DbSet.AsQueryable();

            if (includes != null)
                foreach (var include in includes)
                    result = result.Include(include);


            return GetPagedList(result, filter, start, limit, orderByDescending);
        }

        protected IPagedList<TEntity> GetPagedList(IQueryable<TEntity> result, QueryFilter filter, int start, int limit, bool orderByDescending)
        {
            if (filter != null)
                result = AddQueryFilter(filter, result);

            if (orderByDescending)
                result = result.OrderByDescending(x => x.Id);

            var total = result.Count();

            if (limit > 0)
            {
                result = result
                    .Skip(start)
                    .Take(limit);
            }

            return result.ToPagedList(total);
        }

        public ICollection<T> FromSql<T>(string sql, params object[] parameters) where T : class
        {
            var result = ((DbContext)UnitOfWork).Set<T>().FromSql(sql, parameters).ToList();
            return result;
        }

        #endregion

        #region [ QueryFilter ]

        private static readonly string[] listaNumeros = { "INT16", "INT32", "INT64", "DECIMAL", "DOUBLE", "BYTE", "SBYTE", "FLOAT", "UINT16", "UINT32", "UINT64" };

        protected IQueryable<TEntity> AddQueryFilter(QueryFilter filter, IQueryable<TEntity> pesquisa)
        {
            var tipo = typeof(TEntity);
            Expression<Func<TEntity, bool>> exp = null;
            Expression<Func<TEntity, bool>> expAnd = null;

            if (filter.Filters != null)
                foreach (var f in filter.Filters)
                {
                    var property = tipo.GetProperty(f.Property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (property == null)
                        continue;

                    Type propertyType = property.PropertyType;
                    var isNullable = Nullable.GetUnderlyingType(propertyType) != null;
                    object valor;
                    if (isNullable)
                    {
                        if (f.Value == null)
                            valor = null;
                        else
                            if (propertyType.GetTypeInfo().IsEnum)
                            valor = Enum.Parse(propertyType.GetGenericArguments()[0], f.Value.ToString(), true);
                        else
                            valor = Convert.ChangeType(f.Value.ToString(), propertyType.GetGenericArguments()[0]);
                    }
                    else
                    {
                        if (listaNumeros.Contains(propertyType.Name.ToUpper()))
                        {
                            long i;
                            if (!Int64.TryParse(f.Value.ToString(), out i))
                                continue;
                        }
                        if (propertyType.GetTypeInfo() != null && propertyType.GetTypeInfo().IsEnum)
                        {
                            var enumType = Enum.Parse(propertyType, f.Value.ToString());
                            valor = Convert.ChangeType(enumType, propertyType);
                        }
                        else
                            valor = Convert.ChangeType(f.Value.ToString(), propertyType);
                    }

                    var param = Expression.Parameter(typeof(TEntity), "p");

                    if (f.TypeFilterAnd)
                    {
                        if (propertyType.Name == "string" || propertyType.Name == "String")
                        {
                            expAnd = expAnd == null
                                ? GetExpression<TEntity>(f.Property, f.Value.ToString().ToUpper())
                                : GetExpression<TEntity>(f.Property, f.Value.ToString().ToUpper()).And(expAnd);
                        }
                        else
                        {
                            var expressao = Expression.Equal(Expression.Property(param, property), Expression.Constant(valor, propertyType));
                            expAnd = expAnd == null
                                ? Expression.Lambda<Func<TEntity, bool>>(expressao, param)
                                : Expression.Lambda<Func<TEntity, bool>>(expressao, param).And(expAnd);
                        }
                    }
                    else
                    {
                        if (propertyType.Name == "string" || propertyType.Name == "String")
                        {
                            exp = exp == null
                                ? GetExpression<TEntity>(f.Property, f.Value.ToString().ToUpper())
                                : GetExpression<TEntity>(f.Property, f.Value.ToString().ToUpper()).Or(exp);
                        }
                        else
                        {
                            var expressao = Expression.Equal(Expression.Property(param, property), Expression.Constant(valor, propertyType));
                            exp = exp == null
                                ? Expression.Lambda<Func<TEntity, bool>>(expressao, param)
                                : Expression.Lambda<Func<TEntity, bool>>(expressao, param).Or(exp);
                        }
                    }
                }

            if (exp != null)
                pesquisa = pesquisa.Where(exp);

            if (expAnd != null)
                pesquisa = pesquisa.Where(expAnd);

            return pesquisa;
        }

        private static Expression<Func<T, bool>> GetExpression<T>(string propertyName, string propertyValue)
        {
            var parameterExp = Expression.Parameter(typeof(T), "type");
            var propertyExp = Expression.Property(parameterExp, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var someValue = Expression.Constant(propertyValue, typeof(string));
            var containsMethodExp = Expression.Call(propertyExp, method, someValue);

            return Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);

        }

        #endregion
    }
}