using Microsoft.AspNetCore.Mvc.Filters;

namespace MYTICKET.BASE.API.Filters
{
    public class ResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            // Do something before the result executes.
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            // Do something after the result executes.
        }
    }
}
