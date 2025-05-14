using Microsoft.AspNetCore.Identity;

namespace PROG7311POE_ST10178800.Models
{
    public class Employee : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Role { get; set; } 
    }
}
