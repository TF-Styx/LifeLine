using Shared.Contracts.Request.EmployeeService.EducationDocument;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using Shared.Kernel.Results;
using System.Net.Http.Json;

namespace LifeLine.Employee.Service.Client.Services.Employee.EducationDocument
{
    internal sealed class EducationDocumentService(HttpClient httpClient, string employeeId) 
        : BaseHttpService<EducationDocumentResponse, string>(httpClient, $"api/employees/{employeeId}/education-documents"), IEducationDocumentService
    {
        public async Task<Result> UpdateEducationDocumentAsync(Guid educationDocumentId, UpdateEducationDocumentRequest request)
        {
            try
            {
                var response = await HttpClient.PatchAsJsonAsync($"{Url}/{educationDocumentId}", request, JsonSerializerOptions);

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(ErrorCode.UpdateHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.UpdateHttp, $"Произошла ошибка при изменении данных в послеучебных документах!\n{ex}"));
            }
        }

        public async Task<Result> DeleteEducationDocumentAsync(Guid educationDocumentId)
        {
            try
            {
                var response = await HttpClient.DeleteAsync($"{Url}/{educationDocumentId}");

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(ErrorCode.DeleteHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.DeleteHttp, $"Произошла ошибка при удалении послеучебного документа!\n{ex}"));
            }
        }
    }
}
