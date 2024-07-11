using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Linq;
using CapsValidationProyect.Persistence.DapperConnection.Employee;
using CapsValidationProyect.Persistence.DapperConnection.Pagination;

namespace CapsValidationProyect.Application.Queries.Employee
{
    public class Query
    {
        public class ListQuery : IRequest<PaginationModel>
        {
            public string Name { get; set; }
            public int NumeroPagina { get; set; }
            public int CantidadElementos { get; set; }
        }

        public class Handler : IRequestHandler<ListQuery, PaginationModel>
        {
            private readonly IPagination _Pagination;
            public Handler(IPagination Pagination)
            {
                _Pagination = Pagination;
            }

            public async Task<PaginationModel> Handle(ListQuery request, CancellationToken cancellationToken)
            {
                var storedProcedure = "usp_get_employee_pagination";
                var ordenamiento = "FullName";
                var parametros = new Dictionary<string, object>();
                return await _Pagination.ReturnPagination(storedProcedure, request.NumeroPagina, request.CantidadElementos, parametros, ordenamiento);


            }
        }

    }
}