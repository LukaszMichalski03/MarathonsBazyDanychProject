using BDProject_MarathonesApp.ViewModels;

namespace BDProject_MarathonesApp.Interfaces
{
    public interface IUserRepository
    {
        
        bool AddUser(string name, string lastname, string login, string password);

        Task<UserVM?> FindUserByLoginPassword(string login, string password);
        Task<UserVM?> FindUserById(int id);
    }
}
