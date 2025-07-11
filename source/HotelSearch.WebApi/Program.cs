using System.Text;
using System.Text.Json.Serialization;
using HotelSearch.Application.Services;
using HotelSearch.DataAccess.Repositories;
using HotelSearch.Domain.Commands;
using HotelSearch.Domain.Repositories;
using HotelSearch.Domain.Services;
using HotelSearch.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace HotelSearch.WebApi;

public class Program
{
    private static WebApplicationBuilder builder;
    public static void Main(string[] args)
    {
        builder = WebApplication.CreateBuilder(args);
        
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .WriteTo.Console();
        });


        builder.Services.AddControllers();
        
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        
        ConfigureServices(builder.Services);
        var app = builder.Build();
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel search endpoint v1");
            });
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        
        app.Lifetime.ApplicationStarted.Register(async () =>
        {
            
            // comment return line to seed some hotels

            return;
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var hotelService = services.GetRequiredService<IHotelService>();

            for (int i = 1; i < 11; i++)
            {
                var newHotel = new HotelUpsertCommand(null, $"Hotel {i}", 16 + i , 44.8 + i , 99 + i, null);
                hotelService.Upsert(newHotel);
            }

        });
        
        app.Run();
    }

    static void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options => {
            options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
        });
        
        services.AddHealthChecks();
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
        
        // Register services
        services.AddTransient<IHotelService, HotelService>();
        
        // Register repositories
        services.AddSingleton<IHotelRepository, HotelRepository>(); // singleton because it keeps in memory all hotel data
       
        services.AddOpenApi();
        
        services.AddSwaggerGen(o =>
        {
            o.UseInlineDefinitionsForEnums();
            
            o.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Version = "v1",
                Title = "Hotel search endpoint"
            });
            
            o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter your JWT token",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT"
            });
            o.EnableAnnotations();
            
            o.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] {}
                }
            });
        });
        
    }
}