using Microsoft.AspNetCore.Http;
using Shared.Contracts.Request.Shared;

namespace Shared.Api
{
    public static class ApiExtensions
    {
        public static FileInput? ToFileInput(this IFormFile? formFile)
        {
            FileInput? imageInput = null;
            if (formFile is not null)
            {
                imageInput = new FileInput(
                    formFile.OpenReadStream(),
                    formFile.FileName,
                    formFile.ContentType);

                return imageInput;
            }

            return imageInput;
        }
    }
}
