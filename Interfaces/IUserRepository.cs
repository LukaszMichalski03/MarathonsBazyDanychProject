using BDProject_MarathonesApp.Models;

namespace BDProject_MarathonesApp.Interfaces
{
    public interface IUserRepository
    {
        
        bool AddUser(string name, string lastname, string login, string password);

        Task<User?> FindUserByLoginPassword(string login, string password);
        Task<User?> FindUserById(int id);
    }
}
