namespace EmployeeManagement.Model
{

    
    public static class UserRoles
    {
        public const string MANAGER = "MANAGER";
        public const string EMPLOYEE = "EMPLOYEE";
    }



    public class Employee
    {

        public Employee() {

            LeaveRequests = new List<LeaveRequest>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        //since as per now , we didn't require any complex operations that involve with Position , just keep a string
        public string Position { get; set; } = null!;
        public bool IsManager { get; set; } = false;

        public int ManagerId { get; set; }

        public List<LeaveRequest> LeaveRequests { get; set; }

    }
}
