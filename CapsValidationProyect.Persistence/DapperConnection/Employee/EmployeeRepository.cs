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

        public async Task<int> Update(int id, int departmentId, string firstName, string middleName, string lastName, string mothersLastName)
        {
            var storeProcedure = "usp_update_employee";
            try
            {
                using var connection = _factoryConnection.GetConnection();
                var parameters = new DynamicParameters();
                parameters.Add("Id", id);
                parameters.Add("DepartmentId", departmentId);
                parameters.Add("FirstName", firstName);
                parameters.Add("MiddleName", middleName);
                parameters.Add("LastName", lastName);
                parameters.Add("MothersLastName", mothersLastName);
                parameters.Add("CurrentUser", "Test");
                parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await connection.ExecuteAsync(
                    storeProcedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                var returnValue = parameters.Get<int>("ReturnValue");
                _factoryConnection.CloseConnection();
                return returnValue;
            }
            catch (Exception e)
            {
                throw new Exception("Could not edit Employee data", e);
            }
        }


        public async Task<int> Delete(int id)
        {
            var storeProcedure = "usp_delete_employee";

            try
            {
                var connection = _factoryConnection.GetConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                parameters.Add("@CurrentUser", "Test");
                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue); // Parameter for return value

                await connection.ExecuteAsync(storeProcedure, parameters, commandType: CommandType.StoredProcedure);

                var result = parameters.Get<int>("@ReturnValue"); // Get the return value from the stored procedure

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
                var parameters = new DynamicParameters();
                parameters.Add("@DepartmentId", departmentId);
                parameters.Add("@FirstName", firstName);
                parameters.Add("@MiddleName", middleName);
                parameters.Add("@LastName", lastName);
                parameters.Add("@MothersLastName", mothersLastName);
                parameters.Add("@CurrentUser", "Test");
                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue); // Parameter for return value

                await connection.ExecuteAsync(storeProcedure, parameters, commandType: CommandType.StoredProcedure);

                var result = parameters.Get<int>("@ReturnValue"); // Get the return value from the stored procedure

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

        public async Task<EmployeeModel> GetById(int id)
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
