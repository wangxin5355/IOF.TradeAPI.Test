using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

/*
 
用例：

	Dictionary<string, object> dic = new Dictionary<string, object>();
	dic.Add("fundAccountId", 2000001);
	dic.Add("symbol", "00700");

	Condition condi = Condition.FromDic(dic);
	var where = condi.ToParametered();
	string sql = "select * from tp_hisorder " + where.whereClause;

	//SQLCommon参数化
	DataSet ds;
	string err;
	bool b = SQLCommon.ExecuteDataset(sql, ConnectionString.NZ_Broker, ref where.paras, out ds, out err);
	var ss = ds.Tables[0].ToJson();

	//SQLCommon直接查
	string sql2 = "select * from tp_hisorder " + condi.ToStringWhere();
	var dt = SQLCommon.ExecuteForFirstTable(sql2, ConnectionString.NZ_Broker);
	ss = dt.ToJson();

	//Dapper参数化
	using (SqlConnection conn = new SqlConnection(ConnectionString.NZ_Broker))
	{
		NzCommon.Dapper.DynamicParameters dp = new NzCommon.Dapper.DynamicParameters();
		foreach (var key in dic.Keys)
		{
			dp.Add(key, dic[key]);
		}
		var ret = NzCommon.Dapper.SqlMapper.Query(conn, sql, dp);
	}
 
*/


namespace IQF.BizCommon.Helper
{
	/// <summary>
	/// 通用查询条件组装（可以防止sql注入）
	/// </summary>
	public class Condition
	{
		/// <summary>
		/// key最长允许多长，默认30，外面可以设置
		/// </summary>
		public static int MaxKeyLen = 30;

		/// <summary>
		/// val最长允许多长，默认100，外面可以设置
		/// </summary>
		public static int MaxValLen = 100;

		class OneCondition
		{
			/// <summary>
			/// 
			/// </summary>
			public string key;

			/// <summary>
			/// 是否已经对val做过合法性检查
			/// </summary>
			public bool isChecked = false;

			/// <summary>
			/// 条件值
			/// </summary>
			public object val;
		}

		#region 成员变量

		private void AddCondition(string key_, object val_, bool isChecked_ = false)
		{
			conditions.Add(key_, new OneCondition() { key = key_, val = val_, isChecked = isChecked_ });
		}

		/// <summary>
		/// 查询条件数据池,只支持一个或多个条件的AND。复杂查询自己去写存储过程。
		/// </summary>
		private Dictionary<string, OneCondition> conditions = new Dictionary<string, OneCondition>();
		/// <summary>
		/// 查询条件数据类型池
		/// </summary>
		private Hashtable _types = new Hashtable();

		/// <summary>
		/// 自定义查询字段
		/// </summary>
		public string fileds = "*";

		/// <summary>
		/// 自定义查询条件
		/// </summary>
		public string condiation = string.Empty;

		/// <summary>
		/// 页码
		/// </summary>
		public int Page = 1;
		/// <summary>
		/// 页大小
		/// </summary>
		public int PageSize = 20;
		/// <summary>
		/// 排序
		/// </summary>
		public string OrderBy = string.Empty;

		#endregion

		#region 添加查询条件

		#region sring类型
		/// <summary>
		/// 添加查询条件
		/// </summary>
		/// <param name="key">字段名</param>
		/// <param name="value">值</param>
		public void Add(string key, string value)
		{
			AddCondition(key, value);
			_types.Add(key, "string");
		}

		/// <summary>
		///大于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddGreaterThan(string key, string value)
		{
			AddCondition(key, value, true);
			_types.Add(key, "greaterthan");
		}

		/// <summary>
		///大于等于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddGreaterThanEqual(string key, string value)
		{
			AddCondition(key, value, true);
			_types.Add(key, "greaterthanequal");
		}

		/// <summary>
		///小于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddLessThan(string key, string value)
		{
			AddCondition(key, value, true);
			_types.Add(key, "lessthan");
		}

		/// <summary>
		///小于等于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddLessThanEqual(string key, string value)
		{
			AddCondition(key, value, true);
			_types.Add(key, "lessthanequal");
		}
		#endregion


		#region 时间类型
		/// <summary>
		///大于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddGreaterThan(string key, DateTime value)
		{
			AddCondition(key, value, true);
			_types.Add(key, "greaterthantime");
		}

		/// <summary>
		///大于等于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddGreaterThanEqual(string key, DateTime value)
		{
			AddCondition(key, value, true);
			_types.Add(key, "greaterthanequaltime");
		}

		/// <summary>
		/// 添加查询条件
		/// </summary>
		/// <param name="key">字段名</param>
		/// <param name="value">值</param>
		public void Add(string key, DateTime value)
		{
			Add(key, value, value);
		}

