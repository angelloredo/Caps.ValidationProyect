using CapsValidationProyect.Application.DTO;
using CapsValidationProyect.Application.DTO.User;
using CapsValidationProyect.Application.Interfaces.User;
using CapsValidationProyect.Persistence;
using CapsValidationProyect.Persistence.CapsValidationProyect.Domain.Models;
using CapsValidationProyect.Persistence.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapsValidationProyect.Application.Queries.User
{
    public class CurrentUser
    {
        public class Execute : IRequest<UserDTO> { }

        public class Handler : IRequestHandler<Execute, UserDTO>
        {
            private readonly UserManager<EmployeeUser> _userManager;
            private readonly IJwtGenerator _jwtGenerador;
            private readonly IUserSerssion _usuarioSesion;

            private readonly CapsTestContext _context;
            public Handler(UserManager<EmployeeUser> userManager, IJwtGenerator jwtGenerador, IUserSerssion usuarioSesion, CapsTestContext context)
            {
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
                _usuarioSesion = usuarioSesion;
                _context = context;
            }
            public async Task<UserDTO> Handle(Execute request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(_usuarioSesion.GetUserSession());

                var resultadoRoles = await _userManager.GetRolesAsync(usuario);
                var listaRoles = new List<string>(resultadoRoles);

                var imagenPerfil = usuario.PorfilePhoto;
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
                        DisplayName = usuario.FullName,
                        UserName = usuario.UserName ?? "",
                        Token = _jwtGenerador.CreateToken(usuario, listaRoles),
                        Email = usuario.Email ?? "",
                        ProfileImage = imagenCliente
                    };
                }
                else
                {
                    return new UserDTO
                    {
                        DisplayName = usuario.FullName ?? "",
                        UserName = usuario.UserName ?? "",
                        Token = _jwtGenerador.CreateToken(usuario, listaRoles),
                        Email = usuario.Email ?? ""
                    };
                }
            }
        }
    }
}
