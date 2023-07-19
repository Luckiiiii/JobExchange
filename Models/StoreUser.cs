using Microsoft.AspNetCore.Identity;

namespace JobExchange.Models
{
    public class StoreUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public Employer Employers { get; set; }
    }
}