		/// <summary>
		///小于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddLessThan(string key, DateTime value)
		{
			AddCondition(key, value, true);
			_types.Add(key, "lessthantime");
		}

		/// <summary>
		///小于等于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddLessThanEqual(string key, DateTime value)
		{
			AddCondition(key, value, true);
			_types.Add(key, "lessthanequaltime");
		}

		/// <summary>
		/// 添加查询条件
		/// </summary>
		/// <param name="key">字段名</param>
		/// <param name="value1">起始时间</param>
		/// <param name="value2">结束时间</param>
		public void Add(string key, DateTime value1, DateTime value2)
		{
			string t1 = (0 == value1.Hour) ? value1.ToString("yyy-MM-dd 00:00:00.000") : value1.ToString("yyy-MM-dd HH:mm:ss.fff");
			string t2 = (0 == value2.Hour) ? value2.ToString("yyy-MM-dd 23:59:59.000") : value2.ToString("yyy-MM-dd HH:mm:ss.fff");

			AddCondition(key, string.Format(" '{0}' and '{1}' ", t1, t2), true);
			_types.Add(key, "datetime");
		}		
		#endregion

		#region long类型
		/// <summary>
		/// 添加查询条件
		/// </summary>
		/// <param name="key">字段名</param>
		/// <param name="value">值</param>
		public void Add(string key, long value)
		{
			AddCondition(key, value);
			_types.Add(key, "number");
		}

		/// <summary>
		///大于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddGreaterThan(string key, long value)
		{
			AddCondition(key, value, true);
			_types.Add(key, "greaterthan");
		}

		/// <summary>
		///大于等于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddGreaterThanEqual(string key, long value)
		{
			AddCondition(key, value, true);
			_types.Add(key, "greaterthanequal");
		}

		/// <summary>
		///小于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddLessThan(string key, long value)
		{
			AddCondition(key, value, true);
			_types.Add(key, "lessthan");
		}

		/// <summary>
		///小于等于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddLessThanEqual(string key, long value)
		{
			AddCondition(key, value, true);
			_types.Add(key, "lessthanequal");
		}

		public void AddOrKey(string key1, long value1, string key2, long value2)
		{

			AddCondition(key1, string.Format(" {0}={1} ", key1, value1), true);
			_types.Add(key1, "or_key1");
			AddCondition(key2, string.Format(" {0}={1} ", key2, value2), true);
			_types.Add(key2, "or_key2");
		}
		#endregion

		#region int类型
		/// <summary>
		/// 添加查询条件
		/// </summary>
		/// <param name="key">字段名</param>
		/// <param name="value">值</param>
		public void Add(string key, int value)
		{
			AddCondition(key, value);
			_types.Add(key, "number");
		}

		/// <summary>
		///大于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddGreaterThan(string key, int value)
		{
			AddCondition(key, value, true);
			_types.Add(key, "greaterthan");
		}

		/// <summary>
		///大于等于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddGreaterThanEqual(string key, int value)
		{
			AddCondition(key, value, true);
			_types.Add(key, "greaterthanequal");
		}

		/// <summary>
		///小于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddLessThan(string key, int value)
		{
			AddCondition(key, value, true);
			_types.Add(key, "lessthan");
		}

		/// <summary>
		///小于等于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddLessThanEqual(string key, int value)
		{
			AddCondition(key, value, true);
			_types.Add(key, "lessthanequal");
		}

		/// <summary>
		///不等于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddNoEqual(string key, object value)
		{
			AddCondition(key, value);
			_types.Add(key, "noequal");
		}

		#endregion


		#region decimal类型
		/// <summary>
		/// 添加查询条件
		/// </summary>
		/// <param name="key">字段名</param>
		/// <param name="value">值</param>
		public void Add(string key, decimal value)
		{
			AddCondition(key, value);
			_types.Add(key, "number");
		}

		/// <summary>
		///大于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddGreaterThan(string key, decimal value)
		{
			AddCondition(key, value);
			_types.Add(key, "greaterthan");
		}

		/// <summary>
		///大于等于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddGreaterThanEqual(string key, decimal value)
		{
			AddCondition(key, value);
			_types.Add(key, "greaterthanequal");
		}

		/// <summary>
		///小于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddLessThan(string key, decimal value)
		{
			AddCondition(key, value);
			_types.Add(key, "lessthan");
		}

		/// <summary>
		///小于等于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddLessThanEqual(string key, decimal value)
		{
			AddCondition(key, value);
			_types.Add(key, "lessthanequal");
		}

		#endregion

