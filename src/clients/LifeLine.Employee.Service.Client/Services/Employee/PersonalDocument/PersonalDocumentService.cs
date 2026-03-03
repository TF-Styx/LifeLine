using Shared.Contracts.Request.EmployeeService.PersonalDocument;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using System.Net.Http.Json;
using Terminex.Common.Results;
using Shared.Kernel.Errors;

namespace LifeLine.Employee.Service.Client.Services.Employee.PersonalDocument
{
    internal sealed class PersonalDocumentService(HttpClient httpClient, string employeeId)
        : BaseHttpService<PersonalDocumentResponse, string>(httpClient, $"api/employees/{employeeId}/personal-documents"), IPersonalDocumentService
    {
        public async Task<Result> UpdatePersonalDocumentAsync(Guid personalDocumentId, UpdatePersonalDocumentRequest request)
        {
            try
            {
                var response = await HttpClient.PatchAsJsonAsync($"{Url}/{personalDocumentId}", request, JsonSerializerOptions);

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(AppErrors.UpdateHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(AppErrors.UpdateHttp, $"Произошла ошибка при изменении данных в персональных документах!\n{ex}"));
            }
        }

        public async Task<Result> DeletePersonalDocumentAsync(Guid personalDocumentId)
        {
            try
            {
                var response = await HttpClient.DeleteAsync($"{Url}/{personalDocumentId}");

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(AppErrors.DeleteHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(AppErrors.DeleteHttp, $"Произошла ошибка при удалении персонального документа!\n{ex}"));
            }
        }
    }
}
