using BDProject_MarathonesApp.Interfaces;
using BDProject_MarathonesApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BDProject_MarathonesApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository databaseRepository)
        {
            _userRepository = databaseRepository;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            UserVM? user = await _userRepository.FindUserByLoginPassword(email, password);
            if(user is not null) return RedirectToAction("Index", "Home", new {id = user.Id});
            return View();
        }
        
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(string name, string lastName, string email, string password)
        {
            bool result = _userRepository.AddUser(name, lastName, email, password);
            if (result) return RedirectToAction("Index", "Home"); 
            return View();
        }
    }
}
