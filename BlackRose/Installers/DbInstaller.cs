using BlackRose.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DbContext = BlackRose.Data.DbContext;

namespace BlackRose.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddDbContext<DbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<Microsoft.EntityFrameworkCore.DbContext>();

            services.AddSingleton<IPictureService, PictureService>();
        }
    }
}