		#region double类型
		/// <summary>
		/// 添加查询条件
		/// </summary>
		/// <param name="key">字段名</param>
		/// <param name="value">值</param>
		public void Add(string key, double value)
		{
			AddCondition(key, value);
			_types.Add(key, "number");
		}

		/// <summary>
		///大于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddGreaterThan(string key, double value)
		{
			AddCondition(key, value);
			_types.Add(key, "greaterthan");
		}

		/// <summary>
		///大于等于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddGreaterThanEqual(string key, double value)
		{
			AddCondition(key, value);
			_types.Add(key, "greaterthanequal");
		}

		/// <summary>
		///小于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddLessThan(string key, double value)
		{
			AddCondition(key, value);
			_types.Add(key, "lessthan");
		}

		/// <summary>
		///小于等于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddLessThanEqual(string key, double value)
		{
			AddCondition(key, value);
			_types.Add(key, "lessthanequal");
		}

		#endregion

		#region float类型
		/// <summary>
		/// 添加查询条件
		/// </summary>
		/// <param name="key">字段名</param>
		/// <param name="value">值</param>
		public void Add(string key, float value)
		{
			AddCondition(key, value);
			_types.Add(key, "number");
		}

		/// <summary>
		///大于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddGreaterThan(string key, float value)
		{
			AddCondition(key, value);
			_types.Add(key, "greaterthan");
		}

		/// <summary>
		///大于等于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddGreaterThanEqual(string key, float value)
		{
			AddCondition(key, value);
			_types.Add(key, "greaterthanequal");
		}

		/// <summary>
		///小于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddLessThan(string key, float value)
		{
			AddCondition(key, value);
			_types.Add(key, "lessthan");
		}

		/// <summary>
		///小于等于指定值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddLessThanEqual(string key, float value)
		{
			AddCondition(key, value);
			_types.Add(key, "lessthanequal");
		}

		#endregion

		public static Condition FromDic<T>(Dictionary<string, T> dic)
		{
			Condition condi = new Condition();
			if (dic != null)
			{
				foreach (var key in dic.Keys)
				{
					var val = dic[key];
					condi.AddCondition(key, val);
				}
			}
			return condi;
		}

		#endregion

		#region 输出查询条件

		/// <summary>
		/// 输出查询条件，形如 " where fundAccountId='2000001' and symbol='00700'" 或者 " "
		/// </summary>
		/// <returns></returns>
		public string ToStringWhere()
		{
			string ss = ToString();
			if (ss.Length == 0)
			{
				return " ";
			}
			else
			{
				string ret = " where " + ss.TrimStart(' ').Substring(3);
				return ret;
			}
		}

