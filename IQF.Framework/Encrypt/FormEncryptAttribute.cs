using IQF.Framework;
using IQF.Framework.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace IQF.Framework.Encrypt
{
	/// <summary>
	/// 表单请求加解密
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class FormEncryptAttribute : ActionFilterAttribute
	{
		public bool Ignore { get; set; }

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var interceptor = context.HttpContext.RequestServices.GetService(typeof(IFormEncryptInterceptor)) as IFormEncryptInterceptor;
			if (interceptor != null)
			{
				interceptor.BeforeRequest(context);
			}

			if (context.ActionDescriptor.Parameters == null ||
				context.ActionDescriptor.Parameters.Count <= 0)
			{
				base.OnActionExecuting(context);
				return;
			}
			var actionParams = context.ActionDescriptor.Parameters.Where(w => w.BindingInfo.BindingSource == BindingSource.Form).Cast<ControllerParameterDescriptor>().ToList();
			if (actionParams.Count <= 0)
			{
				base.OnActionExecuting(context);
				return;
			}

			if (!context.HttpContext.Request.HasFormContentType)
			{
				context.HttpContext.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
				return;
			}

			//是否存在必传参数
			var needParam = actionParams.Exists(c => !c.ParameterInfo.IsOptional);
			//请求参数是否为空
			var isNullParam = context.HttpContext.Request.Form == null ||
				!context.HttpContext.Request.Form.ContainsKey("param") ||
				string.IsNullOrWhiteSpace(context.HttpContext.Request.Form["param"]);
			//没有必传参数，并且请求参数为空
			if (!needParam && isNullParam)
			{
				base.OnActionExecuting(context);
				return;
			}
			//有必传参数，并且请求参数为空
			if (needParam && isNullParam)
			{
				var ret = BuildResult(-1, "请求参数不合法");
				context.Result = new ContentResult() { Content = ret };
				return;
			}

			var param = context.HttpContext.Request.Form["param"];
			var json = TripleDESCryptogram.Decrypt(param);
			var jobject = JObject.Parse(json);
			foreach (var actionParam in actionParams)
			{
				if (!actionParam.ParameterType.IsPrimitive && actionParam.ParameterType != typeof(string))
				{
					var paramObj = JsonHelper.Deserialize(json, actionParam.ParameterType);
					this.AddOrSetArg(context, actionParam.Name, paramObj);
					continue;
				}

				var obj = jobject.GetValue(actionParam.Name);
				if (obj == null)
				{
					if (!actionParam.ParameterInfo.IsOptional)
					{
						var ret = BuildResult(-1, "缺少参数：" + actionParam.Name);
						context.Result = new ContentResult() { Content = ret };
						return;
					}
					continue;
				}
				var val = obj.ToObject(actionParam.ParameterType);
				this.AddOrSetArg(context, actionParam.Name, val);
			}

			base.OnActionExecuting(context);
		}

		public override void OnActionExecuted(ActionExecutedContext context)
		{
			if (context.Result == null)
			{
				base.OnActionExecuted(context);
				return;
			}
			var resultType = context.Result.GetType();
			var property = resultType.GetProperty("Value");
			if (property == null)
			{
				property = resultType.GetProperty("Content");
			}
			if (property == null)
			{
				throw new NotSupportedException("表单请求加解密不支持的ActionResult:" + resultType.FullName);
			}

			var val = property.GetValue(context.Result);
			string json = null;
			if (val.GetType() == typeof(string))
			{
				json = val.ToString();
			}
			else
			{
				json = JsonHelper.Serialize(val);
			}
			var encryptSrc = TripleDESCryptogram.Encrypt(json);

			context.Result = new ContentResult() { Content = encryptSrc };

			base.OnActionExecuted(context);

			var interceptor = context.HttpContext.RequestServices.GetService(typeof(IFormEncryptInterceptor)) as IFormEncryptInterceptor;
			if (interceptor != null)
			{
				interceptor.AfterResponse(context);
			}
		}

		private void AddOrSetArg(ActionExecutingContext context, string name, object val)
		{
			if (context.ActionArguments.ContainsKey(name))
			{
				context.ActionArguments[name] = val;
			}
			else
			{
				context.ActionArguments.Add(name, val);
			}
		}

		private string BuildResult(int errorNo, string errorInfo)
		{
			var ret = new ResultInfo() { Error_no = errorNo, Error_info = errorInfo };
			var resp = JsonHelper.Serialize(ret);
			var encryptSrc = TripleDESCryptogram.Encrypt(resp);
			return encryptSrc;
		}
	}
}


