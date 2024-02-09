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
            // Set default manager ID if the role is MANAGER
            if (Role == UserRoleEnum.MANAGER)
            {
                ManagerId = 111; //super admin id
            }
        }
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Position { get; set; } = null!;

        public UserRoleEnum Role { get; set; } = UserRoleEnum.EMPLOYEE;
        public int ManagerId { get; set; }

        public List<LeaveRequest> LeaveRequests { get; set; }

    }
}
