using BDProject_MarathonesApp.Interfaces;
using BDProject_MarathonesApp.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

namespace BDProject_MarathonesApp.Data
{
    public class RaceRepository : IRaceRepository
    {
        private readonly string connectionString;

        public RaceRepository(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");

            // Sprawdź, czy connectionString nie jest nullem
            if (string.IsNullOrEmpty(this.connectionString))
            {
                throw new InvalidOperationException("ConnectionString is null or empty.");
            }
        }

        


        public async Task<List<Race>> GetAllRaces()
        {
            List<Race> races = new List<Race>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT biegi.adres_biegu, biegi.Id, nazwa_biegu, opis_biegu, dystans, data_biegu, miasto FROM biegi, adresy WHERE adres_biegu=adresy.Id ORDER BY data_biegu ASC";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Jeżeli udało się odczytać dane, utwórz obiekt ClubVM i dodaj go do listy
                            Race race = new Race
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("nazwa_biegu"),
                                Description = reader.GetString("opis_biegu"),
                                Distance = reader.GetDouble("dystans"),
                                Date = reader.GetDateTime("data_biegu"),
                                Address = new Address
                                {
                                   Id = reader.GetInt32("adres_biegu"),
								   City = reader.GetString("miasto")
                                }
                                
                            };

                            races.Add(race);
                        }
                    }
                }
            }
            return races;
        }
        public async Task<List<Race>> GetFinishedRaces()
        {
            List<Race> races = new List<Race>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT biegi.Id, nazwa_biegu, opis_biegu, dystans, data_biegu,  wojewodztwo, miasto, ulica, nr_budynku, kod_pocztowy FROM biegi, adresy WHERE adres_biegu=adresy.Id AND data_biegu < CURDATE() ORDER BY data_biegu DESC";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Jeżeli udało się odczytać dane, utwórz obiekt ClubVM i dodaj go do listy
                            Race race = new Race
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("nazwa_biegu"),
                                Description = reader.GetString("opis_biegu"),
                                Distance = reader.GetDouble("dystans"),
                                Date = reader.GetDateTime("data_biegu"),
                                Address = new Address
                                {
                                    City = reader.GetString("miasto")
                                }

                            };

                            races.Add(race);
                        }
                    }
                }
            }
            return races;
        }
        public async Task<List<Race>> GetAllNotFinishedRaces()
        {
            List<Race> races = new List<Race>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT biegi.Id, nazwa_biegu, opis_biegu, dystans, data_biegu,  wojewodztwo, miasto, ulica, nr_budynku, kod_pocztowy FROM biegi, adresy WHERE adres_biegu=adresy.Id AND data_biegu > CURDATE() ORDER BY data_biegu ASC";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Jeżeli udało się odczytać dane, utwórz obiekt ClubVM i dodaj go do listy
                            Race race = new Race
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("nazwa_biegu"),
                                Description = reader.GetString("opis_biegu"),
                                Distance = reader.GetDouble("dystans"),
                                Date = reader.GetDateTime("data_biegu"),
                                Address = new Address
                                {
                                    City = reader.GetString("miasto")
                                }

                            };

                            races.Add(race);
                        }
                    }
                }
            }
            return races;
        }
        public async Task<Race?> GetRaceById(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT biegi.adres_biegu, biegi.Id AS Id, nazwa_biegu, opis_biegu, dystans, data_biegu, wojewodztwo, miasto, ulica, nr_budynku, kod_pocztowy FROM biegi, adresy WHERE adresy.Id = adres_biegu AND biegi.Id = {id}";


                using (MySqlCommand command = new MySqlCommand(query, connection))
                {


                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            
                            return new Race
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("nazwa_biegu"),
                                Description = reader.GetString("opis_biegu"),
                                Distance = reader.GetDouble("dystans"),
                                Date = reader.GetDateTime("data_biegu"),
                                Address = new Address
                                {
                                    Id = reader.GetInt32("adres_biegu"),
									Region = reader.IsDBNull(reader.GetOrdinal("wojewodztwo")) ? null : reader.GetString("wojewodztwo"),
                                    City = reader.IsDBNull(reader.GetOrdinal("miasto")) ? null : reader.GetString("miasto"),
                                    Street = reader.IsDBNull(reader.GetOrdinal("ulica")) ? null : reader.GetString("ulica"),
                                    BuildingNumber = reader.IsDBNull(reader.GetOrdinal("nr_budynku")) ? null : reader.GetString("nr_budynku"),
                                    PostalCode = reader.IsDBNull(reader.GetOrdinal("kod_pocztowy")) ? null : reader.GetString("kod_pocztowy"),
                                },
                            };
                        }
                    }
                    
                    return null;
                }
            }
        }
        public async Task<List<Score>> GetScoreBoard(int id)
        {
            List<Score> results = new List<Score>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT wyniki.Id AS id, wyniki.uczestnik_id AS uczestnik_id, miejsce, czas_ukonczenia, uczestnicy.nr_startowy AS nr_startowy, uzytkownicy.imie AS imie_uzytkownik, uzytkownicy.nazwisko AS nazwisko, uzytkownicy.klub_id AS klub_id, kluby.nazwa AS nazwa_klubu " +
                               $"FROM wyniki " +
                               $"JOIN uczestnicy ON wyniki.uczestnik_id = uczestnicy.Id " +
                               $"JOIN uzytkownicy ON uczestnicy.uzytkownik_id = uzytkownicy.Id " +
                               $"LEFT JOIN kluby ON uzytkownicy.klub_id = kluby.Id " +
                               $"WHERE wyniki.bieg_id = {id}";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Jeżeli udało się odczytać dane, utwórz obiekt Score i dodaj go do listy
                            Score result = new Score
                            {
                                Place = reader.GetInt32("miejsce"),
                                Finish_Time = reader.GetTimeSpan("czas_ukonczenia"),
                                Runner = reader.IsDBNull(reader.GetOrdinal("uczestnik_id"))
                                    ? null
                                    : new Participant
                                    {
                                        StartingNumber = reader.GetInt32("nr_startowy"),
                                        User = new User
                                        {
                                            Name = reader.GetString("imie_uzytkownik"),
                                            LastName = reader.GetString("nazwisko"),
                                            Club = reader.IsDBNull(reader.GetOrdinal("klub_id"))
                                                ? null
                                                : new Club
                                                {
                                                    Id = reader.GetInt32("klub_id"),
                                                    Name = reader.GetString("nazwa_klubu")
                                                }
                                        }
                                    }
                            };

                            results.Add(result);
                        }
                    }
                }
            }
            return results;
        }


        public async Task<bool> DeleteRace(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string checkQuery = $"SELECT COUNT(*) FROM biegi WHERE Id={id}";
                using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                {
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (count == 0)
                    {
                        return false;
                    }
                }

                string deleteQuery = $"DELETE FROM biegi WHERE Id={id}";
                using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
                {
                    deleteCommand.ExecuteNonQuery();
                }

                return true;
            }
        }

		public async Task<bool> UpdateRace(int Id, string Name, string Description, double Distance, DateTime Date, int? AddressId, string? Region, string? City, string? Street, string? PostalCode, string? BuildingNumber)
		{
			string dateTime = Date.ToString("yyyy-MM-dd HH:mm:ss");
			int rowsAffected = 0;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				connection.Open();
				string query = $"UPDATE biegi SET nazwa_biegu = '{Name}', opis_biegu = '{Description}', dystans = {Distance}, data_biegu = '{dateTime}' WHERE biegi.Id = {Id} ";
				if (AddressId != null)
                {
				    query = $"UPDATE biegi JOIN adresy ON biegi.adres_biegu = adresy.Id SET biegi.nazwa_biegu = '{Name}', biegi.opis_biegu = '{Description}', biegi.dystans = {Distance}, biegi.data_biegu = '{dateTime}', adresy.wojewodztwo='{Region}', adresy.miasto='{City}', adresy.ulica='{Street}', adresy.nr_budynku = '{BuildingNumber}', adresy.kod_pocztowy = '{PostalCode}' WHERE biegi.Id = {Id};";
				}
				using (MySqlCommand command = new MySqlCommand(query, connection))
				{
					rowsAffected = await command.ExecuteNonQueryAsync();

					
				}
			}
			return rowsAffected > 0;
		}

        
    }
}
