using BDProject_MarathonesApp.Interfaces;
using BDProject_MarathonesApp.Models;
using BDProject_MarathonesApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BDProject_MarathonesApp.Controllers
{
    public class ResultController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRaceRepository _raceRepository;

        public ResultController(IUserRepository userRepository, IRaceRepository raceRepository)
        {

            this._userRepository = userRepository;
            this._raceRepository = raceRepository;
        }
        
        public async Task<IActionResult> Index(int id)
        {
            List<Race> races = await _raceRepository.GetFinishedRaces();
            User? user = await _userRepository.FindUserById(id);
            HomeIndexVM homeIndexVM = new HomeIndexVM
            {
                Races = races,
                User = user
            };
            return View(homeIndexVM);
        }
        public async Task<IActionResult> Scoreboard(int id)
        {
            var scores = await _raceRepository.GetScoreBoard(id);
            if (scores is not null)
            {
                return View(scores);
            }
            return NoContent();
        }
    }
}
