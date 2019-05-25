using System.ComponentModel;

namespace IQF.Framework
{
	/// <summary>
	/// 系统平台
	/// </summary>
	public enum PlatformOs
	{
		/// <summary>
		/// IOS
		/// </summary>
		[Description("IOS")]
		Ios = 1,
		/// <summary>
		/// 安卓
		/// </summary>
		[Description("安卓")]
		Ard = 2,
		/// <summary>
		/// windows pc
		/// </summary>
		[Description("windows pc")]
		WinPc = 3
	}

	/// <summary>
	/// 产品定义
	/// </summary>
	public enum ProductType
	{
		/// <summary>
		/// 盈益云交易
		/// </summary>
		[Description("盈益云交易")]
		YingYiYun = 1,
		/// <summary>
		/// 盈宽财经
		/// </summary>
		[Description("盈宽财经")]
		YingKuanCaiJing = 2,
		/// <summary>
		/// 期货淘金者
		/// </summary>
		[Description("期货淘金者")]
		TaoJinZhe = 3
	}

	public enum DeviceEnum
	{
		[Description("未知")]
		None = -1,

		/// <summary>
		/// 盈益云交易 iphone
		/// 发布给代理的版本
		/// </summary>
		[Description("盈益云交易苹果")]
		InQuantFutureIPhone = 1000,

		/// <summary>
		/// 盈益云交易 android
		/// 发布给代理的版本
		/// </summary>
		[Description("盈益云交易安卓")]
		InQuantFutureArd = 1001,

		/// <summary>
		/// 盈益云交易 WindowsPC客户端
		/// 发布给代理的版本
		/// </summary>
		[Description("盈益云交易PC")]
		InQuantFutureWindows = 1002,

		/// <summary>
		/// 盈宽财经 iphone
		/// </summary>
		[Description("盈宽财经苹果")]
		InQuantOfficialIPhone = 1003,

		/// <summary>
		/// 盈宽财经 android
		/// </summary>
		[Description("盈宽财经安卓")]
		InQuantOfficialArd = 1004,

		/// <summary>
		/// 盈宽财经 Windows
		/// </summary>
		[Description("盈宽财经PC")]
		InQuantOfficialWindows = 1005,

		/// <summary>
		/// 期货淘金者 iPhone
		/// </summary>
		[Description("期货淘金者苹果")]
		InQuantIPhone = 1006,
		/// <summary>
		/// 期货淘金者 android
		/// </summary>
		[Description("期货淘金者安卓")]
		InQuantArd = 1007,
		/// <summary>
		/// 期货淘金者 windows
		/// </summary>
		[Description("期货淘金者PC")]
		InQuantWindows = 1008,

		/// <summary>
		/// 盈益云交易 H5
		/// </summary>
		[Description("盈益云交易H5")]
		InQuantFutureH5 = 1009,
		/// <summary>
		/// 盈宽财经 H5
		/// </summary>
		[Description("盈宽财经H5")]
		InQuantOfficialH5 = 1010,
		/// <summary>
		/// 期货淘金者 H5
		/// </summary>
		[Description("期货淘金者H5")]
		InQuantH5 = 1011
	}

	/// <summary>
	/// 交易所
	/// </summary>
	public enum Exchange
	{
		/// <summary>
		/// 未知
		/// </summary>
		[Description("")]
		NONE = 0,
		/// <summary>
		/// 上交所
		/// </summary>
		[Description("上交所")]
		SHSE = 1,
		/// <summary>
		/// 深交所
		/// </summary>
		[Description("深交所")]
		SZSE = 2,
		/// <summary>
		/// 中金所
		/// </summary>
		[Description("中金所")]
		CFFEX = 3,
		/// <summary>
		/// 上期所
		/// </summary>
		[Description("上期所")]
		SHFE = 4,
		/// <summary>
		/// 大商所
		/// </summary>
		[Description("大商所")]
		DCE = 5,
		/// <summary>
		/// 郑商所
		/// </summary>
		[Description("郑商所")]
		CZCE = 6,
		/// <summary>
		/// 港交所
		/// </summary>
		[Description("港交所")]
		HKSE = 7,
		/// <summary>
		/// 纳斯达克市场
		/// </summary>
		[Description("纳斯达克")]
		NASDAQ = 8,
		/// <summary>
		/// 纽约证券交易所
		/// </summary>
		[Description("纽约证券交易所")]
		NYSE = 9,
		/// <summary>
		/// 全美证券交易所
		/// </summary>
		[Description("全美证券交易所")]
		AMEX = 10,
		/// <summary>
		/// 新三板
		/// </summary>
		[Description("新三板")]
		SBSE = 11,
		/// <summary>
		/// 伦敦商品交易所
		/// </summary>
		[Description("伦敦商品交易所")]
		LME = 12,
		/// <summary>
		/// 马来西亚衍生产品交易所
		/// </summary>
		[Description("马来西亚衍生产品交易所")]
		BMD = 13,
		/// <summary>
		/// 东京商品交易所
		/// </summary>
		[Description("东京商品交易所")]
		TOCOM = 14,
		/// <summary>
		/// 上海国际能源交易中心
		/// </summary>
		[Description("上海国际能源交易中心")]
		INE = 15
	}

