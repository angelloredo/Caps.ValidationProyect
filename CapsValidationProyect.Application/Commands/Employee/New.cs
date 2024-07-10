using System;
using System.Threading;
using System.Threading.Tasks;
using CapsValidationProyect.Persistence.DapperConnection.Employee;
using FluentValidation;
using MediatR;

namespace CapsValidationProyect.Application.Commands.Employee
{
    public class New
    {
        public class Command : IRequest<Unit>
        {
            public int DepartmentId { get; set; }

            public string FirstName { get; set; } = null!;

            public string MiddleName { get; set; } = null!;

            public string LastName { get; set; } = null!;

            public string MothersLastName { get; set; } = null!;
        }

        public class ExecuteValidator : AbstractValidator<Command>
        {
            public ExecuteValidator()
            {
                RuleFor(x => x.DepartmentId).NotEmpty();
                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IEmployee _EmployeeRepository;
            public Handler(IEmployee EmployeeRepository)
            {
                _EmployeeRepository = EmployeeRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {

                var result = await _EmployeeRepository.Create(request.DepartmentId, request.FirstName, request.MiddleName, request.LastName, request.MothersLastName);

                if (result > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("The Employee could not be inserted");
            }

            
        }
    }
}
