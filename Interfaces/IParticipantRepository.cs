using BDProject_MarathonesApp.Models;

namespace BDProject_MarathonesApp.Interfaces
{
    public interface IParticipantRepository
    {
        Task<int> GetNewStartingNumber(int id);
        Task<List<Participant>> GetRaceParticipants(int raceId);
        Task<List<Participant>> GetUserParticipations(int userId);
        Task<Participant> GetUserParticipant(int ParticipantId);
        Task<bool> CreateParticipant(int userId, int raceId);        
        Task<bool> DeleteParticipant(int id);
        
    }
}
