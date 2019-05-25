using Dapper;
using IQF.BizCommon.Data.Entity;
using IQF.Framework.Cache;
using System.Collections.Generic;

namespace IQF.BizCommon.Data
{
	public class FilterWordDao
    {
        private static readonly ICacheInterceptor Cache = CacheInterceptorFactory.Create(GetFilterWordList, 24 * 60 * 60);
        private static List<FilterWordEntity> GetCacheFiterWords()
        {
            return Cache.Execute(new List<FilterWordEntity>());
        }

        private static List<FilterWordEntity> GetFilterWordList()
        {
            var filterWords = GetDbFilterWords();
            if (filterWords == null || filterWords.Count <= 0)
            {
                return null;
            }

            var ret = new List<FilterWordEntity>();
            foreach (var item in filterWords)
            {
                ret.Add(item);
            }

            return ret;
        }

        public static List<FilterWordEntity> GetFilterWords()
        {
            var result = GetCacheFiterWords();
            return result;
        }

        private static List<FilterWordEntity> GetDbFilterWords()
        {
            using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFUser))
            {
                return conn.Query<FilterWordEntity>("select * from [FilterWord]").AsList();
            }
        }
    }
}
