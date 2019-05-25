using Dapper;
using IQF.BizCommon.Data.Entity;
using IQF.Framework.Cache;
using System.Collections.Generic;
using System.Linq;

namespace IQF.BizCommon.Data
{
	/// <summary>
	/// 手续费
	/// </summary>
	public class ExchFareDao
	{
		private readonly static ICacheInterceptor exchFareCache = CacheInterceptorFactory.Create(GetDbExchFares, 4 * 60 * 60);

		/// <summary>
		/// 获取合约手续费相关数据
		/// </summary>
		/// <param name="varietyID">品种ID</param>
		/// <param name="fareType">手续费类型：1开仓;2-平仓;3交割</param>
		/// <param name="code">合约代码</param>
		/// <returns></returns>
		public static ExchFareEntity Get(long varietyID, int fareType, string code)
		{
			var allContract = GetCacheExchFares();
			if (allContract == null)
			{
				return null;
			}

			List<ExchFareEntity> exchFares_temp = allContract.Where(i => i.VarietyID == varietyID && i.Code == code && i.FareType == fareType).ToList();
			if (exchFares_temp == null || exchFares_temp.Count == 0)
			{
				exchFares_temp = allContract.Where(i => i.VarietyID == varietyID && i.Code == "!" && i.FareType == fareType).ToList();
			}

			if (exchFares_temp != null && exchFares_temp.Count > 0)
			{
				return exchFares_temp[0];
			}
			return null;
		}


		/// <summary>
		/// 获取缓存的合约相关保证金数据
		/// </summary>
		/// <returns></returns>
		public static List<ExchFareEntity> GetCacheExchFares()
		{
			return exchFareCache.Execute<List<ExchFareEntity>>();
		}

		/// <summary>
		/// 获取所有合约手续费相关信息
		/// </summary>
		/// <returns></returns>
		private static List<ExchFareEntity> GetDbExchFares()
		{
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				return conn.Query<ExchFareEntity>("SELECT * FROM ExchFare;").AsList();
			}
		}

	}//End class ExchFareDao
}//End namespace IQF.BizCommon.Data
