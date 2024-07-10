using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace CapsValidationProyect.Persistence.DapperConnection.Employee
{
    public class EmployeeRepository : IEmployee
    {
        private readonly IFactoryConnection _factoryConnection;

        public EmployeeRepository(IFactoryConnection factoryConnection)
        {
            _factoryConnection = factoryConnection;
        }

        public async Task<int> Update(int departmentId, string firstName, string middleName, string lastName, string mothersLastName)
        {
            var storeProcedure = "usp_employee_edit";
            try
            {

                var connection = _factoryConnection.GetConnection();
                var results = await connection.ExecuteAsync(
                    storeProcedure,
                    new
                    {
                        DepartmentId = departmentId,
                        FirstName = firstName,
                        MiddleName = middleName,
                        LastName = lastName,
                        MothersLastName = mothersLastName,
                        CurrentUser = "Test"
                    },
                    commandType: CommandType.StoredProcedure
                );

                _factoryConnection.CloseConnection();
                return results;

            }
            catch (Exception e)
            {
                throw new Exception("Could not edit Employee data", e);
            }

        }

        public async Task<int> Delete(Guid id)
        {
            var storeProcedure = "usp_delete_employee";
            try
            {
                var connection = _factoryConnection.GetConnection();
                var result = await connection.ExecuteAsync(
                    storeProcedure,
                    new
                    {
                        EmployeeId = id,
                        CurrentUser = "Test"
                    },
                    commandType: CommandType.StoredProcedure
                );
                _factoryConnection.CloseConnection();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Employee could not be deleted", e);
            }
        }

        public async Task<int> Create(int departmentId, string firstName, string middleName, string lastName, string mothersLastName)
        {
            var storeProcedure = "usp_create_employee";

            try
            {
                var connection = _factoryConnection.GetConnection();
                var result = await connection.ExecuteAsync(
                storeProcedure,
                new
                {
                    DepartmentId = departmentId,
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    MothersLastName = mothersLastName,
                    CurrentUser = "Test"
                },
                commandType: CommandType.StoredProcedure
                );

                _factoryConnection.CloseConnection();

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("New Employee could not be saved", e);
            }


        }

        public async Task<IEnumerable<EmployeeModel>> GetList()
        {
            IEnumerable<EmployeeModel> EmployeeList = null;
            var storeProcedure = "usp_get_employees";
            try
            {
                var connection = _factoryConnection.GetConnection();
                EmployeeList = await connection.QueryAsync<EmployeeModel>(storeProcedure, null, commandType: CommandType.StoredProcedure);

            }
            catch (Exception e)
            {
                throw new Exception("Error in data query", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }
            return EmployeeList;
        }

        public async Task<EmployeeModel> GetById(Guid id)
        {
            var storeProcedure = "usp_get_employee_by_id";
            EmployeeModel Employee = null;
            try
            {
                var connection = _factoryConnection.GetConnection();
                Employee = await connection.QueryFirstAsync<EmployeeModel>(
                    storeProcedure,
                    new
                    {
                        Id = id
                    },
                    commandType: CommandType.StoredProcedure
                );

                return Employee;

            }
            catch (Exception e)
            {
                throw new Exception("Employee not found", e);
            }


        }
    }
}
