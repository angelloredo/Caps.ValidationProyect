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
    public class UserUpdate
    {
        public class CommandUpdateUser : IRequest<UserDTO>
        {
            public string DisplayName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public GeneralImageDTO? ProfileImage { get; set; }
        }

        public class CommandValidator : AbstractValidator<CommandUpdateUser>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<CommandUpdateUser, UserDTO>
        {
            private readonly CapsTestContext _context;
            private readonly UserManager<EmployeeUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IPasswordHasher<EmployeeUser> _passwordHasher;

            public Handler(CapsTestContext context, UserManager<EmployeeUser> userManager, IJwtGenerator jwtGenerator, IPasswordHasher<EmployeeUser> passwordHasher)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _passwordHasher = passwordHasher;
            }

            public async Task<UserDTO> Handle(CommandUpdateUser request, CancellationToken cancellationToken)
            {
                var userIdentity = await _userManager.FindByNameAsync(request.UserName);
                if (userIdentity == null)
                {
                    throw new HandlerException(HttpStatusCode.NotFound, new { message = "There is no user with this username" });
                }

                var emailExists = await _context.Users.Where(x => x.Email == request.Email && x.UserName != request.UserName).AnyAsync();
                if (emailExists)
                {
                    throw new HandlerException(HttpStatusCode.InternalServerError, new { message = "This email belongs to another user" });
                }

                if (request.ProfileImage != null)
                {

                    userIdentity.PorfilePhoto = Convert.FromBase64String(request.ProfileImage.Data);
                    userIdentity.FileName = request.ProfileImage.Name;
                    userIdentity.FileExt = request.ProfileImage.Extension;
                    userIdentity.LastUpdatedDate = DateTime.UtcNow;

                }

                userIdentity.FullName = request.DisplayName;
                userIdentity.PasswordHash = _passwordHasher.HashPassword(userIdentity, request.Password);
                userIdentity.Email = request.Email;

                var updateResult = await _userManager.UpdateAsync(userIdentity);

                var userRoles = await _userManager.GetRolesAsync(userIdentity);
                var rolesList = new List<string>(userRoles);

                var userProfileImage = userIdentity.PorfilePhoto;
                GeneralImageDTO profileImage = null;
                if (userProfileImage != null)
                {
                    profileImage = new GeneralImageDTO
                    {
                        Data = Convert.ToBase64String(userProfileImage),
                        Name = userIdentity.FileName,
                        Extension = userIdentity.FileExt
                    };
                }

                if (updateResult.Succeeded)
                {
                    return new UserDTO
                    {
                        DisplayName = userIdentity.FileName,
                        UserName = userIdentity.UserName,
                        Email = userIdentity.Email,
                        Token = _jwtGenerator.CreateToken(userIdentity, rolesList),
                        ProfileImage = profileImage
                    };
                }

                throw new Exception("The user could not be updated");
            }
        }
    }
}
