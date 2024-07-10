using System.Data;
namespace CapsValidationProyect.Persistence.DapperConnection
{
    public interface IFactoryConnection
    {
         void CloseConnection();
         IDbConnection GetConnection();
    }
}