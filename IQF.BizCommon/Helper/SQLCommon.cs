using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using IQF.Framework;

namespace IQF.BizCommon.Helper
{
	/// <summary>
	/// Sql协助类
	/// </summary>
	[Obsolete("已废弃，使用dapper", false)]
	public class SQLCommon
	{
		private static string DmlCmd = @"--, ;--, ;, /*, */, @@, char, nchar, varchar, nvarchar, alter,begin, cast, create, cursor, declare, end, exec, execute, fetch, kill, open, sys, sysobjects, syscolumns, table";
		private static string[] ayDmlCmd = null;

		/// <summary>
		/// 执行标准SQL语句，不要求返回结果，适合（增、删、改）
		/// </summary>
		/// <param name="sql">标准SQL语句</param>
		/// <param name="connectionString">连库字符串</param>
		/// <param name="errorMessage">错误信息</param>
		/// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
		public static bool ExecuteNonQuery(string sql, string connectionString, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			try
			{
				if (!InjectFilter(sql))
				{
					return false;
				}

				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(sql, conn))
					{
						conn.Open();
						cmd.ExecuteNonQuery();
						result = true;
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}
		public static bool ExecuteNonQuery(string sql, string connectionString, int commandTimeout, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			try
			{
				if (!InjectFilter(sql))
				{
					return false;
				}

				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(sql, conn))
					{
						cmd.CommandType = CommandType.Text;
						cmd.CommandTimeout = commandTimeout;
						conn.Open();
						cmd.ExecuteNonQuery();
						result = true;
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}
		/// <summary>
		/// 执行标准SQL语句，不要求返回结果，适合（增、删、改）
		/// </summary>
		/// <param name="sql">标准SQL语句</param>
		/// <param name="sqlParameters">参数集合</param>
		/// <param name="connectionString">连库字符串</param>
		/// <param name="errorMessage">错误信息</param>
		/// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
		public static bool ExecuteNonQuery(string sql, ref SqlParameter[] sqlParameters, string connectionString, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;

			try
			{
				if (!InjectFilter(sql))
				{
					return false;
				}

				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(sql, conn))
					{
						for (int i = 0; i < sqlParameters.Length; i++)
						{
							cmd.Parameters.Add(sqlParameters[i]);
						}
						conn.Open();
						cmd.ExecuteNonQuery();
						result = true;
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}
		public static bool ExecuteNonQuery(string sql, ref List<SqlParameter> sqlParameters, string connectionString, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;

			try
			{
				if (!InjectFilter(sql))
				{
					return false;
				}

				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(sql, conn))
					{
						for (int i = 0; i < sqlParameters.Count; i++)
						{
							cmd.Parameters.Add(sqlParameters[i]);
						}
						conn.Open();
						cmd.ExecuteNonQuery();
						result = true;
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}
		/// <summary>
		/// 执行INSERT、UPDATE、DELETE 【王中秋：2016/9/30】
		/// </summary>
		/// <param name="connectionString">数据库连接字符串</param>
		/// <param name="sentence">SQL命令或存储过程名</param>
		/// <param name="parameters">参数数组</param>
		/// <returns>影响的行数</returns>
		public static int ExecuteNonQuery(string connectionString, string sentence, out string errorMessage, DbParameter[] parameters = null)
		{
			int result = 0;
			errorMessage = string.Empty;
			try
			{
				if (!InjectFilter(sentence))
				{
					return result;
				}
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (DbCommand dbCommand = conn.CreateCommand())
					{
						dbCommand.CommandText = sentence;
						dbCommand.CommandTimeout = 0;
						if (parameters != null)
						{
							dbCommand.Parameters.AddRange(parameters);
						}
						conn.Open();
						result = dbCommand.ExecuteNonQuery();
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}
		/// <summary>
		/// 执行标准SQL查询语句，返回记录集。失败返回null
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		public static DataSet ExecuteForDataset(string sql, string connectionString)
		{
			DataSet ds;
			string err;
			if (ExecuteDataset(sql, connectionString, out ds, out err))
			{
				return ds;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 执行标准SQL查询语句，返回记录集
		/// </summary>
		/// <param name="sql">标准SQL查询语句</param>
		/// <param name="connectionString">连库字符串</param>
		/// <param name="ds">查询结果记录集</param>
		/// <param name="errorMessage">错误信息</param>
		/// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
		public static bool ExecuteDataset(string sql, string connectionString, out DataSet ds, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			ds = new DataSet();

			try
			{
				if (!InjectFilter(sql))
				{
					return false;
				}

				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (var command = new SqlCommand(sql, conn))
					{
						command.CommandType = CommandType.Text;

						using (SqlDataAdapter adapter = new SqlDataAdapter())
						{
							adapter.SelectCommand = command;
							adapter.Fill(ds);
							result = true;
						}
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}

		public static bool ExecuteDataset(string sql, string connectionString, int commandTimeout, out DataSet ds, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			ds = new DataSet();

			try
			{
				if (!InjectFilter(sql))
				{
					return false;
				}

				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(sql, conn))
					{
						cmd.CommandType = CommandType.Text;
						cmd.CommandTimeout = commandTimeout;
						using (SqlDataAdapter da = new SqlDataAdapter(cmd))
						{
							da.Fill(ds);
							result = true;
						}
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}
		public static bool ExecuteDataset(string sql, string connectionString, int StartRecordNo, int PageSize, out DataSet ds, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			ds = new DataSet();

			try
			{
				if (!InjectFilter(sql))
				{
					return false;
				}

				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(sql, conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						using (SqlDataAdapter da = new SqlDataAdapter(cmd))
						{
							da.Fill(ds, StartRecordNo, PageSize, "TableName");
							result = true;
						}
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}

		public static bool ExecuteDataset(string sql, string connectionString, ref SqlParameter[] sqlParameters, out DataSet ds, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			ds = new DataSet();

			try
			{
				if (!InjectFilter(sql))
				{
					return false;
				}
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand command = new SqlCommand(sql, conn))
					{
						command.CommandType = CommandType.Text;
						command.Parameters.AddRange(sqlParameters);
						using (SqlDataAdapter adapter = new SqlDataAdapter())
						{
							adapter.SelectCommand = command;
							adapter.Fill(ds);
							result = true;
						}
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}
		public static bool ExecuteDataset(string sql, string connectionString, ref List<SqlParameter> sqlParameters, out DataSet ds, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			ds = new DataSet();

			try
			{
				if (!InjectFilter(sql))
				{
					return false;
				}
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand command = new SqlCommand(sql, conn))
					{
						command.CommandType = CommandType.Text;
						command.Parameters.AddRange(sqlParameters.ToArray());
						using (SqlDataAdapter adapter = new SqlDataAdapter())
						{
							adapter.SelectCommand = command;
							adapter.Fill(ds);
							result = true;
						}
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}
		/// <summary>
		/// 返回结果集的第一张表。失败或者不存在返回null
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		public static DataTable ExecuteForFirstTable(string sql, string connectionString)
		{
			DataSet ds;
			string errorMessage;
			if (ExecuteDataset(sql, connectionString, out ds, out errorMessage))
			{
				if (ds.Tables.Count > 0)
				{
					return ds.Tables[0];
				}
			}
			return null;
		}

		/// <summary>
		/// 返回结果集的第一张表。失败或者不存在返回null: 从第startIndex条取到count条记录
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		public static bool ExecuteForFirstTable(string sql, int startIndex, int count, string connectionString, out DataTable dt, out string msg)
		{
			msg = string.Empty;
			dt = new DataTable();

			try
			{
				if (!InjectFilter(sql))
				{
					return false;
				}

				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(sql, conn))
					{
						cmd.CommandType = CommandType.Text;

						using (SqlDataAdapter da = new SqlDataAdapter(cmd))
						{
							da.Fill(startIndex, count, dt);
							return true;
						}
					}
				}
			}
			catch (Exception e)
			{
				msg = e.Message;
			}
			return true;
		}

		/// <summary>
		/// 返回结果集的第一行。失败或者不存在返回null
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		public static DataRow ExecuteForFirstLine(string sql, string connectionString)
		{
			DataSet ds;
			string errorMessage;
			if (ExecuteDataset(sql, connectionString, out ds, out errorMessage))
			{
				if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					return ds.Tables[0].Rows[0];
				}
			}
			return null;
		}

		/// <summary>
		/// 返回结果集的第一行的第一个字段。失败或者不存在返回null。DBNull返回空字符串""
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		public static string ExecuteForFirstField(string sql, string connectionString)
		{
			var row = ExecuteForFirstLine(sql, connectionString);
			if (row != null && row.ItemArray.Length > 0)
			{
				return row[0].ToString();
			}
			return null;
		}

		public static bool ExcueteScalar(string sql, string connectionString, out object scalar, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			scalar = null;
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(sql, conn))
					{
						conn.Open();
						scalar = cmd.ExecuteScalar();
						result = scalar != null;
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}
		public static bool ExcueteScalar(string sql, string connectionString, ref SqlParameter[] sqlParameters, out object scalar, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			scalar = null;
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(sql, conn))
					{
						conn.Open();
						cmd.Parameters.AddRange(sqlParameters);
						scalar = cmd.ExecuteScalar();
						result = !(scalar is DBNull);
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}

		/// <summary>
		/// 提供通用的调用存储过程的方法（需要返回查询的记录集的调用）
		/// </summary>
		/// <param name="procedureName">存储过程名称</param>
		/// <param name="connectionString">连库字符串</param>
		/// <param name="sqlParameters">存储过程参数数组</param>
		/// <param name="ds">存储过程里面返回的记录集</param>
		/// <param name="errorMessage">错误信息</param>
		/// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
		public static bool ExecuteStoredProcedure(string procedureName, string connectionString, ref SqlParameter[] sqlParameters, out DataSet ds, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			ds = new DataSet();
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(procedureName, conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandTimeout = 180;
						using (SqlDataAdapter da = new SqlDataAdapter(cmd))
						{
							for (int i = 0; i < sqlParameters.Length; i++)
							{
								da.SelectCommand.Parameters.Add(sqlParameters[i]);
							}
							da.Fill(ds);
							result = true;
						}
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}

		/// <summary>
		/// 提供通用的调用存储过程的方法（需要返回查询的记录集的调用）
		/// </summary>
		/// <param name="procedureName">存储过程名称</param>
		/// <param name="connectionString">连库字符串</param>
		/// <param name="sqlParameters">存储过程参数数组</param>
		/// <param name="ds">存储过程里面返回的记录集</param>
		/// <param name="errorMessage">错误信息</param>
		/// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
		public static bool ExecuteStoredProcedure(string procedureName, string connectionString, List<String> resourse, ref SqlParameter[] sqlParameters, out DataSet ds, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			ds = new DataSet();
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(procedureName, conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandTimeout = 180;
						using (SqlDataAdapter da = new SqlDataAdapter(cmd))
						{
							for (int i = 0; i < sqlParameters.Length; i++)
							{
								da.SelectCommand.Parameters.Add(sqlParameters[i]);
							}
							foreach (string str in resourse)
							{
								sqlParameters[0].Value = str;
								da.Fill(ds);
							}
							result = true;

							//20180319_LY,多次反复调用存储过程时，回收SqlCommand的
							cmd.Parameters.Clear();
						}
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}


		public static bool ExecuteStoredProcedure(string procedureName, string connectionString, int commandTimeout, ref SqlParameter[] sqlParameters, out DataSet ds, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			ds = new DataSet();
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(procedureName, conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandTimeout = commandTimeout;
						using (SqlDataAdapter da = new SqlDataAdapter(cmd))
						{
							for (int i = 0; i < sqlParameters.Length; i++)
							{
								da.SelectCommand.Parameters.Add(sqlParameters[i]);
							}
							da.Fill(ds);
							result = true;
						}
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}

		public static bool ExecuteStoredProcedure(string procedureName, string connectionString, ref List<SqlParameter> sqlParameters, out DataSet ds, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			ds = new DataSet();
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(procedureName, conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandTimeout = 180;
						using (SqlDataAdapter da = new SqlDataAdapter(cmd))
						{
							for (int i = 0; i < sqlParameters.Count; i++)
							{
								da.SelectCommand.Parameters.Add(sqlParameters[i]);
							}
							da.Fill(ds);
							result = true;
						}
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}

		/// <summary>
		/// 在已有连接和事务上执行存储过程。只执行，不返回参数。这个函数比较特别，失败是抛异常的。
		/// </summary>
		/// <param name="procedureName"></param>
		/// <param name="conn"></param>
		/// <param name="commandTimeout"></param>
		/// <param name="sqlParameters"></param>
		/// <param name="trans"></param>
		/// <returns></returns>
		public static bool ExecuteStoredProcedureConn(string procedureName, SqlConnection conn, List<SqlParameter> sqlParameters, SqlTransaction trans)
		{
			DataSet ds = new DataSet();
			try
			{
				using (SqlCommand cmd = new SqlCommand(procedureName, conn, trans))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					using (SqlDataAdapter da = new SqlDataAdapter(cmd))
					{
						for (int i = 0; i < sqlParameters.Count; i++)
						{
							da.SelectCommand.Parameters.Add(sqlParameters[i]);
						}
						da.Fill(ds);
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}
			return true;
		}

		/// <summary>
		/// 在已有连接和事务上执行存储过程。失败返回false
		/// </summary>
		/// <param name="procedureName"></param>
		/// <param name="conn"></param>
		/// <param name="commandTimeout"></param>
		/// <param name="sqlParameters"></param>
		/// <param name="trans"></param>
		/// <returns></returns>
		public static bool ExecuteStoredProcedureConn(string procedureName, SqlConnection conn, List<SqlParameter> sqlParameters, SqlTransaction trans, out DataSet ds, out string errorMessage)
		{
			ds = new DataSet();
			try
			{
				using (SqlCommand cmd = new SqlCommand(procedureName, conn, trans))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					using (SqlDataAdapter da = new SqlDataAdapter(cmd))
					{
						for (int i = 0; i < sqlParameters.Count; i++)
						{
							da.SelectCommand.Parameters.Add(sqlParameters[i]);
						}
						da.Fill(ds);
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
				return false;
			}

			errorMessage = "";
			return true;
		}

		public static bool ExecuteStoredProcedure(string procedureName, string connectionString, int commandTimeout, ref List<SqlParameter> sqlParameters, out DataSet ds, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			ds = new DataSet();
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(procedureName, conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandTimeout = commandTimeout;
						using (SqlDataAdapter da = new SqlDataAdapter(cmd))
						{
							for (int i = 0; i < sqlParameters.Count; i++)
							{
								da.SelectCommand.Parameters.Add(sqlParameters[i]);
							}
							da.Fill(ds);
							result = true;
						}
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}

		/// <summary>
		/// 提供通用的调用存储过程的方法（需要返回查询的记录集的调用）
		/// </summary>
		/// <param name="procedureName">存储过程名称</param>
		/// <param name="connectionString">连库字符串</param>
		/// <param name="errorMessage">错误信息</param>
		/// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
		public static bool ExecuteStoredProcedure(string procedureName, string connectionString, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(procedureName, conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						conn.Open();
						cmd.ExecuteNonQuery();
						result = true;
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}

		public static bool ExecuteStoredProcedure(string procedureName, string connectionString, int commandTimeout, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(procedureName, conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandTimeout = commandTimeout;
						conn.Open();
						cmd.ExecuteNonQuery();
						result = true;
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}

		/// <summary>
		///  提供通用的调用存储过程的方法（需要返回查询的记录集的调用）
		/// </summary>
		/// <param name="procedureName">存储过程名称</param>
		/// <param name="connectionString">连库字符串</param>
		/// <param name="ds">存储过程里面返回的记录集</param>
		/// <param name="errorMessage">错误信息</param>
		/// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
		public static bool ExecuteStoredProcedure(string procedureName, string connectionString, out DataSet ds, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			ds = new DataSet();

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(procedureName, conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						using (SqlDataAdapter da = new SqlDataAdapter(cmd))
						{
							da.Fill(ds);
							result = true;
						}
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}

		/// <summary>
		/// 提供通用的调用存储过程的方法（需要返回查询的记录集的调用）
		/// </summary>
		/// <param name="procedureName">存储过程名称</param>
		/// <param name="connectionString">连库字符串</param>
		/// <param name="startRecord">从其开始的从零开始的记录号</param>
		/// <param name="maxRecords">要检索的最大记录数</param>
		/// <param name="sqlParameters">存储过程参数数组</param>
		/// <param name="ds">存储过程里面返回的记录集</param>
		/// <param name="errorMessage">错误信息</param>
		/// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
		public static bool ExecuteStoredProcedure(string procedureName, string connectionString, int startRecord, int maxRecords, ref SqlParameter[] sqlParameters, out DataSet ds, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;
			ds = new DataSet();
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(procedureName, conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						using (SqlDataAdapter da = new SqlDataAdapter(cmd))
						{
							for (int i = 0; i < sqlParameters.Length; i++)
							{
								da.SelectCommand.Parameters.Add(sqlParameters[i]);
							}
							da.Fill(ds, startRecord, maxRecords, "srcTable");
							result = true;
						}
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
			}
			return result;
		}
		/// <summary>
		/// 提供通用的调用存储过程的方法（不需要返回查询的记录集的调用）
		/// </summary>
		/// <param name="procedureName">存储过程名称</param>
		/// <param name="connectionString">连库字符串</param>
		/// <param name="sqlParameters">存储过程参数数组</param>
		/// <param name="errorMessage">错误信息</param>
		/// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
		public static bool ExecuteStoredProcedure(string procedureName, string connectionString, ref SqlParameter[] sqlParameters, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(procedureName, conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						for (int i = 0; i < sqlParameters.Length; i++)
						{
							cmd.Parameters.Add(sqlParameters[i]);
						}
						conn.Open();
						cmd.ExecuteNonQuery();
						result = true;
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;

			}
			return result;
		}

		public static bool ExecuteStoredProcedure(string procedureName, string connectionString, ref List<SqlParameter> sqlParameters, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(procedureName, conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						for (int i = 0; i < sqlParameters.Count; i++)
						{
							cmd.Parameters.Add(sqlParameters[i]);
						}
						conn.Open();
						cmd.ExecuteNonQuery();
						result = true;
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;

			}
			return result;
		}

		public static bool ExecuteStoredProcedure(string procedureName, string connectionString, int commandTimeout, ref SqlParameter[] sqlParameters, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(procedureName, conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandTimeout = commandTimeout;
						for (int i = 0; i < sqlParameters.Length; i++)
						{
							cmd.Parameters.Add(sqlParameters[i]);
						}
						conn.Open();
						cmd.ExecuteNonQuery();
						result = true;
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;

			}
			return result;
		}
		public static bool ExecuteStoredProcedure(string procedureName, string connectionString, int commandTimeout, ref List<SqlParameter> sqlParameters, out string errorMessage)
		{
			bool result = false;
			errorMessage = string.Empty;

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand(procedureName, conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandTimeout = commandTimeout;
						for (int i = 0; i < sqlParameters.Count; i++)
						{
							cmd.Parameters.Add(sqlParameters[i]);
						}
						conn.Open();
						cmd.ExecuteNonQuery();
						result = true;
					}
				}
			}
			catch (Exception e)
			{
				errorMessage = e.Message;

			}
			return result;
		}


		/// <summary>
		/// 利用SqlBulkCopy快速大量导入数据
		/// </summary>
		/// <param name="batchSize">一次批量的插入的数据量</param>
		/// <param name="bulkCopyTimeout">超时时间</param>
		/// <param name="notifyAfter">在插入设定条数后，呼叫相应事件</param>
		/// <param name="tableName">要批量写入的表</param>
		/// <param name="columnMappings">自定义的datatable和数据库的字段的映射</param>
		/// <param name="dataTable">数据集</param>
		/// <returns></returns>
		public static bool ExecuteBySqlBulkCopy(string connectionString, int batchSize, int bulkCopyTimeout, int notifyAfter, string tableName, Dictionary<string, string> columnMappings, DataTable dataTable, out string errorMessage)
		{
			bool result = false;
			errorMessage = "";
			SqlConnection conn = new SqlConnection(connectionString);
			SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(conn);
			sqlBulkCopy.BatchSize = batchSize;
			sqlBulkCopy.BulkCopyTimeout = bulkCopyTimeout;
			sqlBulkCopy.NotifyAfter = notifyAfter;
			sqlBulkCopy.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);

			try
			{
				sqlBulkCopy.DestinationTableName = tableName;
				foreach (string key in columnMappings.Keys)
				{
					sqlBulkCopy.ColumnMappings.Add(key, columnMappings[key]);
				}
				conn.Open();
				sqlBulkCopy.WriteToServer(dataTable);
				result = true;
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
				result = false;
			}
			finally
			{
				if (conn.State == ConnectionState.Open)
				{
					conn.Close();
				}
				sqlBulkCopy.Close();
			}
			return result;
		}
		private static void OnSqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
		{
			//  插入数据后的处理
			return;
		}

		/// <summary>
		/// 批量插入数据。
		/// </summary>
		/// <param name="connectionString">数据库连接串</param>
		/// <param name="destinationTableName">要插入表名称</param>
		/// <param name="dt">数据</param>
		public static void DataTableToSQLServer(string connectionString, string destinationTableName, DataTable dt)
		{
			//LogRecord.writeLogsingle("error.log", "dt.Rows = {0}", dt.Rows.Count);
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
				{
					try
					{
						bulkCopy.DestinationTableName = destinationTableName;//要插入表名称  
						foreach (DataColumn columnItem in dt.Columns)
						{
							bulkCopy.ColumnMappings.Add(columnItem.ColumnName, columnItem.ColumnName);
						}
						bulkCopy.BulkCopyTimeout = 3600;//1小时
						bulkCopy.WriteToServer(dt);
					}
					catch (Exception ex)
					{
						LogRecord.writeLogsingle("error.log", ex.ToString());
					}
					finally
					{
						if (conn.State == ConnectionState.Open)
						{
							conn.Close();
						}
						bulkCopy.Close();
					}
				}
			}
			return;
		}

		/// <summary>
		/// 判断sql是否包含注入攻击。目前只写日志，不做过滤。
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		public static bool InjectFilter(string sql)
		{
			try
			{
				if (ayDmlCmd == null)
				{
					DmlCmd = DmlCmd.Replace(" ", "");
					ayDmlCmd = DmlCmd.Split(',');
				}
			}
			catch (Exception ex)
			{
				LogRecord.writeLogsingle("error", ex.ToString());
			}

			return true;
		}

		/// <summary>
		/// 获取row中某个字段的int值，失败抛异常
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		public static int GetRowInt(DataRow row, string column)
		{
			string ss = row[column].ToString();
			return int.Parse(ss);
		}

		/// <summary>
		/// 失败返回默认值
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		public static int GetRowIntNoThrow(DataRow row, string column, int defVal)
		{
			try
			{
				string ss = row[column].ToString();
				return int.Parse(ss);
			}
			catch (Exception)
			{
				return defVal;
			}
		}

		/// <summary>
		/// 失败抛异常
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		public static string GetRowString(DataRow row, string column)
		{
			string ss = row[column].ToString();
			return ss;
		}

		/// <summary>
		/// 失败抛异常
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		public static double GetRowDouble(DataRow row, string column)
		{
			string ss = row[column].ToString();
			return double.Parse(ss);
		}

		/// <summary>
		/// 失败抛异常
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		public static DateTime GetRowDateTime(DataRow row, string column)
		{
			string ss = row[column].ToString();
			return DateTime.Parse(ss);
		}

		/// <summary>
		/// 失败抛异常
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		public static T GetRowEnum<T>(DataRow row, string column) where T : struct
		{
			string ss = row[column].ToString();
			return ss.ToEnum<T>();
		}

		/// <summary>
		/// 超长或者包含非法字符的表名，是非法的
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="maxTableNameLength"></param>
		/// <returns></returns>
		public static bool IsValidTableName(string tableName, int maxTableNameLength = 30)
		{
			if (tableName.Length > maxTableNameLength)
			{
				return false;
			}
			foreach (var c in tableName.ToCharArray())
			{
				if (!char.IsLetterOrDigit(c) && c != '_')
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// 把DataTable导出为HtmlTable
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string ConvertDataTableToHtmlTable(DataTable dt)
		{
			StringBuilder strHTMLBuilder = new StringBuilder();

			strHTMLBuilder.Append("<table border='1px' cellpadding='1' cellspacing='1' bgcolor='lightyellow' style='font-family:Garamond; font-size:smaller'>");
			strHTMLBuilder.Append("<tr >");
			foreach (DataColumn myColumn in dt.Columns)
			{
				strHTMLBuilder.Append("<th >");
				strHTMLBuilder.Append(myColumn.ColumnName);
				strHTMLBuilder.Append("</th>");
			}
			strHTMLBuilder.Append("</tr>");

			foreach (DataRow myRow in dt.Rows)
			{
				strHTMLBuilder.Append("<tr >");
				foreach (DataColumn myColumn in dt.Columns)
				{
					strHTMLBuilder.Append("<td >");
					strHTMLBuilder.Append(myRow[myColumn.ColumnName].ToString());
					strHTMLBuilder.Append("</td>");

				}
				strHTMLBuilder.Append("</tr>");
			}

			//Close tags. 
			strHTMLBuilder.Append("</table>");

			string Htmltext = strHTMLBuilder.ToString();

			return Htmltext;
		}

		/// <summary>
		/// 执行交易相关的存储过程
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="connstr"></param>
		/// <param name="sps"></param>
		/// <param name="tsr"></param>
		/// <returns></returns>
		public static bool ExecSp(string spName, string connstr, List<SqlParameter> sps, out SpResult sr)
		{
			sr = new SpResult();
			SqlParameter[] ay = sps.ToArray();
			bool ret = SQLCommon.ExecuteStoredProcedure(spName, connstr, ref ay, out sr.ds, out sr.err);
			if (ret)
			{
				sr.ret = (int)sr.ds.Tables[0].Rows[0][0];
				sr.msg = (string)sr.ds.Tables[0].Rows[0][1];
			}
			else
			{
				//sr.err将包含错误信息
				sr.err = string.Format("[{0}] {1}", spName, sr.err);
			}
			return ret;
		}

		/// <summary>
		/// 执行交易相关的存储过程
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="connstr"></param>
		/// <param name="spsArray"></param>
		/// <param name="tsr"></param>
		/// <returns></returns>
		public static bool ExecSp(string spName, string connstr, SqlParameter[] spsArray, out SpResult sr)
		{
			sr = new SpResult();
			bool ret = SQLCommon.ExecuteStoredProcedure(spName, connstr, ref spsArray, out sr.ds, out sr.err);
			if (ret)
			{
				sr.ret = (int)sr.ds.Tables[0].Rows[0][0];
				sr.msg = (string)sr.ds.Tables[0].Rows[0][1];
			}
			else
			{
				//sr.err将包含错误信息
				sr.err = string.Format("[{0}] {1}", spName, sr.err);
			}
			return ret;
		}

		/// <summary>
		/// 在已有连接和事务上执行存储过程。失败可能抛异常。
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="conn"></param>
		/// <param name="sps"></param>
		/// <param name="trans"></param>
		/// <returns></returns>
		public static bool ExecSp(string spName, SqlConnection conn, List<SqlParameter> sps, SqlTransaction trans)
		{
			return SQLCommon.ExecuteStoredProcedureConn(spName, conn, sps, trans);
		}

		/// <summary>
		/// 成功返回数据表（不包括‘0,ok’那张表。可能数量为零）。失败返回null，errorMsg包含错误信息
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="connstr"></param>
		/// <param name="sps"></param>
		/// <param name="errorMsg"></param>
		/// <returns></returns>
		public static List<DataTable> ExecSpTables(string spName, string connstr, List<SqlParameter> sps, out string errorMsg)
		{
			errorMsg = "";
			SpResult sr;
			bool ret = ExecSp(spName, connstr, sps, out sr);
			if (!ret)
			{
				errorMsg = sr.err;
				return null;
			}
			else
			{
				if (sr.ret != 0)
				{
					errorMsg = sr.msg;
					return null;
				}
				else
				{
					List<DataTable> tables = new List<DataTable>();
					for (int i = 1; i < sr.ds.Tables.Count; i++)
					{
						tables.Add(sr.ds.Tables[i]);
					}
					return tables;
				}
			}
		}

		/// <summary>
		/// 执行交易相关的存储过程，并返回结果集第二张表的第一行。失败或不存在返回null
		/// </summary>
		/// <param name="spName"></param>
		/// <param name="connstr"></param>
		/// <param name="sps"></param>
		/// <param name="row"></param>
		/// <returns></returns>
		public static DataRow ExecSpForFirstRow(string spName, string connstr, List<SqlParameter> sps)
		{
			DataRow row = null;
			SpResult sr;
			if (ExecSp(spName, connstr, sps, out sr))
			{
				if (sr.ret == 0 && sr.ds.Tables.Count > 1 && sr.ds.Tables[1].Rows.Count > 0)
				{
					row = sr.ds.Tables[1].Rows[0];
				}
			}

			return row;
		}
	}

	/// <summary>
	/// 存储过程的返回结果
	/// </summary>
	[Obsolete("已废弃，使用dapper", false)]
	public class SpResult
	{
		/// <summary>
		/// 存储过程返回的结果集
		/// </summary>
		public DataSet ds;

		/// <summary>
		/// 如果存储过程执行失败，这个字段存放错误信息
		/// </summary>
		public string err;

		/// <summary>
		/// 存储过程第一张表第一行返回的结果值
		/// </summary>
		public int ret = -1;

		/// <summary>
		/// 存储过程第一张表第一行返回的结果信息
		/// </summary>
		public string msg;
	}
}
