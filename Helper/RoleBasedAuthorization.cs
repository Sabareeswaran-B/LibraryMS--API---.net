using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using LibraryMS.Model;

namespace LibraryMS.Helpers.RBA;

public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var employee = (Employee)context.HttpContext.Items["User"]!;
        if (employee == null)
        {
            context.Result = new JsonResult(new { message = "unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
        else
        {
            if (employee.EmployeeRole != "Admin")
            {
                context.Result = new JsonResult(new { message = "forbidden" })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }    
        }
    }
}
