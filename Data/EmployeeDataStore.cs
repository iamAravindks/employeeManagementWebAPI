using EmployeeManagement.Data.DTO;
using EmployeeManagement.Model;

namespace EmployeeManagement.Data
{
    public class EmployeeDataStore
    {
        private static EmployeeDataStore _instance;

        private static readonly object padlock = new object();

        private List<Employee> _employees;
 

        private EmployeeDataStore()
        {
            _employees = new List<Employee>();
        }
        public static EmployeeDataStore Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new EmployeeDataStore();
                    }
                    return _instance;
                }
            }
        }

        public List<Employee> Employees => _employees;

        //CRUD OPERATIONS FOR EMPLOYEES
        // Assume that the Id is given by the user (like empolyee id 11639 etc.., a custom one)
        //ADD new Employee

        public List<EmployeeBioDto> GetAllEmployees()
        {
            var employeesWithoutPasswords = new List<EmployeeBioDto>();

            foreach (var employee in _employees)
            {
                var employeeWithoutPassword = new EmployeeBioDto
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Position = employee.Position,
                    ManagerId = employee.ManagerId,
                    Email = employee.Email,
                    IsManager = employee.IsManager,
                };

                employeesWithoutPasswords.Add(employeeWithoutPassword);
            }

            return employeesWithoutPasswords;
        }

        public void AddEmployee(Employee employee)
        {
            _employees.Add(employee);
        }
        //Find an Employee

        public EmployeeFullProfileDto FindEmployee(int employeeId)
        {
            var employee=  _employees.FirstOrDefault(e => e.Id == employeeId);

            if(employee != null)
            {
                return new EmployeeFullProfileDto
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Position = employee.Position,
                    ManagerId = employee.ManagerId,
                    Email = employee.Email,
                    IsManager = employee.IsManager,
                    LeaveRequests = employee.LeaveRequests,
                };
            }
            return null;
        }

        public EmployeeFullProfileDto FindEmployee(System.Predicate<Employee> predicate)
        {
            var employee =  _employees.Find(predicate);
            if(employee != null)
            {
                return new EmployeeFullProfileDto
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Position = employee.Position,
                    ManagerId = employee.ManagerId,
                    Email = employee.Email,
                    IsManager = employee.IsManager,
                    LeaveRequests = employee.LeaveRequests,
                };
            }
            return null;
        }

        //DELETE Employee 
        public void DeleteEmployee(int employeeId)
        {
            Employee employeeToRemove = _employees.FirstOrDefault(e => e.Id == employeeId);
            if (employeeToRemove != null)
            {
                _employees.Remove(employeeToRemove);
            }
        }

        //AddleaveRequest
        public void AddLeaveRequest(int employeeId,LeaveRequest leaveRequest)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == employeeId);
            if (employee != null)
            {
                bool hasDuplicate = employee.LeaveRequests.Any(lr =>
                lr.Id == leaveRequest.Id ||
                 lr.StartDate.Date == leaveRequest.StartDate.Date || lr.EndDate.Date == leaveRequest.EndDate.Date
                  );

                if (!hasDuplicate)
                {
                    employee.LeaveRequests.Add(leaveRequest);
                }
                else
                {
                    // Handle duplicate leave request (e.g., throw an exception, return an error message)
                    throw new ArgumentException("Duplicate leave request detected.");
                }
            }

        }

        //Update Leave Request
        public void UpdateLeaveRequest(int employeeId ,int leaveRequestId, LeaveStatus newStatus)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == employeeId);
            if (employee != null)
            {
                var leaveRequest = employee.LeaveRequests.FirstOrDefault(lr => lr.Id == leaveRequestId);
                if(leaveRequest == null)
                {
                    throw new ArgumentException("Wrong leave Id");
                }
                if (leaveRequest != null)
                {
                    leaveRequest.Status = newStatus;
                }
            }
        }

        public List<EmployeeFullProfileDto> GetReportersForManager(int managerId)
        {
            var queryResultEmployees =  _employees.Where(e => e.ManagerId == managerId).ToList();

            var  employess = new List<EmployeeFullProfileDto>();

            foreach (var employee in queryResultEmployees)
            {
                var employeeWithoutPassword = new EmployeeFullProfileDto
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Position = employee.Position,
                    ManagerId = employee.ManagerId,
                    Email = employee.Email,
                    IsManager = employee.IsManager,
                    LeaveRequests = employee.LeaveRequests,
                };

                employess.Add(employeeWithoutPassword);
            }

            return employess;

        }

        public List<LeaveRequest> GetLeaveRequestsForEmployee(int employeeId)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == employeeId);
            return employee != null ? employee.LeaveRequests : new List<LeaveRequest>();
        }

        public List<LeaveRequest> GetLeaveRequestsReportedToManager(int managerId)
        {
            var reporters = GetReportersForManager(managerId);
            var allLeaveRequests = new List<LeaveRequest>();
            foreach (var reporter in reporters)
            {
                allLeaveRequests.AddRange(reporter.LeaveRequests);
            }
            return allLeaveRequests;
        }

        public void Seeder()
        {
            EmployeeDataStore dataStore = Instance;
            var employee1 = new Employee { Id = 1, Name = "John Doe", Position = "Software Engineer", ManagerId = 3, Email = "john@example.com", Password = "password1" };
            var employee2 = new Employee { Id = 2, Name = "Alice Smith", Position = "UI Designer", ManagerId = 3, Email = "alice@example.com", Password = "password2" };
            var employee3 = new Employee { Id = 3, Name = "Bob Johnson", Position = "Project Manager", IsManager = true, Email = "bob@example.com", Password = "password3" };
            var employee4 = new Employee { Id = 4, Name = "Emily Brown", Position = "Software Engineer", ManagerId = 3, Email = "emily@example.com", Password = "password4" };
            var employee5 = new Employee { Id = 5, Name = "Michael Davis", Position = "Software Engineer", ManagerId = 4, Email = "michael@example.com", Password = "password5" };

            // Add employees to the data store
            dataStore.AddEmployee(employee1);
            dataStore.AddEmployee(employee2);
            dataStore.AddEmployee(employee3);
            dataStore.AddEmployee(employee4);
            dataStore.AddEmployee(employee5);

        }

    }
}
