using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data.Models.Users;

namespace RestaurantApp.Data.DataAccess
{
    public class UsersDatabase : IdentityDbContext<User>
    {
        public UsersDatabase(DbContextOptions<UsersDatabase> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
