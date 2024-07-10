using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapsValidationProyect.Persistence.DapperConnection.Employee
{
    public interface IEmployee
    {
        Task<IEnumerable<EmployeeModel>> GetList();

        Task<EmployeeModel> GetById(Guid id);

        Task<int> Create(int departmentId, string firstName, string middleName, string lastName, string mothersLastName);

        Task<int> Update(int departmentId, string firstName, string middleName, string lastName, string mothersLastName);

        Task<int> Delete(Guid id);
    }
}
