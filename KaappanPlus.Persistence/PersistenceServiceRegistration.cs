using KaappaanPlus.Application.Contracts;
using KaappaanPlus.Application.Contracts.IBase;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappanPlus.Persistence.Data;
using KaappanPlus.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappanPlus.Persistence
{
    public static class PersistenceServiceRegistration
    {

        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")
                ));

            services.AddScoped<IAppDbContext>(provider =>
                provider.GetRequiredService<AppDbContext>());

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IAlertRepository, AlertRepository>();
            services.AddScoped<ICitizenRepository, CitizenRepository>();
            services.AddScoped<IAlertRepository, AlertRepository>();
            // ✅ Add Generic + Alert repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAlertRepository, AlertRepository>();
            services.AddScoped<IAlertTypeRepository, AlertTypeRepository>(); // 🔥 THIS LINE IS MANDATORY
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IAlertResponderRepository, AlertResponderRepository>();
            services.AddScoped<IAlertResponderRepository, AlertResponderRepository>(); // ✅ ADD THIS LINE
            return services;
        }
    }
}
