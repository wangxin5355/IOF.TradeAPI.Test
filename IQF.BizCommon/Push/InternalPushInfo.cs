using System;
using System.Collections.Generic;
using System.Text;

namespace IQF.BizCommon.Push
{
	/// <summary>
	/// 内推 SOCKET推送
	/// </summary>
	public interface IInternalPushInfo
	{
		PushType PushType { get; }
	}

	/// <summary>
	/// 内推 SOCKET推送
	/// </summary>
	public class InternalPushInfo : IInternalPushInfo
	{
		/// <summary>
		/// 分隔符
		/// </summary>
		private const byte BREAK = 0;

		/// <summary>
		/// 单条消息终结符
		/// </summary>
		private const byte EOL = 10;

		public InternalPushInfo()
			: this(PushType.UnKonwon, null, null)
		{
		}

		/// <summary>
		/// 内推
		/// </summary>
		/// <param name="pushtype"></param>
		/// <param name="body"></param>
		/// <param name="packType"></param>
		public InternalPushInfo(PushType pushtype, string body, int[] packType)
		{
			PushType = pushtype;
			Body = body;
			PackType = packType;
			this.MsgID = Guid.NewGuid().ToString("N");
			this.AddTime = DateTime.Now;
		}

		/// <summary>
		/// 接收者
		/// </summary>
		[Obsolete("使用ToUserIDs代替")]
		public long ToUserID { get; set; }

		/// <summary>
		/// 接收者
		/// </summary>
		public List<long> ToUserIDs { get; set; }

		/// <summary>
		/// 发送者
		/// </summary>
		public long FromUserID { get; set; }

		/// <summary>
		/// Push消息唯一编号，不重复
		/// </summary>
		public string MsgID { get; set; }

		/// <summary>
		/// 推送类型
		/// </summary>
		public PushType PushType { get; set; }

		/// <summary>
		/// 推送的最终内容
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		/// 要推送的包类型
		/// </summary>
		public int[] PackType { get; set; }

		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime AddTime { get; set; }

		/// <summary>
		/// 生成内推内容
		/// </summary>
		/// <param name="msgType">内推消息类型</param>
		/// <param name="contents">需严格保证顺序</param>
		/// <returns></returns>
		public static string BuildBody(int msgType, params string[] contents)
		{
			var buffer = new List<byte>();
			buffer.AddRange(UTF8Encoding.UTF8.GetBytes(msgType.ToString()));
			foreach (var item in contents)
			{
				buffer.Add(BREAK);
				buffer.AddRange(UTF8Encoding.UTF8.GetBytes(item));
			}
			buffer.Add(EOL);
			return Encoding.UTF8.GetString(buffer.ToArray());
		}
	}
}
