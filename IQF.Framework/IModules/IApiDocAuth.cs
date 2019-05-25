namespace IQF.Framework.IModules
{
	/// <summary>
	/// Api文档权限控制
	/// </summary>
	public interface IApiDocAuth
	{
		/// <summary>
		/// 是否有查看权限
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <param name="url"></param>
		/// <returns></returns>
		bool HasViewAuth(string userName, string password, string url);
	}
}
