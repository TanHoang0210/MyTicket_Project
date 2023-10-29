using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.FileModule.Dtos.UploadFile
{
    public class UploadFileModel
    {
        public IFormFile File { get; set; }
        public string Folder { get; set; }
    }
}
