using Microsoft.Extensions.Options;
using Shared.Contracts.Request.EmployeeService.ContactInformation;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using Shared.Kernel.Errors;
using System.Net.Http.Json;
using System.Text.Json;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Client.Services.Employee.ContactInformation
{
    internal sealed class ContactInformationService(HttpClient httpClient, string employeeId, IOptions<JsonSerializerOptions> options)
        : BaseHttpService<ContactInformationResponse, string>(httpClient, $"api/employees/{employeeId}/contact-informations", options.Value), IContactInformationService
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
