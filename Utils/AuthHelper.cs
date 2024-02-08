using EmployeeManagement.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace EmployeeManagement.Utils
{
    public class AuthHelper : IAuthHelper
    {
        public ClaimsIdentity setClaimsIdForCookie(string id, string role)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Role, role)

        };


            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return claimsIdentity;

        }
    }
}
