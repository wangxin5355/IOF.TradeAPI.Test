using System;
using System.Runtime.Serialization;

namespace IQF.Framework
{
	public interface IResultInfo
	{
		/// <summary>
		/// 错误号 
		/// 0代表成功 负数代表代码层面错误  正数代表业务错误
		/// </summary>
		int Error_no { get; set; }

		/// <summary>
		/// 错误消息
		/// </summary>
		string Error_info { get; set; }

		/// <summary>
		/// 是否出错
		/// </summary>
		bool IsError();
	}

	public interface IResultInfo<T> : IResultInfo
	{
		/// <summary>
		/// 附加数据，失败时数据可能为空
		/// </summary>
		T Data { get; set; }
	}

	/// <summary>
	/// 结果信息
	/// </summary>
	public class ResultInfo : IResultInfo
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		public ResultInfo()
			: this(0, string.Empty)
		{
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="errorNo">错误号 0代表成功 负数代表代码层面错误  正数代表业务错误</param>
		/// <param name="errorMsg">错误消息</param>
		public ResultInfo(int errorNo, string errorMsg)
		{
			this.Error_no = errorNo;
			this.Error_info = errorMsg;
		}

		/// <summary>
		/// 错误号 
		/// 0代表成功 负数代表代码层面错误  正数代表业务错误
		/// </summary>
		public int Error_no { get; set; }

		/// <summary>
		/// 错误消息
		/// </summary>
		public string Error_info { get; set; }

		public bool IsError()
		{
			if (this == null || this.Error_no != 0)
			{
				return true;
			}
			return false;
		}
	}

	/// <summary>
	/// 结果信息
	/// </summary>
	public class ResultInfo<T> : ResultInfo, IResultInfo<T>
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		public ResultInfo()
			: this(0, string.Empty)
		{
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="errorNo">错误号 0代表成功 负数代表代码层面错误  正数代表业务错误</param>
		/// <param name="errorMsg">错误消息</param>
		public ResultInfo(int errorNo, string errorMsg)
			: this(errorNo, errorMsg, default(T))
		{
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="errorNo">错误号 0代表成功 负数代表代码层面错误  正数代表业务错误</param>
		/// <param name="errorMsg">错误消息</param>
		/// <param name="data">附加数据</param>
		public ResultInfo(int errorNo, string errorMsg, T data = default(T))
			: base(errorNo, errorMsg)
		{
			this.Data = data;
		}

		/// <summary>
		/// 附加数据，失败时数据可能为空
		/// </summary>
		public T Data { get; set; }
	}
}
