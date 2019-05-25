using System.Collections.Generic;

namespace IQF.Framework.IModules
{
	/// <summary>
	/// 增量数据源
	/// </summary>
	/// <typeparam name="TData">数据类型</typeparam>
	public interface IIncrDataSource<TData>
	{
		/// <summary>
		/// 获取所有数据
		/// </summary>
		/// <returns></returns>
		List<TData> LoadAll();

		/// <summary>
		/// 获取唯一KEY
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		string GetKey(TData data);

		/// <summary>
		/// 增量加载
		/// </summary>
		/// <param name="loadedDatas">已加载的数据</param>
		/// <returns></returns>
		List<TData> IncrLoad(IEnumerable<TData> loadedDatas);
	}
}
