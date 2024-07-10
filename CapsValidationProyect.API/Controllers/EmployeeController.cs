using CapsValidationProyect.Application.Commands.Employee;
using CapsValidationProyect.Application.Queries.Employee;
using CapsValidationProyect.Persistence.DapperConnection.Employee;
using CapsValidationProyect.Persistence.DapperConnection.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapsValidationProyect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<PaginationModel>> GetEmployees()
        {
            return await Mediator.Send(new Query.ListQuery());
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(New.Command data)
        {
            return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Update(int id, Update.Execute data)
        {
            data.DepartmentId = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new Delete.Execute { Id = id });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeModel>> GetById(Guid id)
        {
            return await Mediator.Send(new QueryId.Execute { Id = id });
        }
    }
}
