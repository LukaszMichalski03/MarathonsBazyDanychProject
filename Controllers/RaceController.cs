using BDProject_MarathonesApp.Interfaces;
using BDProject_MarathonesApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BDProject_MarathonesApp.Controllers
{
    public class RaceController : Controller
    {
        
        private readonly IUserRepository _userRepository;
        private readonly IRaceRepository _raceRepository;

        public RaceController (IUserRepository userRepository, IRaceRepository raceRepository)
        {
           
            this._userRepository = userRepository;
            this._raceRepository = raceRepository;
        }
        public async Task<IActionResult> Details(int id)
        {
            RaceVM? raceVM = await _raceRepository.GetRaceById(id);
            if (raceVM is not null)
            {
                return View(raceVM);               
            }
            return NoContent();
        }
    }
}
