using BDProject_MarathonesApp.Models;

namespace BDProject_MarathonesApp.Interfaces
{
    public interface IRaceRepository
    {
        Task<List<Race>> GetAllRaces();
        Task<List<Race>> GetFinishedRaces();
        Task<Race?> GetRaceById(int id);
        Task<List<Score>> GetScoreBoard(int id);
        Task<bool> DeleteRace(int id);
    }
}
