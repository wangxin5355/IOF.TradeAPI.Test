using Dapper;
using IQF.BizCommon.Data.Entity;
using IQF.Framework;
using IQF.Framework.Cache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IQF.BizCommon.Data
{
	public class SpotContractDao
	{
		private readonly static ICacheInterceptor SpotContractCache = CacheInterceptorFactory.Create(GetFromDB);

		private readonly static ICacheInterceptor SoptByExchangeCache = CacheInterceptorFactory.Create<Exchange, List<SpotContractDetail>>(GetSoptByExchangeFormDb);

		/// <summary>
		/// 通过现货名称获取现货数据
		/// </summary>
		/// <param name="spotName"></param>
		/// <returns></returns>
		public static SpotContractDetail Get(string spotName)
		{
			var list = GetAll();
			if (null == list)
			{
				return null;
			}
			return list.Find(a => a.SpotName == spotName);
		}
		public static SpotContractDetail Get(long spotId)
		{
			var list = GetAll();
			if (null == list)
			{
				return null;
			}
			return list.FirstOrDefault(p => p.SpotID == spotId);
		}
		public static SpotContractDetail GetBySpotCode(string spotCode)
		{
			var all = GetAll();
			return all.FirstOrDefault(p => string.Equals(p.SpotCode, spotCode, StringComparison.CurrentCultureIgnoreCase));
		}
		/// <summary>
		/// 获取所有品种
		/// </summary>
		/// <returns></returns>
		public static List<int> GetAllVarietyId()
		{
			var varietyIds = new List<int>();
			var allDatas = GetAll();
			if (allDatas == null || allDatas.Count <= 0) return varietyIds;
			varietyIds = allDatas.Select(p => p.VarietyID).ToList();
			return varietyIds;
		}
		/// <summary>
		/// 根据交易所获取品种
		/// </summary>
		/// <param name="exchange"></param>
		/// <returns></returns>
		public static List<int> GetVarietyIds(Exchange exchange)
		{
			var spotContracts = GetSpotContractByExchange(exchange);
			var varietyIds = new List<int>();
			if (spotContracts == null || spotContracts.Count <= 0)
			{
				return varietyIds;
			}

			varietyIds = spotContracts.Select(p => p.VarietyID).ToList();
			return varietyIds;
		}

		/// <summary>
		/// 根据交易所获取现货合约
		/// </summary>
		/// <param name="exchange"></param>
		/// <returns></returns>
		public static List<SpotContractDetail> GetSpotContractByExchange(Exchange exchange)
		{
			return SoptByExchangeCache.Execute<List<SpotContractDetail>>(args: exchange);
		}

		public static List<SpotContractDetail> GetAll()
		{
			return SpotContractCache.Execute<List<SpotContractDetail>>();
		}

		private static List<SpotContractDetail> ConvertSpot(IEnumerable<SpotContractEntity> datas)
		{
			if (datas == null || datas.Count() <= 0)
			{
				return null;
			}
			var details = new List<SpotContractDetail>();
			foreach (var item in datas)
			{
				details.Add(new SpotContractDetail
				{
					SpotID = item.SpotID,
					SpotCode = item.SpotCode,
					SpotName = item.SpotName,
					VarietyID = item.VarietyID
				});
			}
			return details;
		}

		private static List<SpotContractDetail> GetSoptByExchangeFormDb(Exchange exchange)
		{
			var sql = @"SELECT [SpotID]
				,[SpotCode]
				,[SpotName]
				,[VarietyID]
			FROM [DB_IQFData].[dbo].[SpotContract] t where t.VarietyID in (SELECT [ID]
			FROM [DB_IQFData].[dbo].[Variety] where EID=@exchange)";
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				var datas = conn.Query<SpotContractEntity>(sql).AsList();
				return ConvertSpot(datas);
			}
		}

		private static List<SpotContractDetail> GetFromDB()
		{
			string sql = "select * from SpotContract";
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				var datas = conn.Query<SpotContractEntity>(sql).AsList();

				return ConvertSpot(datas);
			}
		}
	}
	public class SpotContractDetail
	{
		public long SpotID { get; set; }

		public string SpotCode { get; set; }

		public string SpotName { get; set; }

		public int VarietyID { get; set; }
	}
}
