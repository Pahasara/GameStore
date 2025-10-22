using System.ComponentModel.DataAnnotations;

namespace GameStore.DTOs.Requests;

public class CreateUserRequest
{
    [Required(ErrorMessage = "Name is required")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
    [MaxLength(20, ErrorMessage = "Name must be less than 20 characters")]
    public string Username { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(3, ErrorMessage = "Password must be at least 3 characters")]
    [MaxLength(20, ErrorMessage = "Password must be less than 20 characters")]
    public string Password { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "First Name is required")]
    [MinLength(3, ErrorMessage = "First Name must be at least 3 characters")]
    [MaxLength(20, ErrorMessage = "First Name must be less than 20 characters")]
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } =  string.Empty;
}
