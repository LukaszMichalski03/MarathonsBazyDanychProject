using BDProject_MarathonesApp.Models;

namespace BDProject_MarathonesApp.Interfaces
{
	public interface IClubRepository
	{
		Task<Club?> GetClubById(int id);
		Task<List<Club>> GetAllClubs();
		Task<bool> JoinClub(int id, int userId);
		Task<bool> LeaveClub(int userId);


    }
}
