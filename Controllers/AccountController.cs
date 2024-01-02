using Microsoft.AspNetCore.Mvc;

namespace BDProject_MarathonesApp.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {
            
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string name, string username, string login, string password)
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
