using Microsoft.AspNetCore.Identity;

namespace PlaneTicketWeb.Models
{
    public class AspNetUserModel : IdentityUser
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
