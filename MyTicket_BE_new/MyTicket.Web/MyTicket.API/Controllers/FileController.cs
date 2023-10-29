using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MYTICKET.BASE.API.Controller;
using MYTICKET.BASE.API.Filters;
using MYTICKET.BASE.SERVICE.Common.Validations;
using MYTICKET.UTILS.Net.MimeTypes;
using MYTICKET.UTILS;
using MYTICKET.WEB.SERVICE.FileModule.Abstracts;
using MYTICKET.WEB.SERVICE.FileModule.Dtos.UploadFile;

namespace MYTICKET.WEB.API.Controllers
{
    [Route("api/file")]
    [ApiController]
    public class FileController : ApiControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Xem file
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="file"></param>
        /// <param name="download"></param>
        /// <returns></returns>
        [HttpGet("get")]
        [TypeFilter(typeof(ExceptionFilter))]
        public IActionResult GetFile([FromQuery] string folder, [FromQuery] string file, [FromQuery] bool download)
        {

            var result = _fileService.GetFile(folder, file);

            if (download)
            {
                return File(result, MimeTypeNames.ApplicationOctetStream, file);
            }
            return FileByFormat(result, file);
        }

        /// <summary>
        /// Tải lên file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("upload")]
        public APIResponse UploadFile([FileExtention][FileMaxLength] IFormFile file, string folder)
            => new(_fileService.UploadFile(new UploadFileModel
            {
                File = file,
                Folder = folder,
            }));
    }
}
