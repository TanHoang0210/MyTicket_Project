using Hangfire.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.UTILS.Hangfire
{
    public static class HangfireUltils
    {
        public static string SetBackGroundStatus(this IStorageConnection connection, string backgroundJobId)
        {
            JobData jobData = connection.GetJobData(backgroundJobId);
            return jobData is not null ? jobData.State : string.Empty;
        }
    }
}
