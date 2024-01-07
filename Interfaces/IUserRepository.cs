using BDProject_MarathonesApp.Models;

namespace BDProject_MarathonesApp.Interfaces
{
    public interface IUserRepository
    {
        
        bool AddUser(string name, string lastname, string login, string password);


        Task<int> AddAddressRetId(string region, string city, string street, string postalCode, string buildingNumber);


		Task<User?> FindUserByLoginPassword(string login, string password);
        Task<User?> FindUserById(int id);
        Task<int> GetNewAddressId();
        Task<List<User>> GetAllUsers();
        Task<int?> GetAddressId(Address address);
		Task<bool> UpdateUserProfile(User user);
        Task<bool> UpdateAddress(Address address);
        Task<bool> DeleteUser(int id);

	}
}
