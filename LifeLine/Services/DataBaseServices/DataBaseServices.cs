using LifeLine.MVVM.Models.MSSQL_DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public async Task<T> FindByValueAsync<T>(string propertyName, object value, Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
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
                    return await query.FirstOrDefaultAsync(x => EF.Property<object>(x, propertyName).Equals(value));
                }
                catch (Exception)
                {

                    throw new Exception($"Свойство <<{propertyName}>> не найдено");
                }
            }
        }

        public async Task<T> FindByValueAsync<T>(Dictionary<string, object> propertyValues, Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
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
                    var parametr = Expression.Parameter(typeof(T), "e"); // Создаем параметр для лямбда выражения

                    Expression predicate = null; // Создаем выражение

                    foreach (var propValue in propertyValues)
                    {
                        var property = Expression.Property(parametr, propValue.Key); // Получаем свойство по ключу

                        var compretion = Expression.Equal(property, Expression.Constant(propValue.Value)); // Создаем выражение для сравнения свойств с его значением

                        predicate = predicate == null ? compretion : Expression.AndAlso(predicate, compretion); // Если predicate уже имеет значение, комбинируем через &&
                    }

                    var lambda = Expression.Lambda<Func<T, bool>>(predicate, parametr); // Создаем лямбда выражение

                    query = query.Where(lambda); // Применяем фильтрацию через одно Where с логическими <<&&>> <<И>>  

                    return await query.FirstOrDefaultAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка при поиске по свойствам <<{ex.Message}>>");
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

        public void RemoveRange<T>(IEnumerable<T> entity) where T : class
        {
            using (var context = _contextFactory())
            {
                context.Set<T>().RemoveRange(entity);
                context.SaveChanges();
            }
        }

        public async Task<T> FindIdAsync<T>(int id_entity) where T : class
        {
            using (var context = _contextFactory())
            {
                return await context.Set<T>().FindAsync(id_entity);
            }
        }

        public int Count<T>(Func<IQueryable<T>, IQueryable<T>> include = null) where T : class
        {
            using (var context = _contextFactory())
            {
                IQueryable<T> query = context.Set<T>();

                if (include != null)
                {
                    query = include(query);
                }

                return query.Count();
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
