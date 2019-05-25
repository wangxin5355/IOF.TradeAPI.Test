using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace IQF.Framework.Middleware
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate next;

        private readonly string response;

        public ExceptionHandleMiddleware(RequestDelegate next, string response = null)
        {
            this.next = next;

            this.response = response;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                HandleExceptionAsync(context, ex);
                if (string.IsNullOrWhiteSpace(response))
                {
                    throw;
                }
                await context.Response.WriteAsync(response);
            }
        }

        private static void HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var errorInfo = context.Request.Method + " request url:" + context.Request.Path + context.Request.QueryString + Environment.NewLine;
            if (context.Request.HasFormContentType)
            {
                errorInfo += "FormContent:" + context.Request.Form["param"] + Environment.NewLine;
            }
            errorInfo += ex.ToString();
            LogRecord.writeLogsingle("exceptionError", errorInfo);
        }
    }
}
