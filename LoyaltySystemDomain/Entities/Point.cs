namespace LoyaltySystemDomain.Entities
{
	public class Point
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int Value { get; set; }
		public virtual User User { get; set; }
	}
}
