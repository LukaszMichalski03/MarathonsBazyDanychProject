using BDProject_MarathonesApp.Interfaces;
using BDProject_MarathonesApp.ViewModels;
using MySql.Data.MySqlClient;

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

        public async Task<List<RaceVM>> GetAllRaces()
        {
            List<RaceVM> races = new List<RaceVM>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT biegi.Id, nazwa_biegu, opis_biegu, dystans, data_biegu, miasto FROM biegi, adresy WHERE adres_biegu=adresy.Id ORDER BY data_biegu ASC";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Jeżeli udało się odczytać dane, utwórz obiekt ClubVM i dodaj go do listy
                            RaceVM race = new RaceVM
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("nazwa_biegu"),
                                Description = reader.GetString("opis_biegu"),
                                Distance = reader.GetDouble("dystans"),
                                Date = reader.GetDateTime("data_biegu"),
                                City = reader.GetString("miasto")
                            };

                            races.Add(race);
                        }
                    }
                }
            }
            return races;
        }
        public async Task<RaceVM?> GetRaceById(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT biegi.Id AS Id, nazwa_biegu, opis_biegu, dystans, data_biegu, wojewodztwo, miasto, ulica, nr_budynku, kod_pocztowy FROM biegi, adresy WHERE adresy.Id = adres_biegu AND biegi.Id = {Convert.ToInt32(id)}";


                using (MySqlCommand command = new MySqlCommand(query, connection))
                {


                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            
                            return new RaceVM
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("nazwa_biegu"),
                                Description = reader.GetString("opis_biegu"),
                                Distance = reader.GetDouble("dystans"),
                                Date = reader.GetDateTime("data_biegu"),
                                Address = new AddressVM
                                {
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
    }
}
