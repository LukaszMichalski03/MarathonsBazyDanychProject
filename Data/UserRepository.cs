using BDProject_MarathonesApp.Interfaces;
using BDProject_MarathonesApp.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Xml.Linq;


namespace BDProject_MarathonesApp.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString;

        public UserRepository(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");

            // Sprawdź, czy connectionString nie jest nullem
            if (string.IsNullOrEmpty(this.connectionString))
            {
                throw new InvalidOperationException("ConnectionString is null or empty.");
            }
        }

        public bool AddUser(string name, string lastName, string login, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"INSERT INTO Uzytkownicy (imie, nazwisko, login, haslo) VALUES ('{name}', '{lastName}', '{login}', '{password}')";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }

        public async Task<User?> FindUserById(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT uzytkownicy.adres_id AS adres_id, uzytkownicy.Id AS Id, klub_id, imie,haslo, nazwisko, login, adresy.wojewodztwo AS wojewodztwo, adresy.miasto AS miasto, adresy.ulica AS ulica, adresy.kod_pocztowy, adresy.nr_budynku FROM uzytkownicy LEFT JOIN adresy ON uzytkownicy.adres_id = adresy.Id WHERE uzytkownicy.id = {id};";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
							
							return new User
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("imie"),
                                LastName = reader.GetString("nazwisko"),
                                Login = reader.GetString("login"),
                                Password = reader.GetString("haslo"),
                                Address = reader.IsDBNull(reader.GetOrdinal("adres_id"))
                                    ? null
                                    :new Address
                                    {
                                        Id=reader.GetInt32("adres_id"),
                                        Region = reader.GetString("wojewodztwo"),
                                        City = reader.GetString("miasto"),
                                        Street = reader.GetString("ulica"),
                                        PostalCode = reader.GetString("kod_pocztowy"),
                                        BuildingNumber = reader.GetString("nr_budynku"),
                                    },
                                Club = reader.IsDBNull(reader.GetOrdinal("klub_id"))
                                ? null: new Club
                                {
                                    Id = reader.GetInt32("klub_id"),
                                }
                            };
                        }
                    }
                    return null;
                }
            }
        }

        public async Task<User?> FindUserByLoginPassword(string login, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, imie, nazwisko, login FROM uzytkownicy WHERE login=@Login AND haslo=@Password LIMIT 1";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("imie"),
                                LastName = reader.GetString("nazwisko"),
                                Login = reader.GetString("login")
                            };
                        }
                    }
                    return null;
                }
            }
        }

		public async Task<bool> UpdateUserProfile(User user)
		{
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				connection.Open();

				string query = $"UPDATE uzytkownicy SET imie= '{user.Name}', nazwisko='{user.LastName}', login='{user.Login}', haslo='{user.Password}', adres_id = {user.Address.Id} WHERE Id={user.Id} ";

				using (MySqlCommand command = new MySqlCommand(query, connection))
				{
					int rowsAffected =await command.ExecuteNonQueryAsync();

					return rowsAffected > 0;
				}
			}
		}
		public async Task<bool> UpdateAddress(Address address)
		{
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				connection.Open();

				string query = $"UPDATE adresy SET wojewodztwo= '{address.Region}', miasto='{address.City}', ulica='{address.City}', kod_pocztowy='{address.PostalCode}' , nr_budynku='{address.BuildingNumber}' WHERE Id={address.Id} ";

				using (MySqlCommand command = new MySqlCommand(query, connection))
				{
					int rowsAffected = await command.ExecuteNonQueryAsync();

					return rowsAffected > 0;
				}
			}
		}

		public async Task<int> AddAddressRetId(string region, string city, string street, string postalCode, string buildingNumber)
		{
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
                int number = await GetNewAddressId();
				connection.Open();

				string query = $"INSERT INTO adresy (Id, wojewodztwo, miasto, ulica, kod_pocztowy, nr_budynku) VALUES ({number}, '{region}', '{city}', '{street}', '{postalCode}', '{buildingNumber}')";

				using (MySqlCommand command = new MySqlCommand(query, connection))
				{
					int rowsAffected = command.ExecuteNonQuery();

					return number;
				}
			}
		}
		public async Task<int> GetNewAddressId()
		{
			int number = 1;

			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				connection.Open();

				string query = $"SELECT MAX(Id) AS Id  FROM adresy";

				using (MySqlCommand command = new MySqlCommand(query, connection))
				{
					using (MySqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read() && !reader.IsDBNull(reader.GetOrdinal("Id")))
						{
							number = reader.GetInt32("Id") + 1;
						}

					}
				}
			}

			return number;
		}
		public async Task<int?> GetAddressId(Address address)
		{
			int? number = null;

			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				connection.Open();

				string query = $"SELECT Id FROM adresy WHERE wojewodztwo = '{address.Region}' AND miasto= '{address.City}' AND ulica= '{address.Street}' AND kod_pocztowy='{address.PostalCode}'AND nr_budynku= '{address.BuildingNumber}'";

				using (MySqlCommand command = new MySqlCommand(query, connection))
				{
					using (MySqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							if (!reader.IsDBNull(reader.GetOrdinal("Id")))
							{
								number = reader.GetInt32("Id");
								break;
							}
						}

					}
				}
			}

			return number;
		}

        public async Task<bool> DeleteUser(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"DELETE FROM uzytkownicy WHERE Id = {id}";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT Id, imie, nazwisko, login, haslo FROM uzytkownicy";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Jeżeli udało się odczytać dane, utwórz obiekt ClubVM i dodaj go do listy
                            User user = new User
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("imie"),
                                LastName = reader.GetString("nazwisko"),
                                Login = reader.GetString("login"),
                                Password = reader.GetString("haslo")

                            };

                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }
    }
}
