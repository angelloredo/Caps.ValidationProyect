using System.Data;
using CapsValidationProyect.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace CapsValidationProyect.Persistence.DapperConnection
{
    public class FactoryConnection : IFactoryConnection
    {
        private IDbConnection _connection;
        private readonly IOptions<AppSettings> _configs;
        public FactoryConnection(IOptions<AppSettings> configs){
             _configs = configs;
        }
        public void CloseConnection()
        {
            if(_connection != null && _connection.State == ConnectionState.Open){
                _connection.Close();
            }
        }

        public IDbConnection GetConnection()
        {
            if(_connection == null){
                _connection = new SqlConnection(_configs.Value.ConnectionString);
            }
            if(_connection.State != ConnectionState.Open){
                _connection.Open();
            }
            return _connection;
        }
    }
}