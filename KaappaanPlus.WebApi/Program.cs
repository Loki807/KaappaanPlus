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

                    // ⭐ REQUIRED FOR SIGNALR ON MOBILE (Connect via Query parameter)
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.Request.Path;
                            
                            if (!string.IsNullOrEmpty(accessToken) && path.Value.Contains("/alertHub"))
                            {
                                // Console.WriteLine($"🔑 [SignalR] Token found for path: {path}");
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            // ⭐ FIXED CORS FOR MOBILE / NGROK
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                    policy
                        .SetIsOriginAllowed(origin => true) // ✅ Allows: localhost, ngrok, d12q3zkftvfo29.cloudfront.net
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials() // Essential for SignalR
                );
            });

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var signalRBuilder = builder.Services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
            });

            // ☁️ CLOUD SCALABILITY: Use Azure SignalR if configured
            var azureSignalR = builder.Configuration["Azure:SignalR:ConnectionString"];
            if (!string.IsNullOrWhiteSpace(azureSignalR))
            {
                Console.WriteLine("🚀 Using Azure SignalR Service for High Scale");
                signalRBuilder.AddAzureSignalR(azureSignalR);
            }

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
