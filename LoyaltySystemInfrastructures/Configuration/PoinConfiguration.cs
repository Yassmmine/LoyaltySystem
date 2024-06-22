
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using LoyaltySystemDomain.Entities;

namespace LoyaltySystemInfrastructures.Configuration
{
	public class PoinConfiguration : IEntityTypeConfiguration<Point>
	{
		public void Configure(EntityTypeBuilder<Point> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(a=>a.Id).UseIdentityColumn(1,1);
			
		}
	}
}
