using Dapper;
using IQF.BizCommon.Data.Entity;
using IQF.Framework;
using IQF.Framework.Cache;
using IQF.Framework.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace IQF.BizCommon.Data
{
	/// <summary>
	/// 品种数据访问
	/// </summary>
	public class VarietyDao
	{
		private const int NightBeginTime = 210000;

		private static readonly ICacheInterceptor VariSortCache = CacheInterceptorFactory.Create(GetVariSortList);

		private static readonly ICacheInterceptor VeriDetailCache = CacheInterceptorFactory.Create(GetVariDetailFromDB);

		private static readonly ICacheInterceptor VariIDCache = CacheInterceptorFactory.Create(BuildMappingInfo);

		/// <summary>
		/// 最后一次读取数据库的时间
		/// </summary>
		private static DateTime lastReadDbTime = DateTime.MinValue;

		/// <summary>
		/// 根据交易所和品种代码获取品种信息
		/// </summary>
		/// <param name="exchange"></param>
		/// <param name="variCode"></param>
		/// <returns></returns>
		public static VarietyDetail Get(Exchange exchange, string variCode)
		{
			long vid = GetVarietyID(exchange, variCode);
			return Get(vid);
		}

		/// <summary>
		/// 根据交易所和品种代码获取品种信息
		/// </summary>
		/// <param name="exchange"></param>
		/// <param name="variCode"></param>
		/// <returns></returns>
		public static VarietyDetail Get(string variCode)
		{
			long vid = GetVarietyID(variCode);
			return Get(vid);
		}

		/// <summary>
		/// 失败返回0，带缓存
		/// </summary>
		/// <param name="exchange"></param>
		/// <param name="variCode"></param>
		/// <returns></returns>
		public static long GetVarietyID(Exchange exchange, string variCode)
		{
			return GetVarietyID(variCode);
		}

		/// <summary>
		/// 失败返回0，带缓存
		/// </summary>
		/// <param name="exchange"></param>
		/// <param name="variCode"></param>
		/// <returns></returns>
		public static long GetVarietyID(string variCode)
		{
			RemoveInvalidCache();
			var dict = VariIDCache.Execute<Dictionary<string, long>>();
			if (dict == null || string.IsNullOrWhiteSpace(variCode) || !dict.ContainsKey(variCode))
			{
				return 0;
			}
			return dict[variCode];
		}

		/// <summary>
		/// 根据品种编号获取品种信息，带缓存
		/// </summary>
		/// <param name="verietyID"></param>
		/// <returns></returns>
		public static VarietyDetail Get(long verietyID)
		{
			var all = GetCacheVeriDetail();
			if (all == null)
			{
				return null;
			}
			if (all.ContainsKey(verietyID))
			{
				return all[verietyID];
			}
			else
			{
				return new VarietyDetail();
			}
		}

		/// <summary>
		/// 根据行业分类获取品种
		/// </summary>
		/// <param name="indCatgId">行业id</param>
		/// <returns></returns>
		public static List<VarietyDetail> GetByIndCatg(int indCatgId)
		{
			var all = GetCacheVeriDetail();
			if (all == null)
			{
				return null;
			}
			return all.Values.Where(w => w.VariIndCatgList.Exists(e => e.IndCatgID == indCatgId)).ToList();
		}

		/// <summary>
		/// 获取所有行业信息
		/// </summary>
		/// <returns></returns>
		public static List<VeriIndCatg> GetAllIndCatg()
		{
			var all = GetCacheVeriDetail();
			if (all == null)
			{
				return null;
			}
			var allCatg = all.Values.Select(s => s.VariIndCatgList);
			var result = new List<VeriIndCatg>();
			foreach (var list in allCatg)
			{
				foreach (var item in list)
				{
					if (!result.Exists(e => e.IndCatgID == item.IndCatgID))
					{
						result.Add(item);
					}
				}
			}
			return result;
		}
		/// <summary>
		/// 获取含有夜盘的 品种
		/// </summary>
		/// <returns></returns>
		public static List<VarietyDetail> GetNightVeriDetail()
		{
			var all = GetCacheVeriDetail();
			if (all == null)
			{
				return null;
			}
			return all.Values.Where(p => p.TradeTimeList.Exists(x => x.BeginTime >= NightBeginTime)).ToList();
		}
		/// <summary>
		/// 根据品种ID和当前时间 判断下个间隔时间
		/// </summary>
		/// <param name="time">当前时间</param>
		/// <param name="vid">品种ID</param>
		/// <param name="minute">间隔分钟</param>
		/// <returns></returns>
		public static DateTime GetNextTime(DateTime time, long vid, int minute)
		{
			var nextTime = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);

			int i = 0;
			var preTime = nextTime;
			while (i < minute)
			{
				nextTime = nextTime.AddMinutes(1);

				if (TimeDao.IsTradingTime(nextTime, vid))
				{
					if (preTime.AddMinutes(1) == nextTime)
					{
						i = i + 1;
					}
					preTime = nextTime;
				}
			}
			var hhmmss = nextTime.GetHHMMSS();
			var variety = Get(vid);
			var timeRange = variety.GetTidyTradeTime();
			for (int j = 0; j < timeRange.Count; j++)
			{
				if (timeRange[j].EndTime != hhmmss) continue;
				if (j + 1 < timeRange.Count)
				{
					hhmmss = timeRange[j + 1].BeginTime;
				}
				var date = TimeDao.GetTradeDate(nextTime);
				nextTime = new DateTime(date.Year, date.Month, date.Day, hhmmss / 10000, hhmmss / 100 % 100, 0);
			}
			return nextTime;
		}
		public static DateTime GetNextTimeByContractId(DateTime time, List<TradeTimeRange> timeRange, long contractId, int minute)
		{
			var nextTime = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);

			int i = 0;
			var preTime = nextTime;
			while (i < minute)
			{
				nextTime = nextTime.AddMinutes(1);

				if (TimeDao.IsTradingTimeByContractId(nextTime, contractId))
				{
					if (preTime.AddMinutes(1) == nextTime)
					{
						i = i + 1;
					}
					preTime = nextTime;
				}
			}
			var hhmmss = nextTime.GetHHMMSS();
			for (int j = 0; j < timeRange.Count; j++)
			{
				if (timeRange[j].EndTime != hhmmss) continue;
				if (j + 1 < timeRange.Count)
				{
					hhmmss = timeRange[j + 1].BeginTime;
				}
				var date = TimeDao.GetTradeDate(nextTime);
				nextTime = new DateTime(date.Year, date.Month, date.Day, hhmmss / 10000, hhmmss / 100 % 100, 0);
			}
			return nextTime;
		}

		/// <summary>
		/// 获取品种排序信息
		/// </summary>
		/// <returns></returns>
		public static List<VariSort> GetCacheVariSorts()
		{
			return VariSortCache.Execute(new List<VariSort>());
		}

		/// <summary>
		/// 获取缓存的品种详情数据
		/// </summary>
		/// <returns></returns>
		public static Dictionary<long, VarietyDetail> GetCacheVeriDetail()
		{
			RemoveInvalidCache();
			return VeriDetailCache.Execute<Dictionary<long, VarietyDetail>>();
		}

		private static Dictionary<string, long> BuildMappingInfo()
		{
			var dict = GetCacheVeriDetail();
			if (dict == null)
			{
				return null;
			}
			var temp = new Dictionary<string, long>();
			foreach (var item in dict.Values)
			{
				temp.Add(item.VarietyCode, item.VarietyID);
			}
			return temp;
		}

		private static Dictionary<long, VarietyDetail> GetVariDetailFromDB()
		{
			lastReadDbTime = DateTime.Now;
			var result = GetDbVeriDetail();
			if (result == null)
			{
				return null;
			}
			return result.ToDictionary(k => k.VarietyID, v => v);
		}

		/// <summary>
		/// 删除无效缓存
		/// 品种信息会不定时更新，开盘前强制重刷缓存数据
		/// </summary>
		private static void RemoveInvalidCache()
		{
			var nowTime = DateTime.Now;
			if (lastReadDbTime >= nowTime)
			{
				return;
			}
			if (nowTime.Hour != 8 && nowTime.Hour != 20)//当前时间在开盘时间前1小时
			{
				return;
			}
			if ((nowTime - lastReadDbTime).Minutes > 5) //当前时间差大于5分钟时，删除缓存
			{
				ClearCache();
			}
		}

		/// <summary>
		/// 清除缓存，紧急处理时使用，比如新增品种时开盘前没有维护好
		/// </summary>
		public static void ClearCache()
		{
			VariIDCache.Remove();

			VeriDetailCache.Remove();

			VariSortCache.Remove();
		}

		/// <summary>
		/// 从数据库获取品种详情
		/// </summary>
		/// <returns></returns>
		public static List<VarietyDetail> GetDbVeriDetail()
		{
			var varieties = GetDbVarieties();
			if (varieties == null || varieties.Count <= 0)
			{
				return null;
			}

			var openTimes = GetDbVarietyOpenTimes();
			if (openTimes == null)
			{
				return null;
			}

			var indCatgList = GetDbIndCatgList();
			if (indCatgList == null)
			{
				return null;
			}

			var veriIndList = GetDbVeriIndList();
			if (veriIndList == null)
			{
				return null;
			}

			var f10List = GetVarietyInfoList();


			var result = new List<VarietyDetail>();
			foreach (var variety in varieties)
			{
				var detail = new VarietyDetail();
				detail.EID = (Exchange)variety.EID;
				detail.Lots = variety.Lots;
				detail.PriceStep = variety.PriceStep;
				detail.State = variety.State;

				detail.TradeTimeList = new List<TradeTimeRange>();
				foreach (var item in openTimes.Where(w => w.VID == variety.ID))
				{
					var range = new TradeTimeRange();
					range.BeginTime = Convert.ToInt32(item.BeginTime.Replace(":", ""));
					range.EndTime = Convert.ToInt32(item.EndTime.Replace(":", ""));

					detail.TradeTimeList.Add(range);
				}

				detail.Type = variety.Type;
				detail.VarietyCode = variety.VarietyCode;
				detail.VarietyName = variety.VarietyName;
				detail.VarietyID = variety.ID;

				detail.VariIndCatgList = new List<VeriIndCatg>();
				foreach (var item in veriIndList.Where(w => w.VerietyID == variety.ID))
				{
					var indCatg = indCatgList.FirstOrDefault(f => f.Id == item.IndCatgID);
					if (indCatg == null)
					{
						continue;
					}
					var catg = new VeriIndCatg();
					catg.IndCatgID = item.IndCatgID;
					catg.Name = indCatg.Name;

					detail.VariIndCatgList.Add(catg);
				}

				detail.F10 = new Dictionary<string, string>();

				foreach (var source in f10List.Where(p => p.VarietyID == variety.ID))
				{
					if (detail.F10.ContainsKey(source.KeyName))
					{
						detail.F10[source.KeyName] = source.KeyValue;
					}
					else
					{
						detail.F10.Add(source.KeyName, source.KeyValue);
					}

				}
				result.Add(detail);
			}
			return result;
		}

		/// <summary>
		/// 获取所有品种信息
		/// </summary>
		/// <returns></returns>
		private static List<VarietyEntity> GetDbVarieties()
		{
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				return conn.Query<VarietyEntity>("select * from Variety;").AsList();
			}
		}

		/// <summary>
		/// 获取所有品种信息 开盘时间
		/// </summary>
		/// <returns></returns>
		private static List<VarietyOpenTimeEntity> GetDbVarietyOpenTimes()
		{
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				return conn.Query<VarietyOpenTimeEntity>("select * from VarietyOpenTime;").AsList();
			}
		}

		/// <summary>
		/// 获取所有行业分类信息
		/// </summary>
		/// <returns></returns>
		private static List<IndustryCatgEntity> GetDbIndCatgList()
		{
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				return conn.Query<IndustryCatgEntity>("select * from IndustryCatg;").AsList();
			}
		}

		/// <summary>
		/// 获取所有品种行业信息
		/// </summary>
		/// <returns></returns>
		private static List<VarietyIndustryEntity> GetDbVeriIndList()
		{
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				return conn.Query<VarietyIndustryEntity>("select * from VarietyIndustry;").AsList();
			}
		}
		/// <summary>
		/// 获取所有品种相关信息
		/// </summary>
		/// <returns></returns>
		private static List<VarietyInfoEntity> GetVarietyInfoList()
		{
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				return conn.Query<VarietyInfoEntity>("select * from varietyInfo").AsList();
			}
		}

		/// <summary>
		/// 获取品种排序值
		/// </summary>
		/// <returns></returns>
		private static List<VariSort> GetVariSortList()
		{
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				return conn.Query<VariSort>("select * from VarietySort").AsList();
			}
		}

	}

	/// <summary>
	/// 品种详情
	/// </summary>
	[DataContract]
	public class VarietyDetail
	{
		private const int NightBeginTime = 210000;

		/// <summary>
		/// 品种编号
		/// </summary>
		[DataMember]
		public long VarietyID { get; set; }

		/// <summary>
		/// 交易所
		/// </summary>
		[DataMember]
		public Exchange EID { get; set; }

		/// <summary>
		/// 品种代码
		/// </summary>
		[DataMember]
		public string VarietyCode { get; set; }

		/// <summary>
		/// 品种名称
		/// </summary>
		[DataMember]
		public string VarietyName { get; set; }

		/// <summary>
		/// 品种类型
		/// </summary>
		[DataMember]
		public int Type { get; set; }

		/// <summary>
		/// 状态
		/// </summary>
		[DataMember]
		public int State { get; set; }

		/// <summary>
		/// 每手股数
		/// </summary>
		public int Lots { get; set; }

		/// <summary>
		/// 最小变动价位
		/// </summary>
		[DataMember]
		public decimal PriceStep { get; set; }

		/// <summary>
		/// 交易时间范围列表
		/// </summary>
		[DataMember]
		public List<TradeTimeRange> TradeTimeList { get; set; }

		/// <summary>
		/// 品种行业分类列表
		/// </summary>
		[DataMember]
		public List<VeriIndCatg> VariIndCatgList { get; set; }

		/// <summary>
		/// F10数据
		/// </summary>
		[DataMember]
		public Dictionary<string, string> F10 { get; set; }

		public VarietyDetail()
		{
			TradeTimeList = new List<TradeTimeRange>();
			VariIndCatgList = new List<VeriIndCatg>();
		}
		/// <summary>
		/// 是否有夜盘
		/// </summary>
		/// <returns></returns>
		public bool IsNight()
		{
			if (this.TradeTimeList == null)
			{
				return false;
			}

			return this.TradeTimeList.Any(p => p.BeginTime >= NightBeginTime);
		}

		public int GetOpenTime()
		{
			if (this.TradeTimeList == null)
			{
				return 90000;
			}
			if (IsNight())
			{
				return this.TradeTimeList.Where(w => w.BeginTime >= NightBeginTime).OrderBy(o => o.BeginTime).Select(s => s.BeginTime).First();
			}
			return this.TradeTimeList.Where(w => w.BeginTime < NightBeginTime).OrderBy(o => o.BeginTime).Select(s => s.BeginTime).First();
		}

		public int GetEndTime()
		{
			if (this.TradeTimeList == null)
			{
				return 150000;
			}
			var tradeTimeRange = GetTidyTradeTime().LastOrDefault();
			if (tradeTimeRange != null) return tradeTimeRange.EndTime;
			return 150000;
		}

		/// <summary>
		/// 交易时间整理安开盘时间先后排序
		/// 白天结束后，变为下一个交易日，所以交易时间从夜盘开始
		/// </summary>
		/// <returns></returns>
		public List<TradeTimeRange> GetTidyTradeTime()
		{
			var result = new List<TradeTimeRange>();

			var list = this.TradeTimeList.Where(w => w.BeginTime >= NightBeginTime).OrderBy(o => o.BeginTime);
			result.AddRange(list);
			list = this.TradeTimeList.Where(w => w.BeginTime < NightBeginTime).OrderBy(o => o.BeginTime);
			result.AddRange(list);
			return result;
		}
		public override string ToString()
		{
			return JsonHelper.Serialize(this);
		}
	}// End class VarietyDetail.

	/// <summary>
	/// 交易时间范围
	/// </summary>
	public class TradeTimeRange
	{
		/// <summary>
		/// 开始时间 HHmmss
		/// </summary>
		public int BeginTime { get; set; }

		/// <summary>
		/// 结束时间 HHmmss
		/// </summary>
		public int EndTime { get; set; }
	}

	/// <summary>
	/// 品种行业分类
	/// </summary>
	public class VeriIndCatg
	{
		/// <summary>
		/// 行业分类编号
		/// </summary>
		public int IndCatgID { get; set; }

		/// <summary>
		/// 行业名称
		/// </summary>
		public string Name { get; set; }
	}

	/// <summary>
	/// 品种排序信息
	/// </summary>
	public class VariSort
	{
		/// <summary>
		/// 品种编号
		/// </summary>
		public long VarietyID { get; set; }
		/// <summary>
		/// 交易所
		/// </summary>
		public Exchange Exchange { get; set; }
		/// <summary>
		/// 排序号
		/// </summary>
		public int SortNum { get; set; }
	}
}