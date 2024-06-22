using LoyaltySystemDomain.Entities;
using System.Threading.Tasks;

namespace LoyaltySystemInfrastructures.Interface
{
	public interface IUnitOfWork
	{
        IBaseRepository<Point> PointRepository { get; set; }
	    IBaseRepository<User> UserRepository { get; set; }
		Task<int> CommitAsync();
		void RollBack();
	}
}
