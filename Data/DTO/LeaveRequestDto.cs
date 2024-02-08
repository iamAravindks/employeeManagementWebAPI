using EmployeeManagement.Model;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Data.DTO
{
    public class LeaveRequestDto
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        [StringLength(100, ErrorMessage = "Reason must be at most 100 characters long")]
        public string Reason { get; set; } = null!;
    }
}
