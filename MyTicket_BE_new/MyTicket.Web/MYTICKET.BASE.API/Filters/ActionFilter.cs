using Microsoft.AspNetCore.Mvc.Filters;

namespace MYTICKET.BASE.API.Filters
{
    public class ActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do something before the action executes.
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Do something after the action executes.
        }
    }
}
