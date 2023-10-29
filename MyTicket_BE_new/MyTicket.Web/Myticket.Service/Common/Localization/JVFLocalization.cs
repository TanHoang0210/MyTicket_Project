using Microsoft.AspNetCore.Http;
using MYTICKET.UTILS.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.Common.Localization
{
    public class JVFLocalization : LocalizationBase
    {
        public JVFLocalization(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, "STE.Application.Common.Localization.SourceFiles")
        {
        }
    }
}
