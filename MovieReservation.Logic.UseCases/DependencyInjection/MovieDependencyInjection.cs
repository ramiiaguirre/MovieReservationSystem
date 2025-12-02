using Microsoft.Extensions.DependencyInjection;
using MovieReservation.Logic.UseCases;

public static class MovieDependencyInjection
{
    public static IServiceCollection AddMovieUseCases(this IServiceCollection services)
    {
        services.AddScoped<IMovieCrudUseCases, MovieCrudUseCases>();
        
        return services;
    }
}