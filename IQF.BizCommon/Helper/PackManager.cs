using IQF.Framework;
using IQF.Framework.Cache;
using IQF.Framework.Serialization;
using IQF.Framework.Util;
using System.Collections.Generic;
using System.Linq;

namespace IQF.BizCommon.Helper
{
	public static class PackManager
	{
		private readonly static ICacheInterceptor ProductInfoCache = CacheInterceptorFactory.Create(GetAllProductInfo, 10 * 60);

		/// <summary>
		/// 是否为APP
		/// </summary>
		/// <param name="packType"></param>
		/// <returns></returns>
		public static bool IsApp(int packType)
		{
			var ret =
				packType == (int)DeviceEnum.InQuantFutureIPhone ||
				packType == (int)DeviceEnum.InQuantFutureArd ||
				packType == (int)DeviceEnum.InQuantOfficialIPhone ||
				packType == (int)DeviceEnum.InQuantOfficialArd ||
				packType == (int)DeviceEnum.InQuantIPhone ||
				packType == (int)DeviceEnum.InQuantArd;
			return ret;
		}

		/// <summary>
		/// 是否为windows客户端
		/// </summary>
		/// <param name="packType"></param>
		/// <returns></returns>
		public static bool IsWindows(int packType)
		{
			var ret =
				packType == (int)DeviceEnum.InQuantFutureWindows ||
				packType == (int)DeviceEnum.InQuantOfficialWindows ||
				packType == (int)DeviceEnum.InQuantWindows;
			return ret;
		}

		/// <summary>
		/// 是否为IOS客户端
		/// </summary>
		/// <param name="packType"></param>
		/// <returns></returns>
		public static bool IsIos(int packType)
		{
			var ret = packType == (int)DeviceEnum.InQuantFutureIPhone
				|| packType == (int)DeviceEnum.InQuantOfficialIPhone
				|| packType == (int)DeviceEnum.InQuantIPhone;
			return ret;
		}

		/// <summary>
		/// 是否为安卓客户端
		/// </summary>
		/// <param name="packType"></param>
		/// <returns></returns>
		public static bool IsAndroid(int packType)
		{
			var ret = packType == (int)DeviceEnum.InQuantFutureArd
				|| packType == (int)DeviceEnum.InQuantOfficialArd
				|| packType == (int)DeviceEnum.InQuantArd;
			return ret;
		}

		/// <summary>
		/// 是否为盈益云交易
		/// </summary>
		/// <param name="packType"></param>
		/// <returns></returns>
		public static bool IsCloudTrade(int packType)
		{
			var ret = packType == (int)DeviceEnum.InQuantFutureIPhone ||
				packType == (int)DeviceEnum.InQuantFutureArd ||
				packType == (int)DeviceEnum.InQuantFutureWindows ||
				packType == (int)DeviceEnum.InQuantFutureH5;
			return ret;
		}

		/// <summary>
		/// 是否为盈宽财经
		/// </summary>
		/// <param name="packType"></param>
		/// <returns></returns>
		public static bool IsInQuantFut(int packType)
		{
			var ret = packType == (int)DeviceEnum.InQuantOfficialIPhone ||
				packType == (int)DeviceEnum.InQuantOfficialArd ||
				packType == (int)DeviceEnum.InQuantOfficialWindows ||
				packType == (int)DeviceEnum.InQuantOfficialH5;
			return ret;
		}

		/// <summary>
		/// 是否为期货淘金者
		/// </summary>
		/// <param name="packType"></param>
		/// <returns></returns>
		public static bool IsInQuant(int packType)
		{
			var ret = packType == (int)DeviceEnum.InQuantIPhone ||
				packType == (int)DeviceEnum.InQuantArd ||
				packType == (int)DeviceEnum.InQuantWindows ||
				packType == (int)DeviceEnum.InQuantH5;
			return ret;
		}

