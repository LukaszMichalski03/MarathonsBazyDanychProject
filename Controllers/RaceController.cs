﻿using BDProject_MarathonesApp.Interfaces;
using BDProject_MarathonesApp.Models;
using BDProject_MarathonesApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BDProject_MarathonesApp.Controllers
{
    public class RaceController : Controller
    {
        
        private readonly IUserRepository _userRepository;
        private readonly IRaceRepository _raceRepository;
        private readonly IParticipantRepository _participantRepository;

        public RaceController (IUserRepository userRepository, IRaceRepository raceRepository, IParticipantRepository participantRepository)
        {
           
            this._userRepository = userRepository;
            this._raceRepository = raceRepository;
            this._participantRepository = participantRepository;
        }
        public async Task<IActionResult> Index(int id)
        {
            List<Participant> participations = await _participantRepository.GetUserParticipations(id);
            RaceIndexVM raceIndexVM = new RaceIndexVM
            {
                Participants = participations,
                UserId = id
            };
            return View(raceIndexVM);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteParticipant(int id)
        {
            var userPart = await _participantRepository.GetUserParticipant(id);
            int userId = userPart.User.Id;
            await _participantRepository.DeleteParticipant(id);
            return RedirectToAction("Index", new {id = userId });
        }
        public async Task<IActionResult> Details(int id, int userId)
        {
            Race? race = await _raceRepository.GetRaceById(id);
            
            if (race is not null)
            {
                RaceDetailsVM raceDetailsVM = new RaceDetailsVM
                {
                    Race = race,
                    UserId = userId
                };
                return View(raceDetailsVM);               
            }
            return NoContent();
        }
        [HttpPost]
        public async Task<IActionResult> SignForARun(int id, int userId)
        {
            var result = await _participantRepository.CreateParticipant(userId, id);

            if(result) return RedirectToAction("Index", "Home", new {id = userId});
            

            return NoContent();
        }


    }
}
