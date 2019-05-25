using Dapper;
using IQF.BizCommon.Data.Entity;
using System.Collections.Generic;

namespace IQF.BizCommon.Data
{
	public class HisSpotFuturesPriceDao
    {

        public static bool AddHisDatas(List<HisSpotFuturesPriceEntity> entities)
        {
            using (var conn=ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFData))
            {
                var sql = "insert into hisSpotFuturesPrice(VarietyCode,VarietyName,SpotPrice,LastFutureSymbol,LastFuturePrice,MainFutureSymbol,MainFuturePrice,HqTime) " +
                          "values (@VarietyCode,@VarietyName,@SpotPrice,@LastFutureSymbol,@LastFuturePrice,@MainFutureSymbol,@MainFuturePrice,@HqTime);";
                return conn.Execute(sql, entities) > 0;
            }
        }

    }
}