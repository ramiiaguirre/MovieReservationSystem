using MovieReservation.Logic.UseCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MovieReservation.DataAccess.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<MovieReservationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        // Repositorios
        services.AddScoped(typeof(IRepository<>), typeof(RepositoryEF<>));
        services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();
        
        return services;
    }
}