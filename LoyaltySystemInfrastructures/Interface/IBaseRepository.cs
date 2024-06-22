using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace LoyaltySystemInfrastructures.Interface;

public interface IBaseRepository<T> where T : class
{
	Task<T> FirstOrDefaultAsyncAsNoTracking(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
	Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
	Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
	Task<int> SaveChangesAsync();
	int SaveChanges();
	void Edit(T entity);
	void Attach(T entity);
	void Update(T entity);
	void AttachRange(List<T> entity);
	void UpdateRange(List<T> entity);
	Task AddRangeAsync(List<T> entity);
	Task AddAsync(T entity);
	Task<T> FirstOrDefaultAsync();
	Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
	T FirstOrDefault(Expression<Func<T, bool>> predicate);
	Task<List<T>> GetAllByWhereAsync(Expression<Func<T, bool>> predicate);
	Task<List<T>> GetAllByWhereAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
	Task<List<T>> GetAllByWhereAsyncAsNoTracking(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
	Task<List<T>> GetAllAsync();
	IQueryable<T> GetAllQueryable();
	IQueryable<T> GetAllQueryableByWhere(Expression<Func<T, bool>> predicate);
	IQueryable<T> GetAllQueryableByWhere(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes);
	void DeleteByEntity(T entity);
	bool DeleteRange(Expression<Func<T, bool>> predicate);
	void DeleteRange(IEnumerable<T> list);
	Task DeleteAsync(Expression<Func<T, bool>> predicate);
	Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
	bool Any(Expression<Func<T, bool>> predicate);
	Task<C> MaxAsync<C>(Expression<Func<T, C>> result, Expression<Func<T, bool>> filter);
	Task<C> MaxAsync<C>(Expression<Func<T, C>> result);
	Task<decimal> SumAsync(Expression<Func<T, decimal>> result, Expression<Func<T, bool>> filter);
	Task<decimal> CountAsync(Expression<Func<T, bool>> filter);
	Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate);
	Task<int> ExecuteUpdateAsync(Expression<Func<T, bool>> predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> Property);
}



