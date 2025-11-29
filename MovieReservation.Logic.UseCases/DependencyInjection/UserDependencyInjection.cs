using Microsoft.Extensions.DependencyInjection;

public static class UserDependencyInjection
{
    public static IServiceCollection AddUserUseCases(this IServiceCollection services)
    {
        // Casos de uso de Users
        services.AddScoped<IGetUsers, GetUsers>();
        services.AddScoped<IGetUserById, GetUserById>();
        services.AddScoped<ICreateUser, CreateUser>();
        
        return services;
    }
}
