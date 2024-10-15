using LifeLine.MVVM.Models.MSSQL_DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LifeLine.Services.DataBaseServices
{
    public class DataBaseServices(Func<EmployeeManagementContext> ContextFactory) : IDataBaseServices
    {
        private readonly Func<EmployeeManagementContext> _contextFactory = ContextFactory;

        public async Task<IEnumerable<T>> GetDataTableAsync<T>(Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
        {
            using (var context = _contextFactory())
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                return await query.ToListAsync();
            }
        }

        public async Task<List<T>> GetDataTableListAsync<T>(Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
        {
            using (var context = _contextFactory())
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                return await query.ToListAsync();
            }
        }

        public async Task<T> GetByIdAsync<T>(int id, string propertyName, Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
        {
            using (var context = _contextFactory())
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                try
                {
                    return await query.FirstOrDefaultAsync(x => EF.Property<int>(x, propertyName) == id);
                }
                catch (Exception)
                {
                    throw new Exception($"Свойство <<{propertyName}>> не найдено");
                }
            }
        }
        public async Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            using (var context = _contextFactory())
            {
                return await context.Set<T>().AnyAsync(predicate);
            }
        }
        public async Task AddAsync<T>(T entity) where T : class
        {
            using (var context = _contextFactory())
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity), "Объект не найден!!");
                }

                await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();
            }
        }
        public async Task UpdateAsync<T>(T entity) where T : class
        {
            using (var context = _contextFactory())
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity), "Объект не найден!!");
                }

                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync<T>(T entity) where T : class
        {
            using (var context = _contextFactory())
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity), "Объект не найден!!");
                }
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
            }
        }
        public async Task<T> FindIdAsync<T>(int id_entity) where T : class
        {
            using (var context = _contextFactory())
            {
                return await context.Set<T>().FindAsync(id_entity);
            }
        }
        public async Task<IEnumerable<T>> GetByConditionAsync<T>(
                     Expression<Func<T, bool>> predicate,
                     Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
        {
            using (var context = _contextFactory())
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                return await query.Where(predicate).ToListAsync();
            }
        }
        /// <summary>
        /// С поддержкой Union
        /// 
        /// var results = await dataService.GetByConditionsWithUnionAsync<Killer>(
        ///    k => k.Name.Contains("Ghost"), 
        ///    k => k.Name.Contains("Myers"),
        ///    query => query.Include(k => k.Weapons));
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="firstCondition"></param>
        /// <param name="secondCondition"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetByConditionsWithUnionAsync<T>(
                     Expression<Func<T, bool>> firstCondition,
                     Expression<Func<T, bool>> secondCondition,
                     Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
        {
            using (var context = _contextFactory())
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                var firstQuery = query.Where(firstCondition);
                var secondQuery = query.Where(secondCondition);

                var unionQuery = firstQuery.Union(secondQuery);

                return await unionQuery.ToListAsync();
            }
        }
    }
}
