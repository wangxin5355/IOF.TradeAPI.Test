using Dapper;
using IQF.BizCommon.Data.Entity;
using IQF.Framework;
using IQF.Framework.Cache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IQF.BizCommon.Data
{
	public class AllContractDao
	{
		private static readonly ICacheInterceptor contractDetailCache = CacheInterceptorFactory.Create(GetContractFromDB);

		/// <summary>
		/// 合约编号、合约代码映射关系
		/// 主要为了性能优化
		/// </summary>
		private static readonly ICacheInterceptor idSymbolMappingCache = CacheInterceptorFactory.Create(BuildMappingInfo);
        /// <summary>
        /// 获取期现合约
        /// </summary>
        /// <returns></returns>
        public static List<ContractDetail> GetAllFutureSpotContract()
        {
            var allFutureSpots = new List<ContractDetail>();
            var allVarietyIds = SpotContractDao.GetAllVarietyId();
            var allFutureContracts = GetActiveContracts().Where(p => p.FutureType == FutureType.General).ToList();
            allFutureContracts = allFutureContracts.Where(p => allVarietyIds.Contains((int)p.VarietyID)).ToList();
            var allSpotContracts = SpotContractDao.GetAll();
            foreach (var item in allFutureContracts)
            {
                var spotContract = allSpotContracts.FirstOrDefault(p => p.VarietyID == item.VarietyID);
                if (spotContract == null)
                {
                    continue;
                }

                var contract = new ContractDetail();
                contract.ContractID = item.ContractID;
                contract.VarietyID = item.VarietyID;
                contract.Symbol = string.Format("{0}&XH", item.Symbol);
                contract.ContractName = string.Format("{0}期现", item.ContractName);
                contract.Exchange = item.Exchange;
                contract.FutureType = FutureType.FutureSpot;
                contract.ExpireDate = item.ExpireDate;
                contract.EndDeliverDate = item.EndDeliverDate;
                allFutureSpots.Add(contract);
            }
            return allFutureSpots;
        }
        /// <summary>
        /// 获取所有现货合约
        /// </summary>
        /// <returns></returns>
        public static List<ContractDetail> GetAllSpotContract()
        {
            var allSpotContracts = SpotContractDao.GetAll();
            var allFutureSpots = new List<ContractDetail>();
            foreach (var contract in allSpotContracts)
            {
                var model = new ContractDetail();
                var variety = VarietyDao.Get(contract.VarietyID);
                if (variety != null)
                {
                    model.Exchange = variety.EID;
                }
                model.ContractID = contract.SpotID;
                model.VarietyID = contract.VarietyID;
                model.Symbol = contract.SpotCode;
                model.ContractName = contract.SpotName;
                model.FutureType = FutureType.Spot;
                allFutureSpots.Add(model);
            }

            return allFutureSpots;
        }
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
		/// 获取常规合约
		/// </summary>
		/// <returns></returns>
		public static List<ContractDetail> GetGeneralContract()
		{
			return GetCacheContractDetails().Where(p => p.FutureType == FutureType.General).ToList();
		}
		/// <summary>
		/// 获取活跃常规合约
		/// </summary>
		/// <returns></returns>
		public static List<ContractDetail> GetActiveGeneralContract()
		{
			return GetCacheContractDetails().Where(p => p.FutureType == FutureType.General && p.ExpireDate >= DateTime.Now.Date).ToList();
		}
		/// <summary>
		/// 获取所有指数合约
		/// </summary>
		/// <returns></returns>
		public static List<ContractDetail> GetIndexContract()
		{
			return GetCacheContractDetails().Where(p => p.FutureType == FutureType.Index).ToList();
		}
		/// <summary>
		/// 行业指数
		/// </summary>
		/// <returns></returns>
		public static List<ContractDetail> GetIndustryIndexContract()
		{
			return GetIndexContract().Where(p => p.ContractID > 97000 && p.ContractID < 98000).ToList();
		}
		/// <summary>
		/// 是否为行业指数
		/// </summary>
		/// <param name="contractId"></param>
		/// <returns></returns>
		public static bool IsIndustryIndexContract(long contractId)
		{
			if (contractId > 97000 && contractId < 98000) return true;
			return false;
		}

		/// <summary>
		/// 品种指数
		/// </summary>
		/// <returns></returns>
		public static List<ContractDetail> GetVarietiesIndexContract()
		{
			return GetIndexContract().Where(p => p.ContractID > 98000).ToList();
		}

		/// <summary>
		/// 获取主力合约
		/// </summary>
		/// <returns></returns>
		public static List<ContractDetail> GetMainContract()
		{
			return GetCacheContractDetails().Where(p => p.FutureType == FutureType.Main).ToList();
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

			var vContracts = GetDbVirtualContracts();
			if (vContracts != null)
			{
				foreach (var contract in vContracts)
				{
					var detail = new ContractDetail
					{
						ContractID = contract.ContractID,
						VarietyID = contract.VarietyID,
						Symbol = contract.Symbol,
						ContractName = contract.ContractName,
						Exchange = (Exchange)contract.Exchange,
						OpenDate = contract.OpenDate,
						ExpireDate = contract.ExpireDate,
						FutureType = contract.FutureType,
						StartDeliverDate = contract.StartDeliverDate,
						EndDeliverDate = contract.StartDeliverDate
					};
					result.Add(contract.ContractID, detail);
				}
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

		private static List<ContractDetail> GetDbVirtualContracts()
		{
			lastReadDbTime = DateTime.Now;
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
			{
				//过滤掉垃圾数据
				return conn.Query<ContractDetail>("select * from VirtualContract ;").AsList();
			}
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
	}
}