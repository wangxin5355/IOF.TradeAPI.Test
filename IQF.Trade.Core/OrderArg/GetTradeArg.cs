using System;

namespace IQF.Trade.Core.OrderArg
{
    /// <summary>
    /// 获取成交请求参数
    /// </summary>
    [Serializable]
	public class GetTradeArg
	{
		/// <summary>
		/// 开始时间
		/// </summary>
		public DateTime StartTime { get; set; }

		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime EndTime { get; set; }
	}
}
