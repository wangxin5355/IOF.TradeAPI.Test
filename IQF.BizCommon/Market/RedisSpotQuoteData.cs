using IQF.BizCommon.Data;
using IQF.Framework;
using IQF.Framework.Cache;
using System;
using System.Text;

namespace IQF.BizCommon.Market
{
	/// <summary>
	/// 现货行情相关数据
	/// </summary>
	public class RedisSpotQuoteData
	{
		private const char Sperator = '_';

		private static readonly ICacheInterceptor spotQuoteDataCache = CacheInterceptorFactory.Create<long, SpotQuoteData>(GetQuoteDataFromRedis);

		public static bool GetQuoteData(long contractId, out SpotQuoteData quoteData)
		{
			try
			{
				quoteData = spotQuoteDataCache.Execute<SpotQuoteData>(null, contractId);
			}
			catch (Exception e)
			{
				LogRecord.writeLogsingle("error", "GetSpotQuoteDataInnerCodeNowPx:" + e.ToString());
				quoteData = null;
			}
			return quoteData != null;
		}

		private static SpotQuoteData GetQuoteDataFromRedis(long contractId)
		{
			string key = GetSpotQuoteDataKey(contractId);
			string result = RedisManager.QuoteRedis.Get<string>(key);
			var quoteData = ToQuoteData(result);
			var contract = SpotContractDao.Get(contractId);
			if (contract != null && quoteData != null)
			{
				if (contract.VarietyID == 10) //鸡蛋
				{
					quoteData.LastPx = quoteData.LastPx * 500;
					quoteData.PreClosePx = quoteData.PreClosePx * 500;
				}

				if (contract.VarietyID == 39) //玻璃品种
				{
					quoteData.LastPx = quoteData.LastPx * 80;
					quoteData.PreClosePx = quoteData.PreClosePx * 80;
				}
			}
			return quoteData;
		}

		private static SpotQuoteData ToQuoteData(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			var arr = value.Split(Sperator);
			if (arr.Length < 12)
			{
				return null;
			}

			var data = new SpotQuoteData();
			data.LastPx = arr[0].ToFloat();
			data.OpenPx = arr[1].ToFloat();
			data.HighPx = arr[2].ToFloat();
			data.LowPx = arr[3].ToFloat();
			data.PreClosePx = arr[4].ToFloat();
			data.HqDate = arr[5].ToInt();
			data.HqTime = arr[6].ToInt();
			data.PreSettlementPx = arr[7].ToFloat();
			data.PreOpenInterest = arr[8].ToLong();
			data.OpenInterest = arr[9].ToLong();
			data.TotalVolumeTrade = arr[10].ToLong();
			data.TotalValueTrade = arr[11].ToFloat();

			return data;
		}

		/// <summary>
		/// 设置最新价
		/// </summary>
		/// <param name="contractId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static bool SetInnerCodeNowPx(Int64 contractId, SpotQuoteData data)
		{
			try
			{
				string key = GetSpotQuoteDataKey(contractId);
				var sb = new StringBuilder();
				sb.Append(data.LastPx);
				sb.Append(Sperator);
				sb.Append(data.OpenPx);
				sb.Append(Sperator);
				sb.Append(data.HighPx);
				sb.Append(Sperator);
				sb.Append(data.LowPx);
				sb.Append(Sperator);
				sb.Append(data.PreClosePx);
				sb.Append(Sperator);
				sb.Append(data.HqDate);
				sb.Append(Sperator);
				sb.Append(data.HqTime);
				sb.Append(Sperator);
				sb.Append(data.PreSettlementPx);
				sb.Append(Sperator);
				sb.Append(data.PreOpenInterest);
				sb.Append(Sperator);
				sb.Append(data.OpenInterest);
				sb.Append(Sperator);
				sb.Append(data.TotalVolumeTrade);
				sb.Append(Sperator);
				sb.Append(data.TotalValueTrade);
				RedisManager.QuoteRedis.Set(key, sb.ToString());
				return true;
			}
			catch (Exception exp)
			{
				LogRecord.writeLogsingle("error", "SetSpotQuoteDataInnerCodeNowPx:" + exp.ToString());
				return false;
			}
		}

		private static string GetSpotQuoteDataKey(Int64 cID)
		{
			return string.Format("SpotMarket:{0}:px", cID);
		}

	}
	/// <summary>
	/// 现货行情实体
	/// </summary>
	public class SpotQuoteData
	{
		/// <summary>
		/// 现价
		/// </summary>
		public float LastPx { get; set; }

		/// <summary>
		/// 开盘价
		/// </summary>
		public float OpenPx { get; set; }

		/// <summary>
		/// 最高价
		/// </summary>
		public float HighPx { get; set; }

		/// <summary>
		/// 最低价
		/// </summary>
		public float LowPx { get; set; }

		/// <summary>
		/// 昨收
		/// </summary>
		public float PreClosePx { get; set; }

		/// <summary>
		/// 日期 yyyyMMdd
		/// </summary>
		public int HqDate { get; set; }

		/// <summary>
		/// 时间 HHmmss
		/// </summary>
		public int HqTime { get; set; }

		/// <summary>
		/// 前结算
		/// </summary>
		public float PreSettlementPx { get; set; }

		/// <summary>
		/// 昨持仓
		/// </summary>
		public Int64 PreOpenInterest { get; set; }
		/// <summary>
		/// 持仓量
		/// </summary>
		public Int64 OpenInterest { get; set; }
		/// <summary>
		/// 总成交量
		/// </summary>
		public Int64 TotalVolumeTrade { get; set; }

		/// <summary>
		/// 总成交额
		/// </summary>
		public float TotalValueTrade { get; set; }
		public DateTime GetHqTime()
		{
			if (HqDate <= 0 || HqTime <= 0) return DateTime.MinValue;
			var hqDateTime = this.HqDate.ToDate();
			hqDateTime = hqDateTime.AddHours(HqTime / 10000);
			hqDateTime = hqDateTime.AddMinutes(HqTime / 100 % 100);
			hqDateTime = hqDateTime.AddSeconds(HqTime % 100);

			return hqDateTime;
		}
	}
}