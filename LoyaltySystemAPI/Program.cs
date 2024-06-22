using LoyaltySystemApplication.Services.implementation;
using LoyaltySystemApplication.Services.Interfaces;
using LoyaltySystemInfrastructures;
using LoyaltySystemInfrastructures.Implementation;
using LoyaltySystemInfrastructures.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

#region Add authorized for all action
builder.Services.AddControllers(options =>
								{
									// Apply global authorization filter
									var policy = new AuthorizationPolicyBuilder()
													.RequireAuthenticatedUser()
													.Build();
									options.Filters.Add(new AuthorizeFilter(policy));
								});
#endregion
#region Add Swagger with authorized
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

	// Define the security scheme
	var securityScheme = new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
	};

	c.AddSecurityDefinition("Bearer", securityScheme);

	var securityRequirement = new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	};

	c.AddSecurityRequirement(securityRequirement);
});
#endregion
#region Add Services ,RedisCache and Database 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPointsService, PointsService>();
builder.Services.AddDbContext<LoyaltySystemDbContext>(options =>
				 options.UseLazyLoadingProxies()
				 .UseSqlServer(configuration.GetConnectionString("LoyaltySystemDB")));

builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
});

#endregion
#region integrate with Keycloak
var keycloakSettings = builder.Configuration.GetSection("Keycloak");

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		var keycloakSettings = builder.Configuration.GetSection("Keycloak");
		options.Authority = keycloakSettings["Authority"];
		options.Audience = keycloakSettings["ClientId"];
		options.RequireHttpsMetadata = false;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			NameClaimType = ClaimTypes.Name,
			RoleClaimType = ClaimTypes.Role,
			ValidateIssuer = true,
			ValidIssuer = keycloakSettings["Authority"], // This should match the 'iss' claim in your token
			ValidateAudience = true,
			ValidAudiences = new[] { keycloakSettings["ClientId"], "swagger", "account" }, // Add "account" as a valid audience
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(keycloakSettings["ClientSecret"]))
		};
	});
#endregion
builder.Services.AddAuthorization();
builder.Host.UseSerilog((context, configuration) =>
	configuration.ReadFrom.Configuration(context.Configuration));


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
