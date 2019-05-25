using IQF.BizCommon.Data;
using IQF.BizCommon.Market.Entity;
using IQF.Framework;
using IQF.Framework.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQF.BizCommon.Market
{
	public static class RedisQuoteData
	{
		private const char Sperator = '_';
		private const string _mainContractKey = "MainContract";
		private const string _mainContractMapKey = "MainContractMap";
		private const string _mainContinuityKey = "MainContinuity";
		private static readonly ICacheInterceptor mainContractCache = CacheInterceptorFactory.Create<string, List<long>>(RedisManager.QuoteRedis.Get<List<long>>);
		private const string OutFutureMarketKey = "OutFutures";

		/// <summary>
		/// 设置主力合约
		/// </summary>
		/// <param name="contracIDs"></param>
		/// <returns></returns>
		public static bool SetMainContractId(List<Int64> contracIDs)
		{
			try
			{
				RedisManager.QuoteRedis.Set(_mainContractKey, contracIDs);
				return true;
			}
			catch (Exception ex)
			{
				LogRecord.writeLogsingle("error", "SetMainContractId" + ex);
				return false;
			}
		}
		/// <summary>
		/// 设置当前品种 对应的主力合约
		/// </summary>
		/// <param name="contractId"></param>
		/// <param name="varietyID"></param>
		/// <returns></returns>
		public static bool SetMainContractIdMap(long contractId, long varietyID)
		{
			try
			{
				RedisManager.QuoteRedis.Set(string.Format("{0}:{1}", _mainContractMapKey, varietyID), contractId);
				return true;
			}
			catch (Exception e)
			{
				LogRecord.writeLogsingle("error", "SetMainContractIdMap" + e);
				return false;
			}
		}
		/// <summary>
		/// 根据品种ID获取主力合约ID
		/// </summary>
		/// <param name="varietyID"></param>
		/// <returns></returns>
		public static long GetMainContractId(long varietyID)
		{
			try
			{
				var all = GetMainContractId();
				if (all == null || all.Count <= 0)
				{
					return 0;
				}
				foreach (var contractID in all)
				{
					var contract = ContractDao.Get(contractID);
					if (contract != null && contract.VarietyID == varietyID)
					{
						return contractID;
					}
				}
				return 0;
			}
			catch (Exception e)
			{
				LogRecord.writeLogsingle("error", "GetMainContractId" + e);
				return 0;
			}
		}

		/// <summary>
		/// 设置主力连续
		/// </summary>
		/// <param name="varietyId"></param>
		/// <param name="infos"></param>
		/// <returns></returns>
		public static bool SetMainContinuity(long varietyId, List<MainContinuityInfo> infos)
		{
			try
			{
				var key = string.Format("{0}:{1}", _mainContinuityKey, varietyId);
				return RedisManager.QuoteRedis.Set(key, infos);

			}
			catch (Exception e)
			{
				LogRecord.writeLogsingle("error", "GetMainContractId" + e);
				return false;
			}
		}
		/// <summary>
		/// 获取主力连续数据
		/// </summary>
		/// <param name="varietyId"></param>
		/// <param name="infos"></param>
		/// <returns></returns>
		public static List<MainContinuityInfo> GetMainContinuity(long varietyId)
		{
			try
			{
				var key = string.Format("{0}:{1}", _mainContinuityKey, varietyId);
				return RedisManager.QuoteRedis.Get<List<MainContinuityInfo>>(key);
			}
			catch (Exception e)
			{
				LogRecord.writeLogsingle("error", "GetMainContractId" + e);
				return null;
			}
		}

		/// <summary>
		/// 根据合约ID判断是否为主力合约
		/// </summary>
		/// <param name="ContractID"></param>
		/// <returns></returns>
		public static bool IsMainContract(Int64 ContractID)
		{
			var ids = GetMainContractId();
			return ids.Any(p => p.Equals(ContractID));
		}

		/// <summary>
		/// 获取所有主力合约 ID
		/// </summary>
		/// <returns></returns>
		public static List<Int64> GetMainContractId()
		{
			try
			{
				return mainContractCache.Execute<List<long>>(null, _mainContractKey);
			}
			catch (Exception ex)
			{
				LogRecord.writeLogsingle("error", "GetMainContractId:" + ex);
				return new List<long>();
			}
		}

		/// <summary>
		/// 设置盘口数据
		/// </summary>
		/// <param name="contractId"></param>
		/// <param name="handIcap"></param>
		/// <returns></returns>
		public static bool SetHandIcap(Int64 contractId, HandIcapCount handIcap)
		{
			try
			{
				string key = GetHandIcapKey(contractId);
				var sb = new StringBuilder();
				sb.Append(handIcap.InSize);
				sb.Append(Sperator);
				sb.Append(handIcap.OutSize);
				sb.Append(Sperator);
				sb.Append(handIcap.TotalHands);
				RedisManager.QuoteRedis.Set(key, sb.ToString());
				return true;
			}
			catch (Exception ex)
			{
				LogRecord.writeLogsingle("error", "HandIcap:" + ex.ToString());
				return false;
			}
		}

		/// <summary>
		/// 获取盘口
		/// </summary>
		/// <param name="contractId"></param>
		/// <param name="handIcap"></param>
		/// <returns></returns>
		public static bool GetHandIcap(Int64 contractId, out HandIcapCount handIcap)
		{
			string key = GetHandIcapKey(contractId);
			try
			{
				var value = RedisManager.QuoteRedis.Get<string>(key);
				if (string.IsNullOrWhiteSpace(value))
				{
					handIcap = null;
					return false;
				}
				var arr = value.Split(Sperator);
				if (arr.Length < 3)
				{
					handIcap = null;
					return false;
				}
				handIcap = new HandIcapCount();
				handIcap.InSize = arr[0].ToLong();
				handIcap.OutSize = arr[1].ToLong();
				handIcap.TotalHands = arr[2].ToLong();
				return true;

			}
			catch (Exception ex)
			{
				handIcap = null;
				LogRecord.writeLogsingle("error", "HandIcap:" + ex.ToString());
				return false;
			}
		}

		/// <summary>
		/// 设置最新价
		/// </summary>
		/// <param name="contractId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static bool SetInnerCodeNowPx(Int64 contractId, QuoteData data)
		{
			try
			{
				string key = GetQuoteDataKey(contractId);
				var val = GetQuoteDataVal(data);

				RedisManager.QuoteRedis.Set(key, val);
				return true;
			}
			catch (Exception exp)
			{
				LogRecord.writeLogsingle("error", "SetInnerCodeNowPx:" + exp.ToString());
				return false;
			}
		}

		public static string GetQuoteDataVal(QuoteData data)
		{
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
			sb.Append(data.UpLimitPx);
			sb.Append(Sperator);
			sb.Append(data.DownLimitPx);
			sb.Append(Sperator);
			sb.Append(data.AvgPx);
			sb.Append(Sperator);
			sb.Append(data.OpenInterest);
			sb.Append(Sperator);
			sb.Append(data.AddPos);
			sb.Append(Sperator);
			sb.Append(data.CurrHands);
			sb.Append(Sperator);
			sb.Append(data.BidPx1);
			sb.Append(Sperator);
			sb.Append(data.BidSize1);
			sb.Append(Sperator);
			sb.Append(data.AskPx1);
			sb.Append(Sperator);
			sb.Append(data.AskSize1);
			sb.Append(Sperator);
			sb.Append(data.TotalVolumeTrade);
			sb.Append(Sperator);
			sb.Append(data.TotalValueTrade);
			sb.Append(Sperator);
			sb.Append(data.SettlementPx);
			return sb.ToString();
		}

		/// <summary>
		/// 获取外盘抓取行情
		/// </summary>
		/// <returns></returns>
		public static List<OutFutureMarketEntity> GetOutFutureMarket()
		{
			var outFutures = RedisManager.QuoteRedis.Get<List<OutFutureMarketEntity>>(OutFutureMarketKey);
			return outFutures;
		}

		/// <summary>
		/// 获取最新行情
		/// </summary>
		/// <param name="contractId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static bool GetQuoteData(long contractId, out QuoteData data)
		{
			data = GetQuoteData(contractId);
			return data != null;
		}

		/// <summary>
		/// 获取最新行情
		/// </summary>
		/// <param name="contractId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static QuoteData GetQuoteData(long contractId)
		{
			try
			{
				var key = GetQuoteDataKey(contractId);
				var value = RedisManager.QuoteRedis.Get<string>(key);
				return ToQuoteData(value);
			}
			catch (Exception ex)
			{
				LogRecord.writeLogsingle("error", "GetQuoteData:" + ex.ToString());
				return null;
			}
		}

		/// <summary>
		/// 批量获取innerCode现价
		/// 2017-8-1 赵旸 新增批量获取数据
		/// </summary>
		/// <param name="cIDs">合约编号数据集</param>
		/// <returns></returns>
		public static Dictionary<long, QuoteData> GetQuoteData(IEnumerable<long> contracts)
		{
			if (contracts == null || contracts.Count() <= 0)
			{
				return new Dictionary<long, QuoteData>();
			}

			var allKeyDict = new Dictionary<string, long>();
			foreach (var contractID in contracts.Distinct())
			{
				string key = GetQuoteDataKey(contractID);
				allKeyDict.Add(key, contractID);
			}

			var dict = RedisManager.QuoteRedis.GetAll<string>(allKeyDict.Keys);

			var result = new Dictionary<long, QuoteData>();
			foreach (var dataKey in dict.Keys)
			{
				var data = ToQuoteData(dict[dataKey]);
				if (data == null)
				{
					continue;
				}
				if (!allKeyDict.ContainsKey(dataKey))
				{
					continue;
				}
				var contractID = allKeyDict[dataKey];
				result.Add(contractID, data);
			}
			return result;
		}

		public static QuoteData ToQuoteData(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			var arr = value.Split(Sperator);
			if (arr.Length < 22)
			{
				return null;
			}

			var data = new QuoteData();
			data.LastPx = arr[0].ToFloat();
			data.OpenPx = arr[1].ToFloat();
			data.HighPx = arr[2].ToFloat();
			data.LowPx = arr[3].ToFloat();
			data.PreClosePx = arr[4].ToFloat();
			data.HqDate = arr[5].ToInt();
			data.HqTime = arr[6].ToInt();
			data.PreSettlementPx = arr[7].ToFloat();
			data.PreOpenInterest = arr[8].ToLong();
			data.UpLimitPx = arr[9].ToFloat();
			data.DownLimitPx = arr[10].ToFloat();
			data.AvgPx = arr[11].ToFloat();
			data.OpenInterest = arr[12].ToLong();
			data.AddPos = arr[13].ToLong();
			data.CurrHands = arr[14].ToLong();
			data.BidPx1 = arr[15].ToFloat();
			data.BidSize1 = arr[16].ToInt();
			data.AskPx1 = arr[17].ToFloat();
			data.AskSize1 = arr[18].ToInt();
			data.TotalVolumeTrade = arr[19].ToLong();
			data.TotalValueTrade = arr[20].ToFloat();
			data.SettlementPx = arr[21].ToFloat();

			return data;
		}

		public static string GetQuoteDataKey(Int64 cID)
		{
			return string.Format("IQFMarket:{0}:px", cID);
		}

		private static string GetHandIcapKey(Int64 cID)
		{
			return string.Format("HandIcap:{0}", cID);
		}
	}
	/// <summary>
	/// 主力连续
	/// </summary>
	public class MainContinuityInfo
	{
		public long VarietyID { get; set; }
		public string Symbol { get; set; }
		public Exchange Exchange { get; set; }
		public DateTime KlineDate { get; set; }
	}
	/// <summary>
	/// 盘口统计数据
	/// </summary>
	public class HandIcapCount
	{
		/// <summary>
		/// 总手
		/// </summary>
		public Int64 TotalHands { get; set; }
		/// <summary>
		/// 外盘
		/// </summary>
		public Int64 OutSize { get; set; }

		/// <summary>
		/// 内盘
		/// </summary>
		public Int64 InSize { get; set; }

	}

	/// <summary>
	/// 行情数据
	/// </summary>
	public class QuoteData
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
		/// 涨停价
		/// </summary>
		public float UpLimitPx { get; set; }

		/// <summary>
		/// 跌停价
		/// </summary>
		public float DownLimitPx { get; set; }

		/// <summary>
		/// 成交均价
		/// </summary>
		public float AvgPx { get; set; }

		/// <summary>
		/// 持仓量
		/// </summary>
		public Int64 OpenInterest { get; set; }

		/// <summary>
		/// 增仓 
		/// 当前持仓量- 上一笔持仓量
		/// </summary>
		public Int64 AddPos { get; set; }

		/// <summary>
		/// 现手
		/// 现成交量-上一笔成交量
		/// </summary>
		public Int64 CurrHands { get; set; }

		/// <summary>
		/// 买一价
		/// </summary>
		public float BidPx1 { get; set; }

		/// <summary>
		/// 买一量
		/// </summary>
		public int BidSize1 { get; set; }

		/// <summary>
		/// 卖一价
		/// </summary>
		public float AskPx1 { get; set; }

		/// <summary>
		/// 卖一量
		/// </summary>
		public int AskSize1 { get; set; }

		/// <summary>
		/// 总成交量
		/// </summary>
		public Int64 TotalVolumeTrade { get; set; }

		/// <summary>
		/// 总成交额
		/// </summary>
		public float TotalValueTrade { get; set; }

		/// <summary>
		/// 结算
		/// </summary>
		public float SettlementPx { get; set; }

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