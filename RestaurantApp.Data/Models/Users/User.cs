using Microsoft.AspNetCore.Identity;

namespace RestaurantApp.Data.Models.Users
{
    public class User : IdentityUser
    {
        public int BirthYear { get; set; }
    }
}
