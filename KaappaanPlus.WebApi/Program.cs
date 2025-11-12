
using KaappaanPlus.Application;
using KaappaanPlus.Application.Common.Exceptions;
using KaappaanPlus.Infrastructure;
using KaappaanPlus.WebApi.Hubs;
using KaappanPlus.Persistence;
using KaappanPlus.Persistence.Data;
using KaappanPlus.Persistence.Seeds;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace KaappaanPlus.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddApplicationServices();          // Application Layer
            builder.Services.AddPersistence(builder.Configuration);  // Persistence Layer
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // JWT auth
            var jwt = builder.Configuration.GetSection("JwtSettings");
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwt["Issuer"],
                        ValidAudience = jwt["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!))
                    };
                });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                    policy.WithOrigins("http://localhost:4200") // Angular
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });

            builder.Services.AddAuthorization();




            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSignalR();

            var app = builder.Build();

            // ✅ Run all seeders ONCE at startup
            // ✅ Run all seeders ONCE at startup
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await SeedDataRunner.RunAllAsync(db);
            }


            app.UseMiddleware<ErrorHandlingMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("AllowAngular");
            app.MapHub<AlertHub>("/alertHub");
            app.MapControllers();

            


            app.Run();
        }
    }
}
