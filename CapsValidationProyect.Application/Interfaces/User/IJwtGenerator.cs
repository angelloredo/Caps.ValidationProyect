
using CapsValidationProyect.Persistence.CapsValidationProyect.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapsValidationProyect.Application.Interfaces.User
{
    public interface IJwtGenerator
    {
        string CreateToken(EmployeeUser usuario, List<string> roles);
    }
}
