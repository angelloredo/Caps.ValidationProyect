using CapsValidationProyect.Persistence.DapperConnection.Employee;
using MediatR;

namespace CapsValidationProyect.Application.Commands.Employee
{
    public class Delete
    {
        public class Execute : IRequest<Unit>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Execute, Unit>
        {
            private readonly IEmployee _EmployeeRepositorio;
            public Handler(IEmployee EmployeeRepositorio)
            {
                _EmployeeRepositorio = EmployeeRepositorio;
            }

            public async Task<Unit> Handle(Execute request, CancellationToken cancellationToken)
            {
                var resultados = await _EmployeeRepositorio.Delete(request.Id);
                if (resultados > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo Delete el Employee");
            }
        }

    }
}