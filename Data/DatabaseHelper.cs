using System.Data.SqlClient;

namespace BDProject_MarathonesApp.Data
{
    public class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void AddUser(string name, string lastName, string login, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = $"INSERT INTO Uzytkownicy (imie, nazwisko, login, haslo) VALUES ('{name}', {lastName}, {login}, {password})";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
