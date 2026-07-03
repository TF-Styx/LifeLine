using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Kernel.Errors;
using Terminex.Common.Results;

namespace Shared.Api.Extensions
{
    public static class ErrorMappingExtensions
    {
        public static IActionResult MapActionResult(this Controller controller, IReadOnlyList<Error> errors)
        {
            if (errors == null || errors.Count == 0)
                return controller.StatusCode(StatusCodes.Status500InternalServerError);

            var statusCode = errors[0].ErrorCode.Name switch
            {
                nameof(ErrorCode.Unknown) or
                nameof(ErrorCode.None) or
                nameof(ErrorCode.Null) or
                nameof(ErrorCode.Empty) or
                nameof(ErrorCode.Exist) or
                nameof(ErrorCode.NotExist) or
                nameof(ErrorCode.NotFound) or
                nameof(ErrorCode.InvalidRequest) or
                nameof(ErrorCode.InvalidResponse) => StatusCodes.Status404NotFound,

                nameof(ErrorCode.Save) or
                nameof(ErrorCode.Server) or
                nameof(ErrorCode.Create) or
                nameof(ErrorCode.Update) or
                nameof(ErrorCode.Delete) or
                nameof(AppErrors.CreateHttp) or
                nameof(AppErrors.UpdateHttp) or
                nameof(AppErrors.DeleteHttp) or
                nameof(ErrorCode.Connection) => StatusCodes.Status500InternalServerError,

                nameof(ErrorCode.Conflict) => StatusCodes.Status409Conflict,

                nameof(ErrorCode.Unauthorized) => StatusCodes.Status401Unauthorized,

                nameof(ErrorCode.Forbidden) => StatusCodes.Status403Forbidden,

                _ or 
                nameof(ErrorCode.BadRequest) or
                nameof(AppErrors.SRPVerificationFailed) or
                nameof(ErrorCode.Validation) => StatusCodes.Status400BadRequest,
            };

            return controller.StatusCode(statusCode, errors.ToList());
        }
    }
}
