using APBD_Zadanie_4.Models;
using APBD_Zadanie_4.DTOs;
using System.Data.SqlClient;

namespace APBD_Zadanie_4.Repositories
{
    public class AnimalRepo : IAnimalRepo
    {
        private readonly string _connectionString;
        public AnimalRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task AddAnimal(AnimalPost animalPost)
        {
            string query = $"INSERT INTO [dbo].[Animal] ([ID], [Name], [Description], [Category], [Area]) VALUES (@ID, @Name, @Description, @Category, @Area)";

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.Parameters.AddWithValue("@ID", animalPost.ID);
                sqlCommand.Parameters.AddWithValue("@Name", animalPost.Name);
                sqlCommand.Parameters.AddWithValue("@Description", animalPost.Description);
                sqlCommand.Parameters.AddWithValue("@Category", animalPost.Category);
                sqlCommand.Parameters.AddWithValue("@Area", animalPost.Area);

                await connection.OpenAsync();

                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> DoesExist(int animalID)
        {
            string query = $"SELECT COUNT(*) FROM [dbo].[Animal] WHERE [ID] = {animalID}";

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(query, connection);

                await connection.OpenAsync();

                int count = (int)await sqlCommand.ExecuteScalarAsync();

                return count > 0;
            }
        }

        public async Task UpdateAnimal(int animalID, AnimalPut animalPut)
        {
            string query = $"UPDATE [dbo].[Animal] SET [Name] = @Name, [Description] =  @Description, [Category] = @Category, [Area] = @Area WHERE [ID] = {animalID}";

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.Parameters.AddWithValue("@Name", animalPut.Name);
                sqlCommand.Parameters.AddWithValue("@Description", animalPut.Description);
                sqlCommand.Parameters.AddWithValue("@Category", animalPut.Category);
                sqlCommand.Parameters.AddWithValue("@Area", animalPut.Area);

                await connection.OpenAsync();

                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        public async Task<ICollection<Animal>> GetSync(string orderBy)
        {
            string query = $"SELECT * FROM Animal ORDER BY {orderBy}";
            var animals = new List<Animal>();

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                await connection.OpenAsync();

                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    int ID = reader.GetOrdinal("ID");
                    int Name = reader.GetOrdinal("Name");
                    int Description = reader.GetOrdinal("Description");
                    int Category = reader.GetOrdinal("Category");
                    int Area = reader.GetOrdinal("Area");

                    while(await reader.ReadAsync())
                    {
                        animals.Add(new Animal
                        {
                            ID = reader.GetInt32(ID),
                            Name = reader.GetString(Name),
                            Description = reader.GetString(Description),
                            Category = reader.GetString(Category),
                            Area = reader.GetString(Area)
                        });
                    }
                }
            }

            return animals;
        }

        public async Task DeleteAnimal(int animalId)
        {
            string query = $"DELETE FROM [dbo].[Animal] WHERE [ID] = {animalId}";

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(query, connection);

                await connection.OpenAsync();

                await sqlCommand.ExecuteNonQueryAsync();
            }
        }
    }
}