using LoyaltySystemDomain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LoyaltySystemInfrastructures;
public class LoyaltySystemDbContext : DbContext
{
	public LoyaltySystemDbContext(DbContextOptions<LoyaltySystemDbContext> options) : base(options)
	{
	}



	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{

		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	public DbSet<User> Users { get; set; }
	public DbSet<Point> Points { get; set; }

}
