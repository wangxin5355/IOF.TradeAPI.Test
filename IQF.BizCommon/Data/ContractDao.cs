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
	/// 合约数据访问
	/// </summary>
	public class ContractDao
	{
		private static readonly ICacheInterceptor contractDetailCache = CacheInterceptorFactory.Create(GetContractFromDB);

		/// <summary>
		/// 合约编号、合约代码映射关系
		/// 主要为了性能优化
		/// </summary>
		private static readonly ICacheInterceptor idSymbolMappingCache = CacheInterceptorFactory.Create(BuildMappingInfo);

		/// <summary>
		/// 最后一次读取数据库的时间
		/// </summary>
		private static DateTime lastReadDbTime = DateTime.MinValue;

		/// <summary>
		/// 获取活跃的合约信息
		/// </summary>
		/// <returns></returns>
		public static List<ContractDetail> GetActiveContracts()
		{
			return GetCacheContractDetails().Where(p => p.ExpireDate >= DateTime.Now.Date).ToList();
		}

		/// <summary>
		/// 获取合约编号
		/// </summary>
		/// <param name="symbol"></param>
		/// <returns></returns>
		public static long GetContractID(string symbol)
		{
			var mapping = GetAllMapping();
			if (mapping == null || !mapping.ContainsKey(symbol))
			{
				return -1;
			}
			return mapping[symbol];
		}

		/// <summary>
		/// 获取合约详情数据
		/// </summary>
		/// <param name="exchange">交易所</param>
		/// <param name="symbol">合约代码</param>
		/// <returns></returns>
		public static ContractDetail Get(Exchange exchange, string symbol)
		{
			var contract = Get(symbol);
			if (contract == null || contract.Exchange != exchange)
			{
				return null;
			}
			return contract;
		}

		/// <summary>
		/// 获取合约详情数据
		/// </summary>
		/// <param name="symbol"></param>
		/// <returns></returns>
		public static ContractDetail Get(string symbol)
		{
			var contractID = GetContractID(symbol);
			if (contractID == -1)
			{
				return null;
			}
			return Get(contractID);
		}

		/// <summary>
		/// 获取合约详情数据
		/// </summary>
		/// <param name="contractID">合约编号</param>
		/// <returns></returns>
		public static ContractDetail Get(long contractID)
		{
			var all = GetAll();
			if (all == null || !all.ContainsKey(contractID))
			{
				return null;
			}
			return all[contractID];
		}

		/// <summary>
		/// 获取缓存的合约详情数据
		/// 可能会包含期权和股票的数据，默认仅返回期货数据
		/// </summary>
		/// <returns></returns>
		public static List<ContractDetail> GetCacheContractDetails()
		{
			var all = GetAll();
			if (all == null)
			{
				return new List<ContractDetail>();
			}
			return all.Values.ToList();
		}

		/// <summary>
		/// 获取所有合约数据
		/// </summary>
		/// <returns></returns>
		public static Dictionary<long, ContractDetail> GetAll()
		{
			RemoveInvalidCache();
			return contractDetailCache.Execute(new Dictionary<long, ContractDetail>());
		}

		/// <summary>
		/// 获取所有合约映射关系
		/// </summary>
		/// <returns></returns>
		private static Dictionary<string, long> GetAllMapping()
		{
			RemoveInvalidCache();
			return idSymbolMappingCache.Execute(new Dictionary<string, long>());
		}

		private static Dictionary<long, ContractDetail> GetContractFromDB()
		{
			var contracts = GetDbContracts();
			if (contracts == null)
			{
				return null;
			}

			var result = new Dictionary<long, ContractDetail>();
			foreach (var contract in contracts)
			{
				var detail = new ContractDetail
				{
					ContractID = contract.ContractID,
					VarietyID = contract.VarietyID,
					Symbol = contract.Code,
					ContractName = contract.ContractName,
					Exchange = (Exchange)contract.Exchange,
					OpenDate = contract.OpenDate,
					ExpireDate = contract.ExpireDate,
					FutureType = FutureType.General,
					StartDeliverDate = contract.StartDeliverDate,
					EndDeliverDate = contract.StartDeliverDate
				};
				result.Add(contract.ContractID, detail);
			}
			return result;
		}

		private static Dictionary<string, long> BuildMappingInfo()
		{
			var all = GetAll();
			if (all == null || all.Count <= 0)
			{
				return null;
			}

			return all.ToDictionary(k => k.Value.Symbol, v => v.Key);
		}

		/// <summary>
		/// 删除无效缓存
		/// 合约信息会不定时更新，开盘前强制重刷缓存数据
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
			contractDetailCache.Remove();
			idSymbolMappingCache.Remove();
		}

		/// <summary>
		/// 获取所有合约信息
		/// </summary>
		/// <returns></returns>
		private static List<ContractEntity> GetDbContracts()
		{
			lastReadDbTime = DateTime.Now;
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				//过滤掉垃圾数据
				return conn.Query<ContractEntity>("select * from Contract t where t.VarietyID > 0;").AsList();
			}
		}
	}// End class ContractDao.

	/// <summary>
	/// 合约详情
	/// </summary>
	[DataContract]
	public class ContractDetail
	{
		/// <summary>
		/// 合约ID
		/// </summary>
		[DataMember]
		public long ContractID { get; set; }
		/// <summary>
		/// 品种编号
		/// </summary>
		[DataMember]
		public long VarietyID { get; set; }
		/// <summary>
		/// 合约代码
		/// </summary>
		[DataMember]
		public string Symbol { get; set; }
		/// <summary>
		/// 合约名称
		/// </summary>
		[DataMember]
		public string ContractName { get; set; }
		/// <summary>
		/// 交易所
		/// </summary>
		[DataMember]
		public Exchange Exchange { get; set; }

		/// <summary>
		///上市日期
		/// </summary>
		[DataMember]
		public DateTime OpenDate { get; set; }
		/// <summary>
		/// 到期日
		/// </summary>
		[DataMember]
		public DateTime ExpireDate { get; set; }
		/// <summary>
		/// 开始交割日期
		/// </summary>
		[DataMember]
		public DateTime StartDeliverDate { get; set; }
		/// <summary>
		/// 最后交割日期
		/// </summary>
		[DataMember]
		public DateTime EndDeliverDate { get; set; }
		/// <summary>
		/// 合约类型
		/// </summary>
		[DataMember]
		public FutureType FutureType { get; set; }

		public override string ToString()
		{
			return JsonHelper.Serialize(this);
		}

		public ContractDetail()
		{
			FutureType = FutureType.General;
		}

		/// <summary>
		/// 是否到期
		/// </summary>
		/// <returns></returns>
		public bool IsExpire()
		{
			return this.ExpireDate < DateTime.Now.Date;
		}
	}// End class ContractDetail.
}

public enum FutureType
{
    /// <summary>
    /// 常规合约
    /// </summary>
    General = 1,
    /// <summary>
    /// 主连合约
    /// </summary>
    Main = 2,
    /// <summary>
    /// 指数合约
    /// </summary>
    Index = 3,
    /// <summary>
    /// 期现
    /// </summary>
    FutureSpot = 4,
    /// <summary>
    /// 现货
    /// </summary>
    Spot = 5,
    /// <summary>
    /// 套利品种
    /// </summary>
    ArbitrageVariety = 6,
    /// <summary>
    /// 日仓单数据
    /// </summary>
    DailyStock = 7
}
