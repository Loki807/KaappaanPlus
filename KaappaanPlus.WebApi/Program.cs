using KaappaanPlus.Application;
using KaappaanPlus.Application.Common.Exceptions;
using KaappaanPlus.Infrastructure;
using KaappaanPlus.WebApi.Hubs;
using KaappanPlus.Persistence;
using KaappanPlus.Persistence.Data;
using KaappanPlus.Persistence.Seeds;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace KaappaanPlus.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationServices();
            builder.Services.AddPersistence(builder.Configuration);
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // JWT
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
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!))
                    };
                });

            // ⭐ FIXED CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                    policy
                        .WithOrigins(
                            "https://kaappaan.netlify.app",
                            "http://localhost:4200"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                );
            });

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            // ⭐ MUST BE FIRST MIDDLEWARE
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            // Error handling middleware LAST
            app.UseMiddleware<ErrorHandlingMiddleware>();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await SeedDataRunner.RunAllAsync(db);
            }

            app.MapHub<AlertHub>("/alertHub");
            app.MapControllers();

            app.Run();
        }
    }
}
