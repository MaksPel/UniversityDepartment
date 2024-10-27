using Contracts;
using Microsoft.EntityFrameworkCore;
using UniversityDepartment.Application.Services;
using UniversityDepartment.Persistence;

namespace UniversityDepartment.Web.Extensions;

public static class ServiceExtensions
{
	public static void ConfigureCors(this IServiceCollection services) =>
		services.AddCors(options =>
		{
			options.AddPolicy("CorsPolicy", builder =>
			builder.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader());
		});

	public static void ConfigureSqlContext(this IServiceCollection services,
		IConfiguration configuration) =>
		services.AddDbContext<UniversityDepartmentContext>(opts =>
			opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b =>
			{
				b.EnableRetryOnFailure();
			})
		);

	public static void ConfigureRepositoryManager(this IServiceCollection services) =>
		services.AddScoped<IRepositoryManager, RepositoryManager>();

	public static void ConfigureServiceManager(this IServiceCollection services) =>
		services.AddScoped<IServiceManager, ServiceManager>();
}