	/// <summary>
	/// 币种
	/// </summary>
	public enum Currency
	{
		/// <summary>
		/// 境内人民币
		/// </summary>
		[Description("人民币")]
		CNY = 0,
		/// <summary>
		/// 美元
		/// </summary>
		[Description("美元")]
		USD = 1,
		/// <summary>
		/// 港币
		/// </summary>
		[Description("港币")]
		HKD = 2
	}

	/// <summary>
	/// 投资品种类型
	/// </summary>
	public enum ContractType
	{
		/// <summary>
		/// 未知
		/// </summary>
		[Description("未知")]
		NONE = 0,
		/// <summary>
		/// 股票
		/// </summary>
		[Description("股票")]
		Stock = 1,
		/// <summary>
		/// 期货
		/// </summary>
		[Description("期货")]
		Future = 2,
		/// <summary>
		/// 期权
		/// </summary>
		[Description("期权")]
		Option = 3
	}

	/// <summary>
	/// 委托类型
	/// </summary>
	public enum OrderType
	{
		/// <summary>
		/// 限价
		/// </summary>
		[Description("限价")]
		LMT = 0,

		/// <summary>
		/// 市价
		/// </summary>
		[Description("市价")]
		MKT = 1,
		/// <summary>
		/// 对手价
		/// </summary>
		[Description("对手价")]
		CPT = 2,
		/// <summary>
		/// 排队价
		/// </summary>
		[Description("排队价")]
		LINE = 3
	}

	/// <summary>
	/// 委托状态
	/// </summary>
	public enum OrderStatus
	{
		/// <summary>
		/// 未知
		/// </summary>
		/// 
		[Description("未知")]
		None = -1,

		/// <summary>
		/// 未发（下单指令还未发送到下游）
		/// </summary>
		[Description("未发")]
		NotSent = 0,

		/// <summary>
		/// 1 已发（下游已收到订单）
		/// </summary>
		[Description("已发")]
		Sended = 1,

		/// <summary>
		/// 2 已报（下单指令已报给交易所）
		/// </summary>
		[Description("已报")]
		Accepted = 2,

		/// <summary>
		/// 部分成交
		/// </summary>
		[Description("部分成交")]
		PartiallyFilled = 3,

		/// <summary>
		/// 4 已撤（可能已经部分成交，要看看filled字段）
		/// </summary>
		[Description("已撤单")]
		Cancelled = 4,

		/// <summary>
		/// 5 全部成交
		/// </summary>
		[Description("全部成交")]
		Filled = 5,

		/// <summary>
		/// 6 已拒绝
		/// </summary>
		[Description("已拒绝")]
		Rejected = 6,

		/// <summary>
		/// 7 撤单请求已发送，但不确定当前状态
		/// </summary>
		[Description("待撤")]
		PendingCancel = 7
	}

	/// <summary>
	/// 买卖方向
	/// </summary>
	public enum OrderSide
	{
		/// <summary>
		/// 买入
		/// </summary>
		[Description("买")]
		Buy = 'B',

		/// <summary>
		/// 卖出
		/// </summary>
		[Description("卖")]
		Sell = 'S'
	}

	/// <summary>
	/// 开平仓
	/// </summary>
	public enum Offset
	{
		/// <summary>
		/// 未知
		/// </summary>
		[Description("")]
		None = 0,

		/// <summary>
		/// 开仓
		/// </summary>
		[Description("开仓")]
		Open = 1,

		/// <summary>
		/// 平仓
		/// </summary>
		[Description("平仓")]
		Close = 2,

		/// <summary>
		/// 平今
		/// </summary>
		[Description("平今")]
		CloseToday = 3,

		/// <summary>
		/// 平昨
		/// </summary>
		[Description("平昨")]
		CloseYesterday = 4
	}

	/// <summary>
	/// 持仓方向
	/// </summary>
	public enum PosSide
	{
		/// <summary>
		/// 净持仓
		/// </summary>
		/// 
		[Description("净持仓")]
		Net = 0,
		/// <summary>
		/// 多仓
		/// </summary>
		/// 
		[Description("多仓")]
		Long = 1,
		/// <summary>
		/// 空仓
		/// </summary>
		/// 
		[Description("空仓")]
		Short = 2
	}
}
