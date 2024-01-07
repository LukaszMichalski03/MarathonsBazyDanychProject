using BDProject_MarathonesApp.Models;

namespace BDProject_MarathonesApp.Interfaces
{
    public interface IRaceRepository
    {
        Task<List<Race>> GetAllRaces();
        Task<List<Race>> GetFinishedRaces();
        Task<List<Race>> GetAllNotFinishedRaces();
        Task<Race?> GetRaceById(int id);
        Task<List<Score>> GetScoreBoard(int id);
        Task<bool> DeleteRace(int id);
        Task<bool> UpdateRace(int Id, string Name, string Description, double Distance, DateTime Date, int? AddressId, string? Region, string? City, string? Street, string? PostalCode, string? BuildingNumber);
    }
}
