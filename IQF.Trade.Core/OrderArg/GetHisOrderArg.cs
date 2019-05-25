using System;

namespace IQF.Trade.Core.OrderArg
{
    public class GetHisOrderArg
	{
		/// <summary>
		/// 起始时间
		/// </summary>
		public DateTime BeginTime { get; set; }

		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime EndTime { get; set; }

		private int requestNum = 200;
		/// <summary>
		/// 请求行数（默认值200）
		/// </summary>
		public int RequestNum
		{
			get { return requestNum; }
			set { if (value > 0) requestNum = value; }
		}
	}
}
