using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IQF.Framework.Middleware
{
	public class LogRequestMiddleware
	{
		private readonly RequestDelegate next;

		public LogRequestMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				var info = new StringBuilder(context.Request.Method + " Request:" + context.Request.Path + context.Request.QueryString);
				if (context.Request.HasFormContentType)
				{
					info.AppendLine();
					info.Append("FormContent : ");
					foreach (var item in context.Request.Form)
					{
						info.AppendFormat("{0}={1}&", item.Key, item.Value);
					}
					if (info[info.Length - 1] == '&')
					{
						info.Remove(info.Length - 1, 1);
					}
				}
				else if (context.Request.Body.CanRead && context.Request.ContentLength > 0)
				{
					info.AppendLine();
					context.Request.EnableBuffering();
					var bodyAsText = await new StreamReader(context.Request.Body).ReadToEndAsync();
					info.Append("BodyContent : " + bodyAsText);
					context.Request.Body.Position = 0;
				}
				LogRecord.writeLogsingle("Request", info.ToString());
			}
			finally
			{
				await next(context);
			}
		}
	}
}
