using Microsoft.Data.SqlClient;
using System.Data;

namespace api_zombies.Data
{
    public class DataContext
    {
        private readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection CreateConnection() => new SqlConnection(_configuration.GetConnectionString("AzureConnection"));
    }
}
