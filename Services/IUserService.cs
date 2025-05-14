using PROG7311POE_ST10178800.Models;

public interface IUserService
{
    Task RegisterUserAsync(Employee user, string password, string role); 
    Task<Employee?> GetUserByEmailAsync(string email);                    
    Task<Employee?> GetUserByIdAsync(string id);                          
    Task<List<Employee>> GetUsersInRoleAsync(string role);                
}
