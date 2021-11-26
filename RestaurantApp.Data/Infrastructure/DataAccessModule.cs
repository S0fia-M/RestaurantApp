using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantApp.Data.DataAccess;
using RestaurantApp.Data.Models.Users;

namespace RestaurantApp.Data.Infrastructure
{
    public static class DataAccessModule
    {
        public static IServiceCollection AddRestaurantDataAccessModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UsersDatabase>(options =>
              options.UseSqlServer(configuration.GetConnectionString("UsersConnection")));
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<UsersDatabase>();

            services.AddDbContext<ApplicationDatabase>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DomainConnection")));

            return services;
        }
    }
}
