using BDProject_MarathonesApp.ViewModels;

namespace BDProject_MarathonesApp.Interfaces
{
    public interface IRaceRepository
    {
        Task<List<RaceVM>> GetAllRaces();
        Task<RaceVM?> GetRaceById(int id);
    }
}
