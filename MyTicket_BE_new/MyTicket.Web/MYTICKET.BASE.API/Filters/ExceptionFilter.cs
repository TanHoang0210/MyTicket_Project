using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MYTICKET.UTILS;
using MYTICKET.UTILS.ConstantVariables.Shared;
using MYTICKET.UTILS.CustomException;
using MYTICKET.UTILS.Localization;
using System.Text.Json;

namespace MYTICKET.BASE.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILogger<ExceptionFilter>>();
            var mapErrorCode = context.HttpContext.RequestServices.GetService<MapErrorCodeBase>();
            var localization = context.HttpContext.RequestServices.GetService<LocalizationBase>();

            var request = context.HttpContext.Request;
            string errStr = $"Path = {request.Path}, Query = {JsonSerializer.Serialize(request.Query)}";
            ErrorCode errorCode;
            string message = context.Exception.Message;
            if (context.Exception is UserFriendlyException userFriendlyException)
            {
                errorCode = userFriendlyException.ErrorCode;
                try
                {
                    message = !string.IsNullOrWhiteSpace(userFriendlyException.MessageLocalize) 
                        ? localization.Localize(userFriendlyException.MessageLocalize)
                            : mapErrorCode.GetErrorMessage(userFriendlyException.ErrorCode);
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
                logger?.LogInformation(context.Exception, $"{context.Exception.GetType()}: {errStr}, ErrorCode = {errorCode}, Message = {message}");
            }
            else
            {
                logger?.LogError(context.Exception, $"{context.Exception.GetType()}: {errStr}, Message = {message}");
                errorCode = ErrorCode.InternalServerError;
            }
            APIResponse response = new(StatusCode.Error, nameof(ExceptionFilter), (int)errorCode, message);
            context.Result = new JsonResult(response);
        }
    }
}
