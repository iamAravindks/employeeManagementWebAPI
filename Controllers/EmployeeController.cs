using EmployeeManagement.Data;
using EmployeeManagement.Data.DTO;
using EmployeeManagement.Model;
using EmployeeManagement.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        EmployeeDataStore dataStore;
        IAuthHelper _authHelper;
        public EmployeeController(IAuthHelper authHelper)
        {
            _authHelper = authHelper;
            dataStore = EmployeeDataStore.Instance;
            dataStore.Seeder();
        }

        [HttpPost("/employee/login")]

        public async Task<IActionResult> Login([FromBody] EmployeeLoginDto employeeLoginDto)
        {
            var employee = dataStore.FindEmployee(e => e.Email == employeeLoginDto.Email && e.Password == employeeLoginDto.password);
            if (employee == null)
            {
                return Unauthorized("Invalid credentils");
            }
            var role = employee.IsManager ? UserRoles.MANAGER : UserRoles.EMPLOYEE;
            await setCookie($"{employee.Id}", role);
            return Ok(employee);
        }

        [HttpGet("/employees")]
        public IActionResult GetAllEmployees()
        {
            var employess = dataStore.GetAllEmployees();
            return Ok(employess);
        }
        //!TODO : Need Authorization for employee

        [HttpGet("/employees/{id}")]
        public IActionResult GetEmployee(int id)
        {
            var employee = dataStore.FindEmployee(id);
            if(employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        //!TODO : Need Authorization for Employee

        [HttpPost("/employee/{id}/leaveapply")]
        public IActionResult ApplyLeave([FromRoute] int id, [FromBody] LeaveRequestDto leaveRequestDto)
        {
            try
            {
                var employee = dataStore.FindEmployee(id);
                if (employee == null)
                {
                    return NotFound();
                }
                var leaveRequest = new LeaveRequest
                {
                    EmployeeId = employee.Id,
                    StartDate = leaveRequestDto.StartDate,
                    EndDate = leaveRequestDto.EndDate,
                    Id = leaveRequestDto.Id,
                    ManagerId = employee.ManagerId,
                    Reason = leaveRequestDto.Reason,
                };

                dataStore.AddLeaveRequest(employee.Id, leaveRequest);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //!TODO : Need Authorization for manager

       
        [HttpGet("/managers/{managerId}/reporters")]
        public IActionResult GetManagerReportees([FromRoute]int managerId) {
            var employess = dataStore.GetReportersForManager(managerId);
            return Ok(employess);
        }

        //!TODO : Need Authorization for manager

        [HttpPut("/managers/{managerId}/reportees/{reporteeId}/leaveapprove/{leaveId}")]
        public IActionResult ApproveLeave([FromRoute] int managerId, [FromRoute] int reporteeId, [FromRoute] int leaveId)
        {
            try
            {
                bool isRepoortee = dataStore.GetReportersForManager(managerId).Any(e => e.ManagerId == managerId);

                if (!isRepoortee)
                {
                    return BadRequest("Wrong manager or wrong reportee");
                }

                dataStore.UpdateLeaveRequest(reporteeId, leaveId, LeaveStatus.Approved);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private async Task setCookie(string id,string role)
        {
            var claimsIdentity = _authHelper.setClaimsIdForCookie(id, role);
            await HttpContext.SignInAsync(
                                 CookieAuthenticationDefaults.AuthenticationScheme,
                                 new ClaimsPrincipal(claimsIdentity)
                                  );
        }
    }
}
