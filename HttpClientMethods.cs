using DummyEndpoints.Models;
using DummyEndpoints.Reusable;
using Newtonsoft.Json;
using System.Text;

namespace DummyEndpoints
{
    internal static class HttpClientMethods
    {
        private static HttpClient _httpClient = new HttpClient();
        private static readonly string BaseUrl = "http://localhost:3000/api/v1";




        public static async Task<ResponseModel> GetEmployeesResponse()
        {
            var httpResponse = await _httpClient.GetAsync($"{BaseUrl}/employees");


            if (httpResponse.IsSuccessStatusCode == false)
            {
                return new ResponseModel { status = "not success", data = new List<Employee>(), message = "Not all records has been fetched." };
            }

            var employees = httpResponse.Content.ReadAsStringAsync().Result;
            var employeesAsJson = ReusableFunctions.GetJsonDeserialized<List<Employee>>(employees);

            return new ResponseModel { status = "success", data = employeesAsJson, message = "Successfully! All records has been fetched." }; ;
        }



        public static async Task<ResponseEmployeeCreation> CreateEmployee(EmployeeCreation newEmployee)
        {

            using var httpResponse = await _httpClient.PostAsync($"{BaseUrl}/create", new StringContent(JsonConvert.SerializeObject(newEmployee), Encoding.UTF8, "application/json"));

            if (httpResponse.IsSuccessStatusCode == false)
            {
                return new ResponseEmployeeCreation { status = "not success", data = new EmployeeCreation() };
            }

            var employee = httpResponse.Content.ReadAsStringAsync().Result;
            var employeesAsJson = ReusableFunctions.GetJsonDeserialized<EmployeeCreation>(employee);


            return new ResponseEmployeeCreation { status = "success", data = employeesAsJson };

        }

        public static async Task<ResponseForUpdate> UpdateEmployee(int employeeId, EmployeeCreation newEmployee)
        {
            using var httpResponse = await _httpClient.PutAsync($"{BaseUrl}/update/{employeeId}", new StringContent(JsonConvert.SerializeObject(newEmployee), Encoding.UTF8, "application/json"));
            if (httpResponse.IsSuccessStatusCode == false)
            {
                return new ResponseForUpdate { status = "not success", data = new EmployeeCreation() };
            }

            var employee = httpResponse.Content.ReadAsStringAsync().Result;
            var employeesAsJson = ReusableFunctions.GetJsonDeserialized<EmployeeCreation>(employee);

            return new ResponseForUpdate { status = "success", data = employeesAsJson };
        }


        public static async Task<DeleteEmployeeResponse> DeleteEmployee(int employeeId)
        {
            using var httpResponse = await _httpClient.DeleteAsync($"{BaseUrl}/delete/{employeeId}");
            if (httpResponse.IsSuccessStatusCode == false)
            {
                return new DeleteEmployeeResponse { status = "not success", message = "not successfully! deleted Records" };
            }

            return new DeleteEmployeeResponse { status = "success", message = "successfully! deleted Records" };
        }
    }
}
