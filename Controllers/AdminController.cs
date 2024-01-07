using BDProject_MarathonesApp.Interfaces;
using BDProject_MarathonesApp.Models;
using BDProject_MarathonesApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BDProject_MarathonesApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRaceRepository _raceRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IClubRepository _clubRepository;

        public AdminController(IUserRepository userRepository, IRaceRepository raceRepository, IParticipantRepository participantRepository, IClubRepository clubRepository)
        {

            this._userRepository = userRepository;
            this._raceRepository = raceRepository;
            this._participantRepository = participantRepository;
            this._clubRepository = clubRepository;
        }
        public async Task<IActionResult> AllRaces()
        {
            var races = await _raceRepository.GetAllNotFinishedRaces();
            
            
            return View("Races/Index", races);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateRace(int id)
        {
            var Race = await _raceRepository.GetRaceById(id);
            return View("Races/Edit",Race);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRace(int id)
        {
            await _raceRepository.DeleteRace(id);


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
        [HttpPost]
        public async Task<IActionResult> UpdateRace(int Id, string Name, string Description, double Distance, DateTime Date, int? AddressId, string? Region, string? City, string? Street, string? PostalCode, string? BuildingNumber)
		{
            
			bool result = await _raceRepository.UpdateRace( Id,  Name,  Description,  Distance,  Date,  AddressId,  Region,  City,  Street,  PostalCode,  BuildingNumber);

			return RedirectToAction("AllRaces");
        }
        
        public IActionResult AddRace()
        {
            return View("Races/Create");
        }
		[HttpPost]
		public async Task<IActionResult> CreateRace(string Name, string Description, double Distance, DateTime Date, string? Region, string? City, string? Street, string? PostalCode, string? BuildingNumber)
		{

			bool result = await _raceRepository.CreateRace(Name, Description, Distance, Date, Region, City, Street, PostalCode, BuildingNumber);

			return RedirectToAction("AllRaces");
		}
		public IActionResult AddClub()
		{
			return View("Clubs/Create");
		}
		[HttpPost]
		public async Task<IActionResult> CreateClub(string Name, string Description, string? Region, string? City, string? Street, string? PostalCode, string? BuildingNumber)
		{

			bool result = await _clubRepository.CreateClub(Name, Description,  Region, City, Street, PostalCode, BuildingNumber);

			return RedirectToAction("AllClubs");
		}
		public async Task<IActionResult> AllUsers()
        {
            List<User> users = await _userRepository.GetAllUsers();
            return View("Users/Index", users);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userRepository.DeleteUser(id);
            return RedirectToAction("AllRaces");
        }
        public async Task<IActionResult> UserHistory(int userId)
        {
            var participations = await _participantRepository.GetUserParticipations(userId);
            return View("Users/UserHistory", participations);
        }
        public async Task<IActionResult> AllClubs()
        {
            List<Club> clubs = await _clubRepository.GetAllClubs();
            return View("Clubs/Index", clubs);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteClub(int id)
        {
            await _clubRepository.DeleteClubById(id);
            return RedirectToAction("AllClubs");
        }
        public async Task<IActionResult> GetClubMembers(int id)
        {
            var members = await _clubRepository.GetClubMembers(id);
            return View("Clubs/Members", members);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteClubMember(int userId)
        {
            var result = await _clubRepository.LeaveClub(userId);
            return RedirectToAction("AllClubs");
        }

    }
}
