using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeManagement.Model
{

    
    public static class UserRoles
    {
        public const string MANAGER = "manager";
        public const string EMPLOYEE = "employee";
        public const string ADMIN = "admin";
    }

    public enum UserRoleEnum
    {
        MANAGER,
        EMPLOYEE,
        ADMIN
    }



    public class Employee
    {

        public Employee() {

            LeaveRequests = new List<LeaveRequest>();
            // Ensure that Id cannot be 111 for roles except ADMIN
            if (Role != UserRoleEnum.ADMIN && Id == 111)
            {
                throw new ArgumentException("Id cannot be 111 for roles other than ADMIN.");
            }
        }
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name must be at most 20 characters long")]
        public string Name { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password must be between  6 and  15 characters long")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Position is required")]
        [StringLength(20, ErrorMessage = "Position must be at most 20 characters long")]
        public string Position { get; set; } = null!;
        public UserRoleEnum Role { get; set; } = UserRoleEnum.EMPLOYEE;

        [Required]
        public int ManagerId { get; set; }

        [JsonIgnore]
        public List<LeaveRequest> LeaveRequests { get; set; }

    }
}
