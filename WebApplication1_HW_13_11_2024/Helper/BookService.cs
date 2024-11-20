using Microsoft.Data.SqlClient;

namespace WebApplication1_HW_13_11_2024.Helper
{
    public class BookService
    {
        private readonly string _connectionString;

        public BookService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            var books = new List<Book>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("SELECT Id, Title, Category FROM Books", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        books.Add(new Book
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Category = reader.GetString(2)
                        });
                    }
                }
            }

            return books;
        }

        public async Task<List<Book>> GetBooksByCategoryAsync(string category)
        {
            var books = new List<Book>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("SELECT Id, Title, Category FROM Books WHERE Category = @Category", connection);
                command.Parameters.AddWithValue("@Category", category);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        books.Add(new Book
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Category = reader.GetString(2)
                        });
                    }
                }
            }

            return books;
        }
    }
}
