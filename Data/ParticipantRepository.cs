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

        public async Task<List<Participant>> GetRaceParticipants(int raceId)
        {
            List<Participant> runners = new List<Participant>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT uczestnicy.Id AS id, uczestnicy.bieg_id AS bieg_id, uczestnicy.nr_startowy AS nr_startowy, uzytkownicy.login AS login, uzytkownicy.imie AS imie, uzytkownicy.nazwisko AS nazwisko FROM uzytkownicy, uczestnicy WHERE uczestnicy.bieg_id = {raceId} AND uczestnicy.uzytkownik_id = uzytkownicy.Id;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Participant participant = new Participant
                            {
                                Id = reader.GetInt32("id"),
                                StartingNumber = reader.GetInt32("nr_startowy"),
                                Race = new Race
                                {
                                    Id = reader.GetInt32("bieg_id")
                                },
                                User = new User
                                {
                                    Name = reader.GetString("imie"),
                                    Login = reader.GetString("login"),
                                    LastName = reader.GetString("nazwisko"),
                                }

                            };

                            runners.Add(participant);
                        }
                    }
                }
            }
            return runners;
        }

        public async Task<bool> DeleteParticipant(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string checkQuery = $"SELECT COUNT(*) FROM uczestnicy WHERE Id={id}";
                using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                {
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (count == 0)
                    {
                        return false;
                    }
                }

                string deleteQuery = $"DELETE FROM uczestnicy WHERE Id={id}";
                using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
                {
                    deleteCommand.ExecuteNonQuery();
                }

                return true;
            }
        }

        public async Task<List<Participant>> GetUserParticipations(int userId)
        {
            List<Participant> runners = new List<Participant>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT biegi.nazwa_biegu AS nazwa_biegu, adresy.miasto AS miasto, uzytkownicy.Id AS user_Id, uczestnicy.Id AS id, uczestnicy.bieg_id AS bieg_id, uczestnicy.nr_startowy AS nr_startowy, uzytkownicy.login AS login, uzytkownicy.imie AS imie, uzytkownicy.nazwisko AS nazwisko FROM uzytkownicy, biegi,  uczestnicy, adresy WHERE uzytkownicy.Id= {userId} AND uczestnicy.uzytkownik_id = uzytkownicy.Id AND uczestnicy.bieg_id = biegi.Id AND biegi.adres_biegu=adresy.Id;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Participant participant = new Participant
                            {
                                Id = reader.GetInt32("id"),
                                StartingNumber = reader.GetInt32("nr_startowy"),
                                Race = new Race
                                {
                                    Id = reader.GetInt32("bieg_id"),
                                    Name = reader.GetString("nazwa_biegu"),
                                    Address = new Address
                                    {
                                        City = reader.GetString("miasto")
                                    }
                                },
                                User = new User
                                {
                                    Id = reader.GetInt32("user_Id"),
                                    Name = reader.GetString("imie"),
                                    Login = reader.GetString("login"),
                                    LastName = reader.GetString("nazwisko"),
                                },
                                

                            };

                            runners.Add(participant);
                        }
                    }
                }
            }
            return runners;
        }

        public async Task<Participant> GetUserParticipant(int ParticipantId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT uzytkownicy.Id AS user_Id, uczestnicy.Id AS id, uczestnicy.bieg_id AS bieg_id, uczestnicy.nr_startowy AS nr_startowy, uzytkownicy.login AS login, uzytkownicy.imie AS imie, uzytkownicy.nazwisko AS nazwisko FROM uzytkownicy, uczestnicy WHERE uczestnicy.Id= {ParticipantId} AND uczestnicy.uzytkownik_id = uzytkownicy.Id;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {


                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            Participant participant = new Participant
                            {
                                Id = reader.GetInt32("id"),
                                StartingNumber = reader.GetInt32("nr_startowy"),
                                Race = new Race
                                {
                                    Id = reader.GetInt32("bieg_id")
                                },
                                User = new User
                                {
                                    Id = reader.GetInt32("user_Id"),
                                    Name = reader.GetString("imie"),
                                    Login = reader.GetString("login"),
                                    LastName = reader.GetString("nazwisko"),
                                }

                            };
                            return participant;
                        }
                    }
                    return null;
                }
            }
        }
    }
    
}
