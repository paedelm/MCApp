using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace MCApp.API.Controllers
{
    public class Fallback: Controller
    {
        [HttpGet]
        public IActionResult Index() {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");
        }   
    }
}