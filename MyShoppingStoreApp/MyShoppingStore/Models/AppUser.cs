using Microsoft.AspNetCore.Identity;

namespace MyShoppingStore.Models
{
    public class AppUser: IdentityUser
    {
        public string Occupation { get; set; }
    }
}
