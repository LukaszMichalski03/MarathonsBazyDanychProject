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

                string query = $"SELECT Id, imie, nazwisko, login FROM uzytkownicy WHERE id = {id}";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Jeżeli udało się odczytać dane, zwróć obiekt User
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
                            // Jeżeli udało się odczytać dane, zwróć obiekt User
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


    }
}
