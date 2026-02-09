using System;
using System.Net.Http.Headers;
using System.Threading;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;

using Malwine.Contexts;
using Malwine.Models;
using Malwine.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var services = builder.Services;

services.AddSingleton<ImageConverter>();
services.AddSingleton<DtoFactory>();
services.AddScoped<TMDBSeeder>();
services.AddHttpClient<TMDBSeeder>(http =>
{
  http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI3MWEwZmZjZTJkNTVlZjEwODJlMzc0YTVkM2ZiNzUyOSIsIm5iZiI6MTcyODA2MTc5NS42NjAyMTQsInN1YiI6IjY2ZjNiYWUxNzA5MWQ1NzU1ZDY5ZTQ2NCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.Loki2TelBv2jvuIygW5KiNShQIHGsmOvfyHDeC4TOiE");
});

services.AddControllers();

services.AddAuthorization();
services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();
services.AddCors(options =>
{
  options.AddPolicy("AllowAllOrigin",
      builder => builder.WithOrigins("http://localhost:3000") // Замените на ваш фронтенд адрес
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
});

services.Configure<IdentityOptions>(options =>
{

  // Password settings.
  options.Password.RequireDigit = true;
  options.Password.RequireLowercase = true;
  options.Password.RequireNonAlphanumeric = true;
  options.Password.RequireUppercase = true;
  options.Password.RequiredLength = 6;
  options.Password.RequiredUniqueChars = 1;

  // Lockout settings.
  options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(5);
  options.Lockout.MaxFailedAccessAttempts = 5;
  options.Lockout.AllowedForNewUsers = true;

  // User settings.
  options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
  options.User.RequireUniqueEmail = false;
});

services.ConfigureApplicationCookie(options =>
{
  // Cookie settings
  options.Cookie.HttpOnly = true;
  options.ExpireTimeSpan = TimeSpan.FromDays(5);
  options.SlidingExpiration = true;
  // options.LoginPath = "/Identity/Account/Login";
  // options.AccessDeniedPath = "/Identity/Account/AccessDenied";
  // options.SlidingExpiration = true;
});


services.AddDistributedMemoryCache();
services.AddSession(options =>
{
  options.IdleTimeout = TimeSpan.FromDays(5);
  options.Cookie.HttpOnly = true;
  options.Cookie.IsEssential = true;
});


// services.AddEndpointsApiExplorer();
// services.AddSwaggerGen();

services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlite(configuration.GetConnectionString(nameof(ApplicationDbContext)))
         .UseLazyLoadingProxies()
);

var app = builder.Build();

using var scope = app.Services.CreateScope();

// if (app.Environment.IsDevelopment())
// {
// app.UseSwagger();
// app.UseSwaggerUI();

// var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>()!;


// await applicationDbContext.Database.EnsureDeletedAsync();
// await applicationDbContext.Database.EnsureCreatedAsync();
// }

await scope.ServiceProvider
           .GetRequiredService<TMDBSeeder>()
           .SeedIfNeeded();


app.UseCors("AllowAllOrigin");
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseWebSockets(new WebSocketOptions()
{
  KeepAliveInterval = Timeout.InfiniteTimeSpan,
  AllowedOrigins = { "*" }
});

app.MapControllers();
// app.UseHttpsRedirection();

await app.RunAsync();