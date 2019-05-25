using Dapper;
using IQF.BizCommon.Data.Entity;
using IQF.Framework.Cache;
using System.Collections.Generic;
using System.Linq;

namespace IQF.BizCommon.Data
{
	/// <summary>
	/// 保证金
	/// </summary>
	public class ExchBailDao
	{
		private readonly static ICacheInterceptor exchBailCache = CacheInterceptorFactory.Create(GetDbExchBails, 4 * 60 * 60);

		/// <summary>
		/// 获取合约保证金相关数据
		/// </summary>
		/// <param name="varietyId">品种ID</param>
		/// <param name="hedge">保证金类型：0-投机；1-套保；2-套利</param>
		/// <param name="code">合约代码</param>
		/// <returns></returns>
		public static decimal GetRatio(long varietyId, string code, string hedge = "0")
		{
			var allContract = GetCacheExchBailDetails();
			if (allContract == null)
			{
				return 0.1m;
			}

			List<ExchBailEntity> exchBails_temp = allContract.Where(i => i.VarietyID == varietyId && i.Code == code && i.Hedge == hedge).ToList();
			if (exchBails_temp == null || exchBails_temp.Count == 0)
			{
				exchBails_temp = allContract.Where(i => i.VarietyID == varietyId && i.Code == "!" && i.Hedge == hedge).ToList();
			}

			if (exchBails_temp == null || exchBails_temp.Count == 0)
			{
				exchBails_temp = allContract.Where(i => i.VarietyID == varietyId && i.Code == "!" && i.Hedge == "!").ToList();
			}

			if (exchBails_temp != null && exchBails_temp.Count > 0)
			{
				return exchBails_temp[0].Ratio;
			}
			return 0.1m;

		}


		/// <summary>
		/// 获取合约保证金相关数据
		/// </summary>
		/// <param name="varietyId">品种ID</param>
		/// <param name="hedge">保证金类型：! - 所有；0-投机；1-套保；2-套利</param>
		/// <param name="code">合约代码</param>
		/// <returns></returns>
		public static ExchBailEntity Get(long varietyId, string code, string hedge = "!")
		{
			var allContract = GetCacheExchBailDetails();
			if (allContract == null)
			{
				return null;
			}

			List<ExchBailEntity> exchBails_temp = allContract.Where(i => i.VarietyID == varietyId && i.Code == code && i.Hedge == hedge.ToString()).ToList();
			if (exchBails_temp == null || exchBails_temp.Count == 0)
			{
				exchBails_temp = allContract.Where(i => i.VarietyID == varietyId && i.Code == "!" && i.Hedge == hedge.ToString()).ToList();
			}

			if (exchBails_temp == null || exchBails_temp.Count == 0)
			{
				exchBails_temp = allContract.Where(i => i.VarietyID == varietyId && i.Code == "!" && i.Hedge == "!").ToList();
			}

			if (exchBails_temp != null && exchBails_temp.Count > 0)
			{
				return exchBails_temp[0];
			}
			return null;
		}

		/// <summary>
		/// 获取合约保证金相关数据(模拟交易---特殊处理：览益保证金)
		/// </summary>
		/// <param name="varietyId">品种ID</param>
		/// <param name="hedge">保证金类型：! - 所有；0-投机；1-套保；2-套利</param>
		/// <param name="code">合约代码</param>
		/// <returns></returns>
		public static ExchBailEntity GetSpeical(long varietyId, string code, string hedge = "!")
		{
			var allContract = GetCacheExchBailDetails();
			if (allContract == null)
			{
				return null;
			}

			List<ExchBailEntity> exchBails_temp = allContract.Where(i => i.VarietyID == varietyId && i.Code == "#" && i.Hedge == hedge.ToString()).ToList();
			if (exchBails_temp == null || exchBails_temp.Count == 0)
			{
				exchBails_temp = allContract.Where(i => i.VarietyID == varietyId && i.Code == "#" && i.Hedge == hedge.ToString()).ToList();
			}

			if (exchBails_temp == null || exchBails_temp.Count == 0)
			{
				exchBails_temp = allContract.Where(i => i.VarietyID == varietyId && i.Code == "#" && i.Hedge == "!").ToList();
			}

			if (exchBails_temp == null || exchBails_temp.Count == 0)
			{
				exchBails_temp = allContract.Where(i => i.VarietyID == varietyId && i.Code == "!" && i.Hedge == "!").ToList();
			}

			if (exchBails_temp != null && exchBails_temp.Count > 0)
			{
				return exchBails_temp[0];
			}
			return null;
		}

		/// <summary>
		/// 获取缓存的合约相关保证金数据
		/// </summary>
		/// <returns></returns>
		public static List<ExchBailEntity> GetCacheExchBailDetails()
		{
			return exchBailCache.Execute(new List<ExchBailEntity>());
		}

		/// <summary>
		/// 获取所有合约手续费相关信息
		/// </summary>
		/// <returns></returns>
		private static List<ExchBailEntity> GetDbExchBails()
		{
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				return conn.Query<ExchBailEntity>("select * from ExchBail;").AsList();
			}
		}
	}
}
