using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace IQF.Framework.Encrypt
{
	/// <summary>
	/// 表单加密拦截器
	/// </summary>
	public interface IFormEncryptInterceptor
	{
		void BeforeRequest(ActionExecutingContext context);

		void AfterResponse(ActionExecutedContext context);
	}
}
