using Shared.Contracts.Request.EmployeeService.Assignment;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using Shared.Kernel.Results;
using System.Net.Http.Json;

namespace LifeLine.Employee.Service.Client.Services.Employee.Assignment
{
    public sealed class AssignmentService(HttpClient httpClient, string employeeId) :
        BaseHttpService<AssignmentResponse, string>(httpClient, $"api/employees/{employeeId}/assignments"), IAssignmentService
    {
        public async Task<Result> UpdateAssignmentAsync(Guid assignmentId, Guid contractId, UpdateAssignmentRequest request)
        {
            try
            {
                var response = await HttpClient.PatchAsJsonAsync($"{Url}/{assignmentId}/{contractId}", request, JsonSerializerOptions);

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(ErrorCode.UpdateHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.UpdateHttp, $"Произошла ошибка при изменении данных в назначении!\n{ex}"));
            }
        }
    }
}
