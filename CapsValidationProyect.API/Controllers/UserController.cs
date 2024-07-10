using CapsValidationProyect.Application.Commands.User;
using CapsValidationProyect.Application.DTO.User;
using CapsValidationProyect.Application.Queries.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;

namespace CapsValidationProyect.API.Controllers
{
    [AllowAnonymous]
    public class UserController : BaseController
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(Login.CommandLogin parameters)
        {
            return await Mediator.Send(parameters);
        }

        // http://localhost:5000/api/Usuario/registrar
        [HttpPost("SingUp")]
        public async Task<ActionResult<UserDTO>> SingUp(SingUp.CommandSignUp parameters)
        {
            return await Mediator.Send(parameters);
        }

        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetUser()
        {
            return await Mediator.Send(new CurrentUser.Execute());
        }

        [HttpPut]
        public async Task<ActionResult<UserDTO>> Update(UserUpdate.CommandUpdateUser parameters)
        {
            return await Mediator.Send(parameters);
        }
    }
}
