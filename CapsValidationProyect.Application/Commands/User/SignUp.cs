using FluentValidation;
using CapsValidationProyect.Application.DTO.User;
using CapsValidationProyect.Application.Exceptions;
using CapsValidationProyect.Application.Interfaces.User;
using CapsValidationProyect.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CapsValidationProyect.Persistence.CapsValidationProyect.Domain.Models;

namespace CapsValidationProyect.Application.Commands.User
{
    public class SingUp
    {
        public class CommandSignUp : IRequest<UserDTO>
        {
            public string DisplayName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }

            public string Username { get; set; }
        }

        public class ExecuteValidador : AbstractValidator<CommandSignUp>
        {
            public ExecuteValidador()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
            }
        }



        public class Handler : IRequestHandler<CommandSignUp, UserDTO>
        {
            private readonly CapsTestContext _context;
            private readonly UserManager<EmployeeUser> _userManager;
            private readonly IJwtGenerator _jwtGenerador;
            public Handler(CapsTestContext context, UserManager<EmployeeUser> userManager, IJwtGenerator jwtGenerador)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
            }

            public async Task<UserDTO> Handle(CommandSignUp request, CancellationToken cancellationToken)
            {
                var existe = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();
                if (existe)
                    throw new HandlerException(HttpStatusCode.BadRequest, new { mensaje = "Existe ya un usuario registrado con este email" });

                var existeUserName = await _context.Users.Where(x => x.UserName == request.Username).AnyAsync();
                if (existeUserName)
                    throw new HandlerException(HttpStatusCode.BadRequest, new { mensaje = "Existe ya un usuario con este username" });


                var usuario = new EmployeeUser
                {
                    FullName = request.DisplayName,
                    Email = request.Email,
                    UserName = request.Username,
                    AddedDate = DateTime.UtcNow
                };

                var resultado = await _userManager.CreateAsync(usuario, request.Password);
                if (resultado.Succeeded)
                {
                    return new UserDTO
                    {
                        DisplayName = usuario.FullName,
                        Token = _jwtGenerador.CreateToken(usuario, null),
                        UserName = usuario.UserName,
                        Email = usuario.Email
                    };
                }



                throw new Exception("No se pudo agregar al nuevo usuario");
            }
        }
    }
}
