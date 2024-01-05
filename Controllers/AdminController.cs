using BDProject_MarathonesApp.Interfaces;
using BDProject_MarathonesApp.Models;
using BDProject_MarathonesApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BDProject_MarathonesApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRaceRepository _raceRepository;
        private readonly IParticipantRepository _participantRepository;

        public AdminController(IUserRepository userRepository, IRaceRepository raceRepository, IParticipantRepository participantRepository)
        {

            this._userRepository = userRepository;
            this._raceRepository = raceRepository;
            this._participantRepository = participantRepository;
        }
        public async Task<IActionResult> AllRaces()
        {
            var races = await _raceRepository.GetAllRaces();
            
            
            return View("Races/Index", races);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateRace(int id)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRace(int id)
        {
            await _raceRepository.DeleteRace(id); // ustawić kaskadowe usuwanie w bazie danych!!!!!!!!!!!!!!!!!!!!!!!!


            return RedirectToAction("AllRaces"); 
        }
        public async Task<IActionResult> GetParticipants(int raceId)
        {
            List<Participant> runners = await _participantRepository.GetRaceParticipants(raceId);
            AdminRunnersVM adminRunnersVM = new AdminRunnersVM
            {
                Raceid = raceId,
                Participants = runners
            };
            return View("Runners/Index", adminRunnersVM);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteParticipant(int raceId, int id)
        {
            await _participantRepository.DeleteParticipant(id);
            return RedirectToAction("GetParticipants", new { raceId = raceId });

        }
        [HttpPost]
        public async Task<IActionResult> CreateParticipantRecord(int id, int raceId)
        {
            await _participantRepository.CreateParticipant(id, raceId);
            return RedirectToAction("GetParticipants", new { raceId = raceId });
        }
    }
}
