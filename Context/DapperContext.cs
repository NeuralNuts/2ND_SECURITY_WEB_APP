using System.Data;
using System.Data.SqlClient;

namespace _2ND_SECURITY_WEB_APP.Context
{
    public class DapperContext
    {
        #region Dapper instalized
        private readonly IConfiguration _conConfig;
        private readonly string? _connectionString;
        #endregion

        #region Gets database connection string for dapper
        public DapperContext(IConfiguration conConfig)
        {
            _conConfig = conConfig;
            _connectionString = _conConfig.GetConnectionString("SqlConnection");
        }
        #endregion
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
