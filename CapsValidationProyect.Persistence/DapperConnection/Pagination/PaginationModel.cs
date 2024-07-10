using System.Collections.Generic;
namespace CapsValidationProyect.Persistence.DapperConnection.Pagination
{
    public class PaginationModel
    {
        public List<IDictionary<string,object>> RecordList {get;set;}
        public int TotalRecords {get;set;}
        public int TotalPages {get;set;}
    }
}