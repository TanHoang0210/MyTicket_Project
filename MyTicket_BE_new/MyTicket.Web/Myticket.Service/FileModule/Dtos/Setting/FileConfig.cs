using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.FileModule.Dtos.Setting
{
    public class FileConfig
    {
        public string Path { get; set; }
        public double LimitUpload { get; set; }
        public string AllowExtension { get; set; }
    }
}
