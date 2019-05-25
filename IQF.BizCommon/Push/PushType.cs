namespace IQF.BizCommon.Push
{
	/// <summary>
	/// 推送类型
	/// app根据不同的推送类型跳转到不同的界面
	/// </summary>
	public enum PushType
	{
		/// <summary>
		/// 未知
		/// </summary>
		UnKonwon = 0,

		/// <summary>
		///七指禅策略信息
		/// </summary>
		QzcStrategy = 1,

		/// <summary>
		/// 直播间策略
		/// 之前直播间策略和量化策略不区分
		/// 现在区分开
		/// </summary>
		LiveStrategy = 2,

		/// <summary>
		/// 止盈/损消息
		/// </summary>
		StopOrder = 3,

		/// <summary>
		/// 七指禅锦囊消息
		/// </summary>
		QzcBag = 4,

		/// <summary>
		/// 云托管
		/// </summary>
		CloudFollow = 5,

		/// <summary>
		/// 成交回报
		/// </summary>
		ExecutionReport = 6,

		/// <summary>
		/// 行情预警
		/// </summary>
		MarketPrewarning = 7,

		/// <summary>
		/// 量化策略
		/// </summary>
		QuantStrategy = 8,

		/// <summary>
		/// 系统消息
		/// </summary>
		System = 9,

		/// <summary>
		/// 条件单
		/// </summary>
		ConditionOrder = 10,

		/// <summary>
		/// 模拟交易
		/// </summary>
		VirTrade = 11,

		#region 七指禅 100-200
		/// <summary>
		/// 七指禅财富消息
		/// </summary>
		QzcFortune = 100,

		/// <summary>
		/// 锦囊文章消息
		/// </summary>
		QzcBagMsg = 101,

		/// <summary>
		/// 老师回复问题
		/// </summary>
		QzcTeacherAnswer = 102,

		/// <summary>
		/// 客服消息
		/// </summary>
		QzcServiceMsg = 103,

		/// <summary>
		/// 公共直播间开播消息
		/// </summary>
		QzcPublicLiveRoomStartMsg = 104,

		/// <summary>
		/// 私有直播室开播消息
		/// </summary>
		QzcPrivateLiveRoomStartMsg = 105,
		/// <summary>
		/// VIP趋势增加
		/// </summary>
		QzcVIPTrendIncreased = 106,
		/// <summary>
		/// VIP趋势改变
		/// </summary>
		QzcVIPTrendChanged = 107,
		/// <summary>
		/// VIP策略开仓
		/// </summary>
		QzcVIPStrategyOpen = 108,
		/// <summary>
		/// VIP策略平仓
		/// </summary>
		QzcVIPStrategyClose = 109,
		/// <summary>
		/// 提问回复
		/// </summary>
		QzcAnswerQuestion = 110,
		#endregion
	}
}
