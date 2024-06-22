using LoyaltySystemDomain.Entities;
using LoyaltySystemInfrastructures.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LoyaltySystemInfrastructures.Implementation
{
	public class UnitOfWork : IUnitOfWork
	{
        protected readonly LoyaltySystemDbContext _dbContext;
        public IBaseRepository<Point> PointRepository { get; set; }
        public IBaseRepository<User> UserRepository { get; set; }
		public UnitOfWork(LoyaltySystemDbContext dbContext , IBaseRepository<Point> pointRepository, IBaseRepository<User> userRepository)
		{
			_dbContext = dbContext;
			PointRepository = pointRepository;
			UserRepository = userRepository;
		}


		public async Task<int> CommitAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}

		public void RollBack()
		{
			_dbContext.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);
		}
	}
}

