
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using LoyaltySystemDomain.Entities;

namespace LoyaltySystemInfrastructures.Configuration
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(a=>a.Id).UseIdentityColumn(1,1);
			builder.HasMany(a=>a.Points).WithOne(a => a.User).HasForeignKey(a=>a.UserId);
			builder.HasData(
			  new User { Id=1,Name="Ali"},
			  new User { Id =2, Name ="John"}
			  );
		}
	}
}
