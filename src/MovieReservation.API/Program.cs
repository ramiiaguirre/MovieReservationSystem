using System.Runtime.Intrinsics.Arm;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieReservation.API;
using MovieReservation.API.Data;
using MovieReservation.API.Extensions;
using MovieReservation.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionControllerHandler>();

builder.Services.AddOpenApiCustomConfig();

builder.Services.AddAuthorization()
    .AddAuthentication(x => 
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }) 
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!))
        };
        x.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("jwt", out var token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });
    
builder.Services.AddOptions<JwtSettings>()
    .BindConfiguration(JwtSettings.SECTION_NAME);

builder.Services.AddSingleton<JwtManager>();

builder.Services.AddDbContext<MovieReservationContext>(options =>
    options.UseSqlite("Data Source=../MovieReservation.db"));



builder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryEF<>));

builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IShowTimeService, ShowTimeService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IStatsService, StatsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(option =>
    {
        option.Authentication = new ScalarAuthenticationOptions
        {
            PreferredSecurityScheme = "Bearer"
            // PreferredSecuritySchemes = new List<string>() { "Bearer" }
        };
    });
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MovieReservationContext>();
    await db.Database.MigrateAsync();

    if (app.Environment.IsDevelopment()) {
        Console.WriteLine("Se están cargando las seeds");
        await DatabaseSeeder.SeedAsync(db);
        Console.WriteLine("Terminaron de cargar las seeds");
    }
}

app.UseExceptionHandler();

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
