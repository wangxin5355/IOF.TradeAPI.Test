using System;

namespace IQF.BizCommon.Push
{
	/// <summary>
	/// 外推 APP PUSH
	/// </summary>
	public interface IExternalPushInfo
	{
		PushType PushType { get; set; }
	}

	/// <summary>
	/// 外推消息:APP PUSH  
	/// </summary>
	public class PushInfo : IExternalPushInfo
	{
		public PushInfo()
			: this(null, null, null, 0, null, null, PushType.UnKonwon)
		{
		}

		/// <summary>
		/// userId deviceId 二选一
		/// </summary>
		/// <param name="title"></param>
		/// <param name="titleContent"></param>
		/// <param name="body"></param>
		/// <param name="fromUserId"></param>
		/// <param name="toUserIds"></param>
		/// <param name="packType"></param>
		/// <param name="pushType"></param>
		/// <param name="deviceId"></param>
		public PushInfo(string title, string titleContent, string body, long fromUserId, long[] toUserIds, int[] packType, PushType pushType)
		{
			Title = title;
			TitleContent = titleContent;
			Body = body;
			ToUserIDs = toUserIds;
			PackType = packType;
			PushType = pushType;
			FromUserID = fromUserId;
			this.PushTime = DateTime.Now;
		}

		/// <summary>
		/// 标题
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 标题下面的内容
		/// </summary>
		public string TitleContent { get; set; }

		/// <summary>
		///消息完全内容
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		/// 用户ID
		/// </summary>
		public long[] ToUserIDs { get; set; }
		/// <summary>
		/// 设备类型
		/// </summary>
		public int[] PackType { get; set; }
		/// <summary>
		/// 推送类型
		/// </summary>
		public PushType PushType { get; set; }
		/// <summary>
		/// 推送发起用户ID（虚拟用户）
		/// </summary>
		public long FromUserID { get; set; }
		/// <summary>
		/// 推送时间
		/// </summary>
		public DateTime PushTime { get; set; }
	}
}
