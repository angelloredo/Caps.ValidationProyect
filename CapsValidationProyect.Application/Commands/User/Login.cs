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
    public class Login
    {
        public class CommandLogin : IRequest<UserDTO>
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class ExecuteValidation : AbstractValidator<CommandLogin>
        {
            public ExecuteValidation()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<CommandLogin, UserDTO>
        {

            private readonly UserManager<EmployeeUser> _userManager;
            private readonly SignInManager<EmployeeUser> _signInManager;
            private readonly IJwtGenerator _jwtGenerador;

            private readonly CapsTestContext _context;
            public Handler(UserManager<EmployeeUser> userManager, SignInManager<EmployeeUser> signInManager, IJwtGenerator jwtGenerador, CapsTestContext context)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerador = jwtGenerador;
                _context = context;
            }
            public async Task<UserDTO> Handle(CommandLogin request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email);
                if (usuario == null)
                {
                    throw new HandlerException(HttpStatusCode.Unauthorized);
                }

                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);
                var resultadoRoles = await _userManager.GetRolesAsync(usuario);
                var listaRoles = new List<string>(resultadoRoles);



                var imagenPerfil = usuario.PorfilePhoto;

                if (resultado.Succeeded)
                {
                    if (imagenPerfil != null)
                    {
                        var imagenCliente = new GeneralImageDTO
                        {
                            Data = Convert.ToBase64String(imagenPerfil),
                            Extension = usuario.FileExt,
                            Name = usuario.FileName
                        };
                        return new UserDTO
                        {
                            DisplayName = usuario.FileName,
                            Token = _jwtGenerador.CreateToken(usuario, listaRoles),
                            UserName = usuario.UserName ?? "",
                            Email = usuario.Email ?? "",
                            ProfileImage = imagenCliente
                        };
                    }
                    else
                    {
                        return new UserDTO
                        {
                            DisplayName = usuario.FullName,
                            Token = _jwtGenerador.CreateToken(usuario, listaRoles),
                            UserName = usuario.UserName ?? "",
                            Email = usuario.Email ?? "",
                            Image = ""
                        };
                    }
                }

                throw new HandlerException(HttpStatusCode.Unauthorized);
            }
        }

    }
}
