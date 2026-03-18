using System.Reflection;
using System.Text;
using Books.Application.Interfaces.Healpers;
using Books.Application.Interfaces.Repositories;
using Books.Application.Interfaces.Services;
using Books.Application.Mapping;
using Books.Application.Queries.Country;
using Books.Application.Service;
using Books.Application.Validators;
using Books.Infrastructure.Commands.Country;
using Books.Infrastructure.Configuration;
using Books.Infrastructure.Data;
using Books.Infrastructure.Helpers;
using Books.Infrastructure.Repositories;
using Books.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Globalization;
using Books.Api.Exeptions;

namespace Books.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            var jwtSettings = configuration
            .GetSection("Jwt")
            .Get<JwtSettings>()
            ?? throw new Exception("JWT settings not configured.");

            builder.Services.Configure<JwtSettings>(
                configuration.GetSection("Jwt"));

            var cachingSettings = configuration
                .GetSection("CachingSettings")
                .Get<CachingSettings>()
                ?? throw new Exception("CachingSettings settings not configured.");

            builder.Services.Configure<CachingSettings>(
                configuration.GetSection("CachingSettings"));

            builder.Services.Configure<RabbitMqSettings>(
                builder.Configuration.GetSection("RabbitMq")
            );

            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.Services.AddAutoMapper(
                _ => { }, //пустий конфігураційний делегат.
                typeof(BookProfile).Assembly,
                typeof(GenreProfile).Assembly,
                typeof(AuthorProfile).Assembly,
                typeof(UserProfile).Assembly);
            // ================= CORS =================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCountryCommand).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCountryByIdQuery).Assembly));

            builder.Services.AddMemoryCache();
            builder.Services.AddDbContext<LibraryDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            //      builder.Services.AddDbContext<LibraryDbContext>(options =>
            //          options.UseMySql(
            //          configuration.GetConnectionString("ConnectionToMySql"),
            //          ServerVersion.AutoDetect(
            //        configuration.GetConnectionString("ConnectionToMySql")
            //    )));

            builder.Services.AddValidatorsFromAssemblyContaining<BookValidation>();

            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddScoped<IBookRepository,BookRepository>();
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<IGenreRepository, GenreRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IHashHelper, HashHelper>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            //=============================Redis===========================
            //=============================Redis===========================
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var serviceName = configuration["CachingSettings:ServiceName"];

                if (!string.IsNullOrEmpty(serviceName) &&
                    string.Equals(serviceName, "Radis", StringComparison.OrdinalIgnoreCase))
                {
                    var redisConfig = configuration.GetConnectionString("Redis");
                    return ConnectionMultiplexer.Connect(redisConfig);
                }

                // Если ServiceName != "Radis", Redis не подключаем
                return null;
            });

            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IAuthorService, AuthorService>();
            builder.Services.AddScoped<IGenreService, GenreService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<ICachingServices, RedisCachingService>();
            builder.Services.AddScoped<IQueueService, RabbitMqService>();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    Array.Empty<string>()
                }
            });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Key)
                ),

                ClockSkew = TimeSpan.Zero
            };
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseCors("AllowAll");
            app.UseExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
