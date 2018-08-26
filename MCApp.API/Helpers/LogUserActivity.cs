using System;
using System.Security.Claims;
using System.Threading.Tasks;
using MCApp.API.Data;
using MCApp.API.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace MCApp.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            var userid = int.Parse(s: resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var repo = resultContext.HttpContext.RequestServices.GetService<IMicroCreditRepository>();
            User user = await repo.GetUser(userid);
            if (user != null)
            {
                user.LastActive = DateTime.Now;
                await repo.SaveAll();
            }
        }
    }
}