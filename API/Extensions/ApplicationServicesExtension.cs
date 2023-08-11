using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services, IConfiguration config)
        {
            Services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            Services.AddCors();
            Services.AddTransient<IUserRepository, UserRepository>();
            Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            Services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            Services.AddScoped<IPhotoService, PhotoService>();
            return Services;
        }


    }
}