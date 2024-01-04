using BDProject_MarathonesApp.Interfaces;
using BDProject_MarathonesApp.Models;
using MySql.Data.MySqlClient;

namespace BDProject_MarathonesApp.Data
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly string connectionString;

        public ParticipantRepository(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");

            // Sprawdź, czy connectionString nie jest nullem
            if (string.IsNullOrEmpty(this.connectionString))
            {
                throw new InvalidOperationException("ConnectionString is null or empty.");
            }
        }
        public async Task<int> GetNewStartingNumber(int id)
        {
            int number = 1;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT MAX(nr_startowy) AS nr_startowy FROM uczestnicy WHERE bieg_id={id}";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read() && !reader.IsDBNull(reader.GetOrdinal("nr_startowy")))
                        {
                            number = reader.GetInt32("nr_startowy") + 1;
                        }
                        
                    }
                }
            }

            return number;
        }
        public async Task<bool> CreateParticipant(int userId, int raceId)
        {
            int newStartingNumber = await GetNewStartingNumber(raceId);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery = $"INSERT INTO uczestnicy (bieg_id, nr_startowy, uzytkownik_id) VALUES({raceId}, {newStartingNumber}, {userId})";

                using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                {
                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }


    }
}
