using Microsoft.Data.SqlClient;
using System.Data;

namespace SharedShoppingList.API.Data
{
    public class DapperContext
    {
        private readonly IConfiguration configuration;

        public DapperContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(configuration.GetConnectionString("Default"));
        }
    }
}
