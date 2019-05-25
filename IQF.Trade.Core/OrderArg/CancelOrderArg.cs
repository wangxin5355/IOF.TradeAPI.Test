using IQF.Framework;
using System;

namespace IQF.Trade.Core.OrderArg
{
    /// <summary>
    /// 撤单请求参数定义
    /// </summary>
    [Serializable]
	public class CancelOrderArg
	{
		/// <summary>
		/// 委托编号
		/// </summary>
		public string OrderID { get; set; }

		/// <summary>
		/// 交易所
		/// 用户不会传，自行映射
		/// </summary>
		public Exchange Exchange { get; set; }

		/// <summary>
		/// 合约代码
		/// 用户不会传，自行映射
		/// </summary>
		public string Symbol { get; set; }

	}
}
