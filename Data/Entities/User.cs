using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Data.Data.Entities
{
    public class User : IdentityUser
    {
        // add custom properties
        public DateTime? Birthdate { get; set; }
    }
}
