using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace LifeLine.File.Service.Api.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController(IMinioClient minioClient, IConfiguration configuration) : Controller
    {
        private readonly IMinioClient _minioClient = minioClient;
        private readonly string _backetName = configuration.GetValue<string>("Minio:BucketName") ?? throw new ArgumentNullException("BacketName не сконфигурирован!");

        [HttpGet("link")]
        public async Task<IActionResult> GetLink([FromQuery] string key)
        {
            try
            {
                var statArgs = new StatObjectArgs().WithBucket(_backetName).WithObject(key);

                await _minioClient.StatObjectAsync(statArgs);

                var args = new PresignedGetObjectArgs().WithBucket(_backetName).WithObject(key).WithExpiry(7200);

                var url = await _minioClient.PresignedGetObjectAsync(args);

                return Ok(url);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound("Файл с таким ключом не найден!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
