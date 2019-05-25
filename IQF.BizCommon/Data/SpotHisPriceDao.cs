using Dapper;
using IQF.BizCommon.Data.Entity;
using IQF.Framework.Cache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IQF.BizCommon.Data
{
	public class SpotHisPriceDao
	{
		private static readonly ICacheInterceptor SpotHisPriceCache = CacheInterceptorFactory.Create<long, DateTime, DateTime, List<HisSpotDayKline>>(GetFromDB);

		public static int Add(List<SpotHisPriceEntity> data)
		{
			if (data == null)
			{
				return 0;
			}
			string sql = "insert into SpotHisPrice(SpotID,HighPx,OpenPx,LowPx,LastPx,PreClosePx,Vol,TradeDate) values(@SpotID,@HighPx,@OpenPx,@LowPx,@LastPx,@PreClosePx,@Vol,@TradeDate)";
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				return conn.Execute(sql, data);
			}
		}

		public static int AddOrSet(SpotHisPriceEntity data)
		{
			if (data == null)
			{
				return 0;
			}
			string sql = "update SpotHisPrice set HighPx=@HighPx,OpenPx=@OpenPx,LowPx=@LowPx,LastPx=@LastPx,PreClosePx=@PreClosePx,Vol=@Vol,UpdateTime=getdate() where @SpotID=SpotID and TradeDate=@TradeDate IF @@ROWCOUNT=0 begin insert into SpotHisPrice(SpotID,HighPx,OpenPx,LowPx,LastPx,PreClosePx,Vol,TradeDate) values(@SpotID,@HighPx,@OpenPx,@LowPx,@LastPx,@PreClosePx,@Vol,@TradeDate) end";
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				return conn.Execute(sql, data);
			}
		}

		/// <summary>
		/// 获取现货历史价格
		/// </summary>
		/// <param name="tradeDate">交易日</param>
		/// <returns></returns>
		public static List<SpotHisPriceEntity> Get(DateTime tradeDate)
		{
			string sql = "select * from SpotHisPrice where tradeDate =@tradeDate";
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				return conn.Query<SpotHisPriceEntity>(sql, new { tradeDate }).AsList();
			}
		}

		public static Dictionary<DateTime, HisSpotDayKline> GetDicHisSpotDayKlines(long spotId, DateTime startData)
		{
			var hisSpotDayKlines = GetHisSpotDayKlines(spotId, startData, DateTime.Now);
			var dicHisSpotDayKline = new Dictionary<DateTime, HisSpotDayKline>();
			if (hisSpotDayKlines != null && hisSpotDayKlines.Count > 0)
			{
				foreach (var spotDayKline in hisSpotDayKlines)
				{
					if (!dicHisSpotDayKline.ContainsKey(spotDayKline.TradeDate))
					{
						dicHisSpotDayKline.Add(spotDayKline.TradeDate, spotDayKline);
					}
				}
			}

			return dicHisSpotDayKline;
		}


		/// <summary>
		/// 历史周期性现货数据
		/// </summary>
		/// <param name="spotId"></param>

		/// <returns></returns>
		public static List<HisSpotDayKline> GetHisSpotDayKlines(long spotId, DateTime startDate, DateTime endDate)
		{
			return SpotHisPriceCache.Execute(new List<HisSpotDayKline>(), spotId, startDate, endDate);
		}

		private static List<HisSpotDayKline> GetFromDB(long spotId, DateTime startData, DateTime endDate)
		{
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				var sql = "select spotId,highPx,lowPx,lastPx,PreClosePx,vol,tradeDate from SpotHisPrice where spotId=@spotId and tradeDate>=@tradeDate and tradeDate<=@endDate order by tradeDate desc";
				return conn.Query<HisSpotDayKline>(sql, new { spotId = spotId, tradeDate = startData, endDate }).ToList();
			}
		}

		/// <summary>
		/// 获取最新现货日K线
		/// </summary>
		/// <param name="soptId"></param>
		/// <returns></returns>
		public static HisSpotDayKline GetLastDayKline(long soptId)
		{
			var hisDayKlines = GetHisSpotDayKlines(soptId, DateTime.Now.AddDays(-100), DateTime.Now);
			if (hisDayKlines == null || hisDayKlines.Count <= 0) return null;
			return hisDayKlines.OrderByDescending(p => p.TradeDate).FirstOrDefault();
		}

	}

	public class HisSpotDayKline
	{
		public long SpotID { get; set; }

		public double HighPx { get; set; }

		public double OpenPx { get; set; }

		public double LowPx { get; set; }

		public double LastPx { get; set; }

		public double PreClosePx { get; set; }

		public int Vol { get; set; }

		public DateTime TradeDate { get; set; }
	}
}
