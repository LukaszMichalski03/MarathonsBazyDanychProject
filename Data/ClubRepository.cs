using BDProject_MarathonesApp.Interfaces;
using BDProject_MarathonesApp.Models;
using MySql.Data.MySqlClient;

namespace BDProject_MarathonesApp.Data
{
	public class ClubRepository : IClubRepository
	{

		private readonly string connectionString;

		public ClubRepository(IConfiguration configuration)
		{
			this.connectionString = configuration.GetConnectionString("DefaultConnection");

			// Sprawdź, czy connectionString nie jest nullem
			if (string.IsNullOrEmpty(this.connectionString))
			{
				throw new InvalidOperationException("ConnectionString is null or empty.");
			}
		}

        public async Task<bool> DeleteClubById(int id)
        {
            using(MySqlConnection connection = new MySqlConnection(connectionString))
			{
				connection.Open();
				string query = $"DELETE FROM kluby WHERE Id={id}";//kaskadowe usuwanie w bd
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
        }

        public async Task<List<Club>> GetAllClubs()
		{
			List<Club> races = new List<Club>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				connection.Open();

				string query = $"SELECT kluby.Id, kluby.nazwa, kluby.opis, adres_id, adresy.wojewodztwo, adresy.miasto, adresy.ulica, adresy.kod_pocztowy, adresy.nr_budynku FROM kluby, adresy WHERE kluby.adres_id=adresy.Id;";

				using (MySqlCommand command = new MySqlCommand(query, connection))
				{
					using (MySqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							// Jeżeli udało się odczytać dane, utwórz obiekt ClubVM i dodaj go do listy
							Club race = new Club
							{
								Id = reader.GetInt32("Id"),
								Name = reader.GetString("nazwa"),
								Description = reader.GetString("opis"),

								Address = reader.IsDBNull(reader.GetOrdinal("adres_id"))

									? null
									: new Address
									{
										Id = reader.GetInt32("adres_id"),
										Region = reader.GetString("wojewodztwo"),
										City = reader.GetString("miasto"),
										Street = reader.GetString("ulica"),
										PostalCode = reader.GetString("kod_pocztowy"),
										BuildingNumber = reader.GetString("nr_budynku"),
									}

							};

							races.Add(race);
						}
					}
				}
			}
			return races;
		}

		public async Task<Club?> GetClubById(int id)
        {
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				connection.Open();

				string query = $"SELECT kluby.Id, kluby.nazwa, kluby.opis, adres_id, adresy.wojewodztwo, adresy.miasto, adresy.ulica, adresy.kod_pocztowy, adresy.nr_budynku FROM kluby, adresy WHERE kluby.id = {id} AND kluby.adres_id=adresy.Id;";

				using (MySqlCommand command = new MySqlCommand(query, connection))
				{


					using (MySqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{

							return new Club
							{
								Id = reader.GetInt32("Id"),
								Name = reader.GetString("nazwa"),
								Description = reader.IsDBNull(reader.GetOrdinal("opis")) ? null : reader.GetString("opis"),
								Address = reader.IsDBNull(reader.GetOrdinal("adres_id"))

									? null
									: new Address
									{
										Id = reader.GetInt32("adres_id"),
										Region = reader.GetString("wojewodztwo"),
										City = reader.GetString("miasto"),
										Street = reader.GetString("ulica"),
										PostalCode = reader.GetString("kod_pocztowy"),
										BuildingNumber = reader.GetString("nr_budynku"),
									}
							};
						}
					}
					return null;
				}
			}
		}

        public async Task<List<User>> GetClubMembers(int clubId)
        {
            List<User> races = new List<User>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT uzytkownicy.Id AS Id, uzytkownicy.imie, uzytkownicy.nazwisko, uzytkownicy.login, adresy.miasto, uzytkownicy.adres_id AS adres_id " +
               $"FROM kluby " +
               $"JOIN uzytkownicy ON kluby.Id = uzytkownicy.klub_id " +
               $"LEFT JOIN adresy ON uzytkownicy.adres_id = adresy.Id " +
               $"WHERE kluby.Id = {clubId}";


                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Jeżeli udało się odczytać dane, utwórz obiekt ClubVM i dodaj go do listy
                            User race = new User
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("imie"),
                                LastName = reader.GetString("nazwisko"),
                                Login = reader.GetString("login"),
                                

                                Address = reader.IsDBNull(reader.GetOrdinal("adres_id"))

                                    ? null
                                    : new Address
                                    {
                                        
                                        
                                        City = reader.GetString("miasto"),
                                        

                                    }

                            };

                            races.Add(race);
                        }
                    }
                }
            }
            return races;
        }

        public async Task<bool> JoinClub(int id, int userId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"UPDATE uzytkownicy SET klub_id = {id} WHERE Id={userId} ";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> LeaveClub( int userId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = $"UPDATE uzytkownicy SET klub_id = null WHERE Id={userId} ";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
        }
    }
}
