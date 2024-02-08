using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Data.DTO
{
    public class EmployeeLoginDto
    {
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = null!;

        [Required]

        public string password { get; set; } = null!;
    }
}
