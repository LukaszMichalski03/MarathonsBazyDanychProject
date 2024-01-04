using BDProject_MarathonesApp.Interfaces;
using BDProject_MarathonesApp.Models;
using BDProject_MarathonesApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BDProject_MarathonesApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IRaceRepository _raceRepository;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository, IRaceRepository raceRepository)
        {
            _logger = logger;
            this._userRepository = userRepository;
            this._raceRepository = raceRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            List<Race> races = await _raceRepository.GetAllRaces();
            var userVM = await _userRepository.FindUserById(id);
            if(userVM == null)  return RedirectToAction("Login", "Account");
            HomeIndexVM homeIndexVM = new HomeIndexVM() { 
                Races = races,
                User = userVM
            };
            return View(homeIndexVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