		/// <summary>
		/// 获取产品类型
		/// </summary>
		/// <param name="packType"></param>
		/// <returns></returns>
		public static ProductType GetProductType(int packType)
		{
			if (PackManager.IsInQuant(packType))
			{
				return ProductType.TaoJinZhe;
			}
			else if (PackManager.IsInQuantFut(packType))
			{
				return ProductType.YingKuanCaiJing;
			}
			else
			{
				return ProductType.YingYiYun;
			}
		}

		/// <summary>
		/// 是否同一个包
		/// 盈益云交易和盈宽财经定位两个包
		/// </summary>
		/// <param name="packType1"></param>
		/// <param name="packType2"></param>
		/// <returns></returns>
		public static bool IsSamePack(int packType1, int packType2)
		{
			if (IsCloudTrade(packType1) && IsCloudTrade(packType2))
			{
				return true;
			}
			else if (IsInQuantFut(packType1) && IsInQuantFut(packType2))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// 是否为更高的版本
		/// </summary>
		/// <param name="packType">当前包类型</param>
		/// <param name="currVersionStr">当前版本</param>
		/// <param name="futureArdVer">盈益云交易安卓版本</param>
		/// <param name="officialArdVer">盈宽财经安卓版本</param>
		/// <param name="futureWindowsVer">盈益云交易PC版本</param>
		/// <param name="officialWindowsVer">盈宽财经PC版本</param>
		/// <param name="futureIPhoneVer">盈益云交易IOS版本</param>
		/// <param name="officialIPhoneVer">盈宽财经IOS版本</param>
		/// <returns></returns>
		public static bool IsHigherVersion(int packType,
			string currVersionStr,
			int futureArdVer,
			int officialArdVer,
			int futureWindowsVer,
			int officialWindowsVer,
			int futureIPhoneVer,
			int officialIPhoneVer,
			int taojinzheArdVer = 0,
			int taojinzheIPhoneVer = 0,
			int taojinzheWinVer = 0)
		{
			if (string.IsNullOrWhiteSpace(currVersionStr))
			{
				return false;
			}

			var version = GetVersion(packType, currVersionStr);
			if (packType == (int)DeviceEnum.InQuantFutureArd)
			{
				if (version > futureArdVer) return true;
			}
			if (packType == (int)DeviceEnum.InQuantOfficialArd)
			{
				if (version > officialArdVer) return true;
			}
			if (packType == (int)DeviceEnum.InQuantFutureWindows)
			{
				if (version > futureWindowsVer) return true;
			}
			if (packType == (int)DeviceEnum.InQuantOfficialWindows)
			{
				if (version > officialWindowsVer) return true;
			}
			if (packType == (int)DeviceEnum.InQuantFutureIPhone)
			{
				if (version > futureIPhoneVer) return true;
			}
			if (packType == (int)DeviceEnum.InQuantOfficialIPhone)
			{
				if (version > officialIPhoneVer) return true;
			}

			if (packType == (int)DeviceEnum.InQuantIPhone)
			{
				if (version > taojinzheIPhoneVer) return true;
			}
			if (packType == (int)DeviceEnum.InQuantArd)
			{
				if (version > taojinzheArdVer) return true;
			}
			if (packType == (int)DeviceEnum.InQuantWindows)
			{
				if (version > taojinzheWinVer) return true;
			}


			return false;
		}

		/// <summary>
		/// 从请求接口中获取版本号
		/// PC有BUG，已处理
		/// </summary>
		/// <param name="packType"></param>
		/// <param name="currVersionStr"></param>
		/// <returns></returns>
		public static int GetVersion(int packType, string currVersionStr)
		{
			if (string.IsNullOrWhiteSpace(currVersionStr))
			{
				return 0;
			}

			//PC有BUG，最后一位要保留四位
			if (IsWindows(packType))
			{
				var arr = currVersionStr.Split('.').ToList();
				var lastVersion = arr.LastOrDefault() ?? string.Empty;
				var lastRealVersion = lastVersion.PadLeft(4, '0');
				arr[arr.Count - 1] = lastRealVersion;
				var realVersion = string.Join(".", arr);
				return realVersion.Replace(".", "").Trim().ToInt(0);
			}

			var version = currVersionStr.Replace(".", "").Trim().ToInt(0);
			return version;
		}

		/// <summary>
		/// 获取整形版本号
		/// </summary>
		/// <param name="currVersionStr"></param>
		/// <returns></returns>
		public static int GetVersion(string currVersionStr)
		{
			if (string.IsNullOrWhiteSpace(currVersionStr))
			{
				return 0;
			}

			int version = currVersionStr.Replace(".", "").Trim().ToInt(0);
			return version;
		}

		/// <summary>
		/// 获取公司名称
		/// </summary>
		/// <param name="packType"></param>
		/// <returns></returns>
		public static string GetCompanyName(int packType)
		{
			var info = GetProductInfo(packType);
			if (info == null)
			{
				return string.Empty;
			}
			return info.CompanyName;
		}

		/// <summary>
		/// 获取客服电话
		/// </summary>
		/// <param name="packType"></param>
		/// <returns></returns>
		public static string GetCustomerPhone(int packType)
		{
			var info = GetProductInfo(packType);
			if (info == null)
			{
				return string.Empty;
			}
			return info.CustomerPhone;
		}

		/// <summary>
		/// 获取用户协议
		/// </summary>
		/// <param name="packType"></param>
		/// <returns></returns>
		public static string GetArgeementUrl(int packType)
		{
			var info = GetProductInfo(packType);
			if (info == null)
			{
				return string.Empty;
			}
			return info.SoftwareArgeement;
		}

		/// <summary>
		/// 获取包名称
		/// </summary>
		/// <param name="packType"></param>
		/// <returns></returns>
		public static string GetAppName(int packType)
		{
			var info = GetProductInfo(packType);
			if (info == null)
			{
				return string.Empty;
			}
			return info.ProductName;
		}

		/// <summary>
		/// 获取官网地址
		/// </summary>
		/// <param name="packType"></param>
		/// <returns></returns>
		public static string GetOfficialWebsite(int packType)
		{
			var info = GetProductInfo(packType);
			if (info == null)
			{
				return string.Empty;
			}
			return info.Website;
		}

		/// <summary>
		/// 获取产品信息，失败返回NULL
		/// </summary>
		/// <param name="packType"></param>
		/// <returns></returns>
		private static ProductInfo GetProductInfo(int packType)
		{
			var all = ProductInfoCache.Execute<List<ProductInfo>>();
			if (all == null)
			{
				return null;
			}
			return all.FirstOrDefault(f => f.PackType == packType);
		}

		private static List<ProductInfo> GetAllProductInfo()
		{
			try
			{
				var resp = HttpWebResponseUtility.HttpGet("http://inapi.inquant.cn/appmanager/appinfo/getallproductinfo");
				var ret = JsonHelper.Deserialize<ResultInfo<List<ProductInfo>>>(resp);
				if (ret.IsError())
				{
					return null;
				}
				return ret.Data;
			}
			catch (System.Exception ex)
			{
				LogRecord.writeLogsingle("PackManagerError", ex.ToString());
				return null;
			}
		}

		/// <summary>
		/// 产品信息
		/// </summary>
		private class ProductInfo
		{
			/// <summary>
			/// 包类型
			/// </summary>
			public int PackType { get; set; }
			/// <summary>
			/// 产品名称
			/// </summary>
			public string ProductName { get; set; }
			/// <summary>
			/// 软件协议
			/// </summary>
			public string SoftwareArgeement { get; set; }
			/// <summary>
			/// 客服电话
			/// </summary>
			public string CustomerPhone { get; set; }
			/// <summary>
			/// 公司名称
			/// </summary>
			public string CompanyName { get; set; }
			/// <summary>
			/// 官网
			/// </summary>
			public string Website { get; set; }
		}
	}
}
