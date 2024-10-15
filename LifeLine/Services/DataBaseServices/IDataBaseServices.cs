using LifeLine.MVVM.Models.MSSQL_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LifeLine.Services.DataBaseServices
{
    public interface IDataBaseServices
    {
        Task<IEnumerable<T>> GetDataTableAsync<T>(Func<IQueryable<T>, IQueryable<T>> include = null) where T : class;
        Task<List<T>> GetDataTableListAsync<T>(Func<IQueryable<T>, IQueryable<T>> include = null) where T : class;
        Task<T> GetByIdAsync<T>(int id, string propertyName, Func<IQueryable<T>, IQueryable<T>> include = null) where T : class;
        Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task AddAsync<T>(T entity) where T : class;
        Task UpdateAsync<T>(T entity) where T : class;
        Task DeleteAsync<T>(T entity) where T : class;
        Task<T> FindIdAsync<T>(int id_entity) where T : class;
        Task<IEnumerable<T>> GetByConditionAsync<T>(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>> include = null) where T : class;
        Task<IEnumerable<T>> GetByConditionsWithUnionAsync<T>(
            Expression<Func<T, bool>> firstCondition,
            Expression<Func<T, bool>> secondCondition,
            Func<IQueryable<T>, IQueryable<T>> include = null) where T : class;
    }
}
