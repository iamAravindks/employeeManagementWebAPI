namespace EmployeeManagement.Model
{

    public enum LeaveStatus
    {
        Pending,
        Approved,
        Denied
    }
    public class LeaveRequest
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public int ManagerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public LeaveStatus Status { get; set; } = LeaveStatus.Pending; 
    }

}
