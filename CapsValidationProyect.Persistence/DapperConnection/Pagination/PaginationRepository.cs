using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using CapsValidationProyect.Persistence.DapperConnection.Pagination;

namespace CapsValidationProyect.Persistence.DapperConnection.Pagination
{
    public class PaginationRepository : IPagination
    {
        private readonly IFactoryConnection _factoryConnection;
        public PaginationRepository(IFactoryConnection factoryConnection)
        {
            _factoryConnection = factoryConnection;
        }

        public async Task<PaginationModel> ReturnPagination(string storedProcedure, int pageNumber, int pageSize, IDictionary<string, object> filterParams, string sortingColumn)
        {
            PaginationModel paginationModel = new PaginationModel();
            List<IDictionary<string, object>> reportList = null;
            int totalRecords = 0;
            int totalPages = 0;
            try
            {
                var connection = _factoryConnection.GetConnection();
                DynamicParameters parameters = new DynamicParameters();

                foreach (var param in filterParams)
                {
                    parameters.Add("@" + param.Key, param.Value);
                }

                parameters.Add("@PageNumber", pageNumber);
                parameters.Add("@PageSize", pageSize);
                parameters.Add("@Sorting", sortingColumn);

                parameters.Add("@TotalRecords", totalRecords, DbType.Int32, ParameterDirection.Output);
                parameters.Add("@TotalPages", totalPages, DbType.Int32, ParameterDirection.Output);

                var result = await connection.QueryAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                reportList = result.Select(x => (IDictionary<string, object>)x).ToList();
                paginationModel.RecordList = reportList;
                paginationModel.TotalPages = parameters.Get<int>("@TotalPages");
                paginationModel.TotalRecords = parameters.Get<int>("@TotalRecords");

            }
            catch (Exception e)
            {
                throw new Exception("Could not execute the stored procedure", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }

            return paginationModel;
        }
    }
}
