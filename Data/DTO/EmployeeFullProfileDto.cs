using EmployeeManagement.Model;

namespace EmployeeManagement.Data.DTO
{
    public class EmployeeFullProfileDto :EmployeeBioDto
    {
        public List<LeaveRequest> LeaveRequests { get; set; } = null!;

    }
}
