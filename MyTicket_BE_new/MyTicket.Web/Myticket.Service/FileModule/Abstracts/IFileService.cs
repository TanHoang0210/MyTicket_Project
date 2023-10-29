using MYTICKET.WEB.SERVICE.FileModule.Dtos.UploadFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.FileModule.Abstracts
{
    public interface IFileService
    {
        byte[] GetFile(string folder, string fileName);
        string UploadFile(UploadFileModel input);
    }
}
