using AuthDotNetApi.Data;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


Env.Load();

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
builder.Configuration["AppSettings:Token"] = jwtSecret;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var connectionString = $"Server={Environment.GetEnvironmentVariable("DB_SERVER")}" +
                       $";Database={Environment.GetEnvironmentVariable("DB_NAME")}" +
                       $";User={Environment.GetEnvironmentVariable("DB_USER")}" +
                       $";Password={Environment.GetEnvironmentVariable("DB_PASSWORD")}" +
                       $";Port={Environment.GetEnvironmentVariable("DB_PORT")}";

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();
