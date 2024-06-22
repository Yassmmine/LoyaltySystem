using LoyaltySystemInfrastructures;
using LoyaltySystemInfrastructures.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace LoyaltySystemInfrastructures.Implementation;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
	private readonly LoyaltySystemDbContext context;

	public BaseRepository(LoyaltySystemDbContext context)
	{
		this.context = context;
	}
	public async Task<List<T>> GetAllAsync()
	{
		return await context.Set<T>().ToListAsync();
	}

	public IQueryable<T> GetAllQueryable()
	{
		return context.Set<T>();
	}
	public IQueryable<T> GetAllQueryableByWhere(Expression<Func<T, bool>> predicate)
	{
		return context.Set<T>().Where(predicate);
	}
	public async Task<List<T>> GetAllByWhereAsync(Expression<Func<T, bool>> predicate)
	{
		return await context.Set<T>().Where(predicate).ToListAsync();
	}

	public async Task<List<T>> GetAllByWhereAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
	{
		return await InsializeQuery(includes).Where(predicate).ToListAsync();
	}
	public async Task<List<T>> GetAllByWhereAsyncAsNoTracking(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
	{
		return await InsializeQuery(includes).AsNoTracking().Where(predicate).ToListAsync();
	}

	public IQueryable<T> GetAllQueryableByWhere(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
	{
		if (predicate == null)
			return InsializeQuery(includes);
		return InsializeQuery(includes).Where(predicate);
	}


	public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
	{
		return await context.Set<T>().AnyAsync(predicate);
	}
	public bool Any(Expression<Func<T, bool>> predicate)
	{
		return context.Set<T>().Any(predicate);
	}

	public async Task<T> FirstOrDefaultAsync()
	{
		return await context.Set<T>().FirstOrDefaultAsync();
	}
	public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
	{
		return await context.Set<T>().FirstOrDefaultAsync(predicate);
	}
	public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
	{
		return await InsializeQuery(includes).FirstOrDefaultAsync(predicate);
	}
	public async Task<T> FirstOrDefaultAsyncAsNoTracking(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
	{
		return await InsializeQuery(includes).AsNoTracking().FirstOrDefaultAsync(predicate);
	}

	public async Task AddAsync(T entity)
	{
		await context.Set<T>().AddAsync(entity);
	}
	public async Task AddRangeAsync(List<T> entity)
	{
		await context.Set<T>().AddRangeAsync(entity);
	}

	public void DeleteByEntity(T entity)
	{
		context.Set<T>().Remove(entity);
		context.Entry(entity).State = EntityState.Deleted;
	}

	public void Edit(T entity)
	{
		context.Entry(entity).State = EntityState.Modified;
	}

	public void Attach(T entity)
	{
		context.Set<T>().Attach(entity);
	}

	public void Update(T entity)
	{
		context.Set<T>().Update(entity);
	}
	public void AttachRange(List<T> entity)
	{
		context.Set<T>().AttachRange(entity);
	}
	public void UpdateRange(List<T> entity)
	{
		context.Set<T>().UpdateRange(entity);
	}
	public async Task<int> SaveChangesAsync()
	{
		return await context.SaveChangesAsync();
	}

	public int SaveChanges()
	{
		return context.SaveChanges();
	}


	private IQueryable<T> InsializeQuery(params Expression<Func<T, object>>[] includes)
	{
		var query = context.Set<T>().AsQueryable();
		if (includes.Any())
		{
			foreach (var include in includes)
			{
				query = query.Include(include);
			}
		}
		return query;
	}


	public async Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
	{
		return await InsializeQuery(includes).LastOrDefaultAsync(predicate);
	}


	public bool DeleteRange(Expression<Func<T, bool>> predicate)
	{
		var list = context.Set<T>().Where(predicate).AsTracking().ToList();
		if (list.Count > 0)
		{
			context.Set<T>().RemoveRange(list);
			return true;
		}
		return false;
	}
	public void DeleteRange(IEnumerable<T> list)
	{
		if (list.Any())
		{
			context.Set<T>().RemoveRange(list);
		}
	}

	public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
	{
		var list = await context.Set<T>().FirstOrDefaultAsync(predicate);
		if (list != null)
		{
			context.Set<T>().Remove(list);
		}
	}

	public async Task<decimal> SumAsync(Expression<Func<T, decimal>> result, Expression<Func<T, bool>> filter)
	{
		var query = context.Set<T>().AsQueryable();
		if (filter != null)
			query = query.Where(filter);
		var outcome = await query.SumAsync(result);
		return outcome;
	}

	public async Task<decimal> CountAsync(Expression<Func<T, bool>> filter)
	{
		var query = context.Set<T>().AsQueryable();
		if (filter != null)
			query = query.Where(filter);
		var outcome = await query.CountAsync();
		return outcome;
	}
	public async Task<C> MaxAsync<C>(Expression<Func<T, C>> result, Expression<Func<T, bool>> filter)
	{

		var query = context.Set<T>().AsQueryable();
		if (filter != null)
			query = query.Where(filter);

		if (typeof(C) == typeof(Nullable<>))
		{
			query = query.Where(x => x != null);
		}
		var rowCount = await query.CountAsync();
		if (rowCount == 0) return default;

		var outcome = await query.MaxAsync(result);
		if (outcome is null) return default;
		return outcome;
	}

	public async Task<C> MaxAsync<C>(Expression<Func<T, C>> result)
	{

		var query = context.Set<T>().AsQueryable();

		if (typeof(C) == typeof(Nullable<>))
		{
			query = query.Where(x => x != null);
		}
		var rowCount = await query.CountAsync();
		if (rowCount == 0) return default;

		var outcome = await query.MaxAsync(result);
		if (outcome is null) return default;
		return outcome;
	}
	public T FirstOrDefault(Expression<Func<T, bool>> predicate)
	{
		return context.Set<T>().FirstOrDefault(predicate);
	}

	public async Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate)
	{
		return await context.Set<T>().Where(predicate).ExecuteDeleteAsync();
	}

	public async Task<int> ExecuteUpdateAsync(Expression<Func<T, bool>> predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> Property)
	{
		return await context.Set<T>().Where(predicate).ExecuteUpdateAsync(Property);
	}

}

