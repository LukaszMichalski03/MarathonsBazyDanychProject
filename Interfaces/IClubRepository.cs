using BDProject_MarathonesApp.Models;

namespace BDProject_MarathonesApp.Interfaces
{
	public interface IClubRepository
	{
		Task<Club?> GetClubById(int id);
		Task<List<Club>> GetAllClubs();
		Task<List<User>> GetClubMembers(int clubId);
		Task<bool> DeleteClubById(int id);
		Task<bool> JoinClub(int id, int userId);
		Task<bool> LeaveClub(int userId);

		Task<bool> CreateClub(string Name, string Description, string? Region, string? City, string? Street, string? PostalCode, string? BuildingNumber);
	}
}
