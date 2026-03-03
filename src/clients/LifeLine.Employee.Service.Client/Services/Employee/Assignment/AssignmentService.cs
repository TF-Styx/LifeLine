using Shared.Contracts.Request.EmployeeService.Assignment;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using System.Net.Http.Json;
using Terminex.Common.Results;
using Shared.Kernel.Errors;

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
                    return Result.Failure(new Error(AppErrors.UpdateHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(AppErrors.UpdateHttp, $"Произошла ошибка при изменении данных в назначении!\n{ex}"));
            }
        }

        public async Task<Result> DeleteAssignmentContractAsync(Guid assignmentId)
        {
            try
            {
                var response = await HttpClient.DeleteAsync($"{Url}/{assignmentId}");

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(AppErrors.DeleteHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(AppErrors.DeleteHttp, $"Произошла ошибка при удалении назначения!\n{ex}"));
            }
        }
    }
}
