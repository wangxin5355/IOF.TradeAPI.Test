using Dapper;
using IQF.BizCommon.Data.Entity;
using IQF.Framework.Cache;
using System.Collections.Generic;
using System.Linq;

namespace IQF.BizCommon.Data
{

	/// <summary>
	/// 行业指数构成数据
	/// </summary>
	public class IndexComponentDao
	{
		private readonly static ICacheInterceptor indexComponentCache = CacheInterceptorFactory.Create(GetAllFromDB, 2 * 60 * 60);

		/// <summary>
		/// 根据行业指数ID 获取相关 品种指数ID
		/// </summary>
		/// <param name="industryContractId"></param>
		/// <returns></returns>
		public static List<IndexComponentDetail> GetByIndustryContractId(long industryContractId)
		{
			var all = GetAll();
			if (all == null) return null;
			return all.Where(p => p.IndustryContractId == industryContractId).ToList();
		}
		public static IndexComponentDetail Get(long industryContractId, long varietiesContractId)
		{
			var all = GetAll();
			if (all == null) return null;
			return all.FirstOrDefault(p =>
				p.IndustryContractId == industryContractId & p.VarietiesContractId == varietiesContractId);
		}

		public static List<IndexComponentDetail> GetAll()
		{
			return indexComponentCache.Execute<List<IndexComponentDetail>>();
		}

		private static List<IndexComponentDetail> GetAllFromDB()
		{
			var indexComponents = GetDbIndexComponent();
			if (indexComponents == null || indexComponents.Count <= 0)
				return null;
			var allDatas = new List<IndexComponentDetail>();
			foreach (var entity in indexComponents)
			{
				var detail = new IndexComponentDetail
				{
					IndustryContractId = entity.IndustryContractId,
					InitialValue = entity.InitialValue,
					Point = entity.Point,
					VarietyId = entity.VarietyId,
					VarietiesContractId = entity.VarietiesContractId
				};
				allDatas.Add(detail);
			}
			return allDatas;
		}

		public static bool UpdateIndexComponent(List<IndexComponentDetail> details)
		{
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				var sql =
					"update IndexComponent set Point=@Point,InitialValue=@InitialValue where IndustryContractId=@IndustryContractId and VarietiesContractId=@VarietiesContractId";
				return conn.Execute(sql, details) > 0;
			}
		}

		private static List<IndexComponentEntity> GetDbIndexComponent()
		{
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				return conn.Query<IndexComponentEntity>("select * from IndexComponent").ToList();
			}
		}
	}

	public class IndexComponentDetail
	{
		public long IndustryContractId { get; set; }
		public long VarietiesContractId { get; set; }
		public float InitialValue { get; set; }
		public double Point { get; set; }

		public int VarietyId { get; set; }
	}
}