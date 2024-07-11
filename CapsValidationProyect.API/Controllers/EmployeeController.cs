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
    //[AllowAnonymous]
    public class EmployeeController : BaseController
    {
        [HttpPost("GetEmployeePagination")]
        public async Task<ActionResult<PaginationModel>> GetEmployees(Query.ListQuery data)
        {
            return await Mediator.Send(data);
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(New.Command data)
        {
            return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Update(int id, Update.Execute data)
        {

            data.Id = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            return await Mediator.Send(new Delete.Execute { Id = id });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeModel>> GetById(int id)
        {
            return await Mediator.Send(new QueryId.Execute { Id = id });
        }
    }
}
