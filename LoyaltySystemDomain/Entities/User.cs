
namespace LoyaltySystemDomain.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public virtual ICollection<Point> Points { get; set; }
	}
}
