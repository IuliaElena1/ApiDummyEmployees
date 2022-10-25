using DummyEndpoints.Models;

namespace DummyEndpoints
{


    [TestClass]
    public class EmployeeTests
    {

        /// <summary>
        /// Successfully retrieves all employees and counts the number of employees with age number higher than 30
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestEmployee()
        {
            var newEmployee = new EmployeeCreation()
            {
                employee_name = "Maria Ioana",
                employee_salary = "12080",
                employee_age = "31"
            };

            // Successfully retrieves all employees and counts the number of employees with age number higher than 30
            var initialEmployees = await CountEmployeesGraterThan(30);

            // successfully adds new employee with age higher than 30 and assert that operation is successful
            var responseCreate = await HttpClientMethods.CreateEmployee(newEmployee);
            Assert.AreEqual(responseCreate.status, "success");
            var employeeId = responseCreate.data.id;
            var response = await HttpClientMethods.GetEmployeesResponse();
            var employee = response.data.FirstOrDefault(e => e.id == employeeId);
            Assert.IsNotNull(employee);
            Assert.AreEqual(newEmployee.employee_age, employee.employee_age.ToString());

            // successfully updates the employee and asserts that operation is successful
            newEmployee.employee_salary = "15000";
            var responseUpdate = await HttpClientMethods.UpdateEmployee(employeeId, newEmployee);
            Assert.AreEqual(responseUpdate.status, "success");
            Assert.AreEqual(responseUpdate.data.employee_salary, "15000");

            // successfully retrieves all employees and asserts that employees with age number higher than 30 has modified
            var finalEmployees = await CountEmployeesGraterThan(30);
            Assert.AreEqual(finalEmployees - initialEmployees, 1);

            // successfully deletes the employee that he added and asserts the operation is successful
            var responseDelete = await HttpClientMethods.DeleteEmployee(employeeId);
            Assert.AreEqual(responseDelete.message, "successfully! deleted Records");

            var afterDelete = await CountEmployeesGraterThan(30);
            Assert.AreEqual(initialEmployees, afterDelete);
        }

        private static async Task<int> CountEmployeesGraterThan(int age)
        {
            var response = await HttpClientMethods.GetEmployeesResponse();
            Assert.AreEqual(response.status, "success");

            var employees = response.data;
            Assert.IsNotNull(employees);
            return employees.Where(e => e.employee_age > age).ToList().Count;
        }



    }
}