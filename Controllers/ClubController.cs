using BDProject_MarathonesApp.Interfaces;
using BDProject_MarathonesApp.Models;
using BDProject_MarathonesApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BDProject_MarathonesApp.Controllers
{
	public class ClubController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUserRepository _userRepository;
		private readonly IRaceRepository _raceRepository;
		private readonly IClubRepository _clubRepository;

		public ClubController(ILogger<HomeController> logger, IUserRepository userRepository, IRaceRepository raceRepository, IClubRepository clubRepository)
		{
			_logger = logger;
			this._userRepository = userRepository;
			this._raceRepository = raceRepository;
			this._clubRepository = clubRepository;
		}
		public async Task<IActionResult> Index( int userId)
		{
			int id;
			User user = await _userRepository.FindUserById(userId);
			if (user.Club != null)
			{
                id = user.Club.Id;

                Club? club = await _clubRepository.GetClubById(id);
                if (club != null)
                {
                    ClubDetailsVM clubVM = new ClubDetailsVM
                    {
                        Club = club,
                        UserId = userId
                    };
                    return View(clubVM);
                }
            }

            var clubs =await _clubRepository.GetAllClubs();
			ClubDetailsVM clubDetailsVM = new ClubDetailsVM
			{
				Clubs = clubs,
				UserId = userId
			};
			return View("Search", clubDetailsVM);
		}
		public async Task<IActionResult> Details(int id, int userId)
		{
			Club club = await _clubRepository.GetClubById(id);
			ClubDetailsVM clubVM = new ClubDetailsVM
			{
				Club = club,
				UserId = userId
			};

			return View(clubVM);
		}
        
        [HttpPost]
		public async Task<IActionResult> JoinClub(int userId, int id)
		{
			var result = await _clubRepository.JoinClub(id, userId);
			return RedirectToAction("Index", new {userId});
		}
        [HttpPost]
        public async Task<IActionResult> LeaveClub(int userId)
        {
            var result = await _clubRepository.LeaveClub(userId);
            return RedirectToAction("Index", new { userId });
        }

    }
}
