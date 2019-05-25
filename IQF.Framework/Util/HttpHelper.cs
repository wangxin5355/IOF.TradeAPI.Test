using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IQF.Framework.Util
{
	public static class HttpHelper
	{
		/// <summary>
		/// 发起POST同步请求
		/// </summary>
		/// <param name="url"></param>
		/// <param name="postData"></param>
		/// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
		/// <param name="headers">填充消息头</param>        
		/// <returns></returns>
		public static string HttpPost(string url, string postData = null, string contentType = null, int timeOut = 30, Dictionary<string, string> headers = null)
		{
			postData = postData ?? "";
			using (HttpClient client = new HttpClient())
			{
				if (headers != null)
				{
					foreach (var header in headers)
						client.DefaultRequestHeaders.Add(header.Key, header.Value);
				}
				using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
				{
					if (contentType != null)
						httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

					HttpResponseMessage response = client.PostAsync(url, httpContent).Result;
					return response.Content.ReadAsStringAsync().Result;
				}
			}
		}

		/// <summary>
		/// 发起POST异步请求
		/// </summary>
		/// <param name="url"></param>
		/// <param name="postData"></param>
		/// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
		/// <param name="headers">填充消息头</param>        
		/// <returns></returns>
		public static async Task<string> HttpPostAsync(string url, string postData = null, string contentType = null, int timeOut = 30, Dictionary<string, string> headers = null)
		{
			postData = postData ?? "";
			using (HttpClient client = new HttpClient())
			{
				client.Timeout = new TimeSpan(0, 0, timeOut);
				if (headers != null)
				{
					foreach (var header in headers)
						client.DefaultRequestHeaders.Add(header.Key, header.Value);
				}
				using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
				{
					if (contentType != null)
						httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

					HttpResponseMessage response = await client.PostAsync(url, httpContent);
					return await response.Content.ReadAsStringAsync();
				}
			}
		}

		/// <summary>
		/// 发起GET同步请求
		/// </summary>
		/// <param name="url"></param>
		/// <param name="headers"></param>
		/// <param name="contentType"></param>
		/// <returns></returns>
		public static string HttpGet(string url, string contentType = null, Dictionary<string, string> headers = null)
		{
			using (HttpClient client = new HttpClient())
			{
				if (contentType != null)
					client.DefaultRequestHeaders.Add("ContentType", contentType);
				if (headers != null)
				{
					foreach (var header in headers)
						client.DefaultRequestHeaders.Add(header.Key, header.Value);
				}
				HttpResponseMessage response = client.GetAsync(url).Result;
				return response.Content.ReadAsStringAsync().Result;
			}
		}

		/// <summary>
		/// 发起GET异步请求
		/// </summary>
		/// <param name="url"></param>
		/// <param name="headers"></param>
		/// <param name="contentType"></param>
		/// <returns></returns>
		public static async Task<string> HttpGetAsync(string url, string contentType = null, Dictionary<string, string> headers = null)
		{
			using (HttpClient client = new HttpClient())
			{
				if (contentType != null)
					client.DefaultRequestHeaders.Add("ContentType", contentType);
				if (headers != null)
				{
					foreach (var header in headers)
						client.DefaultRequestHeaders.Add(header.Key, header.Value);
				}
				HttpResponseMessage response = await client.GetAsync(url);
				return await response.Content.ReadAsStringAsync();
			}
		}

		/// <summary>
		/// POST 同步
		/// </summary>
		/// <param name="url"></param>
		/// <param name="postStream"></param>
		/// <param name="encoding"></param>
		/// <param name="timeOut"></param>
		/// <returns></returns>
		public static string HttpPost(string url, Dictionary<string, string> formData = null, Encoding encoding = null, int timeOut = 10000)
		{

			HttpClientHandler handler = new HttpClientHandler();

			HttpClient client = new HttpClient(handler);
			MemoryStream ms = new MemoryStream();
			formData.FillFormDataStream(ms);//填充formData
			HttpContent hc = new StreamContent(ms);
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));
			hc.Headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36");
			hc.Headers.Add("Timeout", timeOut.ToString());
			hc.Headers.Add("KeepAlive", "true");

			var t = client.PostAsync(url, hc);
			t.Wait();
			var t2 = t.Result.Content.ReadAsByteArrayAsync();
			return encoding.GetString(t2.Result);
		}

		/// <summary>
		/// 组装QueryString的方法
		/// 参数之间用&连接，首位没有符号，如：a=1&b=2&c=3
		/// </summary>
		/// <param name="formData"></param>
		/// <returns></returns>
		public static string GetQueryString(this Dictionary<string, string> formData)
		{
			if (formData == null || formData.Count == 0)
			{
				return "";
			}

			StringBuilder sb = new StringBuilder();
			var i = 0;
			foreach (var kv in formData)
			{
				i++;
				sb.AppendFormat("{0}={1}", kv.Key, kv.Value);
				if (i < formData.Count)
				{
					sb.Append("&");
				}
			}

			return sb.ToString();
		}

		/// <summary>
		/// 填充表单信息的Stream
		/// </summary>
		/// <param name="formData"></param>
		/// <param name="stream"></param>
		public static void FillFormDataStream(this Dictionary<string, string> formData, Stream stream)
		{
			string dataString = GetQueryString(formData);
			var formDataBytes = formData == null ? new byte[0] : Encoding.UTF8.GetBytes(dataString);
			stream.Write(formDataBytes, 0, formDataBytes.Length);
			stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
		}
	}
}
