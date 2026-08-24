using Microsoft.AspNetCore.Mvc;

public class MovieService
{
    readonly HttpClient _httpClient;

    public MovieService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ApiNaming.ApiName);
    }


    public async Task<List<Movie>> GetMoviesAsync()
    {
        var movies = await _httpClient.GetFromJsonAsync<List<Movie>>("movies");        
        return movies ?? new List<Movie>();
    }

}