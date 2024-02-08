using EmployeeManagement.Model;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Data.DTO
{
    public class EmployeeBioDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name must be at most 20 characters long")]
        public string Name { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Position is required")]
        [StringLength(20, ErrorMessage = "Position must be at most 20 characters long")]
        public string Position { get; set; } = null!;
        public bool IsManager { get; set; } = false;

        [Required]
        public int ManagerId { get; set; }

    }
}
