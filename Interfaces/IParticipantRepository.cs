namespace BDProject_MarathonesApp.Interfaces
{
    public interface IParticipantRepository
    {
        Task<int> GetNewStartingNumber(int id);
        Task<bool> CreateParticipant(int userId, int raceId);
    }
}
