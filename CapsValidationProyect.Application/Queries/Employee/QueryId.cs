using System.Net;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using CapsValidationProyect.Persistence.DapperConnection.Employee;
using CapsValidationProyect.Application.Exceptions;

namespace CapsValidationProyect.Application.Queries.Employee
{
    public class QueryId
    {
        public class Execute : IRequest<EmployeeModel>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Execute, EmployeeModel>
        {
            private readonly IEmployee _EmployeeRepository;
            public Handler(IEmployee EmployeeRepository)
            {
                _EmployeeRepository = EmployeeRepository;
            }

            public async Task<EmployeeModel> Handle(Execute request, CancellationToken cancellationToken)
            {
                var Employee = await _EmployeeRepository.GetById(request.Id);
                if (Employee == null)
                {
                    throw new HandlerException(HttpStatusCode.NotFound, new { mensaje = "No se encontro el Employee" });
                }

                return Employee;
            }
        }

    }
}