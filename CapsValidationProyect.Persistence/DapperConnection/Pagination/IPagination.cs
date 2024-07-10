using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapsValidationProyect.Persistence.DapperConnection.Pagination
{
    public interface IPagination
    {
         Task<PaginationModel> ReturnPagination(
            string storeProcedure, 
            int PageNumber, 
            int ElementsQty, 
            IDictionary<string,object> parametersFilters,
            string sortingColumn
            );
    }
}