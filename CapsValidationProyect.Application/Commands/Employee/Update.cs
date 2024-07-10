using System.Data;
using System;
using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using CapsValidationProyect.Persistence.DapperConnection.Employee;

namespace CapsValidationProyect.Application.Commands.Employee
{
    public class Update
    {
        public class Execute : IRequest<Unit>
        {
            public int DepartmentId { get; set; }

            public string FirstName { get; set; } = null!;

            public string MiddleName { get; set; } = null!;

            public string LastName { get; set; } = null!;

            public string MothersLastName { get; set; } = null!;
        }

        public class ExecuteValida : AbstractValidator<Execute>
        {
            public ExecuteValida()
            {
                RuleFor(x => x.DepartmentId).NotEmpty();
                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty();
            }
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
                var resultado = await _EmployeeRepositorio.Update(request.DepartmentId, request.FirstName, request.MiddleName, request.LastName, request.MothersLastName);
                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo Update la data del Employee");
            }
        }

    }
}