		/// <summary>
		/// 输出查询条件，形如 " and fundAccountId='2000001' and symbol='00700' " 或空字符串
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (string.IsNullOrEmpty(condiation))
			{
				return ToString("and");
			}
			else
			{
				return condiation.ToString();
			}
		}
		/// <summary>
		/// 输出查询条件，形如 " and fundAccountId='2000001' and symbol='00700' " 或空字符串
		/// </summary>
		/// <param name="opt">多条件间连接符</param>
		/// <returns></returns>
		public string ToString(string opt)
		{
			if (!IsValidConditions())
			{
				throw new Exception("违反约定的condition条件");
			}

			if (conditions.Count == 0)
			{
				return "";
			}

			StringBuilder sb = new StringBuilder();
			sb.Append(" ");
			foreach (string key in conditions.Keys)
			{
				//if (sb.ToString().Length > 0)
				//{
				sb.AppendFormat("{0}", opt);
				//}
				var objCondition = conditions[key];

				if (!_types.ContainsKey(key))
				{
					_types.Add(key, "string");
				}

				string value = objCondition.val.ToString();
				if ("string" == _types[key].ToString() && !string.IsNullOrEmpty(value))
				{
					sb.AppendFormat(" {0}='{1}' ", key, value);
				}
				else if ("number" == _types[key].ToString() && !string.IsNullOrEmpty(value))
				{
					sb.AppendFormat(" {0}={1} ", key, value);
				}
				else if ("or_key1" == _types[key].ToString() && !string.IsNullOrEmpty(value))
				{
					sb.AppendFormat(" {0}={1} ", key, value);
				}
				else if ("or_key2" == _types[key].ToString() && !string.IsNullOrEmpty(value))
				{
					sb.AppendFormat(" or {0}={1} ", key, value);
				}
				else if ("datetime" == _types[key].ToString() && !string.IsNullOrEmpty(value))
				{
					sb.AppendFormat(" {0} between {1} ", key, value);
				}
				else if ("lessthantime" == _types[key].ToString() && !string.IsNullOrEmpty(value))
				{
					sb.AppendFormat(" {0}<'{1}' ", key, value);
				}
				else if ("lessthanequaltime" == _types[key].ToString() && !string.IsNullOrEmpty(value))
				{
					sb.AppendFormat(" {0}<='{1}' ", key, value);
				}
				else if ("greaterthantime" == _types[key].ToString() && !string.IsNullOrEmpty(value))
				{
					sb.AppendFormat(" {0}>'{1}' ", key, value);
				}
				else if ("greaterthanequaltime" == _types[key].ToString() && !string.IsNullOrEmpty(value))
				{
					sb.AppendFormat(" {0}>='{1}' ", key, value);
				}
				else if ("lessthan" == _types[key].ToString() && !string.IsNullOrEmpty(value))
				{
					sb.AppendFormat(" {0}<{1} ", key, value);
				}
				else if ("lessthanequal" == _types[key].ToString() && !string.IsNullOrEmpty(value))
				{
					sb.AppendFormat(" {0}<={1} ", key, value);
				}
				else if ("greaterthan" == _types[key].ToString() && !string.IsNullOrEmpty(value))
				{
					sb.AppendFormat(" {0}>{1} ", key, value);
				}
				else if ("greaterthanequal" == _types[key].ToString() && !string.IsNullOrEmpty(value))
				{
					sb.AppendFormat(" {0}>={1} ", key, value);
				}
				else if ("noequal" == _types[key].ToString() && !string.IsNullOrEmpty(value))
				{
					sb.AppendFormat(" {0}<>{1} ", key, value);
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// 参数化的查询条件，可以被NzCommon.Dapper所使用，也可以被SqlCommon所使用
		/// </summary>
		public class ParameteredCondition
		{
			/// <summary>
			/// 形如 " where [name]=@name and [age]=@age" 的参数化查询条件
			/// </summary>
			public string whereClause = " ";

			/// <summary>
			/// whereClause所需的参数
			/// </summary>
			public List<SqlParameter> paras = new List<SqlParameter>();
		}

		/// <summary>
		/// 生成参数化的查询条件
		/// </summary>
		/// <returns></returns>
		public ParameteredCondition ToParametered()
		{
			ParameteredCondition ret = new ParameteredCondition();
			if (conditions.Count == 0)
			{
				return ret;
			}

			ret.whereClause = " where";
			foreach (var one in conditions.Values)
			{
				ret.whereClause += string.Format(" {0}=@{0} and", one.key);
				ret.paras.Add(new SqlParameter(one.key, one.val));
			}
			ret.whereClause = ret.whereClause.Substring(0, ret.whereClause.Length - " and".Length);

			return ret;
		}

		#endregion


		/// <summary>
		/// 验证key是否合法（是否合法的db column name）。查询条件key里面只能包含字母、数字、下划线。而且不能超长。
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool IsValidConditionKey(string key)
		{
			if (key.Length > MaxKeyLen)
			{
				return false;
			}
			foreach (var c in key.ToCharArray())
			{
				if (!char.IsLetterOrDigit(c) && c != '_')
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// val里面不允许出现的字符或字符串。可从外面修改限制条件
		/// </summary>
		public static List<string> BadVals = new List<string>()
		{
			" "
			,"="
			,"'"
			,";"
			,"escape"
			,"quoted_identifier"
		};

		/// <summary>
		/// 验证参数值是否合法
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static bool IsValidConditionVal(string val)
		{
			if (val.Length > MaxValLen)
			{
				return false;
			}

			string low = val.ToLower();
			foreach (var item in BadVals)
			{
				if (low.Contains(item.ToLower()))
				{
					return false;
				}
			}
			return true;
		}


		/// <summary>
		/// 查询条件合法验证
		/// </summary>
		/// <returns></returns>
		public bool IsValidConditions()
		{
			foreach (var key in conditions.Keys)
			{
				if (!IsValidConditionKey(key))
				{
					return false;
				}

				var objCondition = conditions[key];

				if (objCondition.isChecked)
				{
					continue;
				}

				string val = objCondition.val.ToString();
				if (!IsValidConditionVal(val))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// 把形如"a=b&c=d&e=f"这样的http query string转化为安全的sql where条件
		/// </summary>
		/// <param name="queryStringCondition"></param>
		/// <returns></returns>
		public static string GetSafeCondition(string queryStringCondition)
		{
			var ay = queryStringCondition.Split('&');
			Condition condi = new Condition();
			condi.condiation = queryStringCondition;
			foreach (var item in ay)
			{
				int idx = item.IndexOf('=');
				if (idx <= 0)
				{
					continue;
				}
				string key = item.Substring(0, idx);
				string val = item.Substring(idx + 1, item.Length - (idx + 1));

				condi.Add(key, val);
			}
			return condi.ToString();
		}
	}
}
