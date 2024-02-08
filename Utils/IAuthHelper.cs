using EmployeeManagement.Model;
using System.Security.Claims;

namespace EmployeeManagement.Utils
{
    public interface IAuthHelper
    {
        ClaimsIdentity setClaimsIdForCookie(string id , string role);
    }
}
