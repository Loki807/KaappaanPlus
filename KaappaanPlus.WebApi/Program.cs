
using KaappaanPlus.Application;
using KaappaanPlus.Infrastructure;
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

            builder.Services.AddAuthorization();




            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            // ✅ Run Role Seeding once at startup
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await RoleSeeder.SeedAsync(dbContext);
            }

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await RoleSeeder.SeedAsync(dbContext);
                await SystemTenantSeeder.SeedSystemTenantAsync(dbContext);  // ✅ FIRST
                await SuperAdminSeeder.SeedSuperAdminAsync(dbContext);      // ✅ THEN
            }

            app.Run();
        }
    }
}
