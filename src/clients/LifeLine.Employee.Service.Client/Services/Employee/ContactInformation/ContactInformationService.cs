using Shared.Contracts.Request.EmployeeService.ContactInformation;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using System.Net.Http.Json;
using Terminex.Common.Results;
using Shared.Kernel.Errors;

namespace LifeLine.Employee.Service.Client.Services.Employee.ContactInformation
{
    internal sealed class ContactInformationService(HttpClient httpClient, string employeeId)
        : BaseHttpService<ContactInformationResponse, string>(httpClient, $"api/employees/{employeeId}/contact-informations"), IContactInformationService
    {
        public async Task<Result> UpdateContactInformationAsync(UpdateContactInformationRequest request)
        {
            try
            {
                var response = await HttpClient.PatchAsJsonAsync($"{Url}", request, JsonSerializerOptions);

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(AppErrors.UpdateHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(AppErrors.UpdateHttp, $"Произошла ошибка при изменении данных в контактной информации!\n{ex}"));
            }
        }
    }
}
