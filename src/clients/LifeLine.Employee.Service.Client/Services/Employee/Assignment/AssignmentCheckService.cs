using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Client.Services.Employee.Assignment
{
    public sealed class AssignmentCheckService(HttpClient httpClient) : IAssignmentCheckService
    {
        public async Task<Result> HasActiveAssignmentsToDepartmentAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync($"api/assignments/checks/department/{departmentId}", cancellationToken);
            response.EnsureSuccessStatusCode();

            return Result.Success();
        }

        public async Task<Result> HasActiveAssignmentsToPositionAsync(Guid positionId, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync($"api/assignments/checks/position/{positionId}", cancellationToken);
            response.EnsureSuccessStatusCode();

            return Result.Success();
        }
    }
}
