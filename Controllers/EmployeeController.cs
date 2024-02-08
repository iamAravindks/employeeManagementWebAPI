using EmployeeManagement.Data;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        EmployeeDataStore dataStore;
        public EmployeeController()
        {
            dataStore = EmployeeDataStore.Instance;
            dataStore.Seeder();
        }
        [HttpGet("/employees")]
        public IActionResult GetAllEmployees()
        {
            var employess = dataStore.GetAllEmployees();
            return Ok(employess);
        }

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


        //!TODO : Need Authorization

        [HttpGet("/managers/{managerId}/reporters")]
        public IActionResult GetManagerReportees([FromRoute]int managerId) {
            var employess = dataStore.GetReportersForManager(managerId);
            return Ok(employess);
        }
    }
}
