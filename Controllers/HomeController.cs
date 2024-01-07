using BDProject_MarathonesApp.Interfaces;
using BDProject_MarathonesApp.Models;
using BDProject_MarathonesApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Collections.Specialized.BitVector32;

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
        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            User user = await _userRepository.FindUserById(id);
            ProfileVM profileVM = new ProfileVM
            {
                UserId = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Login = user.Login,
                Password = user.Password,
                AddressId = user.Address?.Id,
                Region = user.Address?.Region,
                City = user.Address?.City,
                Street = user.Address?.Street,
                BuildingNumber = user.Address?.BuildingNumber,
                PostalCode = user.Address?.PostalCode,

            };
            return View(profileVM);
        }
		[HttpPost]
		public async Task<IActionResult> UpdateUserProfile(int UserId, string Name, string LastName, string Login, string Password, int? AddressId, string? Region, string? City, string? Street, string? BuildingNumber, string? PostalCode)
		{
			User user1 = new User
			{
				Id = UserId,
				Name = Name,
				LastName = LastName,
				Login = Login,
				Password = Password,
			};

			

			
			
			Address address = new Address
			{
				Id = AddressId,
				Region =    Region,
				City =  City,
				Street = Street,
				BuildingNumber = BuildingNumber,
				PostalCode = PostalCode,
			};
            

            if (address.Id == null)
            {
				int number = await _userRepository.AddAddressRetId(Region, City, Street, PostalCode, BuildingNumber);
                address.Id = number;
			}
            else
            {
				await _userRepository.UpdateAddress(address);
			}
			user1.Address = address;
			int? number1 = await _userRepository.GetAddressId(address);
            if(number1 != null) user1.Address.Id = number1;
            await _userRepository.UpdateUserProfile(user1);

                

			

			return RedirectToAction("Profile", new { id = user1.Id });
		}

		[HttpPost]
		public async Task<IActionResult> UpdateUserAddress(User address)
		{
			bool result = await _userRepository.UpdateAddress(address.Address);
			return NoContent();
		}
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool result = await _userRepository.DeleteUser(id);
            if(result) return RedirectToAction("Login", "Account");
            return NoContent();
        }
	}

}

