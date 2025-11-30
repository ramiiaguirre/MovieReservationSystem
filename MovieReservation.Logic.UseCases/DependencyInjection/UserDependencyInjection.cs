using Microsoft.Extensions.DependencyInjection;
using MovieReservation.Logic.UseCases;
using MovieReservation.Model.Domain;

public static class UserDependencyInjection
{
    public static IServiceCollection AddUserUseCases(this IServiceCollection services)
    {
        services.AddScoped<IGetUsers, GetUsers>();
        services.AddScoped<IGetUserById, GetUserById>();

        services.AddScoped<IRoleUseCases, RoleUseCases>();
        
        return services;
    }
}
