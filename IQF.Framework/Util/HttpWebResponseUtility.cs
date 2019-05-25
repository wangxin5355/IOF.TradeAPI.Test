using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace IQF.Framework.Util
{
	public class HttpWebResponseUtility
	{
		private static readonly string DefaultUserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0";

		static HttpWebResponseUtility()
		{
			if (ServicePointManager.DefaultConnectionLimit < 100)
			{
				ServicePointManager.DefaultConnectionLimit = 100;
			}
			ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(CheckValidationResult);
		}

		#region HTTP POST
		/// <summary>
		/// FORM表单提交数据:application/x-www-form-urlencoded
		/// </summary>
		/// <param name="url"></param>
		/// <param name="parameters"></param>
		/// <param name="timeout"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public static string PostData(
			string url,
			IDictionary<string, string> parameters,
			int timeout = 1000000,
			string userAgent = "",
			Encoding encoding = null,
			CookieCollection cookies = null,
			IWebProxy proxy = null,
			bool isGZip = false,
			string referer = null)
		{
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			using (var response = Post(url, parameters, timeout, userAgent, encoding, cookies, proxy, isGZip, referer))
			{
				using (var stream = response.GetResponseStream())
				{
					using (var reader = new StreamReader(stream, encoding))
					{
						return reader.ReadToEnd();
					}
				}
			}
		}

		/// <summary>  
		/// FORM表单提交数据:application/x-www-form-urlencoded
		/// </summary>  
		/// <returns></returns>  
		public static HttpWebResponse Post(
			string url,
			IDictionary<string, string> parameters,
			int timeout = 1000000,
			string userAgent = "",
			Encoding encoding = null,
			CookieCollection cookies = null,
			IWebProxy proxy = null,
			bool isGZip = false,
			string referer = null)
		{
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			byte[] data = null;
			if (parameters == null || parameters.Count == 0)
			{
				data = new byte[0];
			}
			else
			{
				var buffer = new StringBuilder();
				int i = 0;
				foreach (string key in parameters.Keys)
				{
					if (i > 0)
					{
						buffer.AppendFormat("&{0}={1}", key, parameters[key]);
					}
					else
					{
						buffer.AppendFormat("{0}={1}", key, parameters[key]);
					}
					i++;
				}
				data = encoding.GetBytes(buffer.ToString());
			}
			return PostForResp(url, data, timeout, userAgent, cookies, proxy, isGZip, "application/x-www-form-urlencoded", referer);
		}

		/// <summary>  
		/// 创建POST方式的HTTP请求
		/// </summary>  
		/// <param name="url">请求的URL</param>  
		/// <param name="data">随同请求POST的参数名称及参数值字典</param>  
		/// <param name="timeout">请求的超时时间</param>  
		/// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
		/// <param name="encoding">发送HTTP请求时所用的编码</param>  
		/// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>
		/// <param name="proxy">指定proxy</param>
		/// <param name="isGZip">判断是否使用gzip压缩请求</param>
		/// <param name="contentType">request的http header里面的content-type</param>
		/// <param name="referer">指定post的内容</param>
		/// <returns></returns>  
		public static string HttpPost(
			string url,
			byte[] data,
			int? timeout = null,
			string userAgent = null,
			Encoding encoding = null,
			CookieCollection cookies = null,
			IWebProxy proxy = null,
			bool isGZip = false,
			string contentType = "application/json;charset=utf-8",
			string referer = null)
		{
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			using (var rsp = PostForResp(url, data, timeout, userAgent, cookies, proxy, isGZip, contentType, referer))
			{
				using (var stream = rsp.GetResponseStream())
				{
					using (var reader = new StreamReader(stream, encoding))
					{
						return reader.ReadToEnd();
					}
				}
			}
		}

		/// <summary>  
		/// 创建POST方式的HTTP请求
		/// </summary>  
		/// <param name="url">请求的URL</param>  
		/// <param name="content">随同请求POST的参数名称及参数值字典</param>  
		/// <param name="timeout">请求的超时时间</param>  
		/// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
		/// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
		/// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>
		/// <param name="proxy">指定proxy</param>
		/// <param name="isGZip">判断是否使用gzip压缩请求</param>
		/// <param name="contentType">request的http header里面的content-type</param>
		/// <param name="referer">指定post的内容</param>
		/// <returns></returns>  
		public static string HttpPost(
			string url,
			string content,
			int? timeout = null,
			string userAgent = null,
			Encoding encoding = null,
			CookieCollection cookies = null,
			IWebProxy proxy = null,
			bool isGZip = false,
			string contentType = "application/json;charset=utf-8",
			string referer = null)
		{
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			byte[] data = new byte[0];
			if (!string.IsNullOrWhiteSpace(content))
			{
				data = encoding.GetBytes(content);
			}
			return HttpPost(url, data, timeout, userAgent, encoding, cookies, proxy, isGZip, contentType, referer);
		}

		/// <summary>  
		/// 创建POST方式的HTTP请求
		/// </summary>  
		/// <param name="url">请求的URL</param>  
		/// <param name="data">随同请求POST的参数名称及参数值字典</param>  
		/// <param name="timeout">请求的超时时间</param>  
		/// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
		/// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
		/// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>
		/// <param name="proxy">指定proxy</param>
		/// <param name="isGZip">判断是否使用gzip压缩请求</param>
		/// <param name="contentType">request的http header里面的content-type</param>
		/// <param name="referer">指定post的内容</param>
		/// <returns></returns>  
		public static HttpWebResponse PostForResp(
			string url,
			byte[] data,
			int? timeout = null,
			string userAgent = null,
			CookieCollection cookies = null,
			IWebProxy proxy = null,
			bool isGZip = false,
			string contentType = "application/json;charset=utf-8",
			string referer = null)
		{
			var request = BuildRequest(url, data, "POST", timeout, userAgent, cookies, proxy, isGZip, contentType, referer);
			return request.GetResponse() as HttpWebResponse;
		}

		/// <summary>
		/// 异步post请求
		/// </summary>
		/// <param name="callback">回调函数</param>
		/// <param name="url">请求的URL</param>  
		/// <returns></returns>
		public static bool PostAsync(
			string url,
			byte[] data,
			AsyncCallback callback = null,
			int? timeout = null,
			string userAgent = null,
			CookieCollection cookies = null,
			IWebProxy proxy = null,
			bool isGZip = false,
			string contentType = "application/json;charset=utf-8",
			string referer = null)
		{
			var request = BuildRequest(url, data, "POST", timeout, userAgent, cookies, proxy, isGZip, contentType, referer);
			request.BeginGetResponse(callback, request);
			return true;
		}

		/// <summary>
		/// 异步post请求
		/// </summary>
		/// <param name="callback">回调函数</param>
		/// <param name="url">请求的URL</param>  
		/// <returns></returns>
		public static bool PostAsync(
			string url,
			string content,
			AsyncCallback callback = null,
			int? timeout = null,
			string userAgent = null,
			Encoding encoding = null,
			CookieCollection cookies = null,
			IWebProxy proxy = null,
			bool isGZip = false,
			string contentType = "application/json;charset=utf-8",
			string referer = null)
		{
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			byte[] data = new byte[0];
			if (!string.IsNullOrWhiteSpace(content))
			{
				data = encoding.GetBytes(content);
			}
			return PostAsync(url, data, callback, timeout, userAgent, cookies, proxy, isGZip, contentType, referer);
		}
		#endregion

		#region 工具
		/// <summary>
		/// 从full里面，取出介于start和end之间的文本。失败返回null
		/// </summary>
		/// <param name="html"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public static string GetText(string full, string start, string end)
		{
			int idx1 = full.IndexOf(start);
			if (idx1 < 0)
			{
				return null;
			}
			int idx2 = full.IndexOf(end, idx1 + 1);
			if (idx2 < 0)
			{
				return null;
			}

			string sub = full.Substring(idx1 + start.Length, idx2 - idx1 - start.Length);
			return sub;
		}

		private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true; //总是接受  
		}

		private static HttpWebRequest BuildRequest(
			string url,
			byte[] data,
			string httpMethod,
			int? timeout = null,
			string userAgent = null,
			CookieCollection cookies = null,
			IWebProxy proxy = null,
			bool isGZip = false,
			string contentType = "application/x-www-form-urlencoded",
			string referer = null)
		{
			if (string.IsNullOrEmpty(url))
			{
				throw new ArgumentNullException("http request url is null");
			}

			HttpWebRequest request = null;
			//如果是发送HTTPS请求  
			if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
			{
				request = WebRequest.Create(url) as HttpWebRequest;
				request.ProtocolVersion = HttpVersion.Version10;
			}
			else
			{
				request = WebRequest.Create(url) as HttpWebRequest;
			}

			request.Method = httpMethod;
			request.Proxy = proxy;
			request.ContentType = contentType;
			request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
			request.Headers["Accept-Language"] = "zh-CN,zh;q=0.8";
			request.Headers["Accept-Charset"] = "GBK,utf-8;q=0.7,*;q=0.3";
			request.AllowAutoRedirect = false;
			request.KeepAlive = true;
			request.UserAgent = string.IsNullOrEmpty(userAgent) ? DefaultUserAgent : userAgent;
			if (!string.IsNullOrEmpty(referer)) request.Referer = referer;
			if (timeout.HasValue)
			{
				request.Timeout = timeout.Value;
			}
			if (cookies != null)
			{
				request.CookieContainer = new CookieContainer();
				request.CookieContainer.Add(cookies);
			}
			if (data == null || data.Length <= 0)
			{
				request.ContentLength = 0;
				return request;
			}
			if (isGZip)
			{
				data = GZip.Compress(data);
			}
			request.ContentLength = data.Length;
			using (Stream stream = request.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
			}
			return request;
		}
		#endregion

		#region HTTP GET
		/// <summary>
		/// Http GET请求，返回数据
		/// </summary>
		/// <param name="url">请求的url地址</param>
		/// <returns>http</returns>
		public static string HttpGet(
			string url,
			byte[] data = null,
			int? timeout = null,
			string userAgent = null,
			Encoding encoding = null,
			CookieCollection cookies = null,
			IWebProxy proxy = null,
			bool isGZip = false,
			string contentType = "text/html;charset=UTF-8",
			string referer = null)
		{
			var request = BuildRequest(url, data, "GET", timeout, userAgent, cookies, proxy, isGZip, contentType, referer);

			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			//获取服务器返回
			using (var response = request.GetResponse() as HttpWebResponse)
			{
				using (var stream = response.GetResponseStream())
				{
					using (var reader = new StreamReader(stream, encoding))
					{
						return reader.ReadToEnd();
					}
				}
			}
		}
		#endregion
	}
}
