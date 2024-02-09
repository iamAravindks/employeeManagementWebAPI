using EmployeeManagement.Data;
using EmployeeManagement.Data.DTO;
using EmployeeManagement.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Controllers
{
    [Route("/api/v1.01")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        EmployeeDataStore dataStore;
        public EmployeeController()
        {
            dataStore = EmployeeDataStore.Instance;
            dataStore.Seeder();
        }

        [HttpPost("/employee/login")]

        public IActionResult Login([FromBody] EmployeeLoginDto employeeLoginDto)
        {
            var employee = dataStore.FindEmployee(e => e.Email == employeeLoginDto.Email && e.Password == employeeLoginDto.password);
            if (employee == null)
            {
                return Unauthorized("Invalid credentils");
            }
            var role = employee.Role==UserRoleEnum.ADMIN? UserRoles.ADMIN : (employee.Role == UserRoleEnum.MANAGER ? UserRoles.MANAGER : UserRoles.EMPLOYEE);
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, $"{employee.Id}"),
            new Claim(ClaimTypes.Role, role)

        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity)
                                 );
            return Ok(employee);
        }

        [HttpPost("/employee/logout")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("xyz-cookie-emp");
             HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [Authorize(Roles = $"{UserRoles.MANAGER},{UserRoles.ADMIN}")]
        [HttpPost("/employees")]
        public IActionResult AddEmployee([FromBody] Employee employee) {
            try
            {
                if(employee == null)
                {
                    return BadRequest("Not enough data");
                }
                if(employee.Role==UserRoleEnum.ADMIN)
                {
                    return BadRequest("Admin creation is not permitted");
                }

                var createdEmployee = dataStore.AddEmployee(employee);
                if(createdEmployee == null)
                {
                    return BadRequest("Employee creation");

                }
                return Ok(employee);
                
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet("/employees")]
        //public IActionResult GetAllEmployees()
        //{
        //    var employess = dataStore.GetAllEmployees();
        //    return Ok(employess);
        //}

        [Authorize]
        [HttpGet("/employee/profile")]
        public IActionResult GetEmployee()
        {
            try
            {
                if (!TryGetUserIdFromClaims(out int id))
                {
                    return BadRequest("Invalid or missing user ID claim");
                }
                var employee = dataStore.FindEmployee(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles =$"{UserRoles.EMPLOYEE},{UserRoles.MANAGER}")]
        [HttpPost("/employee/leaveapply")]
        public IActionResult ApplyLeave( [FromBody] LeaveRequestDto leaveRequestDto)
        {
            try
            {
                if (!TryGetUserIdFromClaims(out int id))
                {
                    return BadRequest("Invalid or missing user ID claim");
                }
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


        [Authorize(Roles = $"{UserRoles.ADMIN},{UserRoles.MANAGER}")]
        [HttpGet("/managers/manager/reportees")]
        public IActionResult GetManagerReportees() {
            try
            {
                if (!TryGetUserIdFromClaims(out int id))
                {
                    return BadRequest("Invalid or missing user ID claim");
                }
                var employess = dataStore.GetReportersForManager(id);
                return Ok(employess);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //!TODO : Need Authorization for manager

        [Authorize(Roles =$"{UserRoles.MANAGER},{UserRoles.ADMIN}")]
        [HttpPut("/managers/reportees/{reporteeId}/leaveapprove/{leaveId}")]
        public IActionResult ApproveLeave( [FromRoute] int reporteeId, [FromRoute] int leaveId)
        {
            try
            {
                if (!TryGetUserIdFromClaims(out int id))
                {
                    return BadRequest("Invalid or missing user ID claim");
                }

                bool isRepoortee = dataStore.FindEmployee(reporteeId)?.ManagerId==id;
                
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




        private bool TryGetUserIdFromClaims(out int id)
        {
            id = -1;
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out id))
            {
                return false; 
            }
            return true; 
        }
    }
}
